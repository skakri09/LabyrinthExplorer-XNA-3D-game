using System;
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
        float lineCounter = 0;
        AssemblyLaneEnd lineEnd;

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

            CreateTestCenters();
            CreateHangar();
            CreateAssemblyLane();
        }

        public override void Update(GameTime gameTime, Camera camera)
        {
            base.Update(gameTime, camera);
            if (lineCounter <= 2)
            {
                if (Game.player.Cam.Position.Z >= -3000)
                {
                    Game.player.Cam.Position = new Vector3(Game.player.Cam.Position.X, 150, -35000);
                    ++lineCounter;
                    lineEnd.UpdatePosition(Game.player.Cam.Position);
                }
            }
            else
                Game.player.MakeFootstepSound = true;
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
            Game.SoundManager.PlaySong("space", true);//find new crazy trippy tune
            Game.player.MakeFootstepSound = false;
        }

        private void CreateHangar()
        {
            environment.Add(new Hallway(contentMan,
               new Vector3(2500, -5, 8300), Vector3.Zero, 11.0f));

            environment.Add(new Hangar(contentMan, new Vector3(3800, -30, 15550), new Vector3(0, 90, 0), 15.0f));

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

        private void CreateAssemblyLane()
        {
            lineEnd = new AssemblyLaneEnd(contentMan, new Vector3(2500, -300, -42000),
                new Vector3(0, 90, 0), 15);
            environment.Add(lineEnd);

            for (int i = -40000; i < 5000; i += 3000)
            {
                environment.Add(new AssemblyLane(contentMan,
                new Vector3(2500, 200, i), new Vector3(0, 90, 0), 12));
            }
            environment.Add(new AssemblyLane(contentMan,
                new Vector3(2500, 200, 4750), new Vector3(0, 90, 0), 12));

            environment.Add(new AssemblyLaneEnd(contentMan, new Vector3(2500, -200, 5000), new Vector3(0, 90, 0), 20));
            //environment.Add(new AssemblyLane(contentMan, 
            //    new Vector3(2500, 0, -10000), new Vector3(0, 90, 0), 10));
            //environment.Add(new AssemblyLane(contentMan,
            //    new Vector3(2500, 0, -9000), new Vector3(0, 90, 0), 10));
            //environment.Add(new AssemblyLane(contentMan,
            //    new Vector3(2500, 0, -8000), new Vector3(0, 90, 0), 10));
            //environment.Add(new AssemblyLane(contentMan,
            //    new Vector3(2500, 0, -7000), new Vector3(0, 90, 0), 10));
            //environment.Add(new AssemblyLane(contentMan,
            //    new Vector3(2500, 0, -6000), new Vector3(0, 90, 0), 10));
            //environment.Add(new AssemblyLane(contentMan,
            //    new Vector3(2500, 0, -5000), new Vector3(0, 90, 0), 10));
            //environment.Add(new AssemblyLane(contentMan,
            //    new Vector3(2500, 0, -4000), new Vector3(0, 90, 0), 10));

        }

        private void CreateTestCenters()
        {
        }

    }
}
