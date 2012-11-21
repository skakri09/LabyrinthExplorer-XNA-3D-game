using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabyrinthExplorer
{
    public class DuoLever : AABB, IEnvironmentObject, IInteractableObject
    {
        Lever lever1;
        Lever lever2;
        IInteractableObject OnUseObject;

        public DuoLever(Lever lever1, Lever lever2,
            IInteractableObject LeverUseObject)
        {
            this.lever1 = lever1;
            this.lever2 = lever2;
            this.OnUseObject = LeverUseObject;
            Interactables.AddInteractable(this);
        }

        public void Update(float deltaTime)
        {
            lever1.Update(deltaTime);
            lever2.Update(deltaTime);
        }

        public void Draw(Camera camera)
        {
            lever1.Draw(camera);
            lever2.Draw(camera);
        }

        public void Use()
        {
            throw new Exception("The method or operation is not implemented.");
        }

    }
}
