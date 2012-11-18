using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LabyrinthExplorer
{
    public class Enemy
    {

        private Model model;
        private string modelName;

        private Matrix transformation;

       

        private Vector3 position;
        
        public Enemy(string modelName)
        {
            this.modelName = modelName;
        }

        public void LoadContent(ContentManager content)
        {
            model = content.Load<Model>(@"Models\"+modelName);
        }

        public void Update(float deltaTime)
        {
            
            transformation.Translation = position;
        }

        public void Draw(Camera camera)
        {
            Matrix[] modelTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(modelTransforms);

            foreach (ModelMesh m in model.Meshes)
            {
                foreach (BasicEffect e in m.Effects)
                {
                    e.TextureEnabled = true;

                    e.EnableDefaultLighting();
                    e.World = modelTransforms[m.ParentBone.Index] *
                        Matrix.CreateRotationY(0)
                        * Matrix.CreateTranslation(position);
                   // e.View = Matrix.CreateLookAt(camera.Position,
                     //   Vector3.Zero, Vector3.Up);
                    e.Projection = camera.ProjectionMatrix;
                    //e.World = modelTransforms[m.ParentBone.Index] * transformation;
                    ////e.View = camera.ViewMatrix;
                    //e.Projection = camera.ProjectionMatrix;
                }

                m.Draw();
            }
        }

        public Vector3 Position
        {
            get { return position; }
            set { value = position; }
        }
    }
}
