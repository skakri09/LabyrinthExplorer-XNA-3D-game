using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using Microsoft.Xna.Framework.Audio;

namespace LabyrinthExplorer
{
    public class Portal : EnvironmentObject, IInteractableObject, I3DSoundCustDivFact
    {
        private Vector3 PortToLocation;
        private float cooldown = 3.0f;
        private float cooldownTimer = 0.0f;
        private float waitBeforePorting = 0.5f;

        private bool subbedForPorting = false;

        private Player player;
        private string AreaToPortTo;

        private float distanceDivisionFactor = 300;
        public Portal(ContentManager content,
                     Vector3 position, Vector3 rotation,
                    float scale, Vector3 UseFromDirection, Vector3 portToLocation, 
                    string portToArea = null)
            : base(@"Models\Environment\Portal",
                content, position, rotation, scale)
        {
            PortToLocation = portToLocation;
            CreateUseAABB(UseFromDirection, position, 200, 200);
            emitter = new AudioEmitter();
            emitter.Position = base.Position;

            if (portToArea != null)
                this.AreaToPortTo = portToArea;
            else AreaToPortTo = null;

            base.FogEnd = 2000;
            Interactables.AddInteractable(this);
        }

        public override void OnEnteringArea()
        {
            Game.SoundManager.PlaySound("Portal1", this, -1);
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            cooldownTimer += deltaTime;

          
            if (subbedForPorting)
            {
                if (cooldownTimer >= waitBeforePorting)
                {
                    subbedForPorting = false;
                    if(AreaToPortTo != null)
                    {
                        World.ChangeArea(AreaToPortTo, PortToLocation);
                    }
                    else
                        player.Cam.Position = PortToLocation;
                }
            }
        }
        public void Use(AABB interactingParty)
        {
            if (cooldownTimer > cooldown)
            {
                if (interactingParty is Player)
                {
                    subbedForPorting = true;
                    cooldownTimer = 0.0f;
                    player = (Player)interactingParty;
                    Game.SoundManager.PlaySound("PortalUse", null, 0, false);
                }
                else
                    throw new Exception("interacting party (with portal) is not player");
            }
        }

        public float GetCustomDivisionFactor()
        {
            return distanceDivisionFactor;
        }
    }
}
