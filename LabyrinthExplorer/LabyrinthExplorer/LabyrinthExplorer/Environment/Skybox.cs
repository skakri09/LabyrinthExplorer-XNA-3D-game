using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LabyrinthExplorer
{
    public class Skybox
    {
        Model skyboxModel;
        Matrix[] transforms;
        public Skybox(ContentManager content)
        {
            skyboxModel = content.Load<Model>(@"Models\skybox");
            transforms = new Matrix[skyboxModel.Bones.Count];
            skyboxModel.CopyAbsoluteBoneTransformsTo(transforms);
        }

        public void Draw(Camera camera, GraphicsDevice device)
        {
            foreach (ModelMesh mesh in skyboxModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World =
                      transforms[mesh.ParentBone.Index]
                     * Matrix.CreateScale(1)
                     * Matrix.CreateTranslation(camera.Position)
                     * Matrix.CreateTranslation(Vector3.Zero);//origo
                    effect.View = camera.ViewMatrix;
                    effect.Projection = camera.ProjectionMatrix;

                    effect.EnableDefaultLighting();
                }
                mesh.Draw();
            }
        }
    }
}
