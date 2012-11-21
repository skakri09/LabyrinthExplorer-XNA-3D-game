using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace LabyrinthExplorer
{
    public class Gate : EnvironmentObject, IInteractableObject
    {
        private bool closed;
        private Vector3 openedPosition;
        private Vector3 openingVelocity;

        public Gate(ContentManager content, Vector3 position,
                    Vector3 rotation, float scale, bool isClosed = true)
            :base(@"Models\Environment\Gate", content, position, rotation, scale)
        {
            this.closed = isClosed;
            openedPosition = base.Position;
            openedPosition.Y += GameConstants.WALL_HEIGHT;
            openingVelocity = new Vector3(0, 100, 0);
            CreateCollision(Position, rotation, scale);
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            if (!closed)
            {
                if (base.Position.Y < openedPosition.Y)
                {
                    base.Position += (openingVelocity * deltaTime);
                    CreateCollision(base.Position, base.Rotation, base.Scale);
                }
                else
                    closed = false;
            }
        }
        
        public void Use()
        {
            OpenGate();
        }

        private void OpenGate()
        {
            Game.SoundManager.PlaySound("GateDoorOpening");
            closed = false;
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
