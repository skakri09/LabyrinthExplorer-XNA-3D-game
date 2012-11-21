using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace LabyrinthExplorer
{
    public class Spider : Enemy
    {
        public Spider(Vector3 startPos, Vector3 rotation, string modelName, float scale, ContentManager content)
            :base(modelName, content, scale)
        {
            aiStateMachine = new AiStateMachine(this, new EnemyConstState(), new PatrolZ(new Vector3(4600, 0, 4000), new Vector3(4600, 0, 2000)));
        }
    }
}
