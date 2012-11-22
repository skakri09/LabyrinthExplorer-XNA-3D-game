using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace LabyrinthExplorer
{
    public class Chest : EnvironmentObject, IInteractableObject
    {
        private bool isClosed;

        private Model closedModel;
        private Model openModel;

        public List<ChestItem> ChestInventory = new List<ChestItem>();

        public Chest(ContentManager content,
                     Vector3 position, Vector3 rotation,
                    float scale, Vector3 openFromDirection,
                    bool isClosed = true)
            : base(@"Models\Environment\ChestClosed", 
                content, position, rotation, scale)
        {
            this.isClosed = isClosed;
            closedModel = base.GetModel();
            openModel = content.Load<Model>(@"Models\Environment\ChestOpen");
            CreateUseAABB(openFromDirection, position, 100, 100);
            Interactables.AddInteractable(this);
            emitter = new AudioEmitter();
            emitter.Position = position;
        }

        public Chest(ContentManager content,
             Vector3 position, Vector3 rotation,
            float scale, Vector3 openFromDirection,
            ChestItem[] chestItems,
            bool isClosed = true)
            : base(@"Models\Environment\ChestClosed",
                content, position, rotation, scale)
        {
            ChestInventory.AddRange(chestItems);
            this.isClosed = isClosed;
            closedModel = base.GetModel();
            openModel = content.Load<Model>(@"Models\Environment\ChestOpen");
            CreateUseAABB(openFromDirection, position, 100, 100);
            Interactables.AddInteractable(this);
            emitter = new AudioEmitter();
            emitter.Position = position;
        }

        public void Use(AABB interactingParty)
        {
            if (isClosed)
            {
                Game.SoundManager.PlaySound("ChestOpen");
                base.SetModel(openModel);
                isClosed = false;
            }
            else
            {
                if (ChestInventory.Count == 0)
                {
                    Game.SoundManager.PlaySound("ChestClose");
                    base.SetModel(closedModel);
                    isClosed = true;
                }
                else
                {
                    if (interactingParty is Player)
                    {
                        Player player = (Player)interactingParty;
                        foreach(ChestItem item in ChestInventory)
                        {
                            item.OnChestOpen(interactingParty);
                        }
                        ChestInventory.Clear();
                    }
                }
            }
        }

        public bool ChestContains(ChestItem item)
        {
            return ChestInventory.Contains(item);
        }

        public ChestItem GetItemFromChest(ChestItem item)
        {
            if (!isClosed)
            {
                foreach (ChestItem i in ChestInventory)
                {
                    if (i == item)
                    {
                        ChestItem retItem = i;
                        ChestInventory.Remove(i);
                        return retItem;
                    }
                }
            }
            return null;
        }    
    }
}
