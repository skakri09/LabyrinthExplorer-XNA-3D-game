using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace LabyrinthExplorer
{
    public class Testcenter : EnvironmentObject
    {
        Vector3 originalPosition;
        public Testcenter(ContentManager content,
                            Vector3 position, Vector3 rotation, float scale)
            : base(@"Models\TestCenter", content, position, rotation, scale)
        {
            FogEnd = 999999;
            originalPosition = position;
        }

        public override void OnEnteringArea()
        {
          
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            position.Z = originalPosition.Z + Game.player.Cam.Position.Z;
            position.X = originalPosition.X + Game.player.Cam.Position.X;
        }
    }

    
}
