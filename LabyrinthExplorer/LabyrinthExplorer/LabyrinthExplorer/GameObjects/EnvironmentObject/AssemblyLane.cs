using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LabyrinthExplorer
{
    public class AssemblyLaneEnd : EnvironmentObject
    {
        Vector3 orgPos;

        public AssemblyLaneEnd(ContentManager content,
                            Vector3 position, Vector3 rotation, float scale)
            : base(@"Models\AssemblyLaneEnd", content, position, rotation, scale)
        {
            FogEnd = 5000;
            emitter = new AudioEmitter();
            emitter.Position = position;
            orgPos = position;
        }

        public override void OnEnteringArea()
        {
           
        }
        public void UpdatePosition(Vector3 playerPos)
        {

        }

        public override void Draw(Camera camera, Effect effect)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect _effect in mesh.Effects)
                {
                    _effect.EnableDefaultLighting();

                    _effect.FogEnabled = true;
                    _effect.FogStart = 1000.0f;
                    _effect.FogEnd = FogEnd;
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

    public class AssemblyLane : EnvironmentObject
    {
        public AssemblyLane(ContentManager content,
                            Vector3 position, Vector3 rotation, float scale)
            : base(@"Models\AssemblyLane", content, position, rotation, scale)
        {
            FogEnd = 2000;
            emitter = new AudioEmitter();
            emitter.Position = position;
        }

        public override void OnEnteringArea()
        {
            //play some crazy trippy sound lasting for the duration player is being moved
        }

        public override void Draw(Camera camera, Microsoft.Xna.Framework.Graphics.Effect effect)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect _effect in mesh.Effects)
                {
                    _effect.EnableDefaultLighting();

                    _effect.FogEnabled = true;
                    _effect.FogStart = 1000.0f;
                    _effect.FogEnd = FogEnd;
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
