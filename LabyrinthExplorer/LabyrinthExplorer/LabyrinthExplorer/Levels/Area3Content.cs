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
        private FinalGate finalGate;
        bool pedistalsUnlocked = false;

        private AABB StartArea4AABB;

        public Area3Content(Camera camera)
            :base(camera)
        {
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
                            environment.Add(new AssemblyLane(contentMan, new Vector3(2500, 0, 10100), new Vector3(0, 90, 0), 20));
                            environment.Add(new AssemblyLane(contentMan, new Vector3(2500, 0, 13100), new Vector3(0, 90, 0), 20));
                            environment.Add(new AssemblyLane(contentMan, new Vector3(2500, 0, 16100), new Vector3(0, 90, 0), 20));
                            GameConstants.CAMERA_ZFAR = 30000;
                            bluePedistal.StopSound();
                        }
            }
            if (Game.player.PlayerAABB.CheckCollision(StartArea4AABB) != Vector3.Zero)
            {
                Game.player.inv.RemoveItemsOfType("compass");
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

            SmartPosWall(0, 50, 5000, 50);
            SmartPosWall(50, 100, 50, 4950);
            SmartPosWall(4950, 4950, 4950, 100);


            SmartPosWall(50, 4950, 2150, 4950);
            SmartPosWall(2850, 4950, 4950, 4950);
            #region right half area
          
            SmartPosWall(2950, 100, 2950, 2300);
            SmartPosWall(2950, 2900, 2950, 4950);

            SmartPosWall(2050, 3000, 2050, 4950);
            SmartPosWall(2050, 100, 2050, 2550);

            SmartPosWall(3800, 3650, 3800, 4950);
            SmartPosWall(4150, 2900, 4150, 4400);
            SmartPosWall(4150, 4650, 4150, 4950);
            SmartPosWall(4450, 2900, 4450, 3650);
            SmartPosWall(4450, 4000, 4450, 4950);
            SmartPosWall(3800, 3400, 3800, 2900);
            SmartPosWall(3450, 2850, 4800, 2850);
            SmartPosWall(4800, 2900, 4800, 4400);
            SmartPosWall(4800, 4600, 4800, 4950);
            SmartPosWall(4950, 2550, 4050, 2550);
            SmartPosWall(3700, 2550, 3700, 1750);
            SmartPosWall(4700, 2250, 3750, 2250);
            SmartPosWall(4300, 2250, 4300, 1700);
            SmartPosWall(4950, 2000, 4600, 2000);
            SmartPosWall(4350, 1700, 4950, 1700);

            SmartPosWall(3450, 450, 3700, 450);//moved 150 down
            SmartPosWall(3400, 450, 3400, 4800);
            SmartPosWall(3700, 450, 3700, 1300);
            SmartPosWall(3750, 1300, 4450, 1300);
            #endregion

            #region supermaxroom
            SmartPosWall(0, 2300, 1200, 2300);
            SmartPosWall(1150, 2350, 1150, 2600);
            SmartPosWall(1150, 2950, 1150, 3200);
            SmartPosWall(1200, 3200, 0, 3200);
            SmartPosWall(1200, 2550, 1800, 2550);
            SmartPosWall(1200, 2950, 1800, 2950);

            SmartPosWall(1200, 3200, 1800, 3200);
            SmartPosWall(1200, 2300, 1800, 2300);
            SmartPosWall(1750, 3000, 1750, 3200);
            SmartPosWall(1750, 2350, 1750, 2550);

            #endregion supermaxroom

            #region bottom left half
            SmartPosWall(1600, 3250, 1600, 3900);
            SmartPosWall(1600, 4350, 1600, 5000);
            SmartPosWall(1000, 3850, 400, 3850);
            SmartPosWall(350, 4350, 350, 3450);
            SmartPosWall(1200, 4100, 1200, 4700);
            SmartPosWall(1200, 4100, 750, 4100);
            SmartPosWall(400, 4300, 700, 4300);
            SmartPosWall(700, 4100, 700, 4350);
            SmartPosWall(1200, 4650, -50, 4650);
            SmartPosWall(1600, 3650, 700, 3650);
            SmartPosWall(400, 3450, 1350, 3450);
            #endregion bottom left half

            #region top left half
            SmartPosWall(1750, 2300, 1750, 1750);
            SmartPosWall(1800, 1450, 1800, 950);
            SmartPosWall(1800, 650, 1800, 0);
            SmartPosWall(1500, 0, 1500, 950);
            SmartPosWall(1800, 1200, 1250, 1200);
            SmartPosWall(1200, 1500, 1200, 900);
            SmartPosWall(1200, 350, 1200, 650);
            SmartPosWall(950, 1100, 950, 2300);
            SmartPosWall(1200, 350, 700, 350);
            SmartPosWall(1500, 1500, 1500, 2050);
            SmartPosWall(1000, 1750, 1500, 1750);
            SmartPosWall(1200, 2300, 1200, 2000);
            SmartPosWall(650, 2000, 650, 2300);
            SmartPosWall(650, 1600, 650, 350);
            SmartPosWall(950, 800, 950, 400);
            SmartPosWall(200, 0, 200, 1050);
            SmartPosWall(250, 1000, 650, 1000);
            SmartPosWall(650, 1300, 0, 1300);
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
                new Vector3(2525, 0, 4950), Vector3.Zero, 20);
            environment.Add(finalGate);

            //environment.Add(new Gem("GemRed", contentMan, new Vector3(2500, 150, 3000), 50));
            //environment.Add(new Gem("GemBlue", contentMan, new Vector3(2500, 150, 3000), 50));
            //environment.Add(new Gem("GemYellow", contentMan, new Vector3(2500, 150, 3000), 50));

            CreatePortal(new Vector3(2500, 0, 250), Vector3.Zero, 40.0f,
                Vector3.Forward, new Vector3(2450, GameConstants.CAMERA_PLAYER_EYE_HEIGHT, 2700), "area2");
        }

        private void CreatePedistals()
        {

            redPedistal = new Pedistal(contentMan, new Vector3(350, 0, 2542), Vector3.Zero, 75, "GemRed");
            bluePedistal = new Pedistal(contentMan, new Vector3(350, 0, 2775), Vector3.Zero, 75, "GemBlue", true);
            yellowPedistal = new Pedistal(contentMan, new Vector3(350, 0, 3009), Vector3.Zero, 75, "GemYellow");

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

            Gate gate3 = CreateGate(new Vector3(1500, 0, 2782), new Vector3(0, 90, 0), 29, 15);
            CreateDuoLever(new Vector3(300, 0, 4800), new Vector3(0, 180, 0), 100, Vector3.Right,
                        new Vector3(300, 0, 1850), new Vector3(0, 180, 0), 100, Vector3.Right,
                        gate3, 40.0f);
            environment.Add(new Lever(contentMan, new Vector3(1050, 0, 2782),
                new Vector3(0, 0, 0), 100, Vector3.Left, gate3));

        }

        public override void OnEnteringArea()
        {
            base.OnEnteringArea();
            Game.SoundManager.PlaySong("space", true);
        }
    }
}
