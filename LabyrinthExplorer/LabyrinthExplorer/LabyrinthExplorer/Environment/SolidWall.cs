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

        AABB aabb;

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
    }
}
