using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace LabyrinthExplorer
{
    public class DuoLever : AABB, IEnvironmentObject, IInteractableObject
    {
        Lever lever1;
        Lever lever2;
        IInteractableObject OnUseObject;
        Gate gate;
        private bool lever1Used = false;
        private bool lever2Used = false;
        private bool isOpen = false;

        private float useDuration = 7.0f;
        private float lever1UsedDuration = 0.0f;
        private float lever2UsedDuration = 0.0f;
        private int timesToPlay;

        public DuoLever(Lever lever1, Lever lever2,
            IInteractableObject LeverUseObject, float useDuration)
        {
            this.useDuration = useDuration;
            timesToPlay = (int)Math.Floor(useDuration / 1.270);
            this.lever1 = lever1;
            this.lever2 = lever2;
            this.OnUseObject = LeverUseObject;

            base.SetAABB(GameConstants.MapMinBounds, 
                new Vector3(GameConstants.MapMaxBounds.X, 
                    GameConstants.InteractablesUseHeight, 
                    GameConstants.MapMaxBounds.Z));

            Interactables.AddInteractable(this);
            if (LeverUseObject is Gate)
            {
                gate = (Gate)LeverUseObject;
            }
        }

        public void Update(float deltaTime)
        {
            lever1.Update(deltaTime);
            lever2.Update(deltaTime);
            if (!isOpen)
            {
                if (lever1Used)
                {
                    lever1UsedDuration += deltaTime;
                    if (lever1UsedDuration >= useDuration)
                    {
                        lever1Used = false;
                        lever1.SetUnused();
                    }
                }
                if (lever2Used)
                {
                    lever2UsedDuration += deltaTime;
                    if (lever2UsedDuration >= useDuration)
                    {
                        lever2Used = false;
                        lever2.SetUnused();
                    }
                }
            }
            else
            {
                if (gate.gateState == GateState.CLOSED)
                {
                    lever1.SetUnused();
                    lever2.SetUnused();
                    isOpen = false;
                }
            }
        }

        public void Draw(Camera camera, Effect effect)
        {
            lever1.Draw(camera, effect);
            lever2.Draw(camera, effect);
        }

        public void Use(AABB interactingParty)
        {
            if (!isOpen)
            {
                if (!lever1Used)
                {
                    if (interactingParty.CheckCollision(lever1) != Vector3.Zero)
                    {
                        lever1Used = true;
                        lever1UsedDuration = 0.0f;
                        Game.SoundManager.PlaySound("Clock", null, timesToPlay);
                        lever1.Use(null);
                    }
                }
                if (!lever2Used)
                {
                    if (interactingParty.CheckCollision(lever2) != Vector3.Zero)
                    {
                        lever2Used = true;
                        lever2UsedDuration = 0.0f;
                        Game.SoundManager.PlaySound("Clock", null, timesToPlay);
                        lever2.Use(null);
                    }
                }
                if (lever1Used && lever2Used)
                {
                    Game.SoundManager.StopSound("Clock");
                    OnUseObject.Use(interactingParty);
                    lever1Used = false;
                    lever2Used = false;
                    isOpen = true;
                }
            }
            
        }

        public void OnEnteringArea()
        {
            //if anything
        }

        public void UsedCallback()
        {
            lever1.UsedCallback();
            lever2.UsedCallback();
        }
    }
}
