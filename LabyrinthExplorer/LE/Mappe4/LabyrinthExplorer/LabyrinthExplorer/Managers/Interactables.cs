﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LabyrinthExplorer
{
    public static class Interactables
    {
        //public static List<AABB> interactables = new List<AABB>();

        public static void AddInteractable(AABB interactable)
        {
            if (!World.currentArea.Interactables.Contains(interactable))
                World.currentArea.Interactables.Add(interactable);
        }

        public static bool IsInRange(AABB you, AABB target)
        {
            return (you.CheckCollision(target) != Vector3.Zero);
        }

        public static void RemoveInteractable(AABB interactableToRemove)
        {
            World.currentArea.Interactables.Remove(interactableToRemove);
            //interactables.Remove(interactableToRemove);
        }

        public static List<AABB> GetInteractablesInRange(AABB you)
        {
            List<AABB> retList = new List<AABB>();

            foreach (AABB aabb in World.currentArea.Interactables)
            {
                if (IsInRange(you, aabb))
                    retList.Add(aabb);
            }
            return retList;
        }

    }
}