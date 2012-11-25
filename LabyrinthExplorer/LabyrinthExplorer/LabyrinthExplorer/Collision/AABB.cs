using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LabyrinthExplorer
{
    public class AABB
    {
        Vector3 minPoint, orgMinPoint;
        Vector3 maxPoint, orgMaxPoint;

        public AABB(Vector3 btmFrontLeft, Vector3 btmFrontRight,
            Vector3 btmBackRight, Vector3 btmBackLeft, float height)
        {
            orgMinPoint = minPoint = btmBackLeft;
            orgMaxPoint = maxPoint = new Vector3(btmFrontRight.X, height, btmFrontRight.Z);
            //CollisionManager.AddCollidable(this);
        }

        public AABB(Vector3 position, float padding)
        {
            orgMinPoint = minPoint = new Vector3(position.X - padding, position.Y - padding, position.Z - padding);
            orgMaxPoint = maxPoint = new Vector3(position.X + padding, position.Y + padding, position.Z + padding);
            //CollisionManager.AddCollidable(this);
        }

        public AABB(Vector3 _minPoint, Vector3 _maxPoint)
        {
            SetAABB(_minPoint, _maxPoint);
        }

        public AABB(Vector2 _minPoint, Vector2 _maxPoint)
        {
            SetAABB(new Vector3(_minPoint.X, 0, _minPoint.Y), 
                new Vector3(_maxPoint.X, GameConstants.WALL_HEIGHT, _maxPoint.Y));
        }

        //Empty ctor for setting aabb manually, used by environment objects
        public AABB()
        {
        }

        public void CreateUseAABB(Vector3 useDirection, Vector3 position, float outDist, float sideDist)
        {
            Vector3 minPoint, maxPoint;

            if (useDirection == Vector3.Left)
            {
                minPoint = new Vector3(position.X - outDist, 0, position.Z - sideDist);
                maxPoint = new Vector3(position.X, GameConstants.InteractablesUseHeight, position.Z + sideDist);
            }
            else if (useDirection == Vector3.Right)
            {
                minPoint = new Vector3(position.X, 0, position.Z - sideDist);
                maxPoint = new Vector3(position.X + outDist, GameConstants.InteractablesUseHeight, position.Z + sideDist);
            }
            else if (useDirection == Vector3.Forward)
            {
                minPoint = new Vector3(position.X - sideDist, 0, position.Z);
                maxPoint = new Vector3(position.X + sideDist, GameConstants.InteractablesUseHeight, position.Z + outDist);
            }
            else if(useDirection == Vector3.Backward)
            {
                minPoint = new Vector3(position.X - sideDist, 0, position.Z - outDist);
                maxPoint = new Vector3(position.X + sideDist, GameConstants.InteractablesUseHeight, position.Z + 0);
            }
            else
            {
                minPoint = new Vector3(position.X - outDist, position.Y, position.Z-outDist);
                maxPoint = new Vector3(position.X + outDist, position.Y + GameConstants.InteractablesUseHeight, position.Z + outDist);
            }

            SetAABB(minPoint, maxPoint);
        }

        public void SetAABB(Vector3 _minPoint, Vector3 _maxPoint)
        {
            orgMinPoint = minPoint = _minPoint;
            orgMaxPoint = maxPoint = _maxPoint;
            //World.currentArea.EnvironmentCollidables().Add(this);
            //CollisionManager.AddCollidable(this);
        }

        public void UpdateAABB(Vector3 displacement)
        {
            minPoint = orgMinPoint;
            maxPoint = orgMaxPoint;

            minPoint += displacement;
            maxPoint += displacement;
        }

        public Vector3 CheckCollision(AABB otherAABB)
        {
            if (maxPoint.Z < otherAABB.MinPoint.Z) return Vector3.Zero;
            if (minPoint.Z > otherAABB.MaxPoint.Z) return Vector3.Zero;
            if (maxPoint.X < otherAABB.MinPoint.X) return Vector3.Zero;
            if (maxPoint.Y < otherAABB.MinPoint.Y) return Vector3.Zero;
            if (minPoint.X > otherAABB.MaxPoint.X) return Vector3.Zero;
            if (minPoint.Y > otherAABB.MaxPoint.Y) return Vector3.Zero;

            return minimumTranslation(otherAABB);
        }

        /// <summary>
        /// NOTE: ONLY CEHCKING COLLISION IN X AND Z AXIS! if at any point
        /// Y axis collision is needed, it must be edited inn.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Vector3 minimumTranslation(AABB other)
        {
            Vector2 amin = new Vector2(minPoint.X, minPoint.Z);
            Vector2 amax = new Vector2(maxPoint.X, maxPoint.Z);
            Vector2 bmin = new Vector2(other.MinPoint.X, other.MinPoint.Z);
            Vector2 bmax = new Vector2(other.MaxPoint.X, other.MaxPoint.Z);

            Vector2 mtd = new Vector2();

            float left = (bmin.X - amax.X);
            float right = (bmax.X - amin.X);
            float top = (bmin.Y - amax.Y);
            float bottom = (bmax.Y - amin.Y);

            // box dont intersect   
            if (left > 0 || right < 0) throw new Exception("no intersection");
            if (top > 0 || bottom < 0) throw new Exception("no intersection");

            // box intersect. work out the mtd on both x and y axes.
            if (Math.Abs(left) < right)
                mtd.X = left;
            else
                mtd.X = right;

            if (Math.Abs(top) < bottom)
                mtd.Y = top;
            else
                mtd.Y = bottom;

            // 0 the axis with the largest mtd value.
            if (Math.Abs(mtd.X) < Math.Abs(mtd.Y))
                mtd.Y = 0;
            else
                mtd.X = 0;
            return new Vector3(mtd.X, 0, mtd.Y);
        }

        public Vector3 MinPoint { get { return minPoint; } }

        public Vector3 MaxPoint { get { return maxPoint; } }


        public static AABB UpdateBoundingBox(Model model, Matrix worldTransform)
        {
            // Initialize minimum and maximum corners of the bounding box to max and min values
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            // For each mesh of the model
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    // Vertex buffer parameters
                    int vertexStride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;
                    int vertexBufferSize = meshPart.NumVertices * vertexStride;

                    // Get vertex data as float
                    float[] vertexData = new float[vertexBufferSize / sizeof(float)];
                    meshPart.VertexBuffer.GetData<float>(vertexData);

                    // Iterate through vertices (possibly) growing bounding box, all calculations are done in world space
                    for (int i = 0; i < vertexBufferSize / sizeof(float); i += vertexStride / sizeof(float))
                    {
                        Vector3 transformedPosition = Vector3.Transform(new Vector3(vertexData[i], vertexData[i + 1], vertexData[i + 2]), worldTransform);

                        min = Vector3.Min(min, transformedPosition);
                        max = Vector3.Max(max, transformedPosition);
                    }
                }
            }

            // Create and return bounding box
            return new AABB(min, max);
        }
    }
}
