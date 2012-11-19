using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SkinnedModel;

namespace LabyrinthExplorer
{
    public class Enemy
    {
        //Matrix[] modelTransforms;
        //public Matrix[] boneTransforms;
        //public Matrix[] worldTransforms;
        //public Matrix[] skinTransforms;
        //public Matrix[] originalBonesMatrix;
        //private Model model;
        //private Matrix transformation;
        
        private string modelName;

        private Vector3 velocity;
        private Vector3 position;
        private Vector3 rotation;
        private float modelScale = 6.0f;

        Model currentModel;
        AnimationPlayer animationPlayer;
        AiStateMachine aiStateMachine;

        public Enemy(string modelName)
        {
            this.modelName = modelName;
            //aiStateMachine = new AiStateMachine(this, new EnemyConstState(), new PatrolX(new Vector3(1100, 0, 3300), new Vector3(4600, 0, 3300)));
            aiStateMachine = new AiStateMachine(this, new EnemyConstState(), new PatrolZ(new Vector3(4600, 0, 4000), new Vector3(4600, 0, 2000)));
        }

        public void LoadContent(ContentManager content)
        {
            //model = content.Load<Model>(@"Models\"+modelName);
            //modelTransforms = new Matrix[model.Bones.Count];
            currentModel = content.Load<Model>(@"Models\" + modelName);
            // Look up our custom skinning information.
            SkinningData skinningData = currentModel.Tag as SkinningData;

            if (skinningData == null)
                throw new InvalidOperationException
                    ("This model does not contain a SkinningData tag.");

            // Create an animation player, and start decoding an animation clip.
            animationPlayer = new AnimationPlayer(skinningData);

            AnimationClip clip = skinningData.AnimationClips["Take 001"];
           // clip.Duration = clip.Duration + new TimeSpan(10000000);
            animationPlayer.StartClip(clip);
        }

        public void Update(TimeSpan time, float deltaTime)
        {
            aiStateMachine.Update(deltaTime);
            position += velocity * deltaTime;
            animationPlayer.Update(time, true, Matrix.Identity);
           // model.CopyAbsoluteBoneTransformsTo(modelTransforms);
            //rotation.Y += 10 * deltaTime;
        }

        public void Draw(Camera camera)
        {
            Matrix[] bones = animationPlayer.GetSkinTransforms();

            // Render the skinned mesh.
            foreach (ModelMesh mesh in currentModel.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.SetBoneTransforms(bones);
                    effect.World = Matrix.Identity
                        * Matrix.CreateRotationX(MathHelper.ToRadians(rotation.X))
                        * Matrix.CreateRotationY(MathHelper.ToRadians(rotation.Y))
                        * Matrix.CreateRotationZ(MathHelper.ToRadians(rotation.Z))
                        * Matrix.CreateScale(modelScale)
                        * Matrix.CreateTranslation(position);
                    effect.View = camera.ViewMatrix;
                    effect.Projection = camera.ProjectionMatrix;

                    effect.EnableDefaultLighting();

                    effect.SpecularColor = new Vector3(0.25f);
                    effect.SpecularPower = 16;
                    effect.AmbientLightColor = new Vector3(0.01f, 0.01f, 0.01f);
                    effect.DiffuseColor = new Vector3(0.2f, 0.2f, 0.2f);
                    effect.FogEnabled = true;
                    effect.FogStart = 50;
                    effect.FogEnd = 1000;
                }
                mesh.Draw();
            }

            //foreach (ModelMesh m in model.Meshes)
            //{
            //    foreach (BasicEffect e in m.Effects)
            //    {
            //        e.EnableDefaultLighting();
            //        e.World = modelTransforms[m.ParentBone.Index]
            //            * Matrix.Identity
            //            * Matrix.CreateRotationX(MathHelper.ToRadians(rotation.X))
            //            * Matrix.CreateRotationY(MathHelper.ToRadians(rotation.Y))
            //            * Matrix.CreateRotationZ(MathHelper.ToRadians(rotation.Z))
            //            * Matrix.CreateScale(modelScale)
            //            * Matrix.CreateTranslation(position);
                       
            //        e.View = camera.ViewMatrix;
            //        e.Projection = camera.ProjectionMatrix;
            //    }

            //    m.Draw();
            //}

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
