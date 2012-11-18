using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabyrinthExplorer
{
    class EnemyConstState : AIState
    {
        public void Enter(Enemy owner)
        {

        }

        public void UpdateState(Enemy owner, float deltaTime)
        {
            //play sound stuff
            //check if enemy should be killed or smthing
        }

        public void Exit(Enemy owner)
        {
        }
    }
}
