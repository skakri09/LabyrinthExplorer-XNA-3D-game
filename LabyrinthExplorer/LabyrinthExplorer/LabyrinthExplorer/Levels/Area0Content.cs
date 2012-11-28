using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace LabyrinthExplorer
{
    public class Area0Content : AreaContent
    {
        public Area0Content(Camera camera)
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
        }

        public override void Update(GameTime gameTime, Camera camera)
        {
            base.Update(gameTime, camera);
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
            SmartPosWall(0, 0, 4000, 0);
            SmartPosWall(250, 50, 250, 3950);
            SmartPosWall(100, 3150, 4050, 3150);
            SmartPosWall(800, 50, 800, 2000);
            SmartPosWall(800, 2300, 800, 2650);
            SmartPosWall(850, 1950, 2000, 1950);
            SmartPosWall(1950, 2000, 1950, 2350);
            SmartPosWall(850, 2300, 1950, 2300);
            SmartPosWall(1100, 2350, 1100, 2600);
            SmartPosWall(850, 2600, 1950, 2600);
            SmartPosWall(3450, 3950, 3450, 50);
            SmartPosWall(2550, 3900, 2550, 1500);
            SmartPosWall(2650, 1550, 1700, 1550);
            SmartPosWall(1700, 1550, 1700, 0);
            SmartPosWall(850, 200, 1700, 200);
            SmartPosWall(1700, 2000, 1700, 2300);
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
            environment.Add(new Chest(contentMan, new Vector3(1500, 0, 2140),
                    new Vector3(0, 270, 0), 50, Vector3.Left,
                    new IChestItem[] { new Key(contentMan, "blueGemRoomKey") }));

            Gate gate1 = CreateGate(new Vector3(850, 0, 2158), new Vector3(0, 90, 0), 29, 3);
           
            environment.Add(new Lever(contentMan, new Vector3(750, 0, 2100), Vector3.Left, 100, Vector3.Zero, gate1));
            environment.Add(new Lever(contentMan, new Vector3(1100, 0, 2100), new Vector3(0, 180, 0), 100, Vector3.Right, gate1));

            Gate gate2 = CreateGate(new Vector3(1800, 0, 1782), new Vector3(0, 90, 0), 29, 3);

            environment.Add(new Lever(contentMan, new Vector3(1550, 0, 1600), new Vector3(0, 270, 0), 100, Vector3.Backward, gate2));
            CreateDuoLever(new Vector3(1200, 0, 2540), new Vector3(0, 180, 0), 100, Vector3.Right,
                           new Vector3(2050, 0, 1650), new Vector3(0, 90, 0), 100, Vector3.Forward, gate2, 4.5f);

            CreatePortal(new Vector3(1300, 0, 400),
                   new Vector3(0, 0, 0), 40.0f, Vector3.Zero,
                new Vector3(4000, GameConstants.CAMERA_PLAYER_EYE_HEIGHT, 4575), "area1");

            environment.Add(new Gem("GemBlue", contentMan, new Vector3(1450, 100, 2420), 50));


            environment.Add(new Door(contentMan, new Vector3(1900, 0, 2475), new Vector3(0, 270, 0),
                new Vector3(1900, 0, 2200), 90, Vector3.Right, ref environmentCollidables,
                "blueGemRoomKey"));
            environment.Add(new TextHint(@"Models\Text\GemHint1",contentMan, new Vector3(1325, 150, 2350),
                new Vector3(90, 0, 0), 0.3f));

            environment.Add(new Skeleton(@"Models\Environment\SkeletonSitting", contentMan,
                new Vector3(1200, 0, 2380), Vector3.Zero, 90));
        }

        public override void OnEnteringArea()
        {
            base.OnEnteringArea();
            Game.SoundManager.PlaySound("spiderAmbient", 0.7f, null, -1);
        }
    }
}
