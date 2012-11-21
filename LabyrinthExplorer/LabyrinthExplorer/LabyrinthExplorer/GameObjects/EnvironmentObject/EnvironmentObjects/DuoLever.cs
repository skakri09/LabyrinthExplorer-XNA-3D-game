using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LabyrinthExplorer
{
    public class DuoLever : AABB, IEnvironmentObject, IInteractableObject
    {
        Lever lever1;
        Lever lever2;
        IInteractableObject OnUseObject;

        private bool lever1Used = false;
        private bool lever2Used = false;

        private const float useDuration = 20.0f;
        private float lever1UsedDuration = 0.0f;
        private float lever2UsedDuration = 0.0f;
        private int timesToPlay = (int)Math.Floor(useDuration / 1.270);

        public DuoLever(Lever lever1, Lever lever2,
            IInteractableObject LeverUseObject)
        {
            this.lever1 = lever1;
            this.lever2 = lever2;
            this.OnUseObject = LeverUseObject;

            base.SetAABB(GameConstants.MapMinBounds, GameConstants.MapMaxBounds);

            Interactables.AddInteractable(this);
        }

        public void Update(float deltaTime)
        {
            lever1.Update(deltaTime);
            lever2.Update(deltaTime);
            if (lever1Used)
            {
                lever1UsedDuration += deltaTime;
                if (lever1UsedDuration >= useDuration)
                    lever1Used = false;
            }
            if (lever2Used)
            {
                lever2UsedDuration += deltaTime;
                if (lever2UsedDuration >= useDuration)
                    lever2Used = false;
            }
        }

        public void Draw(Camera camera)
        {
            lever1.Draw(camera);
            lever2.Draw(camera);
        }

        public void Use(AABB interactingParty)
        {
            if (interactingParty.CheckCollision(lever1) != Vector3.Zero)
            {
                lever1Used = true;
                lever1UsedDuration = 0.0f;
                Game.SoundManager.PlaySound("Clock", timesToPlay);
            }
            if (interactingParty.CheckCollision(lever2) != Vector3.Zero)
            {
                lever2Used = true;
                lever2UsedDuration = 0.0f;
                Game.SoundManager.PlaySound("Clock", timesToPlay);
            }

            if (lever1Used && lever2Used)
            {
                OnUseObject.Use(interactingParty);
            }
        }

    }
}
