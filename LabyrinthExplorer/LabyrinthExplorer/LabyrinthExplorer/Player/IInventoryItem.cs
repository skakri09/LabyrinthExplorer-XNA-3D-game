﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LabyrinthExplorer
{
    public interface IInventoryItem
    {
        float GetInventoryScale();//scale used to display item on screen

        void Update(float deltaTime, bool beMovin);

        void Draw(Camera camera, Vector3 screenOffset);
    }
}
