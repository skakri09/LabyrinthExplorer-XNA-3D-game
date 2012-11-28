using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LabyrinthExplorer
{
    public class TextHint : EnvironmentObject
    {
        public TextHint(string fullModelPath, ContentManager content,
                            Vector3 position, Vector3 rotation, float scale)
            :base(fullModelPath, content, position, rotation, scale)
        {

        }

        public override void OnEnteringArea()
        {
            //if anything to do l8r
        }

        public override void Draw(Camera camera, Effect effect)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect _effect in mesh.Effects)
                {
                    _effect.EnableDefaultLighting();
                    _effect.DiffuseColor = new Vector3(0.5f, 0.1f, 0.1f);
                    _effect.AmbientLightColor = new Vector3(1.0f, 0.1f, 0.1f);
                    _effect.SpecularColor = new Vector3(0.5f, 0.1f, 0.1f);
                    _effect.FogEnabled = true;
                    _effect.FogStart = 50.0f;
                    _effect.FogEnd = 800;
                    _effect.World = Matrix.Identity
                        * transformation[mesh.ParentBone.Index]
                        * Matrix.CreateRotationX(MathHelper.ToRadians(rotation.X))
                        * Matrix.CreateRotationY(MathHelper.ToRadians(rotation.Y))
                        * Matrix.CreateRotationZ(MathHelper.ToRadians(rotation.Z))
                        * Matrix.CreateScale(modelScale)
                        * Matrix.CreateTranslation(position);

                    _effect.View = camera.ViewMatrix;
                    _effect.Projection = camera.ProjectionMatrix;
                }
                mesh.Draw();
            }
        }
    }
}
