using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LabyrinthExplorer
{
    
    public class TurnablePilar : EnvironmentObject
    {

        private Vector3 currentRotation;
        private Vector3 unlockRotation;

        /// <summary>
        /// The unlockedRotation must be on format vector3.left/right/foward/backwards
        /// </summary>
        public TurnablePilar(ContentManager content, Vector3 position,
                    Vector3 rotation, Vector3 unlockedRotation, float scale )
            : base(@"Models\Environment\SpiderStatue", content, position, rotation, scale)
        {
            CreateCollision(position, rotation, scale);
        }

        public override void Draw(Camera camera, Microsoft.Xna.Framework.Graphics.Effect effect)
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
                    _effect.FogEnd = 1500;
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
        private void FindCurrentRotation(Vector3 degreesRotation)
        {
            if (degreesRotation.Y == 0)
            {
            }
                
        }

        private void CreateCollision(Vector3 position, Vector3 rotation, float scale)
        {
            Matrix worldTransform = new Matrix();
            worldTransform = Matrix.CreateTranslation(Vector3.Zero)
            * Matrix.CreateRotationY(MathHelper.ToRadians(rotation.Y));
            AABB aabb = UpdateBoundingBox(base.GetModel(), worldTransform);
            Vector3 min = aabb.MinPoint;
            Vector3 max = aabb.MaxPoint;
            min *= scale;
            max *= scale;
            min += position;
            max += position;
            SetAABB(min, max);
            World.currentArea.EnvironmentCollidables().Add(this);
        }
    }
}
