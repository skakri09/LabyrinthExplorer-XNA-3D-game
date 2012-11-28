using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LabyrinthExplorer
{
    /// <summary>
    /// Custom vertex structure used for normal mapping.
    /// </summary>
    public struct NormalMappedVertex : IVertexType
    {
        public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration
        (
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(sizeof(float) * 3, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
            new VertexElement(sizeof(float) * 5, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
            new VertexElement(sizeof(float) * 8, VertexElementFormat.Vector4, VertexElementUsage.Tangent, 0)
        );

        public Vector3 Position;
        public Vector2 TexCoord;
        public Vector3 Normal;
        public Vector4 Tangent;

        public NormalMappedVertex(Vector3 position, Vector2 texCoord, Vector3 normal, Vector4 tangent)
        {
            Position = position;
            TexCoord = texCoord;
            Normal = normal;
            Tangent = tangent;
        }

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return VertexDeclaration; }
        }

        /// <summary>
        /// Given the 3 vertices (position and texture coordinate) and the
        /// face normal of a triangle calculate and return the triangle's
        /// tangent vector. This method is designed to work with XNA's default
        /// right handed coordinate system and clockwise triangle winding order.
        /// Undefined behavior will result if any other coordinate system
        /// and/or winding order is used. The handedness of the local tangent
        /// space coordinate system is stored in the tangent's w component.
        /// </summary>
        /// <param name="pos1">Triangle vertex 1 position</param>
        /// <param name="pos2">Triangle vertex 2 position</param>
        /// <param name="pos3">Triangle vertex 3 position</param>
        /// <param name="texCoord1">Triangle vertex 1 texture coordinate</param>
        /// <param name="texCoord2">Triangle vertex 2 texture coordinate</param>
        /// <param name="texCoord3">Triangle vertex 3 texture coordinate</param>
        /// <param name="normal">Triangle face normal</param>
        /// <param name="tangent">Calculated tangent vector</param>
        public static void CalcTangent(ref Vector3 pos1,
                                       ref Vector3 pos2,
                                       ref Vector3 pos3,
                                       ref Vector2 texCoord1,
                                       ref Vector2 texCoord2,
                                       ref Vector2 texCoord3,
                                       ref Vector3 normal,
                                       out Vector4 tangent)
        {
            // Create 2 vectors in object space.
            // edge1 is the vector from vertex positions pos1 to pos3.
            // edge2 is the vector from vertex positions pos1 to pos2.
            Vector3 edge1 = pos3 - pos1;
            Vector3 edge2 = pos2 - pos1;

            edge1.Normalize();
            edge2.Normalize();

            // Create 2 vectors in tangent (texture) space that point in the
            // same direction as edge1 and edge2 (in object space).
            // texEdge1 is the vector from texture coordinates texCoord1 to texCoord3.
            // texEdge2 is the vector from texture coordinates texCoord1 to texCoord2.
            Vector2 texEdge1 = texCoord3 - texCoord1;
            Vector2 texEdge2 = texCoord2 - texCoord1;

            texEdge1.Normalize();
            texEdge2.Normalize();

            // These 2 sets of vectors form the following system of equations:
            //
            //  edge1 = (texEdge1.x * tangent) + (texEdge1.y * bitangent)
            //  edge2 = (texEdge2.x * tangent) + (texEdge2.y * bitangent)
            //
            // Using matrix notation this system looks like:
            //
            //  [ edge1 ]     [ texEdge1.x  texEdge1.y ]  [ tangent   ]
            //  [       ]  =  [                        ]  [           ]
            //  [ edge2 ]     [ texEdge2.x  texEdge2.y ]  [ bitangent ]
            //
            // The solution is:
            //
            //  [ tangent   ]        1     [ texEdge2.y  -texEdge1.y ]  [ edge1 ]
            //  [           ]  =  -------  [                         ]  [       ]
            //  [ bitangent ]      det A   [-texEdge2.x   texEdge1.x ]  [ edge2 ]
            //
            //  where:
            //        [ texEdge1.x  texEdge1.y ]
            //    A = [                        ]
            //        [ texEdge2.x  texEdge2.y ]
            //
            //    det A = (texEdge1.x * texEdge2.y) - (texEdge1.y * texEdge2.x)
            //
            // From this solution the tangent space basis vectors are:
            //
            //    tangent = (1 / det A) * ( texEdge2.y * edge1 - texEdge1.y * edge2)
            //  bitangent = (1 / det A) * (-texEdge2.x * edge1 + texEdge1.x * edge2)
            //     normal = cross(tangent, bitangent)

            Vector3 t;
            Vector3 b;
            float det = (texEdge1.X * texEdge2.Y) - (texEdge1.Y * texEdge2.X);

            if ((float)Math.Abs(det) < 1e-6f)    // almost equal to zero
            {
                t = Vector3.UnitX;
                b = Vector3.UnitY;
            }
            else
            {
                det = 1.0f / det;

                t.X = (texEdge2.Y * edge1.X - texEdge1.Y * edge2.X) * det;
                t.Y = (texEdge2.Y * edge1.Y - texEdge1.Y * edge2.Y) * det;
                t.Z = (texEdge2.Y * edge1.Z - texEdge1.Y * edge2.Z) * det;

                b.X = (-texEdge2.X * edge1.X + texEdge1.X * edge2.X) * det;
                b.Y = (-texEdge2.X * edge1.Y + texEdge1.X * edge2.Y) * det;
                b.Z = (-texEdge2.X * edge1.Z + texEdge1.X * edge2.Z) * det;

                t.Normalize();
                b.Normalize();
            }

            // Calculate the handedness of the local tangent space.
            // The bitangent vector is the cross product between the triangle face
            // normal vector and the calculated tangent vector. The resulting bitangent
            // vector should be the same as the bitangent vector calculated from the
            // set of linear equations above. If they point in different directions
            // then we need to invert the cross product calculated bitangent vector. We
            // store this scalar multiplier in the tangent vector's 'w' component so
            // that the correct bitangent vector can be generated in the normal mapping
            // shader's vertex shader.

            Vector3 bitangent = Vector3.Cross(normal, t);
            float handedness = (Vector3.Dot(bitangent, b) < 0.0f) ? -1.0f : 1.0f;

            tangent.X = t.X;
            tangent.Y = t.Y;
            tangent.Z = t.Z;
            tangent.W = handedness;
        }
    }
}
