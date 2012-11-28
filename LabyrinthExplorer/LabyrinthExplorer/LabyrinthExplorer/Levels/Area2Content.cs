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

        private TurnablePilar TopPillar;
        private TurnablePilar BottomPillar;
        private TurnablePilar LeftPillar;
        private TurnablePilar RightPillar;

        private List<TurnablePilar> Pillars;

        private bool PillarsUnlocked = false;

        public Area2Content(Camera camera)
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

            randomWhisper = new RandWhisper(30.0f);
        }

        public override void Update(GameTime gameTime, Camera camera)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime, camera);
            randomWhisper.Update(deltaTime);
            UpdatePillarsLock();
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

            if (World.currentDifficulity >= Difficulity.EASY)
            {
                #region "wall" of text :)
                SmartPosWall(50, 50, 50, 4950);
                SmartPosWall(50, 900, 850, 900);
                SmartPosWall(50, 50, 4950, 50);
                SmartPosWall(50, 4950, 4950, 4950);
                SmartPosWall(350, 2550, 350, 1500);
                SmartPosWall(350, 3800, 350, 2900);
                SmartPosWall(350, 4750, 350, 4150);
                SmartPosWall(400, 1700, 1450, 1700);//shrinked 50 to right
                SmartPosWall(650, 2300, 650, 2600);//moved 50 down
                SmartPosWall(700, 3950, 1700, 3950);//moved 50 down
                SmartPosWall(850, 900, 850, 400);
                SmartPosWall(1050, 3750, 1050, 2950);
                SmartPosWall(1250, 4750, 350, 4750);
                SmartPosWall(1250, 4950, 1250, 4500);//moved 50 down
                //SmartPosWall(1250, 4450, 2000, 4450);
                SmartPosWall(1300, 450, 1300, 900);//moved 50 down
                SmartPosWall(1300, 400, 2050, 400);
                SmartPosWall(1450, 2250, 650, 2250);
                SmartPosWall(1450, 2600, 650, 2600);
                SmartPosWall(1450, 2900, 650, 2900);
                SmartPosWall(1450, 2600, 1450, 1700);
                SmartPosWall(1450, 2900, 1450, 3350);
                SmartPosWall(1700, 2200, 1700, 2600);//moved 50 down
                SmartPosWall(1700, 2900, 1700, 3350);
                SmartPosWall(1700, 3650, 1700, 4000);//moved 50 down
                
                //SmartPosWall(2000, 1600, 2950, 1600);
                //SmartPosWall(2050, 900, 1300, 900);
                SmartPosWall(2050, 400, 2050, 900);
                SmartPosWall(2050, 1200, 2900, 1200);
                SmartPosWall(2100, 200, 3150, 200);
                SmartPosWall(2300, 3350, 1700, 3350);//left X bottom middle wallS
                SmartPosWall(2250, 2150, 1700, 2150);
                
                SmartPosWall(2350, 250, 2350, 950);//moved 50 down
                SmartPosWall(2350, 3650, 2350, 4650);//moved 50 down
                SmartPosWall(2350, 4650, 4950, 4650);
                //SmartPosWall(2400, 1250, 2400, 1600);//moved 50 down
                SmartPosWall(2600, 2150, 3100, 2150);
                
                SmartPosWall(2650, 4350, 2650, 4650);//moved 50 down
                SmartPosWall(2650, 4100, 3350, 4100);
                SmartPosWall(2650, 4300, 4600, 4300);
                SmartPosWall(3100, 3350, 2600, 3350);
                SmartPosWall(3100, 2150, 3100, 2600);
                SmartPosWall(3100, 2950, 3100, 3350);
                
                SmartPosWall(3450, 1950, 4950, 1950);
                SmartPosWall(3500, 200, 3500, 1650);
                SmartPosWall(3650, 2900, 3650, 3550);
                SmartPosWall(3650, 3800, 3650, 4300);
                SmartPosWall(3700, 2900, 4600, 2900); //Changed SmarPosWall(3650, 2900, 4600, 2900);
                SmartPosWall(4250, 350, 4250, 50);
                SmartPosWall(3850, 950, 4100, 950);//wall split in two for door
                SmartPosWall(4350, 950, 4700, 950, 125);//^
                SmartPosWall(3850, 1000, 3850, 1650);//added
                SmartPosWall(3850, 1650, 4700, 1650);//locking room at bottom
                SmartPosWall(4300, 4100, 4300, 3850);//move 50 down
                SmartPosWall(4300, 3300, 4600, 3300);
                SmartPosWall(4600, 4100, 4300, 4100);
                SmartPosWall(4600, 4300, 4600, 2600);
                SmartPosWall(4700, 350, 4250, 350);
                
                SmartPosWall(4950, 50, 4950, 4950);
                #endregion
            }

            if(World.currentDifficulity == Difficulity.EASY)
            {
                SmartPosWall(4650, 2600, 5000, 2600);
            }
            if (World.currentDifficulity <= Difficulity.MEDIUM)
            {
                //SmartPosWall(1700, 3600, 2300, 3600);
                SmartPosWall(4700, 2600, 4700, 350);//extended to lock the redGemRoom
            }
            if (World.currentDifficulity >= Difficulity.HARD)
            {
                SmartPosWall(2350, 4650, 1400, 4650);
                SmartPosWall(4700, 1700, 4700, 350);//extended to lock the redGemRoom
                SmartPosWall(2650, 3850, 2650, 4100);//moved 50 down
                SmartPosWall(3100, 2900, 3350, 2900); //SmarPosWall(3100, 2900, 3350, 2900);
                SmartPosWall(3100, 2600, 4600, 2600);
                SmartPosWall(3350, 3800, 2650, 3800);
                SmartPosWall(3350, 2900, 3350, 4100);
                SmartPosWall(1700, 3600, 3100, 3600);
                SmartPosWall(3900, 3300, 3900, 3100);
                SmartPosWall(4450, 3800, 4100, 3800);//1
                SmartPosWall(4450, 3800, 4450, 3450);//2

                SmartPosWall(3950, 3100, 4300, 3100);//move 50 right
                SmartPosWall(3950, 3500, 4150, 3500);//move 50 right
                SmartPosWall(3950, 3650, 4250, 3650);//move 50 right

                SmartPosWall(3900, 3500, 3900, 4100);
                SmartPosWall(3900, 4100, 4100, 4100);

                SmartPosWall(3900, 3300, 4300, 3300);
                SmartPosWall(4250, 3350, 4250, 3650);//moved 50 down
            }
        }

        protected void GenerateCeiling()
        {
            //top part
            ceilings.Add(new NormalMappedCeiling(device,
                new Vector3(0, GameConstants.WALL_HEIGHT, 0),
                new Vector3(0, GameConstants.WALL_HEIGHT, 2200),
                new Vector3(5000, GameConstants.WALL_HEIGHT, 2200),
                new Vector3(5000, GameConstants.WALL_HEIGHT, 0),
                Vector3.Down));

            //bottom part
            ceilings.Add(new NormalMappedCeiling(device,
                new Vector3(0, GameConstants.WALL_HEIGHT, 3350),
                new Vector3(0, GameConstants.WALL_HEIGHT, 5000),
                new Vector3(5000, GameConstants.WALL_HEIGHT, 5000),
                new Vector3(5000, GameConstants.WALL_HEIGHT, 3350),
                Vector3.Down));

            //left (closest to 0)
            ceilings.Add(new NormalMappedCeiling(device,
                new Vector3(0, GameConstants.WALL_HEIGHT, 2200),
                new Vector3(0, GameConstants.WALL_HEIGHT, 3350),
                new Vector3(1750, GameConstants.WALL_HEIGHT, 3350),
                new Vector3(1750, GameConstants.WALL_HEIGHT, 2200),
                Vector3.Down));

            //right
            ceilings.Add(new NormalMappedCeiling(device,
                new Vector3(3100, GameConstants.WALL_HEIGHT, 2200),
                new Vector3(3100, GameConstants.WALL_HEIGHT, 3350),
                new Vector3(5000, GameConstants.WALL_HEIGHT, 3350),
                new Vector3(5000, GameConstants.WALL_HEIGHT, 2200),
                Vector3.Down));
        }

        protected void GenerateFloors()
        {
            CreateFloor(new Vector3(0, 0, 5000), new Vector3(5000, 0, 5000),
                        new Vector3(5000, 0, -5000), new Vector3(0, 0, -5000));
        }

        protected void GenerateEnemies()
        {
         
        }

        protected void GenInteractiveEnvironment()
        {
            CreatePillarsWithLever();

            CreatePortal(new Vector3(450, 0, 725),
                new Vector3(0, 180, 0), 40.0f, Vector3.Forward,
                new Vector3(1175, GameConstants.CAMERA_PLAYER_EYE_HEIGHT, 1800), "area1");

            environment.Add(new Chest(contentMan, new Vector3(4800, 0, 4850),
                    new Vector3(0, -90, 0), 50, Vector3.Left,
                    new IChestItem[] { new Key(contentMan, "yellowGemRoomKey") }));

            environment.Add(new Door(contentMan, new Vector3(4225, 0, 1004), Vector3.Zero,
               new Vector3(4525, 0, 1004), 90, Vector3.Backward, ref environmentCollidables,
               "yellowGemRoomKey"));

            environment.Add(new Gem("GemYellow", contentMan, new Vector3(4250, 100, 1350), 50));

            CreatePortal(new Vector3(2450, 0, 2700),
                   new Vector3(0, 180, 0), 40.0f, Vector3.Backward,
                new Vector3(2500, GameConstants.CAMERA_PLAYER_EYE_HEIGHT, 250), "area3");

            TextHint hintText = new TextHint(@"Models\Text\Area2TextHint", contentMan,
                new Vector3(500, 100, 103), new Vector3(90, 0, 0), 1.0f);
            environment.Add(hintText);

            AddSkeletoAndDays();

            environment.Add(new TextHint(@"Models\Text\TheVoices", contentMan, new Vector3(100, 100, 2800),
               new Vector3(90, 90, 0), 0.3f));
        }

        private void AddSkeletoAndDays()
        {
            environment.Add(new Skeleton(@"Models\Environment\SkeletonSitting", contentMan,
               new Vector3(2500, 0, 4600), new Vector3(0, 180, 0), 90));

            environment.Add(new TextHint(@"Models\Text\5Days", contentMan, new Vector3(2450, 150, 4650),
                new Vector3(90, 180, 0), 0.3f));
            environment.Add(new TextHint(@"Models\Text\5Days", contentMan, new Vector3(2470, 150, 4650),
                new Vector3(90, 180, 0), 0.3f));
            environment.Add(new TextHint(@"Models\Text\5Days", contentMan, new Vector3(2490, 150, 4650),
                new Vector3(90, 180, 0), 0.3f));
            environment.Add(new TextHint(@"Models\Text\5Days", contentMan, new Vector3(2510, 150, 4650),
                new Vector3(90, 180, 0), 0.3f));
            environment.Add(new TextHint(@"Models\Text\3Days", contentMan, new Vector3(2470, 130, 4650),
                new Vector3(90, 180, 0), 0.3f));
            environment.Add(new TextHint(@"Models\Text\5Days", contentMan, new Vector3(2490, 130, 4650),
                new Vector3(90, 180, 0), 0.3f));
            environment.Add(new TextHint(@"Models\Text\5Days", contentMan, new Vector3(2510, 130, 4650),
                new Vector3(90, 180, 0), 0.3f));

            environment.Add(new Skeleton(@"Models\Environment\SkeletonChained", contentMan, new Vector3(4675, 0, 1800),
                new Vector3(0, 270, 0), 90));
        }

        private void CreatePillarsWithLever()
        {
            //pillar 3
            TopPillar = new TurnablePilar(contentMan, new Vector3(2425, -100, 2200),
                 new Vector3(0, 180, 0), Vector3.Forward, 105, new Vector3(1, 0.2f, 0.2f));
            environment.Add(TopPillar);
            environmentCollidables.Add(TopPillar);
            environment.Add(new Lever(contentMan, new Vector3(2400, 0, 1900),
                Vector3.Zero, 100, Vector3.Zero, TopPillar, new Vector3(1, 0.2f, 0.2f)));
            
            //pillar 1
            BottomPillar = new TurnablePilar(contentMan, new Vector3(2450, -100, 3275),
               new Vector3(0, 0, 0), Vector3.Backward, 105, new Vector3(1, 0.2f, 1.0f));
            environment.Add(BottomPillar);
            environmentCollidables.Add(BottomPillar);
            environment.Add(new Lever(contentMan, new Vector3(1100, 0, 4875),
              Vector3.Zero, 100, Vector3.Zero, BottomPillar, new Vector3(1, 0.2f, 1.0f)));

            //pillar 4
            LeftPillar = new TurnablePilar(contentMan, new Vector3(1800, -100, 2750),
               new Vector3(0, 270, 0), Vector3.Left, 105, new Vector3(0.2f, 0.2f, 1.0f));
            environment.Add(LeftPillar);
            environmentCollidables.Add(LeftPillar);
            environment.Add(new Lever(contentMan, new Vector3(1150, 0, 2000),
              Vector3.Zero, 100, Vector3.Zero, LeftPillar, new Vector3(0.2f, 0.2f, 1.0f)));

            //pillar 2
            RightPillar = new TurnablePilar(contentMan, new Vector3(3050, -100, 2750),
                new Vector3(0, 90, 0), Vector3.Right, 105, new Vector3(0.2f, 1.0f, 0.2f));
            environment.Add(RightPillar);
            environmentCollidables.Add(RightPillar);
            environment.Add(new Lever(contentMan, new Vector3(4400, 0, 4000),
              new Vector3(0, 180, 0), 100, Vector3.Zero, RightPillar, new Vector3(0.2f, 1.0f, 0.2f)));

            Pillars = new List<TurnablePilar>();
            Pillars.Add(BottomPillar);
            Pillars.Add(LeftPillar);
            Pillars.Add(RightPillar);
            Pillars.Add(TopPillar);
        }

        private void UpdatePillarsLock()
        {
            if (!PillarsUnlocked)
            {
                int unlockedPillars = 0;
                foreach (TurnablePilar pillar in Pillars)
                {
                    if (pillar.IsUnlocked)
                    {
                        ++unlockedPillars;
                    }
                }
                if (unlockedPillars == Pillars.Count)
                {
                    PillarsUnlocked = true;
                    Game.SoundManager.PlaySound("GroundShaking", null, 4);
                    foreach (TurnablePilar pillar in Pillars)
                    {
                        pillar.Submerge();
                    }
                }
            }

        }

        public override void OnEnteringArea()
        {
            base.OnEnteringArea();
            Game.SoundManager.PlaySound("Area2Ambient", 0.7f, null, -1);
        }
    }
}
