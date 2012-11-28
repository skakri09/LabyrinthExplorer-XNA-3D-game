using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;

namespace LabyrinthExplorer
{
    public static class Shuffler
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }

    public class RandWhisper:I3DSound, I3DSoundCustDivFact
    {
        private float whisperCooldown;
        private float whisperTimer;
        private int whisperIndex = 0;
        private List<string> whispers;

        private Vector3 simulatedPosiion;
        private Vector3 returnPosition;
        private AudioEmitter emitter;
        float positionTimer = 0;
        public RandWhisper(float frequencyOfWhisper)
        {
            simulatedPosiion = new Vector3(2500, -1000, 2500);
            emitter = new AudioEmitter();
            emitter.Position = simulatedPosiion;
            emitter.Up = Vector3.Up;
            whisperCooldown = frequencyOfWhisper;
            whispers = new List<string>();
            whispers.Add("ThereIsNoEscape");
            whispers.Add("DeathIsClose");
            whispers.Add("YouAreWeak");
            whispers.Add("HopeIsAnIllusion");
            whispers.Add("TheyAreComingForYou");
            whispers.Add("GiveInToYourFear");
            whispers.Shuffle();
        }
        public float GetCustomDivisionFactor()
        {
             return 1000;
        }

        public AudioEmitter GetAudioEmitter()
        {
            emitter.Position = returnPosition;
            return emitter;
        }

        public void Update(float deltaTime)
        {
            whisperTimer += deltaTime;
            positionTimer += deltaTime;
            simulatedPosiion = new Vector3((float)Math.Cos(positionTimer)
                / 2, 0, (float)Math.Sin(positionTimer))*5000;    
            
            returnPosition = new Vector3(2500, -1000, 2500) + simulatedPosiion;
            if (whisperTimer >= whisperCooldown)
            {
                whisperTimer = 0;
                Game.SoundManager.PlaySound(GetNextSound(), this);
            }
        }

        string GetNextSound()
        {
            string returnVal = whispers.ElementAt(whisperIndex);
            ++whisperIndex;
            if (whisperIndex == whispers.Count)
            {
                whispers.Shuffle();
                whisperIndex = 0;
            }
            return returnVal;
        }
    }
}
