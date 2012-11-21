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

        public void Use(AABB interactingParty)
        {
            if (isClosed)
            {
                Game.SoundManager.PlaySound("ChestOpen", 1.0f, this);
                base.SetModel(openModel);
                isClosed = false;
            }
            else
            {
                Game.SoundManager.PlaySound("ChestClose", this);
                base.SetModel(closedModel);
                isClosed = true;
            }
        }
    
    }
}
