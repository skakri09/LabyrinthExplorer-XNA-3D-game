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
    public abstract class EnvironmentObject : AABB, IEnvironmentObject, I3DSound
    {
        protected Vector3 position;
        protected Vector3 rotation;
        protected float modelScale;

        protected Matrix[] transformation;
        protected Matrix matrixTranslation;
        protected AudioEmitter emitter;
        protected Model model;

        protected float FogEnd = GameConstants.Radius;

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

        //Ctor used for items like key which wont nescessarily be visible in the world
        public EnvironmentObject(string modelPath, ContentManager content)
        {
            model = content.Load<Model>(modelPath);

            transformation = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transformation);
        }
        public virtual void Update(float deltaTime)
        {
            if(emitter != null)
                emitter.Position = position;

            model.CopyAbsoluteBoneTransformsTo(transformation);
        }

        public AudioEmitter GetAudioEmitter()
        {
            AudioEmitter newEmitter = new AudioEmitter();
            newEmitter.Position = position;
            return newEmitter;
        }

        public virtual void Draw(Camera camera, Effect effect)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect _effect in mesh.Effects)
                {
                    _effect.EnableDefaultLighting();
                    _effect.DiffuseColor = new Vector3(0.8f, 0.8f, 0.8f);
                    _effect.AmbientLightColor = new Vector3(0.8f, 0.8f, 0.8f);
                    _effect.SpecularColor = new Vector3(0.8f, 0.8f, 0.8f);
                    _effect.FogEnabled = true;
                    _effect.FogStart = 50.0f;
                    _effect.FogEnd = 800;
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

        public abstract void OnEnteringArea();

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
