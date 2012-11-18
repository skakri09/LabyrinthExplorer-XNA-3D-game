using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabyrinthExplorer
{
    public interface AIState
    {
        void Enter(Enemy owner);

        void UpdateState(Enemy owner, float deltaTime);

        void Exit(Enemy owner);
    }
}
