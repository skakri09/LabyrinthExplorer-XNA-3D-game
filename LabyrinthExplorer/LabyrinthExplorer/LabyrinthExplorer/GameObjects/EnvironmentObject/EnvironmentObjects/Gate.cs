using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace LabyrinthExplorer
{
    public enum GateState { CLOSED, CLOSING, OPEN, OPENING };
    public class Gate : EnvironmentObject, IInteractableObject
    {
        private Vector3 openedPosition;
        private Vector3 closedPosition;
        private Vector3 openingVelocity;
        private Vector3 closingVelocity;
        private float closeAfter;
        private float gateBeenOpenFor;

        private List<IInteractableObject> interactingParties;

        public Gate(ContentManager content, Vector3 position,
                    Vector3 rotation, float scale, float closeAfterSeconds = 0, bool isClosed = true)
            :base(@"Models\Environment\Gate", content, position, rotation, scale)
        {
            gateState = GateState.CLOSED;
            openedPosition = closedPosition = base.Position;
            openedPosition.Y += GameConstants.WALL_HEIGHT;

            openingVelocity = new Vector3(0, 100, 0);
            closingVelocity = new Vector3(0, -100, 0);

            CreateCollision(Position, rotation, scale);
            if (closeAfterSeconds > 0)
            {
                closeAfter = closeAfterSeconds;
            }
            emitter = new AudioEmitter();
            emitter.Position = base.Position;
            interactingParties = new List<IInteractableObject>();
        }

        public override void OnEnteringArea()
        {
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            if (gateState == GateState.OPENING)
            {
                if (base.Position.Y < openedPosition.Y)
                {
                    base.Position += (openingVelocity * deltaTime);
                    CreateCollision(base.Position, base.Rotation, base.Scale);
                }
                else
                {
                    gateState = GateState.OPEN;
                    gateBeenOpenFor = 0.0f;
                }
            }
            else if (gateState == GateState.CLOSING)
            {
                if (base.Position.Y > closedPosition.Y)
                {
                    base.Position += (closingVelocity * deltaTime);
                    CreateCollision(base.Position, base.Rotation, base.Scale);
                }
                else
                {
                    foreach (IInteractableObject interactable in interactingParties)
                        interactable.UsedCallback();

                    gateState = GateState.CLOSED;
                }
            }
            else if (gateState == GateState.OPEN)
            {
                gateBeenOpenFor += deltaTime;
                if(gateBeenOpenFor >= closeAfter)
                {
                    CloseGate();
                }
            }
        }

        public void Use(AABB interactingParty)
        {
            if (gateState == GateState.OPEN)
            {
                CloseGate();
                if (interactingParty is IInteractableObject)
                {
                    IInteractableObject obj = (IInteractableObject)interactingParty;
                    if (interactingParties.Contains(obj))
                        interactingParties.Add(obj);
                }
            }
            else if (gateState == GateState.CLOSED)
            {
                OpenGate();
                if (interactingParty is IInteractableObject)
                {
                    IInteractableObject obj = (IInteractableObject)interactingParty;
                    if (!interactingParties.Contains(obj))
                        interactingParties.Add(obj);
                }
            }
        }

        private void OpenGate()
        {
            Game.SoundManager.PlaySound("GateDoorOpening", this);
            gateState = GateState.OPENING;
        }

        private void CloseGate()
        {
            Game.SoundManager.PlaySound("GateDoorClosing", this);
            gateState = GateState.CLOSING;
        }

        private void CreateCollision(Vector3 position, Vector3 rotation, float scale)
        {
            Matrix worldTransform = new Matrix();
            worldTransform = Matrix.CreateTranslation(Vector3.Zero)
            * Matrix.CreateRotationY(MathHelper.ToRadians(rotation.Y));
            AABB aabb = UpdateBoundingBox(base.GetModel(), worldTransform);
            Vector3 min = aabb.MinPoint;
            Vector3 max = aabb.MaxPoint;
            min *= scale;
            max *= scale;
            min += position;
            max += position;
            SetAABB(min, max);
        }

        public void UsedCallback()
        {

        }

        public GateState gateState { get; set; }
    }
}
