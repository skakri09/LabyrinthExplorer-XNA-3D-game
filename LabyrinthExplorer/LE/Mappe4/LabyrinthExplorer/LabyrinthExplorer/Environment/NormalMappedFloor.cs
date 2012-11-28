using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace LabyrinthExplorer
{
    public class NormalMappedFloor
    {
        private VertexBuffer vertexBuffer;
        private NormalMappedVertex[] vertices;

        /// <summary>
        /// Generates a floor from the 4 corners. They should be in the following format:
        /// corner1: pos x, neg z. Corner2: neg x, pos z. Corner 3: pos z, neg x. corner 4: neg z and x
        /// </summary>
        public NormalMappedFloor(GraphicsDevice graphicsDevice, Vector3 corner1, Vector3 corner2,
                    Vector3 corner3, Vector3 corner4, Vector3 normal)
        {
            GenerateFloor(graphicsDevice, corner1 * GameConstants.MAP_SCALE, corner2 * GameConstants.MAP_SCALE, corner3 * GameConstants.MAP_SCALE, corner4 * GameConstants.MAP_SCALE, normal);
        }

        /// <summary>
        /// Generates a floor from the 4 given corners
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="corner1"></param>
        /// <param name="corner2"></param>
        /// <param name="corner3"></param>
        /// <param name="corner4"></param>
        /// <param name="normal"></param>
        private void GenerateFloor(GraphicsDevice graphicsDevice, Vector3 corner1, Vector3 corner2,
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
                vertices.Length,BufferUsage.WriteOnly);
            vertexBuffer.SetData(vertices);
        }

        public void Draw(GraphicsDevice graphicsDevice, Effect effect,
                         string colorMapParamName, string normalMapParamName,
                         Texture2D ColorMap, Texture2D NormalMap)
        {
            graphicsDevice.SetVertexBuffer(vertexBuffer);

            effect.Parameters[colorMapParamName].SetValue(ColorMap);
            effect.Parameters[normalMapParamName].SetValue(NormalMap);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);
            }
        }

        public void Draw(GraphicsDevice graphicsDevice, Effect effect,
                         string colorMapParamName, string normalMapParamName,
                         string heightMapParamName, Texture2D ColorMap,
                         Texture2D NormalMap, Texture2D HeightMap)
        {
            graphicsDevice.SetVertexBuffer(vertexBuffer);
           
            effect.Parameters[colorMapParamName].SetValue(ColorMap);
            effect.Parameters[normalMapParamName].SetValue(NormalMap);
            effect.Parameters[heightMapParamName].SetValue(HeightMap);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);
            }
        }
    }
}
