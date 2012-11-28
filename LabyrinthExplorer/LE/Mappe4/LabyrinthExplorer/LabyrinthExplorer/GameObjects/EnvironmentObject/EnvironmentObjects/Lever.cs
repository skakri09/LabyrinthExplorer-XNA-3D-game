using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace LabyrinthExplorer
{
    public class Lever : EnvironmentObject, IInteractableObject, I3DSoundCustDivFact
    {
        private bool isUsed;

        private Model closedModel;
        private Model usedModel;
        private IInteractableObject onUseObject;

        public Lever(ContentManager content,
                     Vector3 position, Vector3 rotation,
                    float scale, Vector3 openFromDirection,
                     IInteractableObject LeverUseObject,
                    bool isUsed = false)
            : base(@"Models\Environment\Lever",
                content, position, rotation, scale)
        {
            this.isUsed = isUsed;
            closedModel = base.GetModel();
            usedModel = content.Load<Model>(@"Models\Environment\LeverUsed");
            CreateUseAABB(openFromDirection, Position, 100, 100);
            this.onUseObject = LeverUseObject;
            Interactables.AddInteractable(this);
            emitter = new AudioEmitter();
            emitter.Position = position;
        }

        //lever for custom color levers
        public Lever(ContentManager content,
                     Vector3 position, Vector3 rotation,
                    float scale, Vector3 openFromDirection,
                     IInteractableObject LeverUseObject,
                    Vector3 color,
                    bool isUsed = false)
            : base(@"Models\Environment\Lever",
                content, position, rotation, scale)
        {
            this.isUsed = isUsed;
            base.color = color;
            closedModel = base.GetModel();
            usedModel = content.Load<Model>(@"Models\Environment\LeverUsed");
            CreateUseAABB(openFromDirection, Position, 100, 100);
            this.onUseObject = LeverUseObject;
            Interactables.AddInteractable(this);
            emitter = new AudioEmitter();
            emitter.Position = position;
        }

        public override void OnEnteringArea()
        {
           
        }

        //Ctor without onUse object, used by duoLever objects
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
            emitter = new AudioEmitter();
            emitter.Position = position;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            //timer for puting lever bac kdown
        }
        public void Use(AABB interactingParty)
        {
            if (!isUsed)
            {
                Game.SoundManager.PlaySound("LeverUsed", this);
                base.SetModel(usedModel);
                if (onUseObject != null && interactingParty != null)
                {
                    onUseObject.Use(this);
                }
                
                isUsed = true;
            }
        }

        public void SetUnused()
        {
            if (isUsed)
            {
                Game.SoundManager.PlaySound("LeverUsed", this);
                base.SetModel(closedModel);
                isUsed = false;
            }
        }

        public void UsedCallback()
        {
            SetUnused();
        }

        public float GetCustomDivisionFactor()
        {
            return 1000;
        }
    }
}
