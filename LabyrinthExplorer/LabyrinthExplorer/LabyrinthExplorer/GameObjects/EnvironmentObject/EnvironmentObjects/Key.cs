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
    public class Key : EnvironmentObject, IInteractableObject, IInventoryItem, ChestItem
    {
        private Matrix[] screenTransforms;
        private Matrix screenWorldMatrix;

        public Key(ContentManager content, Vector3 position, Vector3 rotation, float scale)
            :base(@"Models\Key", content, position, rotation, scale)
        {
            CreateUseAABB(Vector3.Zero, position, 75, 0);
            Interactables.AddInteractable(this);
            emitter = new AudioEmitter();
            emitter.Position = position;

            screenTransforms = new Matrix[base.model.Bones.Count];
            screenWorldMatrix = Matrix.Identity;

        }

        public Key(ContentManager content)
            : base(@"Models\Key", content)
        {
            emitter = new AudioEmitter();
            emitter.Position = position;

            screenTransforms = new Matrix[base.model.Bones.Count];
            screenWorldMatrix = Matrix.Identity;

        }


        public void Use(AABB interactingParty)
        {
            if (interactingParty is Player)
            {
                Game.SoundManager.PlaySound("Loot");
                Interactables.RemoveInteractable(this);
                Game.player.inv.AddItem(new InventoryItem(this, "SecretDoorKey"));
                World.currentArea.RemoveEnvironmentItem(this);
            }
        }

        public float GetInventoryScale()
        {
            return 0.015f;
        }

        public void Update(float deltaTime, Camera camera, Vector3 screenOffset)
        {
            base.model.CopyAbsoluteBoneTransformsTo(screenTransforms);

            screenWorldMatrix = camera.WeaponWorldMatrix(screenOffset.X,
                    screenOffset.Y, screenOffset.Z, GetInventoryScale());
        }

        public void Draw(Camera camera)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect _effect in mesh.Effects)
                {
                    _effect.EnableDefaultLighting();
                    _effect.DiffuseColor = new Vector3(0.8f, 0.8f, 0.8f);
                    _effect.AmbientLightColor = new Vector3(0.8f, 0.8f, 0.8f);
                    _effect.SpecularColor = new Vector3(0.8f, 0.8f, 0.8f);
                    _effect.World =
                    Matrix.CreateRotationZ(MathHelper.ToRadians(90f))
                    *Matrix.CreateRotationX(MathHelper.ToRadians(90f))
                    
                    * screenTransforms[mesh.ParentBone.Index]
                    * screenWorldMatrix;
                    
                    _effect.View = camera.ViewMatrix;
                    _effect.Projection = camera.ProjectionMatrix;

                    _effect.View = camera.ViewMatrix;
                    _effect.Projection = camera.ProjectionMatrix;
                }
                mesh.Draw();
            }
        }

        public void OnChestOpen(AABB chestOpener)
        {
            if (chestOpener is Player)
            {
                Game.SoundManager.PlaySound("Loot");
                Interactables.RemoveInteractable(this);
                Game.player.inv.AddItem(new InventoryItem(this, "SecretDoorKey"));
                World.currentArea.RemoveEnvironmentItem(this);
            }
        }
    }
}
