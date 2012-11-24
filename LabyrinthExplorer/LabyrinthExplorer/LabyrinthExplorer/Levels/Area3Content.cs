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
        }

        public override void Update(GameTime gameTime, Camera camera)
        {
            base.Update(gameTime, camera);
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


            #region left half area
            SmarPosWall(0, 50, 5000, 50);
            SmarPosWall(50, 100, 50, 4950);
            SmarPosWall(50, 4950, 4950, 4950);
            SmarPosWall(4950, 4950, 4950, 100);
            SmarPosWall(2050, 100, 2050, 2300);
            SmarPosWall(2950, 100, 2950, 2300);
            SmarPosWall(2050, 2900, 2050, 4900);
            SmarPosWall(2950, 2900, 2950, 4950);
            
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
            //SmarPosWall(3450, 400, 3700, 400);
            SmarPosWall(3750, 1300, 4450, 1300);
            #endregion

        }

        protected void GenerateCeiling()
        {

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
            
            environment.Add(new Gem("GemYellow", contentMan, new Vector3(2500, 150, 3000), 50));

            CreatePortal(new Vector3(2500, 0, 250), Vector3.Zero, 40.0f,
                Vector3.Forward, new Vector3(2450, GameConstants.CAMERA_PLAYER_EYE_HEIGHT, 2700), "area2");
            Gate gate = CreateGate(new Vector3(3450, 0, 282), new Vector3(0, 90, 0), 29, 3);
            
            environment.Add(new Lever(contentMan, new Vector3(3250, 0, 225), new Vector3(0, 90, 0), 100,
                Vector3.Zero, gate));

            environment.Add(new Lever(contentMan, new Vector3(3800, 0, 225), new Vector3(0, 180, 0), 100,
                Vector3.Zero, gate));
        }

        private void CreatePedistals()
        {
            environment.Add(new Pedistal(contentMan, new Vector3(2200, 0, 4700), Vector3.Zero, 75, "GemRed"));
            environment.Add(new Pedistal(contentMan, new Vector3(2500, 0, 4700), Vector3.Zero, 75, "GemBlue", true));
            environment.Add(new Pedistal(contentMan, new Vector3(2800, 0, 4700), Vector3.Zero, 75, "GemYellow"));
        }
    
        public override void OnEnteringArea()
        {
            base.OnEnteringArea();
            Game.SoundManager.PlaySound("space", 0.7f, null, -1);
        }
    }
}
