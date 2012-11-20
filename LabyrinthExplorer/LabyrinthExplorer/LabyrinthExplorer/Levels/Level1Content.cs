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
        private List<EnvironmentObject> environment;
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
            environment = new List<EnvironmentObject>();
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
            GenerateEnvironment();

            //ctrlRoom = contentMan.Load<Model>(@"Models\ControlRoom");
            //ironMaiden = contentMan.Load<Model>(@"Models\Environment\DoorClosed");
        }

        public void Update(GameTime gameTime, Camera camera)
        {
            float deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
            foreach (Enemy enemy in enemies)
            {
                enemy.Update(gameTime.ElapsedGameTime, deltaTime);
            }
            foreach(EnvironmentObject obj in environment)
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
            foreach (EnvironmentObject obj in environment)
            {
                obj.Draw(camera);
            }

            //Matrix[] transforms = new Matrix[ctrlRoom.Bones.Count];
            //ctrlRoom.CopyAbsoluteBoneTransformsTo(transforms);
            //foreach (ModelMesh mesh in ctrlRoom.Meshes)
            //{
            //    foreach (BasicEffect effect in mesh.Effects)
            //    {
            //        effect.EnableDefaultLighting();

            //        effect.World = Matrix.Identity
            //            * transforms[mesh.ParentBone.Index]
            //            //* Matrix.CreateRotationX(MathHelper.ToRadians(rotation.X))
            //            //* Matrix.CreateRotationY(MathHelper.ToRadians(rotation.Y))
            //            //* Matrix.CreateRotationZ(MathHelper.ToRadians(rotation.Z))
            //            * Matrix.CreateScale(1)
            //            * Matrix.CreateTranslation(new Vector3(-5355, 30, 0));
            //        effect.View = camera.ViewMatrix;
            //        effect.Projection = camera.ProjectionMatrix;


            //    }
            //    mesh.Draw();
            //}
            //Matrix[] transformsi = new Matrix[ironMaiden.Bones.Count];
            //ironMaiden.CopyAbsoluteBoneTransformsTo(transformsi);

            //foreach (ModelMesh mesh in ironMaiden.Meshes)
            //{
            //    foreach (BasicEffect effect in mesh.Effects)
            //    {
            //        effect.EnableDefaultLighting();

            //        effect.World = Matrix.Identity
            //            * transformsi[mesh.ParentBone.Index]
            //            * Matrix.CreateRotationY(MathHelper.ToRadians(180))
            //            * Matrix.CreateScale(75)
            //            * Matrix.CreateTranslation(GameConstants.PLAYER_START_POS);
            //        effect.View = camera.ViewMatrix;
            //        effect.Projection = camera.ProjectionMatrix;


            //    }
            //    mesh.Draw();
            //}
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
            //XWallNegZ = grows out on negative Z
            //XWallPosZ = grows out on positive Z
            //ZWallNegX = grows out on negative X
            //ZWallPosX = grows out on positive X

            //Z Walls - ZWallPosX
            //X Walls - XWallNegZAl

            #region outer wall
            //+Z outer wall
            walls.Add(new SolidWall(device,
                new Vector2(0, 5050), new Vector2(5000, 5050),
                new Vector2(5000, 5000), new Vector2(0, 5000), GameConstants.WALL_HEIGHT));
            //-Z outer wall
            walls.Add(new SolidWall(device,
                 new Vector2(0, 0), new Vector2(5000, 0),
                 new Vector2(5000, -50), new Vector2(0, -50), GameConstants.WALL_HEIGHT));
            //+X outer wall
            walls.Add(new SolidWall(device,
                new Vector2(5000, 5000), new Vector2(5050, 5000),
                new Vector2(5050, -0), new Vector2(5000, 0), GameConstants.WALL_HEIGHT));
            //-X outer wall
            walls.Add(new SolidWall(device,
                 new Vector2(-50, 5000), new Vector2(0, 5000),
                 new Vector2(0, 0), new Vector2(-50, 0), GameConstants.WALL_HEIGHT));
            #endregion

            #region area 1
            walls.Add(new ZWallPosX(device, new Vector2(4200, 4600), new Vector2(4200, 4200)));//1
            walls.Add(new ZWallPosX(device, new Vector2(2600, 4600), new Vector2(2600, 3900)));//2
            walls.Add(new ZWallPosX(device, new Vector2(2000, 4000), new Vector2(2000, 3500)));//3
            walls.Add(new ZWallPosX(device, new Vector2(3200, 4200), new Vector2(3200, 3500)));//4


            walls.Add(new XWallPosZ(device, new Vector2(1500, 4300), new Vector2(2600, 4300)));//5
            walls.Add(new XWallPosZ(device, new Vector2(2050, 3500), new Vector2(3200, 3500)));//6
            walls.Add(new XWallPosZ(device, new Vector2(2600, 4600), new Vector2(4200, 4600)));//7
            walls.Add(new XWallPosZ(device, new Vector2(3200, 4200), new Vector2(4200, 4200)));//8

            walls.Add(new ZWallPosX(device, new Vector2(1500, 4300), new Vector2(1500, 3800)));//9
            walls.Add(new ZWallPosX(device, new Vector2(3700, 4200), new Vector2(3700, 3500)));//10
            #endregion

            #region area 2
            walls.Add(new XWallPosZ(device, new Vector2(0, 4100), new Vector2(1500, 4100)));//1
            walls.Add(new XWallPosZ(device, new Vector2(400, 2850), new Vector2(1000, 2850)));//2

            walls.Add(new ZWallPosX(device, new Vector2(1000, 3800), new Vector2(1000, 2850)));//3
            #endregion

            #region area 3
            walls.Add(new XWallPosZ(device, new Vector2(800, 2400), new Vector2(1600, 2400)));//1
            walls.Add(new XWallPosZ(device, new Vector2(2000, 2700), new Vector2(3050, 2700)));//2
            walls.Add(new XWallPosZ(device, new Vector2(2000, 1800), new Vector2(3000, 1800)));//3
            walls.Add(new XWallPosZ(device, new Vector2(1400, 1150), new Vector2(2650, 1150)));//4
            walls.Add(new XWallPosZ(device, new Vector2(1000, 2000), new Vector2(1450, 2000)));//5
            walls.Add(new XWallPosZ(device, new Vector2(1000, 750), new Vector2(3000, 750)));//6

            walls.Add(new ZWallPosX(device, new Vector2(2000, 2700), new Vector2(2000, 1850)));//7
            walls.Add(new ZWallPosX(device, new Vector2(3000, 2700), new Vector2(3000, 2050)));//8
            walls.Add(new ZWallPosX(device, new Vector2(2650, 2300), new Vector2(2650, 1150)));//9
            walls.Add(new ZWallPosX(device, new Vector2(1400, 2000), new Vector2(1400, 1200)));//10
            walls.Add(new ZWallPosX(device, new Vector2(1000, 2000), new Vector2(1000, 800)));//11
            walls.Add(new ZWallPosX(device, new Vector2(3000, 1400), new Vector2(3000, 750)));//12
            #endregion

            #region area 4
            walls.Add(new ZWallPosX(device, new Vector2(4700, 3500), new Vector2(4700, 1900)));//1
            walls.Add(new ZWallPosX(device, new Vector2(3700, 3100), new Vector2(3700, 750)));//2
            walls.Add(new ZWallPosX(device, new Vector2(4000, 1900), new Vector2(4000, 400)));//3
            walls.Add(new ZWallPosX(device, new Vector2(3600, 400), new Vector2(3600, 0)));//4

            walls.Add(new XWallPosZ(device, new Vector2(3750, 2400), new Vector2(4700, 2400)));//5
            walls.Add(new XWallPosZ(device, new Vector2(4000, 1900), new Vector2(4700, 1900)));//6
            walls.Add(new XWallPosZ(device, new Vector2(3600, 400), new Vector2(4000, 400)));//7
            #endregion

            #region area 5
            walls.Add(new XWallPosZ(device, new Vector2(500, 400), new Vector2(1450, 400)));//1
            walls.Add(new XWallPosZ(device, new Vector2(1800, 400), new Vector2(3350, 400)));//2

            walls.Add(new ZWallPosX(device, new Vector2(3300, 400), new Vector2(3300, 0)));//3
            walls.Add(new ZWallPosX(device, new Vector2(1800, 400), new Vector2(1800, 0)));//4
            walls.Add(new ZWallPosX(device, new Vector2(500, 400), new Vector2(500, 0)));//5
            walls.Add(new ZWallPosX(device, new Vector2(1400, 400), new Vector2(1400, 0)));//6
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
            
        }

        private void GenerateEnvironment()
        {
            environment.Add(new Chest(contentMan, new Vector3(4600, 0, 2150), 
                    new Vector3(0, 0, 0), 50, Vector3.Left));
        }

        #region Ease of creation functions

        private void CreateEnemy(string enemyName)
        {
            Enemy newEnemy = new Enemy(enemyName);
            newEnemy.LoadContent(contentMan);
            enemies.Add(newEnemy);
            //possibly add AABB?
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
