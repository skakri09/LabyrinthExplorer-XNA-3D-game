using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace LabyrinthExplorer
{
    public class Area1Content : AreaContent
    {
        public Area1Content(Camera camera)
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
        }

        public override void Update(GameTime gameTime, Camera camera)
        {
            base.Update(gameTime, camera);
        }

        public override void Draw(GraphicsDevice graphicsDevice, Effect effect,
        Texture2D brickColorMap,Texture2D brickNormalMap,Texture2D brickHeightMap,
        Texture2D stoneColorMap,Texture2D stoneNormalMap,Texture2D stoneHeightMap,
        Texture2D woodColorMap,Texture2D woodNormalMap,Texture2D woodHeightMap)
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

            #region outer wall
            //+Z outer wall
            XPosWal(0, 5000, 5000, 5000);

            //-Z outer wall
            XPosWal(0, 0, 5000, 0);

            //+X outer wall
            ZPosWall(5000, 5000, 5000, 0);

            //-X outer wall
            ZPosWall(-50, 5000, -50, 0);
            #endregion

            #region area 1
            ZPosWall(4200, 4600, 4200, 4200);//1
            ZPosWall(2600, 4600, 2600, 3900);//2
            ZPosWall(2000, 4000, 2000, 3500);//3
            ZPosWall(3200, 4200, 3200, 3500);//4

            XPosWal(1500, 4300, 2600, 4300);//5
            XPosWal(2050, 3500, 3200, 3500);//6
            XPosWal(2600, 4600, 4200, 4600);//7
            XPosWal(3200, 4200, 4200, 4200);//8

            ZPosWall(1500, 4300, 1500, 3800);//9
            ZPosWall(3700, 4200, 3700, 3500);//10

            #endregion

            #region area 2
            XPosWal(0, 4100, 1500, 4100);//1
            XPosWal(400, 2850, 1000, 2850);//2

            ZPosWall(1000, 3800, 1000, 2850);//3
            #endregion

            #region area 3
            XPosWal(800, 2400, 1600, 2400);//1
            XPosWal(2000, 2700, 3050, 2700);//2
            XPosWal(2000, 1800, 3000, 1800);//3
            XPosWal(1400, 1150, 2650, 1150);//4
            XPosWal(750, 2000, 1700, 2000);//5
            XPosWal(750, 750, 3000, 750);//6

            ZPosWall(2000, 2700, 2000, 1850);//7
            ZPosWall(3000, 2700, 3000, 2050);//8
            ZPosWall(2650, 2300, 2650, 1150);//9
            ZPosWall(1650, 2000, 1650, 1200);//10
            ZPosWall(750, 2000, 750, 800);//11
            ZPosWall(3000, 1400, 3000, 750);//12

            XPosWal(2050, 2175, 2400, 2175, 125);
            #endregion

            #region area 4
            ZPosWall(4700, 3500, 4700, 1900);//1
            ZPosWall(3700, 3100, 3700, 750);//2
            ZPosWall(4000, 1900, 4000, 400);//3
            ZPosWall(3600, 400, 3600, 0);//4

            XPosWal(3750, 2400, 4700, 2400);//5
            XPosWal(4000, 1900, 4700, 1900);//6
            XPosWal(3600, 400, 4000, 400);//7
            #endregion

            #region area 5
            XPosWal(500, 400, 1450, 400);//1
            XPosWal(1800, 400, 3350, 400);//2

            ZPosWall(3300, 400, 3300, 0);//3
            ZPosWall(1800, 400, 1800, 0);//4
            ZPosWall(500, 400, 500, 0);//5
            ZPosWall(1400, 400, 1400, 0);//6
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
            enemies.Add(new Spider(new Vector3(3350, GameConstants.WALL_HEIGHT - 50, 3450),
                new Vector3(3350, GameConstants.WALL_HEIGHT - 50, 500),
                "Environment/SmallSpiderBlack", 15, contentMan));
        }

        protected void GenInteractiveEnvironment()
        {
            environment.Add(new Chest(contentMan, new Vector3(4600, 0, 2150),
                    new Vector3(0, -90, 0), 50, Vector3.Left,
                    new IChestItem[] { new Key(contentMan, "blueGemRoomKey") }));

            Gate gate = CreateGate(new Vector3(1375, 0, 982), new Vector3(0, 90, 0), 29, 3);
            environment.Add(new Lever(contentMan, new Vector3(1250, 0, 1150), new Vector3(0, 90, 0), 100, Vector3.Forward, gate));
            CreateDuoLever(new Vector3(2250, 0, 1875), new Vector3(0, 90, 0), 100, Vector3.Forward,
                           new Vector3(1500, 0, 1100), new Vector3(0, 180, 0), 100, Vector3.Backward, gate);

            CreatePortal(new Vector3(1175, 0, 1800),
                    new Vector3(0, 180, 0), 40.0f, Vector3.Backward,
                 new Vector3(450, GameConstants.CAMERA_PLAYER_EYE_HEIGHT, 725), "area2");

            environment.Add(new Gem("GemBlue", contentMan, new Vector3(500, 100, 4500), 50));
            

            environment.Add(new Door(contentMan, new Vector3(2525, 0, 2245), Vector3.Zero,
                new Vector3(2225, 0, 2245), 90, Vector3.Forward, ref environmentCollidables,
                "blueGemRoomKey"));

            environment.Add(new Chest(contentMan, new Vector3(2750, 0, 4300), 
                new Vector3(0, 90, 0), 50, Vector3.Right,
                new IChestItem[]{new Compass(contentMan)}));
        }

        public override void OnEnteringArea()
        {
            base.OnEnteringArea();
            Game.SoundManager.PlaySound("spiderAmbient", 0.7f, null, -1);
        }
    }
}
    