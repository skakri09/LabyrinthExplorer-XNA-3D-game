using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabyrinthExplorer
{
    public interface IChestItem
    {
        void OnChestOpen(AABB chestOpener);
    }
}
