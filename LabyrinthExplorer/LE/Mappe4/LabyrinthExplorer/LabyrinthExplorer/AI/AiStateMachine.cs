using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabyrinthExplorer
{
    public class AiStateMachine
    {
        private Enemy owner;
        private AIState constState;
        private AIState dynamicState;

        public AiStateMachine(Enemy owner, AIState constState, AIState startState)
        {
            this.owner = owner;
            this.constState = constState;
            this.dynamicState = startState;
            
            constState.Enter(owner);
            dynamicState.Enter(owner);
        }


        public void Update(float deltaTime)
        {
            constState.UpdateState(owner, deltaTime);
            dynamicState.UpdateState(owner, deltaTime);
        }

        public void ChangeState(AIState newState)
        {
            dynamicState.Exit(owner);
        }


        public AIState GetCurrentState()
        {
            return dynamicState;
        }
    }
}
