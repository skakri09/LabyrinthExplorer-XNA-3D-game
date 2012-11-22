using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace LabyrinthExplorer
{
    public class AreaContent : IGameArea
    {
        protected List<SolidWall> walls;
        protected List<NormalMappedCeiling> ceilings;
        protected List<NormalMappedFloor> floors;
        protected List<Enemy> enemies;
        protected List<IEnvironmentObject> environment;
        protected List<AABB> environmentCollidables;

        protected ContentManager contentMan;
        protected GraphicsDevice device;

        protected Camera camera;

        public AreaContent(Camera camera)
        {

        }

        public virtual void LoadContent(GraphicsDevice device, ContentManager contentMan)
        {
            this.device = device;
            this.contentMan = contentMan;
        }

        public virtual void Update(GameTime gameTime, Camera camera)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            foreach (Enemy enemy in enemies)
            {
                enemy.Update(deltaTime);
            }
            foreach (IEnvironmentObject obj in environment)
            {
                obj.Update(deltaTime);
            }
        }

        public virtual void Draw(GraphicsDevice graphicsDevice, Effect effect,
        Texture2D brickColorMap,Texture2D brickNormalMap,Texture2D brickHeightMap,
        Texture2D stoneColorMap,Texture2D stoneNormalMap,Texture2D stoneHeightMap,
        Texture2D woodColorMap,Texture2D woodNormalMap,Texture2D woodHeightMap)
        {
            //           ------ Available assets ------                     
            //  "colorMapTexture", "normalMapTexture", "heightMapTexture",
            //  brickColorMap,      brickNormalMap,     brickHeightMap,
            //  stoneColorMap,      stoneNormalMap,     stoneHeightMap,
            //  woodColorMap,       woodNormalMap,      woodHeightMap
            foreach (IEnvironmentObject obj in environment)
            {
                obj.Draw(camera, effect);
            }
            foreach (Enemy enemy in enemies)
            {
                enemy.Draw(camera);
            }

            foreach (SolidWall wall in walls)
            {
                wall.Draw(graphicsDevice, effect, "colorMapTexture",
                        "normalMapTexture", "heightMapTexture",
                        stoneColorMap, stoneNormalMap, stoneHeightMap);
            }
            //foreach (NormalMappedCeiling ceiling in ceilings)
            //{
            //    ceiling.Draw(graphicsDevice, effect, "colorMapTexture",
            //            "normalMapTexture", "heightMapTexture",
            //            stoneColorMap, stoneNormalMap, stoneHeightMap);
            //}
            foreach (NormalMappedFloor floor in floors)
            {
                floor.Draw(graphicsDevice, effect, "colorMapTexture",
                            "normalMapTexture", "heightMapTexture",
                            stoneColorMap, stoneNormalMap, stoneHeightMap);
            }
        }

        public void RemoveEnvironmentItem(IEnvironmentObject item)
        {
            environment.Remove(item);
        }

        public List<AABB> EnvironmentCollidables()
        {
            return environmentCollidables;
        }

       

        #region Ease of creation functions

        protected void CreateDuoLever(Vector3 pos1, Vector3 rot1, float scale1, Vector3 openDir1,
                                    Vector3 pos2, Vector3 rot2, float scale2, Vector3 openDir2,
                                    IInteractableObject onUseObject)
        {
            Lever lever1 = new Lever(contentMan, pos1, rot1, scale1, openDir1);
            Lever lever2 = new Lever(contentMan, pos2, rot2, scale2, openDir2);
            environment.Add(new DuoLever(lever1, lever2, onUseObject));
        }

        protected Gate CreateGate(Vector3 position, Vector3 rotation,
                                float scale, float closeAfter = 0, bool isClosed = true)
        {
            Gate gate = new Gate(contentMan, position, rotation, scale, closeAfter, isClosed);
            environment.Add(gate);
            environmentCollidables.Add(gate);
            return gate;
        }

        protected void SmarPosWall(float x1, float y1, float x2, float y2)
        {
            if (x1 == x2)
            {
                
                if (y1 > y2)
                {
                    ZPosWall(x1, y1, x2, y2);
                }
                else
                {
                    ZPosWall(x2, y2, x1, y1);
                }
            }
            else if (y1 == y2)
            {
                 if (x1 > x2)
                 {
                     XPosWal(x2, y2, x1, y1);
                 }
                 else
                 {
                     XPosWal(x1, y1, x2, y2);
                 }
            }
            else
            {
                throw new Exception("Both X and Y vals are different from eachother");
            }
        }

        protected void ZPosWall(float startX, float startZ, float endX, float endZ, float width = 50)
        {
            ZPosWall(new Vector2(startX, startZ), new Vector2(endX, endZ), width);
        }
        protected void ZPosWall(Vector2 startPos, Vector2 endPos, float width = 50)
        {
            ZWallPosX newWall = new ZWallPosX(device, startPos, endPos, width);
            walls.Add(newWall);
            environmentCollidables.Add(newWall.Aabb);
        }
        protected void XPosWal(float startX, float startZ, float endX, float endZ, float width = 50)
        {
            XPosWal(new Vector2(startX, startZ), new Vector2(endX, endZ), width);
        }
        protected void XPosWal(Vector2 startPos, Vector2 endPos, float width = 50)
        {
            XWallPosZ newWall = new XWallPosZ(device, startPos, endPos, width);
            walls.Add(newWall);
            environmentCollidables.Add(newWall.Aabb);
        }

        protected void CreateFloor(Vector3 frontLeft, Vector3 frontRight,
                                 Vector3 backRight, Vector3 backLeft)
        {
            floors.Add(new NormalMappedFloor(device,
            frontLeft, frontRight, backRight, backLeft,
            Vector3.Up));
        }

        #endregion
    }
}
