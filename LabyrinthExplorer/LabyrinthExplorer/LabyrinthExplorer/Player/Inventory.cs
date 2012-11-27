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
        private Dictionary<Vector3, bool> inventoryPositions;

        public Inventory()
        {
            InventoryItems = new Dictionary<InventoryItem, Vector3>();
            inventoryPositions = new Dictionary<Vector3, bool>();
            
            //worlds most ridiculous way of creating slot positions for the inventory
            for (int i = 0; i < 6; i++)
            {
                inventoryPositions.Add(GetScreenOffset(), false);
                InventoryItems.Add(new InventoryItem(null, i.ToString()), new Vector3(i));
            }
            InventoryItems.Clear();
        }

        public IInventoryItem GetAndRemoveItem(string _identifier)
        {
            foreach (InventoryItem item in InventoryItems.Keys)
            {
                if (item.identifier == _identifier)
                {
                    IInventoryItem returnItem = item.item;
                    FreeScreenOffset(InventoryItems[item]);
                    InventoryItems.Remove(item);
                    return returnItem;
                }
            }
            return null;
        }

        public void AddItem(InventoryItem item)
        {
            if (item.identifier == "compass")
                InventoryItems.Add(item, new Vector3(3.6f, -1.5f, 8));
            else
                InventoryItems.Add(item, GetItemPosition());
                 
        }

        public void AddItems(List<InventoryItem> items)
        {
            foreach (InventoryItem item in items)
            {
                InventoryItems.Add(item, GetItemPosition());
            }
        }

        public void RemoveItem(InventoryItem item)
        {
            FreeScreenOffset(InventoryItems[item]);
            InventoryItems.Remove(item);
        }

        public bool HaveItem(InventoryItem item)
        {
            return (InventoryItems.ContainsKey(item));
        }
        
        public void RemoveItemsOfType(string _identifier)
        {
            for (int i = 0; i < InventoryItems.Count; i++)
            {
                if (InventoryItems.ElementAt(i).Key.identifier == _identifier)
                {
                    FreeScreenOffset(InventoryItems[InventoryItems.ElementAt(i).Key]);
                    InventoryItems.Remove(InventoryItems.ElementAt(i).Key);
                }
            }
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

        private Vector3 GetItemPosition()
        {
            foreach (Vector3 vec in inventoryPositions.Keys)
            {
                if(inventoryPositions[vec] == false)
                {
                    inventoryPositions[vec] = true;
                    return vec;
                }
            }
            throw new Exception("This awesome inventory only support 6 items dumbass");
        }

        private Vector3 GetScreenOffset()
        {
            return new Vector3(4.3f, (1.7f-((float)InventoryItems.Count/1.6f)), 8);
        }

        private void FreeScreenOffset(Vector3 position)
        {
            inventoryPositions[position] = false;
        }
    }
}
