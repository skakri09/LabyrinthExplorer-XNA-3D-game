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

        Matrix[] modelTransforms;

        private Matrix transformation;
        private Vector3 position;
        private Vector3 rotation;
        private float modelScale = 50;

        public Matrix[] boneTransforms;
        public Matrix[] worldTransforms;
        public Matrix[] skinTransforms;
        public Matrix[] originalBonesMatrix;

        public Enemy(string modelName)
        {
            this.modelName = modelName;
        }

        public void LoadContent(ContentManager content)
        {
            model = content.Load<Model>(@"Models\"+modelName);
            modelTransforms = new Matrix[model.Bones.Count];

            //SkinningData skinningData = model.Tag as SkinningData;
            //if (skinningData != null)
            //{
            //    originalBonesMatrix = new Matrix[skinningData.BindPose.Count];
            //    int curBone = 0;
            //    while (curBone < skinningData.BindPose.Count)
            //    {
            //        originalBonesMatrix[curBone] = skinningData.BindPose[curBone];
            //        curBone++;
            //    }
            //}
            //else throw new InvalidOperationException
            //            ("Model does not contain a SkinningData tag.");
        }

        public void Update(float deltaTime)
        {
            
            transformation.Translation = position;
            
            model.CopyAbsoluteBoneTransformsTo(modelTransforms);
            //rotation.Y += 10 * deltaTime;
        }

        public void Draw(Camera camera)
        {
            foreach (ModelMesh m in model.Meshes)
            {
                foreach (BasicEffect e in m.Effects)
                {
                    e.EnableDefaultLighting();
                    e.World = modelTransforms[m.ParentBone.Index]
                        * Matrix.Identity
                        * Matrix.CreateRotationX(MathHelper.ToRadians(rotation.X))
                        * Matrix.CreateRotationY(MathHelper.ToRadians(rotation.Y))
                        * Matrix.CreateRotationZ(MathHelper.ToRadians(rotation.Z))
                        * Matrix.CreateScale(modelScale)
                        * Matrix.CreateTranslation(position);
                       
                    e.View = camera.ViewMatrix;
                    e.Projection = camera.ProjectionMatrix;
                }

                m.Draw();
            }
        }

        public Vector3 Position
        {
            get { return position; }
            set { position = value ; }
        }
        public Matrix Transformation
        {
            get { return transformation; }
        }
    }
}
