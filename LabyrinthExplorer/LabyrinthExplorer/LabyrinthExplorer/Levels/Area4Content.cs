﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LabyrinthExplorer
{
    public class Area4Content : AreaContent
    {
        float lineTimer = 0;
        float lineCounter = 0;
        AssemblyLane assemblyLane;
        bool assemblyLaneDone = false;
        public Area4Content(Camera camera)
            :base(camera)
        {
            this.camera = camera;

            walls = new List<SolidWall>();
            ceilings = new List<NormalMappedCeiling>();
            floors = new List<NormalMappedFloor>();
            enemies = new List<Enemy>();
            environment = new List<IEnvironmentObject>();
            environmentCollidables = new List<AABB>();
        }

        public override void LoadContent(GraphicsDevice device, ContentManager contentMan)
        {
            base.LoadContent(device, contentMan);

            CreateHangar();

            //environment.Add(new AssemblyLane(contentMan, 
        }

        public override void Update(GameTime gameTime, Camera camera)
        {
            base.Update(gameTime, camera);

            if (!assemblyLaneDone)
                HandleAssemblyLine((float)gameTime.ElapsedGameTime.TotalSeconds);
            
        }

        public override void Draw(GraphicsDevice graphicsDevice, Effect effect, Texture2D brickColorMap, Texture2D brickNormalMap, Texture2D brickHeightMap, Texture2D stoneColorMap, Texture2D stoneNormalMap, Texture2D stoneHeightMap, Texture2D woodColorMap, Texture2D woodNormalMap, Texture2D woodHeightMap)
        {
            foreach (IEnvironmentObject obj in environment)
            {
                obj.Draw(camera, effect);
            }
        }

        public override void OnEnteringArea()
        {
            base.OnEnteringArea();
            float a = Game.player.Cam.PitchDegrees;
            Game.SoundManager.PlaySong("space", true);//find new crazy trippy tune
            Game.player.MakeFootstepSound = false;
            Game.player.PlayerAbleToMove = false;
            Game.player.Cam.ToggleAssemblyLaneMode();
        }

        private void HandleAssemblyLine(float deltaTime)
        {
            lineTimer += deltaTime;
            if (lineCounter <= 2)
            {
                float velocity = lineTimer * 10000;
                if (velocity >= 50000)
                {
                    velocity = 50000;
                }
                if (Game.player.Cam.HeadingDegrees <= 90 && Game.player.Cam.HeadingDegrees >= -90)
                    Game.player.Cam.CurrentVelocity = new Vector3(0, 0, -velocity);
                else
                    Game.player.Cam.CurrentVelocity = new Vector3(0, 0, velocity);

                Game.player.Cam.Position = new Vector3(2500, 150, Game.player.Cam.Position.Z);
                if (Game.player.Cam.Position.Z >= -3000)
                {
                    Game.player.Cam.Position = new Vector3(Game.player.Cam.Position.X, 150, -80000);
                    ++lineCounter;
                }
            }
            else
            {
                Game.player.Cam.Position = new Vector3(2500, 150, Game.player.Cam.Position.Z);
                if (Game.player.Cam.Position.Z >= 0)
                {
                    assemblyLaneDone = true;
                    Game.player.MakeFootstepSound = true;
                    Game.player.PlayerAbleToMove = true;
                    Game.player.Cam.ToggleAssemblyLaneMode();
                    Game.player.Cam.Position = new Vector3(2500, 150, 5500);
                    Game.player.Cam.CurrentVelocity = new Vector3(0, 0, 0);
                }
            }
            if (lineTimer >= 10.0f)
            {
                assemblyLaneDone = true;
                Game.player.MakeFootstepSound = true;
                Game.player.PlayerAbleToMove = true;
                Game.player.Cam.ToggleAssemblyLaneMode();
                Game.player.Cam.Position = new Vector3(2500, 150, 5500);
                Game.player.Cam.CurrentVelocity = new Vector3(0, 0, 0);
            }
        }

        private void CreateHangar()
        {
            environment.Add(new AssemblyLaneCollection(contentMan));
            
            environmentCollidables.Add(new AABB(new Vector2(0, 4900), new Vector2(5000, 5500)));

            environment.Add(new Hallway(contentMan,
               new Vector3(2500, -5, 8300), Vector3.Zero, 11.0f));

            environment.Add(new AssemblyLaneEnd(contentMan, new Vector3(2500, -200, 5500), new Vector3(0, 90, 0), 15, true));

            environment.Add(new Hangar(contentMan, new Vector3(3800, -30, 15550), new Vector3(0, 90, 0), 15.0f));

            environment.Add(new AssemblyLaneEnd(contentMan, new Vector3(2500, -200, 5000), new Vector3(0, 90, 0), 20));

            //right corridor collision
            environmentCollidables.Add(new AABB(new Vector2(2650, 4875), new Vector2(2950, 12500)));
            //left corridor collision
            environmentCollidables.Add(new AABB(new Vector2(2000, 4875), new Vector2(2300, 12500)));
            //right positive hangar collision
            environmentCollidables.Add(new AABB(new Vector2(2700, 12000), new Vector2(10000, 12500)));
            //left positive hangar collision
            environmentCollidables.Add(new AABB(new Vector2(0, 12000), new Vector2(2300, 12500)));
            //Back(left) of hangar collision
            environmentCollidables.Add(new AABB(new Vector2(0, 12000), new Vector2(550, 21000)));
            //negative z side hangar collision
            environmentCollidables.Add(new AABB(new Vector2(0, 19700), new Vector2(10000, 21000)));
            //hangar opening collision
            environmentCollidables.Add(new AABB(new Vector2(9000, 10000), new Vector2(10000, 21000)));
        }

    }
}
