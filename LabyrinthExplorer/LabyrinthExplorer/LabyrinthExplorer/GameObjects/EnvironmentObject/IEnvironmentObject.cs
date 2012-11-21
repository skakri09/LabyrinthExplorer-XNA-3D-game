using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabyrinthExplorer
{
    public interface IEnvironmentObject
    {
        void Update(float deltaTime);

        void Draw(Camera camera);

    }
}
