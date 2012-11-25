using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LabyrinthExplorer
{
    public class Compass : IInventoryItem, IChestItem
    {
        Model compassModel;

        private Matrix[] compTransf;
        private Matrix compWorldMatrix;


        public Compass(ContentManager content)
        {
            compassModel = content.Load<Model>(@"Models\CompassMiddle");

            compTransf = new Matrix[compassModel.Bones.Count];
            compWorldMatrix = Matrix.Identity;
        }


        public float GetInventoryScale()
        {
            return 0.02f;
        }

        public void Update(float deltaTime, Camera camera, Vector3 screenOffset)
        {
            compassModel.CopyAbsoluteBoneTransformsTo(compTransf);

            compWorldMatrix = camera.WeaponWorldMatrix(screenOffset.X,
                    screenOffset.Y, screenOffset.Z, GetInventoryScale());
        }

        public void Draw(Camera camera)
        {
            foreach (ModelMesh mesh in compassModel.Meshes)
            {
                foreach (BasicEffect _effect in mesh.Effects)
                {
                    _effect.EnableDefaultLighting();
                    _effect.LightingEnabled = true;
                    _effect.DirectionalLight0.DiffuseColor = new Vector3(1, 1, 1);
                    _effect.DirectionalLight0.SpecularColor = new Vector3(1, 1, 1);
                    _effect.DirectionalLight0.Direction = camera.ViewDirection;

                    _effect.World =
                    Matrix.CreateRotationZ(MathHelper.ToRadians(-camera.HeadingDegrees))
                    * Matrix.CreateRotationY(MathHelper.ToRadians(-20f))
                    * Matrix.CreateRotationX(MathHelper.ToRadians(55f))
                    * compTransf[mesh.ParentBone.Index]
                    * compWorldMatrix;

                    _effect.View = camera.ViewMatrix;
                    _effect.Projection = camera.ProjectionMatrix;
                }
                mesh.Draw();
            }
        }

        public void OnChestOpen(AABB chestOpener)
        {
            if (!Game.player.inv.HaveItemOfType("compass"))
            {
                Game.SoundManager.PlaySound("Loot");
                Game.player.inv.AddItem(new InventoryItem(this, "compass"));
            }
        }
    }
}
