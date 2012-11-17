#region Copyright
//-----------------------------------------------------------------------------
// Copyright (c) 2007-2011 dhpoware. All Rights Reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
// OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
//-----------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LabyrinthExplorer
{
    /// <summary>
    /// The NormalMappedRoom class is used to procedurally generate a room.
    /// A room is a cube with all face normals pointed inwards.
    /// This class generates geometry using the NormalMappedVertex structure.
    /// </summary>
    public class NormalMappedRoom
    {
        private VertexBuffer vertexBuffer;
        private NormalMappedVertex[] vertices;

        private int floorIndex;
        private int ceilingIndex;
        private int wallsIndex;

        public NormalMappedRoom(GraphicsDevice graphicsDevice,
                                float size,
                                float height,
                                float floorTileFactor,
                                float ceilingTileFactor,
                                float wallTileFactorX,
                                float wallTileFactorY)
        {
            GenerateRoom(graphicsDevice, size, height, floorTileFactor, ceilingTileFactor, wallTileFactorX, wallTileFactorY);
        }

        public void Draw(GraphicsDevice graphicsDevice,
                         Effect effect,
                         string colorMapParamName,
                         string normalMapParamName,
                         Texture2D wallColorMap,
                         Texture2D wallNormalMap,
                         Texture2D floorColorMap,
                         Texture2D floorNormalMap,
                         Texture2D ceilingColorMap,
                         Texture2D ceilingNormalMap)
        {
            // Draw the scene geometry.
            graphicsDevice.SetVertexBuffer(vertexBuffer);

            // Draw the walls.

            effect.Parameters[colorMapParamName].SetValue(wallColorMap);
            effect.Parameters[normalMapParamName].SetValue(wallNormalMap);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, wallsIndex, 8);
            }

            // Draw the ceiling.

            effect.Parameters[colorMapParamName].SetValue(ceilingColorMap);
            effect.Parameters[normalMapParamName].SetValue(ceilingNormalMap);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, ceilingIndex, 2);
            }

            // Draw the floor.

            effect.Parameters[colorMapParamName].SetValue(floorColorMap);
            effect.Parameters[normalMapParamName].SetValue(floorNormalMap);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, floorIndex, 2);
            }
        }

        public void Draw(GraphicsDevice graphicsDevice,
                         Effect effect,
                         string colorMapParamName,
                         string normalMapParamName,
                         string heightMapParamName,
                         Texture2D wallColorMap,
                         Texture2D wallNormalMap,
                         Texture2D wallHeightMap,
                         Texture2D floorColorMap,
                         Texture2D floorNormalMap,
                         Texture2D floorHeightMap,
                         Texture2D ceilingColorMap,
                         Texture2D ceilingNormalMap,
                         Texture2D ceilingHeightMap)
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
                graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, wallsIndex, 8);
            }

            // Draw the ceiling.

            effect.Parameters[colorMapParamName].SetValue(ceilingColorMap);
            effect.Parameters[normalMapParamName].SetValue(ceilingNormalMap);
            effect.Parameters[heightMapParamName].SetValue(ceilingHeightMap);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, ceilingIndex, 2);
            }

            // Draw the floor.

            effect.Parameters[colorMapParamName].SetValue(floorColorMap);
            effect.Parameters[normalMapParamName].SetValue(floorNormalMap);
            effect.Parameters[heightMapParamName].SetValue(floorHeightMap);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, floorIndex, 2);
            }
        }

        private void GenerateRoomGeometry(float floorSize,
                                          float wallHeight,
                                          float floorTileFactor,
                                          float ceilingTileFactor,
                                          float wallTileFactorX,
                                          float wallTileFactorY)
        {
#region wrapyourbrainarroundthis
            /* 5--------6
             * |\      /|
             * | 4----7 |
             * | |    | |
             * | |    | |
             * | 0----3 |
             * |/      \|
             * 1--------2
             *
             * +z wall: 6512 tri1: 651 tri2: 126
             * -z wall: 4730 tri1: 473 tri2: 304
             * +x wall: 7623 tri1: 762 tri2: 237
             * -x wall: 5401 tri1: 540 tri2: 015
             * 
             * +y ceiling: 5674 tri1: 567 tri2: 745
             * -y floor:   0321 tri1: 032 tri2: 210
             */
#endregion

            int offset = 0;
            float halfSize = floorSize * 0.5f;

            Vector3[] corners =
            {
                new Vector3(-halfSize, 0.0f, -halfSize),            // 0
                new Vector3(-halfSize, 0.0f,  halfSize),            // 1
                new Vector3( halfSize, 0.0f,  halfSize),            // 2
                new Vector3( halfSize, 0.0f, -halfSize),            // 3

                new Vector3(-halfSize, wallHeight, -halfSize),      // 4
                new Vector3(-halfSize, wallHeight,  halfSize),      // 5
                new Vector3( halfSize, wallHeight,  halfSize),      // 6
                new Vector3( halfSize, wallHeight, -halfSize),      // 7
            };

            Vector2[] wallTexCoords =
            {
                new Vector2(0.0f, 0.0f),                            // top left corner
                new Vector2(wallTileFactorX, 0.0f),                 // top right corner
                new Vector2(wallTileFactorX, wallTileFactorY),      // bottom right corner    
                new Vector2(0.0f, wallTileFactorY)                  // bottom left corner
            };

            Vector2[] floorTexCoords =
            {
                new Vector2(0.0f, 0.0f),                            // top left corner
                new Vector2(floorTileFactor, 0.0f),                 // top right corner
                new Vector2(floorTileFactor, floorTileFactor),      // bottom right corner    
                new Vector2(0.0f, floorTileFactor)                  // bottom left corner
            };

            Vector2[] ceilingTexCoords =
            {
                new Vector2(0.0f, 0.0f),                            // top left corner
                new Vector2(ceilingTileFactor, 0.0f),               // top right corner
                new Vector2(ceilingTileFactor, ceilingTileFactor),  // bottom right corner
                new Vector2(0.0f, ceilingTileFactor)                // bottom left corner
            };

            wallsIndex = offset;

            // -z wall: 4730 tri1: 473 tri2: 304
            vertices[offset++] = new NormalMappedVertex(corners[4], wallTexCoords[0], Vector3.Backward, Vector4.Zero);
            vertices[offset++] = new NormalMappedVertex(corners[7], wallTexCoords[1], Vector3.Backward, Vector4.Zero);
            vertices[offset++] = new NormalMappedVertex(corners[3], wallTexCoords[2], Vector3.Backward, Vector4.Zero);
            vertices[offset++] = new NormalMappedVertex(corners[3], wallTexCoords[2], Vector3.Backward, Vector4.Zero);
            vertices[offset++] = new NormalMappedVertex(corners[0], wallTexCoords[3], Vector3.Backward, Vector4.Zero);
            vertices[offset++] = new NormalMappedVertex(corners[4], wallTexCoords[0], Vector3.Backward, Vector4.Zero);

            // +z wall: 6512 tri1: 651 tri2: 126
            vertices[offset++] = new NormalMappedVertex(corners[6], wallTexCoords[0], Vector3.Forward, Vector4.Zero);
            vertices[offset++] = new NormalMappedVertex(corners[5], wallTexCoords[1], Vector3.Forward, Vector4.Zero);
            vertices[offset++] = new NormalMappedVertex(corners[1], wallTexCoords[2], Vector3.Forward, Vector4.Zero);
            vertices[offset++] = new NormalMappedVertex(corners[1], wallTexCoords[2], Vector3.Forward, Vector4.Zero);
            vertices[offset++] = new NormalMappedVertex(corners[2], wallTexCoords[3], Vector3.Forward, Vector4.Zero);
            vertices[offset++] = new NormalMappedVertex(corners[6], wallTexCoords[0], Vector3.Forward, Vector4.Zero);

            // -x wall: 5401 tri1: 540 tri2: 015
            vertices[offset++] = new NormalMappedVertex(corners[5], wallTexCoords[0], Vector3.Right, Vector4.Zero);
            vertices[offset++] = new NormalMappedVertex(corners[4], wallTexCoords[1], Vector3.Right, Vector4.Zero);
            vertices[offset++] = new NormalMappedVertex(corners[0], wallTexCoords[2], Vector3.Right, Vector4.Zero);
            vertices[offset++] = new NormalMappedVertex(corners[0], wallTexCoords[2], Vector3.Right, Vector4.Zero);
            vertices[offset++] = new NormalMappedVertex(corners[1], wallTexCoords[3], Vector3.Right, Vector4.Zero);
            vertices[offset++] = new NormalMappedVertex(corners[5], wallTexCoords[0], Vector3.Right, Vector4.Zero);

            // +x wall: 7623 tri1: 762 tri2: 237
            vertices[offset++] = new NormalMappedVertex(corners[7], wallTexCoords[0], Vector3.Left, Vector4.Zero);
            vertices[offset++] = new NormalMappedVertex(corners[6], wallTexCoords[1], Vector3.Left, Vector4.Zero);
            vertices[offset++] = new NormalMappedVertex(corners[2], wallTexCoords[2], Vector3.Left, Vector4.Zero);
            vertices[offset++] = new NormalMappedVertex(corners[2], wallTexCoords[2], Vector3.Left, Vector4.Zero);
            vertices[offset++] = new NormalMappedVertex(corners[3], wallTexCoords[3], Vector3.Left, Vector4.Zero);
            vertices[offset++] = new NormalMappedVertex(corners[7], wallTexCoords[0], Vector3.Left, Vector4.Zero);

            // +y ceiling: 5674 tri1: 567 tri2: 745
            ceilingIndex = offset;
            vertices[offset++] = new NormalMappedVertex(corners[5], ceilingTexCoords[0], Vector3.Down, Vector4.Zero);
            vertices[offset++] = new NormalMappedVertex(corners[6], ceilingTexCoords[1], Vector3.Down, Vector4.Zero);
            vertices[offset++] = new NormalMappedVertex(corners[7], ceilingTexCoords[2], Vector3.Down, Vector4.Zero);
            vertices[offset++] = new NormalMappedVertex(corners[7], ceilingTexCoords[2], Vector3.Down, Vector4.Zero);
            vertices[offset++] = new NormalMappedVertex(corners[4], ceilingTexCoords[3], Vector3.Down, Vector4.Zero);
            vertices[offset++] = new NormalMappedVertex(corners[5], ceilingTexCoords[0], Vector3.Down, Vector4.Zero);

            // -y floor: 0321 tri1: 032 tri2: 210
            floorIndex = offset;
            vertices[offset++] = new NormalMappedVertex(corners[0], floorTexCoords[0], Vector3.Up, Vector4.Zero);
            vertices[offset++] = new NormalMappedVertex(corners[3], floorTexCoords[1], Vector3.Up, Vector4.Zero);
            vertices[offset++] = new NormalMappedVertex(corners[2], floorTexCoords[2], Vector3.Up, Vector4.Zero);
            vertices[offset++] = new NormalMappedVertex(corners[2], floorTexCoords[2], Vector3.Up, Vector4.Zero);
            vertices[offset++] = new NormalMappedVertex(corners[1], floorTexCoords[3], Vector3.Up, Vector4.Zero);
            vertices[offset++] = new NormalMappedVertex(corners[0], floorTexCoords[0], Vector3.Up, Vector4.Zero);
        }

        private void GenerateRoom(GraphicsDevice graphicsDevice,
                                  float floorSize,
                                  float wallHeight,
                                  float floorTileFactor,
                                  float ceilingTileFactor,
                                  float wallTileFactorX,
                                  float wallTileFactorY)
        {
            // The room is a cube with all surface normal pointing inwards.
            // We need a total of 36 vertices in the vertex buffer.
            // 6 faces to the room.
            // Each face is made of 2 triangles requiring 6 vertices.

            vertices = new NormalMappedVertex[36];

            GenerateRoomGeometry(floorSize, wallHeight, floorTileFactor,
                ceilingTileFactor, wallTileFactorX, wallTileFactorY);

            // Calculate the tangent vectors for each triangle in the room.

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

            // Create and fill the vertex buffer with the room geometry.

            vertexBuffer = new VertexBuffer(graphicsDevice, typeof(NormalMappedVertex), vertices.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData(vertices);
        }
    }
}
