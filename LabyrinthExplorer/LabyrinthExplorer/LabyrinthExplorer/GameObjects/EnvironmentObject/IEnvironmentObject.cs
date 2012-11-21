using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace LabyrinthExplorer
{
    public interface IEnvironmentObject
    {
        void Update(float deltaTime);

        void Draw(Camera camera, Effect effect);

    }
}
