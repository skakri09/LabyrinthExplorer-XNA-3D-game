using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LabyrinthExplorer
{
    public class AABB
    {
        Vector3 minPoint, orgMinPoint;
        Vector3 maxPoint, orgMaxPoint;

        public AABB(Vector3 btmFrontLeft, Vector3 btmFrontRight,
            Vector3 btmBackRight, Vector3 btmBackLeft, float height)
        {
            orgMinPoint = minPoint = btmBackLeft;
            orgMaxPoint = maxPoint = new Vector3(btmFrontRight.X, height, btmFrontRight.Z);
            CollisionManager.AddCollidable(this);
        }

        public AABB(Vector3 position, float padding)
        {
            orgMinPoint = minPoint = new Vector3(position.X - padding, position.Y - padding, position.Z - padding);
            orgMaxPoint = maxPoint = new Vector3(position.X + padding, position.Y + padding, position.Z + padding);
            CollisionManager.AddCollidable(this);
        }

        public AABB(Vector3 _minPoint, Vector3 _maxPoint)
        {
            SetAABB(_minPoint, _maxPoint);
        }

        //Empty ctor for setting aabb manually, used by environment objects
        public AABB()
        {
        }

        protected void SetAABB(Vector3 _minPoint, Vector3 _maxPoint)
        {
            orgMinPoint = minPoint = _minPoint;
            orgMaxPoint = maxPoint = _maxPoint;
            CollisionManager.AddCollidable(this);
        }

        public void UpdateAABB(Vector3 displacement)
        {
            minPoint = orgMinPoint;
            maxPoint = orgMaxPoint;

            minPoint += displacement;
            maxPoint += displacement;
        }

        public Vector3 CheckCollision(AABB otherAABB)
        {
            if (maxPoint.Z < otherAABB.MinPoint.Z) return Vector3.Zero;
            if (minPoint.Z > otherAABB.MaxPoint.Z) return Vector3.Zero;
            if (maxPoint.X < otherAABB.MinPoint.X) return Vector3.Zero;
            if (maxPoint.Y < otherAABB.MinPoint.Y) return Vector3.Zero;
            if (minPoint.X > otherAABB.MaxPoint.X) return Vector3.Zero;
            if (minPoint.Y > otherAABB.MaxPoint.Y) return Vector3.Zero;

            return minimumTranslation(otherAABB);
        }

        /// <summary>
        /// NOTE: ONLY CEHCKING COLLISION IN X AND Z AXIS! if at any point
        /// Y axis collision is needed, it must be edited inn.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Vector3 minimumTranslation(AABB other)
        {
            Vector2 amin = new Vector2(minPoint.X, minPoint.Z);
            Vector2 amax = new Vector2(maxPoint.X, maxPoint.Z);
            Vector2 bmin = new Vector2(other.MinPoint.X, other.MinPoint.Z);
            Vector2 bmax = new Vector2(other.MaxPoint.X, other.MaxPoint.Z);

            Vector2 mtd = new Vector2();

            float left = (bmin.X - amax.X);
            float right = (bmax.X - amin.X);
            float top = (bmin.Y - amax.Y);
            float bottom = (bmax.Y - amin.Y);

            // box dont intersect   
            if (left > 0 || right < 0) throw new Exception("no intersection");
            if (top > 0 || bottom < 0) throw new Exception("no intersection");

            // box intersect. work out the mtd on both x and y axes.
            if (Math.Abs(left) < right)
                mtd.X = left;
            else
                mtd.X = right;

            if (Math.Abs(top) < bottom)
                mtd.Y = top;
            else
                mtd.Y = bottom;

            // 0 the axis with the largest mtd value.
            if (Math.Abs(mtd.X) < Math.Abs(mtd.Y))
                mtd.Y = 0;
            else
                mtd.X = 0;
            return new Vector3(mtd.X, 0, mtd.Y);
        }

        public Vector3 MinPoint { get { return minPoint; } }

        public Vector3 MaxPoint { get { return maxPoint; } }

    }
}
