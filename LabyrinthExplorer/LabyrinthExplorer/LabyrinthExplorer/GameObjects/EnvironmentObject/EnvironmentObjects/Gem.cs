using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace LabyrinthExplorer
{
    public class Gem : EnvironmentObject, I3DSoundCustDivFact, IInteractableObject, IInventoryItem
    {
        private double sinWaveVar;
        private double sinWaveMultiplier = 5.0;
        private Vector3 originalModelPosition;

        private Vector3 color;
        private string effectName;

        private Matrix[] screenTransforms;
        private Matrix screenWorldMatrix;
        string gemType;

        public Gem(string gemModelName, ContentManager content,
            Vector3 position, float scale)
            :base(@"Models\Environment\"+gemModelName, content, position, Vector3.Zero, scale)
        {
            CreateUseAABB(Vector3.Zero, position, 100, 0);
            Interactables.AddInteractable(this);
            emitter = new AudioEmitter();
            emitter.Position = position;
            originalModelPosition = position;
            GenLightColor(gemModelName);
            GenEffect(gemModelName);
            screenTransforms = new Matrix[base.model.Bones.Count];
            screenWorldMatrix = Matrix.Identity;
            gemType = gemModelName;
        }

        public override void OnEnteringArea()
        {
            Game.SoundManager.PlaySound(effectName, this, -1);
        }

        public override void Update(float deltaTime)
        {
            
            sinWaveVar += (double) deltaTime;
            base.Update(deltaTime);

            position.Y = originalModelPosition.Y + (float)(Math.Sin(sinWaveVar) * sinWaveMultiplier);
        }

        public void Update(float deltaTime, Camera camera, Vector3 screenOffset)
        {
            base.model.CopyAbsoluteBoneTransformsTo(screenTransforms);

            screenWorldMatrix = camera.WeaponWorldMatrix(screenOffset.X,
                    screenOffset.Y, screenOffset.Z, GetInventoryScale());
        }
        
        public override void Draw(Camera camera, Microsoft.Xna.Framework.Graphics.Effect effect)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect _effect in mesh.Effects)
                {
                    _effect.EnableDefaultLighting();
                    _effect.DiffuseColor = color;
                    _effect.AmbientLightColor = color;
                    _effect.SpecularColor = color;
                    _effect.FogEnabled = true;
                    _effect.FogStart = 50.0f;
                    _effect.FogEnd = 3000;
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

        public void Draw(Camera camera)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect _effect in mesh.Effects)
                {
                    _effect.EnableDefaultLighting();
                    _effect.DiffuseColor = color;
                    _effect.AmbientLightColor = color;
                    _effect.SpecularColor = color;
                    _effect.World = screenTransforms[mesh.ParentBone.Index]
                        * screenWorldMatrix;
                    _effect.View = camera.ViewMatrix;
                    _effect.Projection = camera.ProjectionMatrix;

                    _effect.View = camera.ViewMatrix;
                    _effect.Projection = camera.ProjectionMatrix;
                }
                mesh.Draw();
            }
        }
        
        private void GenLightColor(string gemtype)
        {
            if (gemtype == "GemRed")
            {
                effectName = "RedAura";
            }
            else if (gemtype == "GemYellow")
            {
                effectName = "YellowAura";
            }
            else if(gemtype == "GemBlue")
            {
                effectName = "BlueAura";
            }
        }
        private void GenEffect(string gemtype)
        {
            if (gemtype == "GemRed")
            {
                color = new Vector3(0.5f, 0, 0);
            }
            else if (gemtype == "GemYellow")
            {
                color = new Vector3(1, 1, 0);
            }
            else if (gemtype == "GemBlue")
            {
                color = new Vector3(0, 0, 0.5f);
            }
            else
            {
                color = new Vector3(1, 1, 1);
            }
        }
        public float GetCustomDivisionFactor()
        {
            return 800;
        }

        public void Use(AABB interactingParty)
        {
            if (interactingParty is Player)
            {
                Game.SoundManager.PlaySound("GemPickup");
                Game.SoundManager.StopSound(effectName);
                Interactables.RemoveInteractable(this);
                Game.player.inv.AddItem(new InventoryItem(this, gemType));
                World.currentArea.RemoveEnvironmentItem(this);
            }
        }

        public void Use()
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public float GetInventoryScale()
        {
            return 0.4f;
        }
    }
}
