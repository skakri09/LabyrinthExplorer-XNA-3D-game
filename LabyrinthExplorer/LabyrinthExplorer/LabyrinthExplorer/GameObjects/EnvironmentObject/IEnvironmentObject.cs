using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace LabyrinthExplorer
{
    public interface IEnvironmentObject : IOnEnteringArea
    {
        void Update(float deltaTime);

        void Draw(Camera camera, Effect effect);

    }
}
