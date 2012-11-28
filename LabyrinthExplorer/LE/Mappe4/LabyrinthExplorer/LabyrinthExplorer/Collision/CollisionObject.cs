using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LabyrinthExplorer
{
    public class CollisionObject : AABB
    {
        public CollisionObject(Vector2 minPoint, Vector2 maxPoint)
            :base(minPoint, maxPoint)
        {

        }
    }
}
