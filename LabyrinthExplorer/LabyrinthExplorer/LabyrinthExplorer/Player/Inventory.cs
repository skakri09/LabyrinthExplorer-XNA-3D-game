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
        private List<IInventoryItem> InventoryItems;

        public Inventory()
        {
            InventoryItems = new List<IInventoryItem>();
        }

        public void AddItem(IInventoryItem item)
        {
            InventoryItems.Add(item);
        }

        public void RemoveItem(IInventoryItem item)
        {
            InventoryItems.Remove(item);
        }

        public bool HaveItem(IInventoryItem item)
        {
            return (InventoryItems.Contains(item));
        }

        public List<IInventoryItem> GetInventory()
        {
            return InventoryItems;
        }

        public void Update(float deltaTime)
        {
            foreach (IInventoryItem item in InventoryItems)
            {
                item.Update(deltaTime, true);
            }
        }

        public void DrawInventory(GraphicsDevice device, Camera cam)
        {
            foreach (IInventoryItem item in InventoryItems)
            {
                item.Draw(cam, Vector3.Zero);//need implementation
            }
        }

    }
}
