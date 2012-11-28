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

    public class Key : EnvironmentObject, IInteractableObject, IInventoryItem, IChestItem
    {
        private Matrix[] screenTransforms;
        private Matrix screenWorldMatrix;

        Vector3 ambColor;
        Vector3 diffColor;
        Vector3 specColor;

        public string KeyID
        {
            get;
            private set;
        }

        public Key(ContentManager content, Vector3 position, Vector3 rotation, float scale, string keyID)
            :base(@"Models\Key", content, position, rotation, scale)
        {
            CreateUseAABB(Vector3.Zero, position, 75, 0);
            Interactables.AddInteractable(this);
            emitter = new AudioEmitter();
            emitter.Position = position;
            this.KeyID = keyID;
            screenTransforms = new Matrix[base.model.Bones.Count];
            screenWorldMatrix = Matrix.Identity;
            GetKeyColor(keyID);
        }

        public Key(ContentManager content, string keyID)
            : base(@"Models\Key", content)
        {
            emitter = new AudioEmitter();
            emitter.Position = position;
            this.KeyID = keyID;
            screenTransforms = new Matrix[base.model.Bones.Count];
            screenWorldMatrix = Matrix.Identity;
            GetKeyColor(keyID);
        }

        public override void OnEnteringArea()
        {
            
        }
        public void Use(AABB interactingParty)
        {
            if (interactingParty is Player)
            {
                Game.SoundManager.PlaySound("Loot");
                Interactables.RemoveInteractable(this);
                Game.player.inv.AddItem(new InventoryItem(this, KeyID));
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
                    _effect.DiffuseColor = diffColor;// new Vector3(0.8f, 0.8f, 0.8f);
                    _effect.AmbientLightColor = ambColor;// new Vector3(0.8f, 0.8f, 0.8f);
                    _effect.SpecularColor = specColor;// new Vector3(0.8f, 0.8f, 0.8f);
                    _effect.World =
                    Matrix.CreateRotationZ(MathHelper.ToRadians(90f))
                    *Matrix.CreateRotationX(MathHelper.ToRadians(90f))
                    
                    * screenTransforms[mesh.ParentBone.Index]
                    * screenWorldMatrix;
                    
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
                Game.player.inv.AddItem(new InventoryItem(this, KeyID));
                World.currentArea.RemoveEnvironmentItem(this);
            }
        }

        private void GetKeyColor(string keyID)
        {
            List<int> intVal = new List<int>();

            foreach (Char c in KeyID)
            {
                intVal.Add((int)c);
            }            
            
            if(intVal.Count < 3)
            {
                while (intVal.Count < 3)
                    intVal.Add(intVal.ElementAt(0));
            }

            for(int i = 0; i < 3; i++)
            {
                diffColor = new Vector3(intVal.ElementAt(i), intVal.ElementAt(i + 1), intVal.ElementAt(i + 2));
                diffColor.Normalize();
                specColor = ambColor = diffColor;
            }
           
        }

        public void UsedCallback()
        {
        }
    }
}
