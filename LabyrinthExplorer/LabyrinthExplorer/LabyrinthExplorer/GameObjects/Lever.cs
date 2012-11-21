using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LabyrinthExplorer
{
    public class Lever : EnvironmentObject, IInteractableObject
    {
        private bool isUsed;

        private Model closedModel;
        private Model usedModel;

        public Lever(ContentManager content,
                     Vector3 position, Vector3 rotation,
                    float scale, Vector3 openFromDirection,
                    bool isUsed = false)
            : base(@"Models\Environment\Lever",
                content, position, rotation, scale)
        {
            this.isUsed = isUsed;
            closedModel = base.GetModel();
            usedModel = content.Load<Model>(@"Models\Environment\LeverUsed");
            CreateUseAABB(openFromDirection, Position, 100, 100);
            
            Interactables.AddInteractable(this);
        }

        public void Use()
        {
            if (!isUsed)
            {
                Game.SoundManager.PlaySound("LeverUsed");
                base.SetModel(usedModel);
                isUsed = true;
            }
        }

    }
}
