using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace LabyrinthExplorer
{
    class NormalMappedWall
    {
        private VertexBuffer vertexBuffer;
        private NormalMappedVertex[] vertices;

        public NormalMappedWall(GraphicsDevice graphicsDevice,
            Vector3 startPos, Vector3 endPos, Vector3 innVec, float height = GameConstants.WALL_HEIGHT)
        {
             GenerateWall(startPos, endPos, innVec, height, graphicsDevice);
        }

        public NormalMappedWall(GraphicsDevice graphicsDevice, Vector3 corner1, Vector3 corner2,
                    Vector3 corner3, Vector3 corner4, Vector3 normal)
        {
            Vector3[] wallCorners = new Vector3[4];
            wallCorners[0] = corner1;//near left
            wallCorners[1] = corner2;//near  right
            wallCorners[2] = corner3;//far right
            wallCorners[3] = corner4;//far left
            float size = Vector3.Distance(corner1, corner3);
            size /= 100;
            Vector2[] wallTexCoords =
            {
                new Vector2(0.0f, 0.0f),                            // top left corner
                new Vector2(GameConstants.FLOOR_TILE_FACTOR_NORMAL*size, 0.0f),               // top right corner
                new Vector2(GameConstants.FLOOR_TILE_FACTOR_NORMAL*size, GameConstants.FLOOR_TILE_FACTOR_NORMAL*size),  // bottom right corner
                new Vector2(0.0f, GameConstants.FLOOR_TILE_FACTOR_NORMAL*size)                // bottom left corner
            };

            int offset = 0;
            vertices = new NormalMappedVertex[6];
            vertices[offset++] = new NormalMappedVertex(wallCorners[3], wallTexCoords[0], Vector3.Up, Vector4.Zero);//btm left
            vertices[offset++] = new NormalMappedVertex(wallCorners[2], wallTexCoords[1], Vector3.Up, Vector4.Zero);//btm rigth
            vertices[offset++] = new NormalMappedVertex(wallCorners[1], wallTexCoords[2], Vector3.Up, Vector4.Zero);//top right
            vertices[offset++] = new NormalMappedVertex(wallCorners[1], wallTexCoords[2], Vector3.Up, Vector4.Zero);//top right
            vertices[offset++] = new NormalMappedVertex(wallCorners[0], wallTexCoords[3], Vector3.Up, Vector4.Zero);//top left
            vertices[offset++] = new NormalMappedVertex(wallCorners[3], wallTexCoords[0], Vector3.Up, Vector4.Zero);//btm left

            CalcTangent(graphicsDevice);
        }
        /// <summary>
        /// Generates a wall from the starting position, using the innVector
        /// as the normal of the surface and doing the wall perpendicularly 
        /// towards the length
        /// </summary>
        /// <param name="startPos"></param>
        /// <param name="innVec">The normal of the wall. If the wall should point up positive
        /// z axis, 90 degrees on the x axis, the vector would be (0, 0, 1)</param>
        /// <param name="lenght"></param>
        private void GenerateWall(Vector3 startPos, Vector3 endPos, Vector3 innVec, 
            float height, GraphicsDevice graphicsDevice)
        {
           
            float length = Vector3.Distance(startPos, endPos);
            length /= 100;
            Vector2[] wallTexCoords =
            {
                new Vector2(0.0f, 0.0f),                                        // top left corner
                new Vector2(GameConstants.WallTileFactorNormalX*length, 0.0f),         // top right corner
                new Vector2(GameConstants.WallTileFactorNormalX*length, GameConstants.WallTileFactorNormalY),// bottom right corner    
                new Vector2(0.0f, GameConstants.WallTileFactorNormalY)          // bottom left corner
            };

            int offset = 0;
            vertices = new NormalMappedVertex[6];

            Vector3[] wallCorners = new Vector3[4];
            wallCorners[0] = startPos; //Starting point, bottom left corner when facing wall
            wallCorners[1] = endPos; //Bottom right corner when facing wall
            wallCorners[2] = new Vector3(endPos.X, height, endPos.Z);//top right
            wallCorners[3] = new Vector3(startPos.X, height, startPos.Z);//top left
            vertices[offset++] = new NormalMappedVertex(wallCorners[0], wallTexCoords[0], innVec, Vector4.Zero);//btm left
            vertices[offset++] = new NormalMappedVertex(wallCorners[1], wallTexCoords[1], innVec, Vector4.Zero);//btm rigth
            vertices[offset++] = new NormalMappedVertex(wallCorners[2], wallTexCoords[2], innVec, Vector4.Zero);//top right
            vertices[offset++] = new NormalMappedVertex(wallCorners[2], wallTexCoords[2], innVec, Vector4.Zero);//top right
            vertices[offset++] = new NormalMappedVertex(wallCorners[3], wallTexCoords[3], innVec, Vector4.Zero);//top left
            vertices[offset++] = new NormalMappedVertex(wallCorners[0], wallTexCoords[0], innVec, Vector4.Zero);//btm left

            CalcTangent(graphicsDevice);
        }

        
        private void CalcTangent(GraphicsDevice graphicsDevice)
        {
            Vector4 tangent;
            for (int i = 0; i < vertices.Length; i += 3)
            {
                NormalMappedVertex.CalcTangent(
                    ref vertices[i].Position,
                    ref vertices[i + 1].Position,
                    ref vertices[i + 2].Position,
                    ref vertices[i].TexCoord,
                    ref vertices[i + 1].TexCoord,
                    ref vertices[i + 2].TexCoord,
                    ref vertices[i].Normal,
                    out tangent);

                vertices[i].Tangent = tangent;
                vertices[i + 1].Tangent = tangent;
                vertices[i + 2].Tangent = tangent;
            }

            vertexBuffer = new VertexBuffer(graphicsDevice, typeof(NormalMappedVertex),
                vertices.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData(vertices);
        }

        //Returns a directional vector based on the inn vector given to the class. If inn vector
        //is negative z (0, 0, -1), the wall direction is (1, 0, 0)
        private Vector3 FindWallDirection(Vector3 innVec)
        {
            if (innVec.X != 0)
            {
                if (innVec.X < 0)
                    return new Vector3(0, 0, -1);
                else
                    return new Vector3(0, 0, 1);
            }
            else if (innVec.Z != 0)
            {
                if (innVec.Z < 0)
                    return new Vector3(1, 0, 0);
                else
                    return new Vector3(-1, 0, 0);
            }
            else
                Debug.WriteLine("X and Z are both 0, Y not supported by normalMappedWall class");
            return new Vector3(0, 1, 0);
        }



        public void Draw(GraphicsDevice graphicsDevice, Effect effect,
                         string colorMapParamName, string normalMapParamName,
                         Texture2D wallColorMap, Texture2D wallNormalMap)
        {
            // Draw the scene geometry.
            graphicsDevice.SetVertexBuffer(vertexBuffer);

            // Draw the walls.
            effect.Parameters[colorMapParamName].SetValue(wallColorMap);
            effect.Parameters[normalMapParamName].SetValue(wallNormalMap);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);
            }
        }

        public void Draw(GraphicsDevice graphicsDevice, Effect effect,
                         string colorMapParamName, string normalMapParamName,
                         string heightMapParamName, Texture2D wallColorMap,
                         Texture2D wallNormalMap, Texture2D wallHeightMap)
        {
            // Draw the scene geometry. 
            graphicsDevice.SetVertexBuffer(vertexBuffer);

            // Draw the walls.               
            effect.Parameters[colorMapParamName].SetValue(wallColorMap);
            effect.Parameters[normalMapParamName].SetValue(wallNormalMap);
            effect.Parameters[heightMapParamName].SetValue(wallHeightMap);
          

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);
            }
        }
    }
}
