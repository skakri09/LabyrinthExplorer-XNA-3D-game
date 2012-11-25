using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace LabyrinthExplorer
{
    public class HangarSound : I3DSound, I3DSoundCustDivFact
    {
        AudioEmitter emitter;
        Vector3 position;
        float divFact;
        string soundName;
        float volume;

        public HangarSound(string soundName, Vector3 position, float volume = 1, float customDivFact = 300)
        {
            this.soundName = soundName;
            this.volume = volume;
            divFact = customDivFact;
            this.position = position;
            emitter = new AudioEmitter();
            emitter.Position = position;
        }

        public void StartSound()
        {
            Game.SoundManager.PlaySound(soundName, volume, this, -1);
        }

        public AudioEmitter GetAudioEmitter()
        {
            emitter.Position = position;
            return emitter;
        }

        public float GetCustomDivisionFactor()
        {
            return divFact;
        }
    }

    public class Hangar : EnvironmentObject
    {
        HangarSound ww2 = new HangarSound("ww2", new Vector3(5183, 0, 21400), 0.5f, 200);
        HangarSound alex = new HangarSound("alex", new Vector3(5350, 0, 10700), 1, 400);
        HangarSound apollo11 = new HangarSound("apollo11", new Vector3(2445, 0, 21400), 1, 400);
        HangarSound arena = new HangarSound("arena", new Vector3(7800, 0, 21400), 1, 500);
        HangarSound egypt = new HangarSound("egypt", new Vector3(7800, 0, 10700), 1, 500);

        public Hangar(ContentManager content,
            Vector3 position, Vector3 rotation, float scale)
            : base(@"Models\Hangar\Hangar", content, position, rotation, scale)
        {
            FogEnd = 10000;


        }

        public override void OnEnteringArea()
        {
            ww2.StartSound();
            alex.StartSound();
            apollo11.StartSound();
            arena.StartSound();
            egypt.StartSound();
        }
    }
}
