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
        private Matrix[] transformation;

        private string modelName;

        private Vector3 velocity;
        private Vector3 position;
        private Vector3 rotation;
        private float modelScale = 6.0f;

        protected float FogEnd = 1000;
        Model model;
        protected AiStateMachine aiStateMachine;

        public Enemy(string modelName, ContentManager content, float scale)
        {
            this.modelName = modelName;
            LoadContent(content);
        }

        public void LoadContent(ContentManager content)
        {
            model = content.Load<Model>(@"Models\" + modelName);
            transformation = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transformation);
        }

        public void Update(TimeSpan time, float deltaTime)
        {
            if (aiStateMachine != null)
            {
                aiStateMachine.Update(deltaTime);
            }
            
            position += velocity * deltaTime;
        }

        public void Draw(Camera camera)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect _effect in mesh.Effects)
                {
                    _effect.EnableDefaultLighting();
                    //effect.PreferPerPixelLighting = true;
                    _effect.DiffuseColor = new Vector3(0.3f);
                    _effect.AmbientLightColor = new Vector3(0.3f);
                    _effect.FogEnabled = true;
                    _effect.FogStart = 0.0f;
                    _effect.FogEnd = FogEnd;
                    _effect.World = Matrix.Identity
                        * transformation[mesh.ParentBone.Index]
                        * Matrix.CreateRotationY(MathHelper.ToRadians(rotation.X))
                        * Matrix.CreateRotationY(MathHelper.ToRadians(rotation.Y))
                        * Matrix.CreateRotationY(MathHelper.ToRadians(rotation.Z))
                        * Matrix.CreateScale(modelScale)
                        * Matrix.CreateTranslation(position);

                    _effect.View = camera.ViewMatrix;
                    _effect.Projection = camera.ProjectionMatrix;
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

        public void PerformBaseAction()
        {

        }

        public Vector3 Position
        {
            get { return position; }
            set { position = value ; }
        }

        public Vector3 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public Vector3 Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
    }
}
