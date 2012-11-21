using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace LabyrinthExplorer
{
    public class EnvironmentObject : AABB
    {
        private Vector3 position;
        private Vector3 rotation;
        private float modelScale;

        private Matrix[] transformation;
        private Matrix matrixTranslation;

        private Model model;

        
        /// <summary>
        /// Creates a static environment object.
        /// </summary>
        /// <param name="modelPath">full path of model inside the Model folder </param>
        /// <param name="content"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="scale"></param>
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

        //Override if update functionality is required
        public virtual void Update(float deltaTime)
        {
        }

        public void Draw(Camera camera)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.World = Matrix.Identity
                        * transformation[mesh.ParentBone.Index]
                        * Matrix.CreateRotationY(MathHelper.ToRadians(rotation.X))
                        * Matrix.CreateRotationY(MathHelper.ToRadians(rotation.Y))
                        * Matrix.CreateRotationY(MathHelper.ToRadians(rotation.Z))
                        * Matrix.CreateScale(modelScale)
                        * Matrix.CreateTranslation(position);

                    effect.View = camera.ViewMatrix;
                    effect.Projection = camera.ProjectionMatrix;
                }
                mesh.Draw();
            }
        }

        protected void SetModel(Model newModel)
        {
            model = newModel;
            transformation = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transformation);
        }

        protected Model GetModel()
        {
            return model;
        }

        protected Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }
    }
}
