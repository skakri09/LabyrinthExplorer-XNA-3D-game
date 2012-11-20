using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace LabyrinthExplorer
{
    public class EnvironmentObject
    {
        private Vector3 position;
        private Vector3 rotation;
        private float modelScale;

        private Matrix[] transformation;
        private Matrix matrixTranslation;

        private Model model;

        

        public EnvironmentObject(string modelPath, ContentManager content,
            Vector3 position, Vector3 rotation, float scale)
        {
            this.position = position;
            this.rotation = rotation;
            this.modelScale = scale;

            this.matrixTranslation = Matrix.CreateTranslation(position);

            model = content.Load<Model>(modelPath);

            transformation = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transformation);
        }


        void Draw(Camera camera)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.World = Matrix.Identity
                        * transformation[mesh.ParentBone.Index]
                        * Matrix.CreateRotationY(rotation.X)
                        * Matrix.CreateRotationY(rotation.Y)
                        * Matrix.CreateRotationY(rotation.Z)
                        * Matrix.CreateScale(modelScale)
                        * Matrix.CreateTranslation(position);

                    effect.View = camera.ViewMatrix;
                    effect.Projection = camera.ProjectionMatrix;
                }
                mesh.Draw();
            }
        }
    }
}
