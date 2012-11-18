using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LabyrinthExplorer
{
    public class PatrolZ : AIState
    {
        private Vector3 startPos;
        private Vector3 endPos;
        private Vector3 currentTarget;
        private Vector3 otherTarget;
        private Direction currDir;

        float acceleration;
        float maxVelocity;

        float negativeHeading = 0;
        float positiveHeading = 180;

        Vector3 originalVelocity;

        public PatrolZ(Vector3 startPos, Vector3 endPos, float acceleration = 250.0f, float maxVelocity = 250.0f)
        {
            this.startPos = startPos;
            this.endPos = endPos;
            this.acceleration = acceleration;
            this.maxVelocity = maxVelocity;
            currentTarget = endPos;
            otherTarget = startPos;
        }

        public void Enter(Enemy owner)
        {
            owner.Velocity = endPos - startPos;
            Vector3 vel = owner.Velocity;
            vel.Normalize();
            owner.Velocity = vel;
            originalVelocity = owner.Velocity;
            owner.Position = startPos;

            if (originalVelocity.Z < 0)
            {
                currDir = Direction.NEGATIVE_Z;
                owner.Rotation = new Vector3(0, negativeHeading, 0);
            }
            else
            {
                currDir = Direction.POSITIVE_Z;
                owner.Rotation = new Vector3(0, positiveHeading, 0);
            }
        }

        public void UpdateState(Enemy owner, float deltaTime)
        {
            switch (currDir)
            {
                case Direction.NEGATIVE_Z:
                    UpdateNegative(deltaTime, owner);
                    break;
                case Direction.POSITIVE_Z:
                    UpdatePositive(deltaTime, owner);
                    break;
            }
        }

        void UpdateNegative(float deltaTime, Enemy owner)
        {
            float distance = Vector3.Distance(owner.Position, currentTarget);
            if (distance <= 400)
            {
                if (distance <= 200)
                {
                    if (owner.Rotation.Y < positiveHeading)
                    {
                        owner.Velocity = new Vector3(owner.Velocity.X, owner.Velocity.Y, owner.Velocity.Z + (acceleration * deltaTime));
                        owner.Rotation = new Vector3(owner.Rotation.X, owner.Rotation.Y + (90 * deltaTime), 0);
                    }
                    else
                    {
                        currDir = Direction.POSITIVE_Z;
                        ChangeTarget();
                        owner.Velocity = currentTarget - otherTarget;
                        Vector3 vel = owner.Velocity;
                        vel.Normalize();
                        owner.Velocity = vel;
                        originalVelocity = owner.Velocity;
                        owner.Rotation = new Vector3(0, positiveHeading, 0);
                       
                    }
                }
                else
                    owner.Velocity = new Vector3(originalVelocity.X, originalVelocity.Y, originalVelocity.Z * (distance / 400));
            }
            else if (owner.Velocity.Z > -maxVelocity)
            {
                owner.Velocity = new Vector3(owner.Velocity.X, owner.Velocity.Y, owner.Velocity.Z - (acceleration * deltaTime));
                originalVelocity = owner.Velocity;
            }
        }

        void UpdatePositive(float deltaTime, Enemy owner)
        {
            float distance = Vector3.Distance(owner.Position, currentTarget);
            if (distance <= 400)
            {
                if (distance <= 200)
                {
                    if (owner.Rotation.Y > negativeHeading)
                    {
                        owner.Velocity = new Vector3(owner.Velocity.X, owner.Velocity.Y, owner.Velocity.Z - (acceleration * deltaTime));
                        owner.Rotation = new Vector3(owner.Rotation.X, owner.Rotation.Y - (90 * deltaTime), 0);
                    }
                    else
                    {
                        currDir = Direction.NEGATIVE_Z;
                        owner.Rotation = new Vector3(0, negativeHeading, 0);
                        ChangeTarget();
                        owner.Velocity = currentTarget - otherTarget;
                        Vector3 vel = owner.Velocity;
                        vel.Normalize();
                        owner.Velocity = vel;
                        originalVelocity = owner.Velocity;
                    }
                }
                else
                    owner.Velocity = new Vector3(originalVelocity.X, originalVelocity.Y, originalVelocity.Z * (distance / 400));
            }
            else if (owner.Velocity.Z < maxVelocity)
            {
                owner.Velocity = new Vector3(owner.Velocity.X, owner.Velocity.Y, owner.Velocity.Z + (acceleration * deltaTime));
                originalVelocity = owner.Velocity;
            }
        }

        public void Exit(Enemy owner)
        {
            owner.Velocity = Vector3.Zero;
        }

        private void ChangeTarget()
        {
            Vector3 curTemp = currentTarget;
            Vector3 otherTemp = otherTarget;
            otherTarget = curTemp;
            currentTarget = otherTemp;
        }
        private enum Direction
        {
            POSITIVE_Z,
            NEGATIVE_Z
        };
    }
}
