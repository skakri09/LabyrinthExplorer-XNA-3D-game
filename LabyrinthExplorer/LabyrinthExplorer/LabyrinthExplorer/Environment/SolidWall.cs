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
            sides[0] = new NormalMappedWall(graphicsDevice, btmFrontRight, btmFrontLeft, Vector3.Backward, height);

            //-Z wall
            sides[1] = new NormalMappedWall(graphicsDevice, btmBackLeft, btmBackRight, Vector3.Forward, height);

            //-X wall
            sides[2] = new NormalMappedWall(graphicsDevice, btmFrontLeft, btmBackLeft, Vector3.Left, height);

            //+X wall
            sides[3] = new NormalMappedWall(graphicsDevice, btmBackRight, btmFrontRight, Vector3.Right, height);

            aabb = new AABB(btmFrontLeft, btmFrontRight, btmBackRight, btmBackLeft, height);
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

            sides = new NormalMappedWall[4];
            //+Z wall
            sides[0] = new NormalMappedWall(graphicsDevice, btmFrontRight, btmFrontLeft, Vector3.Backward, height);

            //-Z wall
            sides[1] = new NormalMappedWall(graphicsDevice, btmBackLeft, btmBackRight, Vector3.Forward, height);

            //-X wall
            sides[2] = new NormalMappedWall(graphicsDevice, btmFrontLeft, btmBackLeft, Vector3.Left, height);

            //+X wall
            sides[3] = new NormalMappedWall(graphicsDevice, btmBackRight, btmFrontRight, Vector3.Right, height);

            aabb = new AABB(btmFrontLeft, btmFrontRight, btmBackRight, btmBackLeft, height);
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
