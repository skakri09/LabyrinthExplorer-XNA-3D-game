using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LabyrinthExplorer.Managers
{
    public static class Interactables
    {
        public static List<AABB> interactables;

        public static void AddInteractable(AABB interactable)
        {
            if (!interactables.Contains(interactable))
                interactables.Add(interactable);
        }

        public static bool IsInRange(AABB you, AABB target)
        {
            return (you.CheckCollision(target) != Vector3.Zero);
        }

        public static List<AABB> GetInteractablesInRange(AABB you)
        {
            List<AABB> retList = new List<AABB>();

            foreach (AABB aabb in interactables)
            {
                if (IsInRange(you, aabb))
                    retList.Add(aabb);
            }
            return retList;
        }

    }
}
