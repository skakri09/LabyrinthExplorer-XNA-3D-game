using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LabyrinthExplorer
{
    
    public class TurnablePilar : EnvironmentObject, IInteractableObject
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
            currentRotation = rotation;
            unlockedRotation = FindUnlockedDegreeRotation(unlockedRotation);
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
        
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

        }

        private Vector3 FindUnlockedDegreeRotation(Vector3 targetRotation)
        {
            if (targetRotation == Vector3.Forward)
            {
                return new Vector3(0, 0, 0);
            }
            else if (targetRotation == Vector3.Backward)
            {
                return new Vector3(0, 0, 0);
            }
            else if (targetRotation == Vector3.Left)
            {
                return new Vector3(0, 0, 0);
            }
            else if (targetRotation == Vector3.Right)
            {
                return new Vector3(0, 0, 0);
            }
            else
                throw new Exception("TargetRotation not correctly set, must be left/right/forward/backward");
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
        }

        public override void OnEnteringArea()
        {
            ///start whatever sound we want
        }

        public void Use(AABB interactingParty)
        {
            //turn 90 degrees 
            //play some sound
        }
    }
}
