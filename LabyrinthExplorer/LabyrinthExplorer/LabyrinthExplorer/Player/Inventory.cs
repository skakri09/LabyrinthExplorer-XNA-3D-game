using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LabyrinthExplorer
{
    public struct InventoryItem
    {
        public InventoryItem(IInventoryItem item, string identifier)
        {
            this.item = item;
            this.identifier = identifier;
        }

        public IInventoryItem item;
        public string identifier;
    }

    public class Inventory
    {
        private Dictionary<InventoryItem, Vector3> InventoryItems;

        public Inventory()
        {
            InventoryItems = new Dictionary<InventoryItem, Vector3>();
        }

        public void AddItem(InventoryItem item)
        {
            InventoryItems.Add(item, GetScreenOffset());
        }

        public void RemoveItem(InventoryItem item)
        {
            InventoryItems.Remove(item);
        }

        public bool HaveItem(InventoryItem item)
        {
            return (InventoryItems.ContainsKey(item));
        }

        public void Update(float deltaTime, Camera camera)
        {
            for (int i = 0; i < InventoryItems.Count; i++)
            {
                InventoryItems.ElementAt(i).Key.item.Update(
                    deltaTime, camera,
                    InventoryItems.ElementAt(i).Value);

            }
        }

        public bool HaveItemOfType(string _identifier)
        {
            for (int i = 0; i < InventoryItems.Count; i++)
            {
                if(InventoryItems.ElementAt(i).Key.identifier == _identifier)
                {
                    return true;
                }
            }
            return false;
        }

        public void DrawInventory(GraphicsDevice device, Camera cam)
        {
            for (int i = 0; i < InventoryItems.Count; i++)
            {
                InventoryItems.ElementAt(i).Key.item.Draw(cam);

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
