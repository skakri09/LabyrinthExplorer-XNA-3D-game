using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace LabyrinthExplorer
{
    
    public class TurnablePilar : EnvironmentObject, IInteractableObject
    {
        private Vector3 unlockRotation;
        private Vector3 thisInteractionTarget;

        private float rotationVelocity = 0.0f;
        private bool rotating = false;

        private bool submerged = false;
        private bool submerging = false;
        private float submergedYPos = -550.0f;
        private float submergeTimer = 0.0f;
        private bool submergingStart = true;

        AABB interactingPartyForCallback;

        /// <summary>
        /// The unlockedRotation must be on format vector3.left/right/foward/backwards
        /// </summary>
        public TurnablePilar(ContentManager content, Vector3 position,
                    Vector3 rotation, Vector3 unlockedRotation, float scale,
                        Vector3 color)
            : base(@"Models\Environment\SpiderStatue", content, position, rotation, scale)
        {
            CreateCollision(position, rotation, scale);
            base.rotation = rotation;
            unlockRotation = FindUnlockedDegreeRotation(unlockedRotation);
            IsUnlocked = false;
            thisInteractionTarget = base.rotation;
            emitter = new AudioEmitter();
            base.color = color;
        }

        public override void Draw(Camera camera, Microsoft.Xna.Framework.Graphics.Effect effect)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect _effect in mesh.Effects)
                {
                    _effect.EnableDefaultLighting();
                    _effect.DiffuseColor = color;//new Vector3(0.8f, 0.8f, 0.8f);
                    _effect.AmbientLightColor = color;//new Vector3(0.8f, 0.8f, 0.8f);
                    _effect.SpecularColor = color;//new Vector3(0.8f, 0.8f, 0.8f);
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

            if (!submerged && ! submerging)
                UpdateRotation(deltaTime);
            else if (submerging)
                UpdateSubmergion(deltaTime);
                
            //else we are submerged and do nothing
        }
        private void UpdateSubmergion(float deltaTime)
        {
            submergeTimer += deltaTime;
            if (submergeTimer >= 18.0f)//we start submerging after 18 seconds
            {
                if (submergingStart)
                {
                    submergingStart = false;
                    Game.SoundManager.PlaySound("PillarRotate", this);
                }
                if (base.position.Y <= submergedYPos)
                {
                    submerged = true;
                    submerging = false;
                }
                else
                {
                    base.position.Y += (-68 * deltaTime);
                    base.rotation.Y += (100 * deltaTime);
                    CreateCollision(base.position, unlockRotation, base.Scale);
                }
            }
        }

        private void UpdateRotation(float deltaTime)
        {
            if (rotating)
            {
                base.rotation.Y += rotationVelocity * deltaTime;
                if (base.rotation.Y >= thisInteractionTarget.Y)
                {
                    if (interactingPartyForCallback is Lever)
                    {
                        Lever lever = (Lever)interactingPartyForCallback;
                        lever.SetUnused();
                    }
                    rotating = false;
                    //giving 20 degrees leeway in each direction for when its unlocked, as
                    //the rotation wont be exact since we use floats
                    if (base.rotation.Y >= (unlockRotation.Y - 20)
                      && base.rotation.Y <= (unlockRotation.Y + 20))
                    {
                        IsUnlocked = true;
                    }
                    else
                        IsUnlocked = false;
                }
            }
        }
        
        private Vector3 FindUnlockedDegreeRotation(Vector3 targetRotation)
        {
            if (targetRotation == Vector3.Forward)
            {
                return new Vector3(0, 360, 0);//using 360 instead of 0 so we can easily use >= in update
            }
            else if (targetRotation == Vector3.Backward)
            {
                return new Vector3(0, 180, 0);
            }
            else if (targetRotation == Vector3.Left)
            {
                return new Vector3(0, 90, 0);
            }
            else if (targetRotation == Vector3.Right)
            {
                return new Vector3(0, 270, 0);
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
            if (!submerged && !submerging)
            {
                StartRotation(90, 15);
                this.interactingPartyForCallback = interactingParty;
                Game.SoundManager.PlaySound("PillarRotate", this);
            }
        }

        public void Submerge()
        {
            if (!submerging && !submerged)
            {
                submerging = true;
            }
        }

        private void StartRotation(float degreesToRotate, float rotateVelocity)
        {
            if (base.rotation.Y >= 360)
            {
                base.rotation.Y -= 360;
                thisInteractionTarget.Y = 0.0f;
            }
            rotating = true;
            rotationVelocity = rotateVelocity;
            thisInteractionTarget.Y += degreesToRotate;
        }

        public void UsedCallback()
        {
        }

        public bool IsUnlocked
        {
            get;
            private set;
        }
    }
}
