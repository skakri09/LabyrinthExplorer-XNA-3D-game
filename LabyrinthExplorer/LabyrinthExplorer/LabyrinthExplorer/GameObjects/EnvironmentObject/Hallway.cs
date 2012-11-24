using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LabyrinthExplorer
{
    public class Hallway : EnvironmentObject
    {
        public Hallway(ContentManager content,
            Vector3 position, Vector3 rotation, float scale)
            : base(@"Models\Hallway", content, position, rotation, scale)
        {

        }

        public override void OnEnteringArea()
        {
            
        }

        public override void Draw(Camera camera, Effect effect)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect _effect in mesh.Effects)
                {
                    _effect.EnableDefaultLighting();
                    //_effect.DiffuseColor = new Vector3(0.8f, 0.8f, 0.8f);
                    //_effect.AmbientLightColor = new Vector3(0.8f, 0.8f, 0.8f);
                    //_effect.SpecularColor = new Vector3(0.8f, 0.8f, 0.8f);
                    //_effect.FogEnabled = true;
                    //_effect.FogStart = 50.0f;
                    //_effect.FogEnd = FogEnd;
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
