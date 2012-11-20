using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LabyrinthExplorer
{
    public static class CollisionManager
    {
        public static List<AABB> Collidables = new List<AABB>();

        public static void AddCollidable(AABB collidable)
        {
            if (!Collidables.Contains(collidable))
                Collidables.Add(collidable);   
        }

        public static List<AABB> GetCollidables()
        {
            return Collidables;
        }

        public static Vector3 CheckCollision(AABB you)
        {
            Vector3 returnValue = Vector3.Zero;
            foreach (AABB aabb in Collidables)
            {
                if (!aabb.Equals(you))
                {
                    returnValue += aabb.CheckCollision(you);
                }
            }
            return returnValue;
        }
    }
}
