using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LabyrinthExplorer
{
    public class ManWalker : AIState
    {
        private Vector3 leftTarPos = new Vector3(1100, 0, 3300);
        private Vector3 rightTarPos = new Vector3(4600, 0, 3300);
        private Vector3 velocity;
        private Vector3 rotation;
        private Vector3 maxVelocity;

        private Direction currDir;

        public void Enter(Enemy owner)
        {
            velocity = leftTarPos - rightTarPos;
            velocity.Normalize();
            maxVelocity = velocity * 300;

            rotation = new Vector3(0, 90, 0);
            owner.Rotation = rotation;
            owner.Position = rightTarPos;
            owner.Velocity = velocity;

            currDir = Direction.LEFT;
            velocity = maxVelocity;
        }

        public void UpdateState(Enemy owner, float deltaTime)
        {
            switch (currDir)
            {
                case Direction.LEFT:
                    UpdateLeft(deltaTime, owner);
                    break;
                case Direction.RIGHT:
                    UpdateRight(deltaTime, owner);
                    break;
            }
        }

        void UpdateLeft(float deltaTime, Enemy owner)
        {
            float distance = Vector3.Distance(owner.Position, leftTarPos);

            //if (distance <= 400)
            //{
            //    velocity = Vector3.Subtract(velocity, new Vector3(100 * deltaTime, 0, 0));
            //    if(velocity.X <= 0) 
            //        velocity = Vector3.Zero;
            //    if (rotation.Y < 180)
            //        rotation = Vector3.Add(rotation, new Vector3(0, 90 * deltaTime, 0));//+= 90 * deltaTime;
            //}

            owner.Velocity = velocity;
        }

        void UpdateRight(float deltaTime, Enemy owner)
        {
            float distance = Vector3.Distance(owner.Position, rightTarPos);


            owner.Velocity = velocity;
        }

        public void Exit(Enemy owner)
        {
            owner.Velocity = Vector3.Zero;
        }

        private enum Direction
        {
            RIGHT,
            LEFT
        };
    }
}
