using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LabyrinthExplorer
{
    class EnemyConstState : AIState
    {
        private float playerDistToPerformAction;

        private float actionCooldown;
        private float actionCooldownTimer = 0.0f;

        public EnemyConstState(float playerDistanceToPerformAction = 800.0f, float actionCooldown = 10.0f)
        {
            this.playerDistToPerformAction = playerDistanceToPerformAction;
            this.actionCooldown = actionCooldown;
        }

        public void Enter(Enemy owner)
        {
            actionCooldownTimer = 0.0f;
        }

        public void UpdateState(Enemy owner, float deltaTime)
        {
            actionCooldownTimer += deltaTime;
            if (actionCooldownTimer >= actionCooldown)
            {
                if (Vector3.Distance(Game.player.Cam.Position, owner.Position) <= playerDistToPerformAction)
                {
                     actionCooldown = 0.0f;
                     owner.PerformBaseAction();
                }
            }
        }

        public void Exit(Enemy owner)
        {

        }
    }
}
