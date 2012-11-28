using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabyrinthExplorer
{
    public interface IInteractableObject
    {
        void Use(AABB interactingParty);

        //allowing the object being used to tell the user
        //when/if it's finished
        void UsedCallback();
    }
}
