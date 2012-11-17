using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace LabyrinthExplorer.Environment
{
    class NormalMappedWall
    {
        private VertexBuffer vertexBuffer;
        private NormalMappedVertex[] vertices;

        public NormalMappedWall(GraphicsDevice graphicsDevice,
            Vector3 startPos, Vector3 innVec, float lenght)
        {

        }

        /// <summary>
        /// Generates a wall from the starting position, positively 
        /// perpendicularly to the lenght target
        /// </summary>
        /// <param name="startPos"></param>
        /// <param name="innVec"></param>
        /// <param name="lenght"></param>
        private void GenerateWall(Vector3 startPos, Vector3 innVec, float lenght)
        {

        }
    }
}
