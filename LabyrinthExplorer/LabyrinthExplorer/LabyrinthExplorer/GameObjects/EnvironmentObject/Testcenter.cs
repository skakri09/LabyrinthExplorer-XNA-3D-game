using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LabyrinthExplorer
{
    public class Testcenter : EnvironmentObject
    {
        Vector3 originalPosition;
        public Testcenter(ContentManager content,
                            Vector3 position, Vector3 rotation, float scale)
            : base(@"Models\TestCenter", content, position, rotation, scale)
        {
            FogEnd = 999999;
            originalPosition = position;
           
        }

        private void PreCalcTransformations()
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect _effect in mesh.Effects)
                {
                    _effect.World = Matrix.Identity
                        * transformation[mesh.ParentBone.Index]
                        * Matrix.CreateRotationY(MathHelper.ToRadians(rotation.Y))
                        * Matrix.CreateScale(modelScale)
                        * Matrix.CreateTranslation(position);
                }
            }
        }

        public override void Draw(Camera camera, Microsoft.Xna.Framework.Graphics.Effect effect)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect _effect in mesh.Effects)
                {
                    _effect.EnableDefaultLighting();
                    _effect.LightingEnabled = true;
                    _effect.AmbientLightColor = new Vector3(1, 1, 1);
                    _effect.DirectionalLight0.Direction = Vector3.Right;
                    _effect.DirectionalLight0.Enabled = true;
                    _effect.DirectionalLight0.SpecularColor = new Vector3(1, 1, 1);
                    _effect.World = Matrix.Identity
                        * transformation[mesh.ParentBone.Index]
                        // * Matrix.CreateRotationX(MathHelper.ToRadians(rotation.X))
                        * Matrix.CreateRotationY(MathHelper.ToRadians(rotation.Y))
                        //* Matrix.CreateRotationZ(MathHelper.ToRadians(rotation.Z))
                        * Matrix.CreateScale(modelScale)
                        * Matrix.CreateTranslation(position);

                    _effect.View = camera.ViewMatrix;
                    _effect.Projection = camera.ProjectionMatrix;
                }
                mesh.Draw();
            }
        }
        
        public override void OnEnteringArea()
        {
          
        }

        public override void Update(float deltaTime)
        {
            //base.Update(deltaTime);
            position = originalPosition + Game.player.Cam.Position;
        }
    }

    
}
