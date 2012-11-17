using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

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
        }

        public AABB(Vector3 position, float padding)
        {
            orgMinPoint = minPoint = new Vector3(position.X - padding, position.Y - padding, position.Z - padding);
            orgMaxPoint = maxPoint = new Vector3(position.X + padding, position.Y + padding, position.Z + padding);
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

            return Vector3.Zero;
        }

        public Vector3 MinPoint { get { return minPoint; } }

        public Vector3 MaxPoint { get { return maxPoint; } }

    }
}
