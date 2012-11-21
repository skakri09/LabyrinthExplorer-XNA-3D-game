using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace LabyrinthExplorer
{
    public class Level1Content : IGameLevel
    {
        private List<SolidWall> walls;
        private List<NormalMappedCeiling> ceilings;
        private List<NormalMappedFloor> floors;
        private List<Enemy> enemies;
        private List<IEnvironmentObject> environment;
        private List<AABB> environmentCollidables;//stuff stuff in here

        private Texture2D brickColorMap;
        private Texture2D brickNormalMap;
        private Texture2D brickHeightMap;
        private Texture2D stoneColorMap;
        private Texture2D stoneNormalMap;
        private Texture2D stoneHeightMap;
        private Texture2D woodColorMap;
        private Texture2D woodNormalMap;
        private Texture2D woodHeightMap;

        private ContentManager contentMan;
        private GraphicsDevice device;

        private Camera camera;

        public Level1Content(Camera camera)
        {
            this.camera = camera;
            walls = new List<SolidWall>();
            ceilings = new List<NormalMappedCeiling>();
            floors = new List<NormalMappedFloor>();
            enemies = new List<Enemy>();
            environment = new List<IEnvironmentObject>();
            environmentCollidables = new List<AABB>();
        }

        public void LoadContent(GraphicsDevice device, ContentManager contentMan)
        {
            this.device = device;
            this.contentMan = contentMan;

            LoadMaps();
            GenerateWalls();
            GenerateCeiling();
            GenerateFloors();
            GenerateEnemies();
            GenInteractiveEnvironment();
        }

        public void Update(GameTime gameTime, Camera camera)
        {
            float deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
            foreach (Enemy enemy in enemies)
            {
                enemy.Update(deltaTime);
            }
            foreach(IEnvironmentObject obj in environment)
            {
                obj.Update(deltaTime);
            }
        }

        public void Draw(GraphicsDevice graphicsDevice, Effect effect)
        {
            //           ------ Available assets ------                     
            //  "colorMapTexture", "normalMapTexture", "heightMapTexture",
            //  brickColorMap,      brickNormalMap,     brickHeightMap,
            //  stoneColorMap,      stoneNormalMap,     stoneHeightMap,
            //  woodColorMap,       woodNormalMap,      woodHeightMap

            foreach (SolidWall wall in walls)
            {
                wall.Draw(graphicsDevice, effect, "colorMapTexture",
                        "normalMapTexture", "heightMapTexture",
                        stoneColorMap, stoneNormalMap, stoneHeightMap);
            }
            foreach (NormalMappedCeiling ceiling in ceilings)
            {
                ceiling.Draw(graphicsDevice, effect, "colorMapTexture",
                        "normalMapTexture", "heightMapTexture",
                        stoneColorMap, stoneNormalMap, stoneHeightMap);
            }
            foreach (NormalMappedFloor floor in floors)
            {
                floor.Draw(graphicsDevice, effect, "colorMapTexture",
                            "normalMapTexture", "heightMapTexture",
                            stoneColorMap, stoneNormalMap, stoneHeightMap);
            }
            foreach (IEnvironmentObject obj in environment)
            {
                obj.Draw(camera, effect);
            }
            foreach (Enemy enemy in enemies)
            {
                enemy.Draw(camera);
            }
        }

        private void LoadMaps()
        {
            brickColorMap = contentMan.Load<Texture2D>(@"Textures\brick_color_map");
            brickNormalMap = contentMan.Load<Texture2D>(@"Textures\brick_normal_map");
            brickHeightMap = contentMan.Load<Texture2D>(@"Textures\brick_height_map");

            stoneColorMap = contentMan.Load<Texture2D>(@"Textures\stone_color_map");
            stoneNormalMap = contentMan.Load<Texture2D>(@"Textures\stone_normal_map");
            stoneHeightMap = contentMan.Load<Texture2D>(@"Textures\stone_height_map");

            woodColorMap = contentMan.Load<Texture2D>(@"Textures\wood_color_map");
            woodNormalMap = contentMan.Load<Texture2D>(@"Textures\wood_normal_map");
            woodHeightMap = contentMan.Load<Texture2D>(@"Textures\wood_height_map");
        }
        
        private void GenerateWalls()
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

        private void GenerateCeiling()
        {
            ceilings.Add(new NormalMappedCeiling(device,
                new Vector3(-5000, GameConstants.WALL_HEIGHT, 5000), 
                new Vector3(5000, GameConstants.WALL_HEIGHT, 5000),
                new Vector3(5000, GameConstants.WALL_HEIGHT, -5000), 
                new Vector3(-5000, GameConstants.WALL_HEIGHT, -5000), 
                Vector3.Down));
        }

        private void GenerateFloors()
        {
            CreateFloor(new Vector3(-5000, 0, 5000), new Vector3(5000, 0, 5000),
                        new Vector3(5000, 0, -5000), new Vector3(-5000, 0, -5000));
        }

        private void GenerateEnemies()
        {
            enemies.Add(new Spider(new Vector3(3350, GameConstants.WALL_HEIGHT - 50, 3450), 
                new Vector3(3350, GameConstants.WALL_HEIGHT - 50, 500),
                "Environment/SmallSpiderBlack", 15, contentMan));
        }

        private void GenInteractiveEnvironment()
        {
            environment.Add(new Chest(contentMan, new Vector3(4600, 0, 2150), 
                    new Vector3(0, -90, 0), 50, Vector3.Left));

            Gate gate = CreateGate(new Vector3(1375, 0, 982), new Vector3(0, 90, 0), 29, 3);

            CreateDuoLever(new Vector3(2250, 0, 1875), new Vector3(90, 0, 0), 100, Vector3.Forward,
                           new Vector3(1500, 0, 1100), new Vector3(180, 0, 0), 100, Vector3.Backward, gate); 
           
            Portal portal = new Portal(contentMan,new Vector3(1175, 0, 1800), 
                                new Vector3(0, 180, 0), 40.0f, Vector3.Backward,
                                GameConstants.PLAYER_START_POS) ;
            environment.Add(portal);
            Interactables.AddInteractable(portal);
        }

        #region Ease of creation functions

        private void CreateDuoLever(Vector3 pos1, Vector3 rot1, float scale1, Vector3 openDir1,
                                    Vector3 pos2, Vector3 rot2, float scale2, Vector3 openDir2,
                                    IInteractableObject onUseObject)
        {
            Lever lever1 = new Lever(contentMan, pos1, rot1, scale1, openDir1);
            Lever lever2 = new Lever(contentMan, pos2, rot2, scale2, openDir2);
            environment.Add(new DuoLever(lever1, lever2, onUseObject));
        }

        private Gate CreateGate(Vector3 position, Vector3 rotation,
                                float scale, float closeAfter = 0, bool isClosed = true)
        {
            Gate gate = new Gate(contentMan, position, rotation, scale, closeAfter, isClosed);
            environment.Add(gate);
            environmentCollidables.Add(gate);
            return gate;
        }

        private void ZPosWall(float startX, float startZ, float endX, float endZ, float width = 50)
        {
            ZPosWall(new Vector2(startX, startZ), new Vector2(endX, endZ), width);
        }
        private void ZPosWall(Vector2 startPos, Vector2 endPos, float width = 50)
        {
            ZWallPosX newWall = new ZWallPosX(device, startPos, endPos, width);
            walls.Add(newWall);
            environmentCollidables.Add(newWall.Aabb);
        }
        private void XPosWal(float startX, float startZ, float endX, float endZ, float width = 50)
        {
            XPosWal(new Vector2(startX, startZ), new Vector2(endX, endZ), width);
        }
        private void XPosWal(Vector2 startPos, Vector2 endPos, float width = 50)
        {
            XWallPosZ newWall = new XWallPosZ(device, startPos, endPos, width);
            walls.Add(newWall);
            environmentCollidables.Add(newWall.Aabb);
        }

        private void CreateFloor(Vector3 frontLeft, Vector3 frontRight, 
                                 Vector3 backRight, Vector3 backLeft)
        {
            floors.Add(new NormalMappedFloor(device,
            frontLeft, frontRight, backRight, backLeft,
            Vector3.Up));
        }

        #endregion

        public List<AABB> EnvironmentCollidables()
        {
            return environmentCollidables; 
        }
    }
}
