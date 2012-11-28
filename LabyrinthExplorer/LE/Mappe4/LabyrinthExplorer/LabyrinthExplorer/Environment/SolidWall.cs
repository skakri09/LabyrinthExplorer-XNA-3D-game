using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LabyrinthExplorer
{
    public class SolidWall
    {
        NormalMappedWall[] sides;

        private AABB aabb;

        public SolidWall(GraphicsDevice graphicsDevice, 
            Vector3 btmFrontLeft, Vector3 btmFrontRight,
            Vector3 btmBackRight, Vector3 btmBackLeft,
            float height = GameConstants.WALL_HEIGHT )
        {
            sides = new NormalMappedWall[4];
            //+Z wall
            sides[0] = new NormalMappedWall(graphicsDevice, btmFrontRight * GameConstants.MAP_SCALE, btmFrontLeft * GameConstants.MAP_SCALE, Vector3.Backward, height * GameConstants.MAP_SCALE);

            //-Z wall
            sides[1] = new NormalMappedWall(graphicsDevice, btmBackLeft * GameConstants.MAP_SCALE, btmBackRight * GameConstants.MAP_SCALE, Vector3.Forward, height * GameConstants.MAP_SCALE);

            //-X wall
            sides[2] = new NormalMappedWall(graphicsDevice, btmFrontLeft * GameConstants.MAP_SCALE, btmBackLeft * GameConstants.MAP_SCALE, Vector3.Left, height * GameConstants.MAP_SCALE);

            //+X wall
            sides[3] = new NormalMappedWall(graphicsDevice, btmBackRight * GameConstants.MAP_SCALE, btmFrontRight * GameConstants.MAP_SCALE, Vector3.Right, height * GameConstants.MAP_SCALE);

            aabb = new AABB(btmFrontLeft * GameConstants.MAP_SCALE, btmFrontRight * GameConstants.MAP_SCALE, btmBackRight * GameConstants.MAP_SCALE, btmBackLeft * GameConstants.MAP_SCALE, height * GameConstants.MAP_SCALE);
        }

        //Allows creating wall in 2d, as Y is 0 anyway. Using the 2d Y as Z
        public SolidWall(GraphicsDevice graphicsDevice,
                            Vector2 btmFrontLeftNoY, Vector2 btmFrontRightNoY,
                            Vector2 btmBackRightNoY, Vector2 btmBackLeftNoY,
                            float height = GameConstants.WALL_HEIGHT)
        {
            Vector3 btmFrontLeft = new Vector3(btmFrontLeftNoY.X, 0, btmFrontLeftNoY.Y);
            Vector3 btmFrontRight = new Vector3(btmFrontRightNoY.X, 0, btmFrontRightNoY.Y);
            Vector3 btmBackRight = new Vector3(btmBackRightNoY.X, 0, btmBackRightNoY.Y);
            Vector3 btmBackLeft = new Vector3(btmBackLeftNoY.X, 0, btmBackLeftNoY.Y);
            

            sides = new NormalMappedWall[5];
            //+Z wall
            sides[0] = new NormalMappedWall(graphicsDevice, btmFrontRight * GameConstants.MAP_SCALE, btmFrontLeft * GameConstants.MAP_SCALE, Vector3.Backward, height * GameConstants.MAP_SCALE);

            //-Z wall
            sides[1] = new NormalMappedWall(graphicsDevice, btmBackLeft * GameConstants.MAP_SCALE, btmBackRight * GameConstants.MAP_SCALE, Vector3.Forward, height * GameConstants.MAP_SCALE);

            //-X wall
            sides[2] = new NormalMappedWall(graphicsDevice, btmFrontLeft * GameConstants.MAP_SCALE, btmBackLeft * GameConstants.MAP_SCALE, Vector3.Left, height * GameConstants.MAP_SCALE);

            //+X wall
            sides[3] = new NormalMappedWall(graphicsDevice, btmBackRight * GameConstants.MAP_SCALE, btmFrontRight * GameConstants.MAP_SCALE, Vector3.Right, height * GameConstants.MAP_SCALE);

            Vector3 topFrontLeft = new Vector3(btmFrontLeftNoY.X, GameConstants.WALL_HEIGHT, btmFrontLeftNoY.Y);
            Vector3 topFrontRight = new Vector3(btmFrontRightNoY.X, GameConstants.WALL_HEIGHT, btmFrontRightNoY.Y);
            Vector3 topBackRight = new Vector3(btmBackRightNoY.X, GameConstants.WALL_HEIGHT, btmBackRightNoY.Y);
            Vector3 topBackLeft = new Vector3(btmBackLeftNoY.X, GameConstants.WALL_HEIGHT, btmBackLeftNoY.Y);
            //+X wall
            sides[4] = new NormalMappedWall(graphicsDevice, topFrontLeft, topFrontRight, topBackRight, topBackLeft, Vector3.Up);

            aabb = new AABB(btmFrontLeft * GameConstants.MAP_SCALE, btmFrontRight * GameConstants.MAP_SCALE, btmBackRight * GameConstants.MAP_SCALE, btmBackLeft * GameConstants.MAP_SCALE, height * GameConstants.MAP_SCALE);
        }

        public void Draw(GraphicsDevice graphicsDevice, Effect effect,
                         string colorMapParamName, string normalMapParamName,
                         string heightMapParamName, Texture2D wallColorMap,
                         Texture2D wallNormalMap, Texture2D wallHeightMap)
        {
            foreach (NormalMappedWall wall in sides)
            {
                wall.Draw(graphicsDevice, effect, colorMapParamName, 
                    normalMapParamName, wallColorMap, wallNormalMap);
            }
        }

        public AABB Aabb
        {
            get { return aabb; }
        }
    }
}   
