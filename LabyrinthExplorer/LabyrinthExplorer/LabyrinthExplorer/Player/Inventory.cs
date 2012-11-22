using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LabyrinthExplorer
{
    public class Inventory
    {
        private Dictionary<IInventoryItem, Vector3> InventoryItems;

        public Inventory()
        {
            InventoryItems = new Dictionary<IInventoryItem, Vector3>();
        }

        public void AddItem(IInventoryItem item)
        {
            InventoryItems.Add(item, GetScreenOffset());
        }

        public void RemoveItem(IInventoryItem item)
        {
            InventoryItems.Remove(item);
        }

        public bool HaveItem(IInventoryItem item)
        {
            return (InventoryItems.ContainsKey(item));
        }

        public void Update(float deltaTime, Camera camera)
        {
            for (int i = 0; i < InventoryItems.Count; i++)
            {
                InventoryItems.ElementAt(i).Key.Update(
                    deltaTime, camera,
                    InventoryItems.ElementAt(i).Value);

            }
        }

        public void DrawInventory(GraphicsDevice device, Camera cam)
        {
            for (int i = 0; i < InventoryItems.Count; i++)
            {
                InventoryItems.ElementAt(i).Key.Draw(cam);

            }
        }

        private Vector3 GetScreenOffset()
        {
            return new Vector3(4.3f, (1.7f-((float)InventoryItems.Count/1.6f)), 8);
        }

        private void FreeScreenOffset()
        {

        }

    }
}
