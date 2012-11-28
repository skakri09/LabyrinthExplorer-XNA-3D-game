using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LabyrinthExplorer
{
    public class PatrolX : AIState
    {
        private Vector3 startPos;
        private Vector3 endPos;

        private Direction currDir;

        float acceleration;
        float maxVelocity;

        float NegativeHeading = 90;
        float PositiveHeading = 270;

        Vector3 originalVelocity;

        public PatrolX(Vector3 startPos, Vector3 endPos, float acceleration = 250.0f, float maxVelocity = 250.0f)
        {
            this.startPos = startPos;
            this.endPos = endPos;
            this.acceleration = acceleration;
            this.maxVelocity = maxVelocity;
        }

        public void Enter(Enemy owner)
        {
            owner.Velocity = endPos - startPos;
            Vector3 vel = owner.Velocity;
            vel.Normalize();
            owner.Velocity = vel;
            originalVelocity = owner.Velocity;
            owner.Position = startPos;

            if (originalVelocity.X < 0)
            {
                currDir = Direction.NEGATIVE_X;
                owner.Rotation = new Vector3(0, NegativeHeading, 0);
            }
            else
            {
                currDir = Direction.POSITIVE_X;
                owner.Rotation = new Vector3(0, PositiveHeading, 0);
            }
        }

        public void UpdateState(Enemy owner, float deltaTime)
        {
            switch (currDir)
            {
                case Direction.NEGATIVE_X:
                    UpdateLeft(deltaTime, owner);
                    break;
                case Direction.POSITIVE_X:
                    UpdateRight(deltaTime, owner);
                    break;
            }
        }

        void UpdateLeft(float deltaTime, Enemy owner)
        {
            float distance = Vector3.Distance(owner.Position, startPos);
            if (distance <= 400)
            {
                if (distance <= 200)
                {
                    if (owner.Rotation.Y < 270)
                    {
                        owner.Velocity = new Vector3(owner.Velocity.X + (acceleration * deltaTime), owner.Velocity.Y, owner.Velocity.Z);
                        owner.Rotation = new Vector3(owner.Rotation.X, owner.Rotation.Y + (90 * deltaTime), 0);
                    }
                    else
                    {
                        currDir = Direction.POSITIVE_X;
                        owner.Rotation = new Vector3(0, PositiveHeading, 0);
                        owner.Velocity = endPos - startPos;
                        Vector3 vel = owner.Velocity;
                        vel.Normalize();
                        owner.Velocity = vel;
                        originalVelocity = owner.Velocity;
                    }
                }
                else
                    owner.Velocity = new Vector3(originalVelocity.X * (distance / 400), originalVelocity.Y, originalVelocity.Z);
            }
            else if (owner.Velocity.X > -maxVelocity)
            {
                owner.Velocity = new Vector3(owner.Velocity.X - (acceleration * deltaTime), owner.Velocity.Y, owner.Velocity.Z);
                originalVelocity = owner.Velocity;
            }
        }

        void UpdateRight(float deltaTime, Enemy owner)
        {
            float distance = Vector3.Distance(owner.Position, endPos);
            if (distance <= 400)
            {
                if (distance <= 200)
                {
                    if (owner.Rotation.Y > 90)
                    {
                        owner.Velocity = new Vector3(owner.Velocity.X - (acceleration * deltaTime), 0, 0);
                        owner.Rotation = new Vector3(owner.Rotation.X, owner.Rotation.Y - (90 * deltaTime), 0);
                    }
                    else
                    {
                        currDir = Direction.NEGATIVE_X;
                        owner.Rotation = new Vector3(0, 90, 0);
                        owner.Velocity = startPos - endPos;
                        Vector3 vel = owner.Velocity;
                        vel.Normalize();
                        owner.Velocity = vel;
                        originalVelocity = owner.Velocity;
                    }
                }
                else
                    owner.Velocity = new Vector3(originalVelocity.X * (distance / 400), originalVelocity.Z, originalVelocity.Y);
            }
             else if (owner.Velocity.X < maxVelocity)
            {
                owner.Velocity = new Vector3(owner.Velocity.X + (acceleration * deltaTime), owner.Velocity.Y, owner.Velocity.Z);
                originalVelocity = owner.Velocity;
            }
        }

        public void Exit(Enemy owner)
        {
            owner.Velocity = Vector3.Zero;
        }

        private enum Direction
        {
            POSITIVE_X,
            NEGATIVE_X
        };
    }
}
