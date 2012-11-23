using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace LabyrinthExplorer
{
    public class Door : EnvironmentObject, IInteractableObject
    {

        public enum DoorState
        {
            CLOSED,
            OPENING,
            OPEN,
            CLOSING
        }

        private DoorState doorState;
        private Vector3 orgOpenPos;
        private Vector3 orgMoveToPos;
        float openTimer = 0;
        private bool canToggle;
        Vector3 velocity;
        AABB collisionAABB;
        float openingDuration = 3.5f;
        public string OpenedByKeyWithID
        {
            get;
            private set;
        }
        public Door(ContentManager content,
                     Vector3 position, Vector3 rotation,
                    Vector3 moveToPos,
                    float scale, Vector3 openFromDirection,
                    ref List<AABB> collisionList,
                    string openedByKeyWithID,
                    DoorState state = DoorState.CLOSED, bool canToggle = false)
            :base(@"Models\Environment\SecretDoor", content, position, rotation, scale)
        {
            this.OpenedByKeyWithID = openedByKeyWithID;
            this.doorState = state;
            CreateUseAABB(openFromDirection, position, 150, 150);//make more accurate when we see the scale of the door
            Interactables.AddInteractable(this);
            emitter = new AudioEmitter();
            emitter.Position = position;
            orgOpenPos = moveToPos;
            orgMoveToPos = moveToPos;
            this.canToggle = canToggle;
            collisionAABB = new AABB();
            CreateCollision(position, rotation, scale);
            collisionList.Add(collisionAABB);
            velocity = Vector3.Subtract(moveToPos, position);

        }

        public override void OnEnteringArea()
        {
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            UpdateDoorState(deltaTime);
        }

        public void Use(AABB interactingParty)
        {
            if (doorState == DoorState.CLOSED)
            {
                //check if player have the key, then open
                if (interactingParty is Player)
                {
                    Player player = (Player)interactingParty;
                    if(player.inv.HaveItemOfType(OpenedByKeyWithID))
                    {
                        Game.SoundManager.PlaySound("DoorOpen", this);
                        openTimer = 0.0f;
                        doorState = DoorState.OPENING;
                    }
                }
            }
        }

        private void UpdateDoorState(float deltaTime)
        {
            openTimer += deltaTime;
            if (canToggle)
            {
                if (doorState == DoorState.CLOSING)
                {
                    position += velocity * deltaTime * (openingDuration / 10);
                    if (openTimer >= openingDuration)
                    {
                        position = orgOpenPos;
                        doorState = DoorState.CLOSED;
                    }
                    CreateCollision(base.position, base.rotation, base.Scale);
                }
            }
            else if (doorState == DoorState.OPENING)
            {
                position += velocity*deltaTime*(openingDuration/10);
                if(openTimer >= openingDuration)
                {
                    position = orgOpenPos;
                    doorState = DoorState.OPEN; 
                }
                CreateCollision(base.position, base.rotation, base.Scale);
            }
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

            collisionAABB.SetAABB(min, max);
        }

        public void UsedCallback()
        {
        }
    }
}
