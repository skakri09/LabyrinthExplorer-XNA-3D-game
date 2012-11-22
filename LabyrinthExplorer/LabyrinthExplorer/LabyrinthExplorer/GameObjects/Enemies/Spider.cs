using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace LabyrinthExplorer
{
    public class Spider : Enemy, I3DSoundCustDivFact
    {
        float time = 0;
        string spiderSoundName;

        public Spider(Vector3 startPos, Vector3 endPos, string modelName, float scale, ContentManager content)
            :base(modelName, content, scale)
        {
            aiStateMachine = new AiStateMachine(this, new EnemyConstState(700, 15),
                new PatrolZ(startPos, endPos));
            spiderSoundName = "SpiderSteps";
        }
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            time += deltaTime;
        }

        public override void OnEnteringArea()
        {
            Game.SoundManager.PlaySound(spiderSoundName, this, -1);
        }

        public override void PerformBaseAction()
        {
            int soundToPlay = (int)time;
            if(soundToPlay%2 == 0)
                Game.SoundManager.PlaySound("SpiderCurry2", this);
            else
                Game.SoundManager.PlaySound("SpiderCurry1", this);  
        }

        public float GetCustomDivisionFactor()
        {
            return 250;
        }
    }
}
