using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace LabyrinthExplorer
{
    public class AssemblyLane : EnvironmentObject
    {
        public AssemblyLane(ContentManager content,
                            Vector3 position, Vector3 rotation, float scale)
            : base(@"Models\AssemblyLane", content, position, rotation, scale)
        {
            FogEnd = 5000;
            emitter = new AudioEmitter();
            emitter.Position = position;
        }

        public override void OnEnteringArea()
        {
            //play some crazy trippy sound lasting for the duration player is being moved
        }

    }
}
