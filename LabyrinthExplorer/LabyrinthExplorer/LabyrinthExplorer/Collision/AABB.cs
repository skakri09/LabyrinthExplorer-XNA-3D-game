using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LabyrinthExplorer
{
    public class AABB
    {
        Vector3 minPoint;
        Vector3 maxPoint;

        public AABB(Vector3 btmFrontLeft, Vector3 btmFrontRight,
            Vector3 btmBackRight, Vector3 btmBackLeft, float height)
        {
            minPoint = btmBackLeft;
            maxPoint = new Vector3(btmFrontRight.X, height, btmFrontRight.Z);
        }


    }
}
