﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabyrinthExplorer
{
    public interface ChestItem
    {
        void OnChestOpen(AABB chestOpener);
    }
}
