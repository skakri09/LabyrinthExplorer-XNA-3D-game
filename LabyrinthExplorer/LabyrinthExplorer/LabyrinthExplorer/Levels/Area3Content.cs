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


            #region "wall" of text :)

            SmarPosWall(500, 0, 700, 0);
            SmarPosWall(450, 0, 450, 200);

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
   
        }

        public override void OnEnteringArea()
        {
            base.OnEnteringArea();
            Game.SoundManager.PlaySound("space", 0.7f, null, -1);
        }
    }
}
