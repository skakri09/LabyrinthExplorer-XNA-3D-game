using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace LabyrinthExplorer
{
    class Area2Content : AreaContent
    {
        RandWhisper randomWhisper;

        public Area2Content(Camera camera)
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

            GenerateWalls();
            GenerateCeiling();
            GenerateFloors();
            GenerateEnemies();
            GenInteractiveEnvironment();

            randomWhisper = new RandWhisper(30.0f);
        }

        public override void Update(GameTime gameTime, Camera camera)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime, camera);
            randomWhisper.Update(deltaTime);
        }

        public override void Draw(GraphicsDevice graphicsDevice, Effect effect,
        Texture2D brickColorMap, Texture2D brickNormalMap, Texture2D brickHeightMap,
        Texture2D stoneColorMap, Texture2D stoneNormalMap, Texture2D stoneHeightMap,
        Texture2D woodColorMap, Texture2D woodNormalMap, Texture2D woodHeightMap)
        {
            base.Draw(graphicsDevice, effect, brickColorMap, brickNormalMap, brickHeightMap,
                stoneColorMap, stoneNormalMap, stoneHeightMap, woodColorMap, woodNormalMap, woodHeightMap);
        }

        protected virtual void GenerateWalls()
        {
            #region howToUseWalls
            //XWallNegZ = grows out on negative Z
            //XWallPosZ = grows out on positive Z
            //ZWallNegX = grows out on negative X
            //ZWallPosX = grows out on positive X

            //Z Walls - ZWallPosX
            //X Walls - XWallNegZAl
            #endregion


            #region "wall" of text :)
            SmarPosWall(50, 50, 50, 4950);
            SmarPosWall(50, 900, 850, 900);
            SmarPosWall(50, 50, 4950, 50);
            SmarPosWall(50, 4950, 4950, 4950);
            SmarPosWall(350, 2550, 350, 1500);
            SmarPosWall(350, 3800, 350, 2900);
            SmarPosWall(350, 4750, 350, 4150);
            SmarPosWall(350, 1700, 1450, 1700);
            SmarPosWall(650, 2250, 650, 2600);
            SmarPosWall(700, 3950, 1700, 3950);
            SmarPosWall(850, 900, 850, 400);
            SmarPosWall(1050, 3750, 1050, 2900);
            SmarPosWall(1250, 4750, 350, 4750);
            SmarPosWall(1250, 4950, 1250, 4450);
            SmarPosWall(1250, 4450, 2000, 4450);
            SmarPosWall(1300, 400, 1300, 900);
            SmarPosWall(1300, 400, 2050, 400);
            SmarPosWall(1450, 2250, 650, 2250);
            SmarPosWall(1450, 2600, 650, 2600);
            SmarPosWall(1450, 2900, 650, 2900);
            SmarPosWall(1450, 2600, 1450, 1700);
            SmarPosWall(1450, 2900, 1450, 3350);
            SmarPosWall(1700, 2150, 1700, 2600);
            SmarPosWall(1700, 2900, 1700, 3350);
            SmarPosWall(1700, 3600, 1700, 3950);
            SmarPosWall(1700, 3600, 3100, 3600);
            SmarPosWall(2000, 1600, 2950, 1600);
            SmarPosWall(2050, 900, 1300, 900);
            SmarPosWall(2050, 400, 2050, 900);
            SmarPosWall(2050, 1200, 2900, 1200);
            SmarPosWall(2100, 200, 3150, 200);
            SmarPosWall(2200, 3350, 1700, 3350);
            SmarPosWall(2250, 2150, 1700, 2150);
            SmarPosWall(2350, 4650, 1400, 4650);
            SmarPosWall(2350, 200, 2350, 950);
            SmarPosWall(2350, 3600, 2350, 4650);
            SmarPosWall(2350, 4650, 4950, 4650);
            SmarPosWall(2400, 1200, 2400, 1600);
            SmarPosWall(2600, 2150, 3100, 2150);
            SmarPosWall(2650, 3800, 2650, 4100);
            SmarPosWall(2650, 4300, 2650, 4650);
            SmarPosWall(2650, 4100, 3350, 4100);
            SmarPosWall(2650, 4300, 4600, 4300);
            SmarPosWall(3100, 3350, 2600, 3350);
            SmarPosWall(3100, 2150, 3100, 2600);
            SmarPosWall(3100, 2900, 3100, 3350);
            SmarPosWall(3100, 2900, 3350, 2900);
            SmarPosWall(3100, 2600, 4600, 2600);
            SmarPosWall(3350, 3800, 2650, 3800);
            SmarPosWall(3350, 2900, 3350, 4100);
            SmarPosWall(3450, 1950, 4950, 1950);
            SmarPosWall(3500, 200, 3500, 1650);
            SmarPosWall(3650, 2900, 3650, 3550);
            SmarPosWall(3650, 3800, 3650, 4300);
            SmarPosWall(3650, 2900, 4600, 2900);
            SmarPosWall(3850, 950, 4700, 950);
            SmarPosWall(3900, 3300, 3900, 3100);
            SmarPosWall(3900, 3500, 3900, 4100);
            SmarPosWall(3900, 4100, 4100, 4100);
            SmarPosWall(3900, 3500, 4150, 3500);
            SmarPosWall(3900, 3650, 4250, 3650);
            SmarPosWall(3900, 3100, 4300, 3100);
            SmarPosWall(3900, 3300, 4300, 3300);
            SmarPosWall(4250, 350, 4250, 50);
            SmarPosWall(4250, 950, 4250, 1650);
            SmarPosWall(4250, 3300, 4250, 3650);
            SmarPosWall(4250, 1650, 4700, 1650);
            SmarPosWall(4300, 4100, 4300, 3800);
            SmarPosWall(4300, 3300, 4600, 3300);
            SmarPosWall(4450, 3800, 4100, 3800);
            SmarPosWall(4450, 3800, 4450, 3450);
            SmarPosWall(4600, 4100, 4300, 4100);
            SmarPosWall(4600, 4300, 4600, 2600);
            SmarPosWall(4700, 350, 4250, 350);
            SmarPosWall(4700, 950, 4700, 350);
            SmarPosWall(4950, 50, 4950, 4950);
            #endregion

        }

        protected void GenerateCeiling()
        {
            ceilings.Add(new NormalMappedCeiling(device,
                new Vector3(-5000, GameConstants.WALL_HEIGHT, 5000),
                new Vector3(5000, GameConstants.WALL_HEIGHT, 5000),
                new Vector3(5000, GameConstants.WALL_HEIGHT, -5000),
                new Vector3(-5000, GameConstants.WALL_HEIGHT, -5000),
                Vector3.Down));
        }

        protected void GenerateFloors()
        {
            CreateFloor(new Vector3(-5000, 0, 5000), new Vector3(5000, 0, 5000),
                        new Vector3(5000, 0, -5000), new Vector3(-5000, 0, -5000));
        }

        protected void GenerateEnemies()
        {
         
        }

        protected void GenInteractiveEnvironment()
        {
            environment.Add(new TurnablePilar(contentMan, new Vector3(2425, -100, 2200),
                new Vector3(0, 180, 0), Vector3.Backward, 105));
            environment.Add(new TurnablePilar(contentMan, new Vector3(2450, -100, 3200),
               new Vector3(0, 0, 0), Vector3.Forward, 105));
            environment.Add(new TurnablePilar(contentMan, new Vector3(1800, -100, 2750),
               new Vector3(0, 270, 0), Vector3.Left, 105));
            environment.Add(new TurnablePilar(contentMan, new Vector3(3050, -100, 2750),
               new Vector3(0, 90, 0), Vector3.Right, 105));
        }

        public override void OnEnteringArea()
        {
            base.OnEnteringArea();
            Game.SoundManager.PlaySound("Area2Ambient", 0.7f, null, -1);
        }
    }
}
