using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace LabyrinthExplorer
{
    public class Hangar : EnvironmentObject
    {
        public Hangar(ContentManager content,
            Vector3 position, Vector3 rotation, float scale)
            : base(@"Models\Hangar\Hangar", content, position, rotation, scale)
        {
            FogEnd = 10000;
        }

        public override void OnEnteringArea()
        {
           
        }
    }
}
