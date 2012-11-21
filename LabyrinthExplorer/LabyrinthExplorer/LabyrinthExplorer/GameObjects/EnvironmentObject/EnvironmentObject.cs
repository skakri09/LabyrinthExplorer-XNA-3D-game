using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace LabyrinthExplorer
{
    public class EnvironmentObject : AABB, IEnvironmentObject, I3DSound
    {
        private Vector3 position;
        private Vector3 rotation;
        private float modelScale;

        private Matrix[] transformation;
        private Matrix matrixTranslation;
        protected AudioEmitter emitter;
        private Model model;

        protected float FogEnd = GameConstants.Radius;

        /// <summary>
        /// Creates a static environment object.
        /// </summary>
        /// <param name="modelPath">full path of model inside the Model folder </param>
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

        public virtual void Update(float deltaTime)
        {
            if(emitter != null)
                emitter.Position = position;
        }

        public AudioEmitter GetAudioEmitter()
        {
            AudioEmitter newEmitter = new AudioEmitter();
            newEmitter.Position = position;
            return newEmitter;
        }

        public void Draw(Camera camera, Effect effect)
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

        protected Model GetModel()
        {
            return model;
        }

        protected Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        protected Vector3 Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        protected float Scale
        {
            get { return modelScale; }
            set { modelScale = value; }
        }
    }
}
