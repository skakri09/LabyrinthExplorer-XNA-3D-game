using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace LabyrinthExplorer
{
    public class FinalGate : EnvironmentObject, IInteractableObject
    {
        private GateDoor leftDoor;
        private GateDoor rightDoor;

        private GateState currentState;
        private float openingTimer = 0;
        private bool havePlayedSound1 = false;
        private bool havePlayedSound2 = false;
        private bool havePlayedSound3 = false;

        private enum GateState
        {
            CLOSED,
            OPENING,
            OPEN,
        }
        
        public FinalGate(ContentManager content,
            ref List<AABB> collisionList,
            Vector3 position, Vector3 rotation, float scale)
            : base(@"Models\Environment\FinalGatePillars", content, position, rotation, scale)
        {
            CreateDoors(content, ref collisionList, position, scale);
            base.FogEnd = 3000;
            currentState = GateState.CLOSED;
            emitter = new AudioEmitter();
            emitter.Position = position;
        }
        
        private void CreateDoors(ContentManager content,
            ref List<AABB> collisionList,
            Vector3 position, float scale)
        {
            leftDoor = new GateDoor(content, ref collisionList,
                new Vector3(position.X, position.Y, position.Z), scale, @"Models\Environment\FinalGateLeftHalf",
            new Vector3(position.X + 500, position.Y, position.Z));

            rightDoor = new GateDoor(content, ref collisionList,
                new Vector3(position.X, position.Y, position.Z), scale, @"Models\Environment\FinalGateRightHalf",
            new Vector3(position.X - 500, position.Y, position.Z));
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            
            if (currentState == GateState.OPENING)
            {
                openingTimer += deltaTime;
                if (!havePlayedSound1)
                {
                    havePlayedSound1 = true;
                    Game.SoundManager.PlaySound("GateOpening1", this);
                }
                else if (openingTimer >= 6.0f && !havePlayedSound2)
                {
                    havePlayedSound2 = true;
                    Game.SoundManager.PlaySound("GateOpening2", this);
                }
                else if (openingTimer >= 12 && !havePlayedSound3)
                {
                    havePlayedSound3 = true;
                    leftDoor.PlaySound();
                    rightDoor.PlaySound();
                    leftDoor.Open();
                    rightDoor.Open();
                }

                if(openingTimer >= 26.0f)
                {
                    leftDoor.StopOpening();
                    rightDoor.StopOpening();
                }
                
            }
            leftDoor.Update(deltaTime);
            rightDoor.Update(deltaTime);
        }

        public override void Draw(Camera camera, Microsoft.Xna.Framework.Graphics.Effect effect)
        {
            base.Draw(camera, effect);
            leftDoor.Draw(camera, effect);
            rightDoor.Draw(camera, effect);

        }
        public override void OnEnteringArea()
        {
            
        }

        public void Use(AABB interactingParty)
        {
            if (currentState == GateState.CLOSED)
            {
                currentState = GateState.OPENING;
            }
        }

        public void UsedCallback()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        private class GateDoor : EnvironmentObject, IInteractableObject
        {

            private Vector3 moveToPos;
            private Vector3 velocity;
            private GateState currentState;

            public GateDoor(ContentManager content,
            ref List<AABB> collisionList,
            Vector3 position, float scale,
                string meshName, Vector3 _moveToPos)
                :base(meshName, content, position, new Vector3(0, 180, 0), scale)
            {
                base.FogEnd = 3000;

                currentState = GateState.CLOSED;

                CreateCollision(position, new Vector3(0, 180, 0), scale);
                collisionList.Add(this);
                emitter = new AudioEmitter();
                emitter.Position = position;
                this.moveToPos = _moveToPos;
                velocity = Vector3.Subtract(moveToPos, position);
                velocity.Normalize();
                velocity *= 45;//Needs tweaking
            }

            public void PlaySound()
            {
                Game.SoundManager.PlaySound("GateOpening3", this);
            }

            public override void OnEnteringArea()
            {

            }

            public override void Update(float deltaTime)
            {
                base.Update(deltaTime);

                if (currentState == GateState.OPENING)
                {
                    base.position += velocity * deltaTime;
                    CreateCollision(base.position, new Vector3(0, 180, 0), base.Scale);
                }
            }

            public void Open()
            {
                currentState = GateState.OPENING;
            }
            public void StopOpening()
            {
                currentState =  GateState.OPEN;
            }

            public void Use(AABB interactingParty)
            {
                currentState = GateState.OPENING;
            }

            public void UsedCallback()
            {

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
        }

    }
}
