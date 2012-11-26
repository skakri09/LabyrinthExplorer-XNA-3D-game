using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace LabyrinthExplorer
{
    public class Area3Content : AreaContent
    {
        private Pedistal redPedistal;
        private Pedistal bluePedistal;
        private Pedistal yellowPedistal;
        //private Hangar hangar;
        private FinalGate finalGate;
        bool pedistalsUnlocked = false;

        private AABB StartArea4AABB;

        public Area3Content(Camera camera)
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

            StartArea4AABB = new AABB(new Vector2(0, 5000), new Vector2(5000, 5500));
        }

        public override void Update(GameTime gameTime, Camera camera)
        {
            base.Update(gameTime, camera);

            if (!pedistalsUnlocked)
            {
                if (redPedistal.IsUnlocked)
                    if (bluePedistal.IsUnlocked)
                        if (yellowPedistal.IsUnlocked)
                        {
                            pedistalsUnlocked = true;
                            finalGate.Use(null);
                            //environment.Add(hangar);
                            //hangar.OnEnteringArea();
                            bluePedistal.StopSound();
                        }
            }
            if (Game.player.PlayerAABB.CheckCollision(StartArea4AABB) != Vector3.Zero)
            {
                World.ChangeArea("area4", new Vector3(2500, 150, -40000));
            }
        }

        public override void Draw(GraphicsDevice graphicsDevice, Effect effect, 
            Texture2D brickColorMap, Texture2D brickNormalMap, 
            Texture2D brickHeightMap, Texture2D stoneColorMap, Texture2D stoneNormalMap, Texture2D stoneHeightMap, Texture2D woodColorMap, Texture2D woodNormalMap, Texture2D woodHeightMap)
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

            SmarPosWall(0, 50, 5000, 50);
            SmarPosWall(50, 100, 50, 4950);
            SmarPosWall(4950, 4950, 4950, 100);


            SmarPosWall(50, 4950, 2150, 4950);
            SmarPosWall(2850, 4950, 4950, 4950);
            #region right half area
          
            SmarPosWall(2950, 100, 2950, 2300);
            SmarPosWall(2950, 2900, 2950, 4950);

            SmarPosWall(2050, 3000, 2050, 4950);
            SmarPosWall(2050, 100, 2050, 2550);

            SmarPosWall(3800, 3650, 3800, 4950);
            SmarPosWall(4150, 2900, 4150, 4400);
            SmarPosWall(4150, 4650, 4150, 4950);
            SmarPosWall(4450, 2900, 4450, 3650);
            SmarPosWall(4450, 4000, 4450, 4950);
            SmarPosWall(3800, 3400, 3800, 2900);
            SmarPosWall(3450, 2850, 4800, 2850);
            SmarPosWall(4800, 2900, 4800, 4400);
            SmarPosWall(4800, 4600, 4800, 4950);
            SmarPosWall(4950, 2550, 4050, 2550);
            SmarPosWall(3700, 2550, 3700, 1750);
            SmarPosWall(4700, 2250, 3750, 2250);
            SmarPosWall(4300, 2250, 4300, 1700);
            SmarPosWall(4950, 2000, 4600, 2000);
            SmarPosWall(4350, 1700, 4950, 1700);

            SmarPosWall(3450, 450, 3700, 450);//moved 150 down
            SmarPosWall(3400, 450, 3400, 4800);
            SmarPosWall(3700, 450, 3700, 1300);
            SmarPosWall(3750, 1300, 4450, 1300);
            #endregion

            #region supermaxroom
            SmarPosWall(0, 2300, 1200, 2300);
            SmarPosWall(1150, 2350, 1150, 2600);
            SmarPosWall(1150, 2950, 1150, 3200);
            SmarPosWall(1200, 3200, 0, 3200);
            SmarPosWall(1200, 2550, 1800, 2550);
            SmarPosWall(1200, 2950, 1800, 2950);

            SmarPosWall(1200, 3200, 1800, 3200);
            SmarPosWall(1200, 2300, 1800, 2300);
            SmarPosWall(1750, 3000, 1750, 3200);
            SmarPosWall(1750, 2350, 1750, 2550);

            #endregion supermaxroom

            #region bottom left half
            SmarPosWall(1600, 3250, 1600, 3900);
            SmarPosWall(1600, 4350, 1600, 5000);
            SmarPosWall(1000, 3850, 400, 3850);
            SmarPosWall(350, 4350, 350, 3450);
            SmarPosWall(1200, 4100, 1200, 4700);
            SmarPosWall(1200, 4100, 750, 4100);
            SmarPosWall(400, 4300, 700, 4300);
            SmarPosWall(700, 4100, 700, 4350);
            SmarPosWall(1200, 4650, -50, 4650);
            SmarPosWall(1600, 3650, 700, 3650);
            SmarPosWall(400, 3450, 1350, 3450);
            #endregion bottom left half

            #region top left half
            SmarPosWall(1750, 2300, 1750, 1750);
            SmarPosWall(1800, 1450, 1800, 950);
            SmarPosWall(1800, 650, 1800, 0);
            SmarPosWall(1500, 0, 1500, 950);
            SmarPosWall(1800, 1200, 1250, 1200);
            SmarPosWall(1200, 1500, 1200, 900);
            SmarPosWall(1200, 350, 1200, 650);
            SmarPosWall(950, 1100, 950, 2300);
            SmarPosWall(1200, 350, 700, 350);
            SmarPosWall(1500, 1500, 1500, 2050);
            SmarPosWall(1000, 1750, 1500, 1750);
            SmarPosWall(1200, 2300, 1200, 2000);
            SmarPosWall(650, 2000, 650, 2300);
            SmarPosWall(650, 1600, 650, 350);
            SmarPosWall(950, 800, 950, 400);
            SmarPosWall(200, 0, 200, 1050);
            SmarPosWall(250, 1000, 650, 1000);
            SmarPosWall(650, 1300, 0, 1300);
            #endregion top left half
        }

        protected void GenerateCeiling()
        {
            //left half ceiling
            ceilings.Add(new NormalMappedCeiling(device,
                new Vector3(0, GameConstants.WALL_HEIGHT, 0),
                new Vector3(0, GameConstants.WALL_HEIGHT, 5000),
                new Vector3(2100, GameConstants.WALL_HEIGHT, 5000),
                new Vector3(2100, GameConstants.WALL_HEIGHT, 0),
                Vector3.Down));

            //right half ceiling
            ceilings.Add(new NormalMappedCeiling(device,
                new Vector3(2950, GameConstants.WALL_HEIGHT, 0),
                new Vector3(2950, GameConstants.WALL_HEIGHT, 5000),
                new Vector3(5000, GameConstants.WALL_HEIGHT, 5000),
                new Vector3(5000, GameConstants.WALL_HEIGHT, 0),
                Vector3.Down));
        }

        protected void GenerateFloors()
        {
            CreateFloor(new Vector3(0, 0, 0), new Vector3(0, 0, 5000),
                        new Vector3(5000, 0, 5000), new Vector3(5000, 0, 0));
        }

        protected void GenerateEnemies()
        {
         
        }

        protected void GenInteractiveEnvironment()
        {
            CreatePedistals();
            CreateChestsAndDoors();
            CreateGatesAndLevers();

            finalGate = new FinalGate(contentMan, ref environmentCollidables,
                new Vector3(2500, 0, 4950), Vector3.Zero, 20);
            environment.Add(finalGate);

            environment.Add(new Gem("GemYellow", contentMan, new Vector3(500, 150, 2782), 50));
            
            environment.Add(new Gem("GemRed", contentMan, new Vector3(2500, 150, 3000), 50));
            environment.Add(new Gem("GemBlue", contentMan, new Vector3(2500, 150, 3000), 50));
            environment.Add(new Gem("GemYellow", contentMan, new Vector3(2500, 150, 3000), 50));

            CreatePortal(new Vector3(2500, 0, 250), Vector3.Zero, 40.0f,
                Vector3.Forward, new Vector3(2450, GameConstants.CAMERA_PLAYER_EYE_HEIGHT, 2700), "area2");

            environment.Add(new AssemblyLane(contentMan, new Vector3(2500, 0, 10100), new Vector3(0,90,0), 20));
            //environment.Add(new Hallway(contentMan,
            //   new Vector3(2500, -5, 8300), Vector3.Zero, 11.0f));


            //hangar = new Hangar(contentMan, new Vector3(3800, -30, 15550), new Vector3(0, 90, 0), 15.0f);
            ////environment.Add(hangar);


            ////right corridor collision
            //environmentCollidables.Add(new AABB(new Vector2(2650, 4875), new Vector2(2950, 12500)));
            ////left corridor collision
            //environmentCollidables.Add(new AABB(new Vector2(2000, 4875), new Vector2(2300, 12500)));
            ////right positive hangar collision
            //environmentCollidables.Add(new AABB(new Vector2(2700, 12000), new Vector2(10000, 12500)));
            ////left positive hangar collision
            //environmentCollidables.Add(new AABB(new Vector2(0, 12000), new Vector2(2300, 12500)));
            ////Back(left) of hangar collision
            //environmentCollidables.Add(new AABB(new Vector2(0, 12000), new Vector2(550, 21000)));
            ////negative z side hangar collision
            //environmentCollidables.Add(new AABB(new Vector2(0, 19700), new Vector2(10000, 21000)));
            ////hangar opening collision
            //environmentCollidables.Add(new AABB(new Vector2(9000, 10000), new Vector2(10000, 21000)));
        }

        private void CreatePedistals()
        {
            redPedistal = new Pedistal(contentMan, new Vector3(2200, 0, 4700), Vector3.Zero, 75, "GemRed");
            bluePedistal = new Pedistal(contentMan, new Vector3(2500, 0, 4700), Vector3.Zero, 75, "GemBlue", true);
            yellowPedistal = new Pedistal(contentMan, new Vector3(2800, 0, 4700), Vector3.Zero, 75, "GemYellow");

            environment.Add(redPedistal);
            environment.Add(bluePedistal);
            environment.Add(yellowPedistal);
        }

        private void CreateTestCenters()
        {
            TestCenters = new List<IEnvironmentObject>();

            for (int i = -100; i < 100; i += 20)
            {
                TestCenters.Add(new Testcenter(contentMan, new Vector3(100, 80, i),
                    new Vector3(0, -90, 0), 0.001f));
                TestCenters.Add(new Testcenter(contentMan, new Vector3(100, 70, i),
                   new Vector3(0, -90, 0), 0.001f));
                TestCenters.Add(new Testcenter(contentMan, new Vector3(100, 60, i),
                   new Vector3(0, -90, 0), 0.001f));
                TestCenters.Add(new Testcenter(contentMan, new Vector3(100, 50, i),
                   new Vector3(0, -90, 0), 0.001f));
                TestCenters.Add(new Testcenter(contentMan, new Vector3(100, 40, i),
                   new Vector3(0, -90, 0), 0.001f));
                TestCenters.Add(new Testcenter(contentMan, new Vector3(100, 30, i),
                   new Vector3(0, -90, 0), 0.001f));
                TestCenters.Add(new Testcenter(contentMan, new Vector3(100, 20, i),
                    new Vector3(0, -90, 0), 0.001f));
                TestCenters.Add(new Testcenter(contentMan, new Vector3(100, 10, i),
                   new Vector3(0, -90, 0), 0.001f));
                TestCenters.Add(new Testcenter(contentMan, new Vector3(100, 0, i),
                   new Vector3(0, -90, 0), 0.001f));
                TestCenters.Add(new Testcenter(contentMan, new Vector3(100, -10, i),
                   new Vector3(0, -90, 0), 0.001f));
                TestCenters.Add(new Testcenter(contentMan, new Vector3(100, -20, i),
                   new Vector3(0, -90, 0), 0.001f));
                TestCenters.Add(new Testcenter(contentMan, new Vector3(100, -30, i),
                   new Vector3(0, -90, 0), 0.001f));

                //TestCenters.Add(new Testcenter(contentMan, new Vector3(100, 85, i),
                //   new Vector3(0, -90, 0), 0.001f));
                //TestCenters.Add(new Testcenter(contentMan, new Vector3(100, 75, i),
                //   new Vector3(0, -90, 0), 0.001f));
                //TestCenters.Add(new Testcenter(contentMan, new Vector3(100, 65, i),
                //   new Vector3(0, -90, 0), 0.001f));
                //TestCenters.Add(new Testcenter(contentMan, new Vector3(100, 55, i),
                //   new Vector3(0, -90, 0), 0.001f));
                //TestCenters.Add(new Testcenter(contentMan, new Vector3(100, 45, i),
                //   new Vector3(0, -90, 0), 0.001f));
                //TestCenters.Add(new Testcenter(contentMan, new Vector3(100, 35, i),
                //   new Vector3(0, -90, 0), 0.001f));
                //TestCenters.Add(new Testcenter(contentMan, new Vector3(100, 25, i),
                //    new Vector3(0, -90, 0), 0.001f));
                //TestCenters.Add(new Testcenter(contentMan, new Vector3(100, 15, i),
                //   new Vector3(0, -90, 0), 0.001f));
                //TestCenters.Add(new Testcenter(contentMan, new Vector3(100, 5, i),
                //   new Vector3(0, -90, 0), 0.001f));
                //TestCenters.Add(new Testcenter(contentMan, new Vector3(100, -5, i),
                //   new Vector3(0, -90, 0), 0.001f));
                //TestCenters.Add(new Testcenter(contentMan, new Vector3(100, -15, i),
                //   new Vector3(0, -90, 0), 0.001f));
                //TestCenters.Add(new Testcenter(contentMan, new Vector3(100, -25, i),
                //   new Vector3(0, -90, 0), 0.001f));
            }
            //TestCenters.Add(new Testcenter(contentMan, new Vector3(800, 100, -1000),
            //   new Vector3(0, -90, 0), 0.01f));


            //TestCenters.Add(new Testcenter(contentMan, new Vector3(800, 200, -200),
            //   new Vector3(0, -90, 0), 0.01f));

            //TestCenters.Add(new Testcenter(contentMan, new Vector3(800, 300, 110),
            //    new Vector3(0, -90, 0), 0.01f));

            //TestCenters.Add(new Testcenter(contentMan, new Vector3(800, 400, 200),
            //   new Vector3(0, -90, 0), 0.01f));


            //TestCenters.Add(new Testcenter(contentMan, new Vector3(800, 500, 300),
            //   new Vector3(0, -90, 0), 0.01f));
        }

        private void CreateChestsAndDoors()
        {
            //bottom door
            environment.Add(new Door(contentMan, new Vector3(1925, 0, 3150), Vector3.Zero, new Vector3(1625, 0, 3150),
               90, Vector3.Backward, ref environmentCollidables, "bottomAreaKey"));
            //chest with key for bottom door
            environment.Add(new Chest(contentMan, new Vector3(3200, 0, 1400), new Vector3(0, 180, 0), 50, Vector3.Zero,
                new IChestItem[] { new Key(contentMan, "bottomAreaKey") }));


            //top door
            environment.Add(new Door(contentMan, new Vector3(1925, 0, 2400), Vector3.Zero, new Vector3(1625, 0, 2400),
               90, Vector3.Forward, ref environmentCollidables, "topAreaKey"));
            //chest with key to top door
            environment.Add(new Chest(contentMan, new Vector3(950, 0, 4250), Vector3.Zero, 50, Vector3.Forward,
                new IChestItem[] { new Key(contentMan, "topAreaKey") }));

        }

        private void CreateGatesAndLevers()
        {
            //right half gates and levers
            //top gate
            Gate gate1 = CreateGate(new Vector3(3450, 0, 282), new Vector3(0, 90, 0), 29, 3);

            environment.Add(new Lever(contentMan, new Vector3(3250, 0, 225), new Vector3(0, 90, 0), 100,
                Vector3.Zero, gate1));

            environment.Add(new Lever(contentMan, new Vector3(3650, 0, 275), new Vector3(0, 180, 0), 100,
                Vector3.Zero, gate1));

            //bottom gate
            Gate gate2 = CreateGate(new Vector3(3193, 0, 2260), Vector3.Zero, 31, 3);

            environment.Add(new Lever(contentMan, new Vector3(3225, 0, 2110), new Vector3(0, 270, 0), 100,
                Vector3.Zero, gate2));


            //supermax gates and levers
            //outer supermax gate
            Gate gate3 = CreateGate(new Vector3(1700, 0, 2782), new Vector3(0, 90, 0), 29, 45);
            //getting-in lever
            environment.Add(new Lever(contentMan, new Vector3(300, 0, 4800),
                new Vector3(0, 180, 0), 100, Vector3.Right, gate3));
            //getting-out lever
            environment.Add(new Lever(contentMan, new Vector3(1550, 0, 2782),
                new Vector3(0, 0, 0), 100, Vector3.Left, gate3));

            //inner supermax gate
            Gate gate4 = CreateGate(new Vector3(1200, 0, 2782), new Vector3(0, 90, 0), 29, 45);
            //getting in lever
            environment.Add(new Lever(contentMan, new Vector3(300, 0, 1850),
                new Vector3(0, 180, 0), 100, Vector3.Right, gate4));
            //getting out lever
            environment.Add(new Lever(contentMan, new Vector3(1050, 0, 2782),
                new Vector3(0, 0, 0), 100, Vector3.Left, gate4));
        }

        public override void OnEnteringArea()
        {
            base.OnEnteringArea();
            Game.SoundManager.PlaySong("space", true);
        }
    }
}
