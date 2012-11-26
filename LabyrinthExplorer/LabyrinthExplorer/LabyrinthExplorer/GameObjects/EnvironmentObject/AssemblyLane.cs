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

        public bool RenderEnds { get; set; }

        public AssemblyLaneEnd(ContentManager content,
            Vector3 playerPos, Vector3 rotation, 
            float scale, bool isStatic = false)
            : base(@"Models\AssemblyLaneEnd", content)
        {
            FogEnd = 5000;
            if (!isStatic)
                base.position = new Vector3(2500, -100, playerPos.Z - 10000);
            else
                base.position = position;

            base.Scale = scale;
            base.rotation = rotation;
            emitter = new AudioEmitter();
            emitter.Position = position;
            orgPos = position;
        }

        public override void OnEnteringArea()
        {
        }

        public void UpdatePosition(Vector3 playerPos)
        {
            base.position = new Vector3(2500, -100, playerPos.Z - 10000);
        }

        public override void Draw(Camera camera, Effect effect)
        {
            if (RenderEnds)
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

    public class AssemblyLaneCollection : IEnvironmentObject
    {
        List<IEnvironmentObject> AssemblyLanes;
        private AssemblyLaneEnd lineEnd;

        public AssemblyLaneCollection(ContentManager content)
        {
            AssemblyLanes = new List<IEnvironmentObject>();
            CreateLaneCollection(content);
        }

        private void CreateLaneCollection(ContentManager contentMan)
        {
            lineEnd = new AssemblyLaneEnd(contentMan, new Vector3(2500, -300, -42000),
                new Vector3(0, 90, 0), 15);
            lineEnd.RenderEnds = true;
            AssemblyLanes.Add(lineEnd);

            for (int i = -100000; i < 4000; i += 3000)
            {
                AssemblyLanes.Add(new AssemblyLane(contentMan,
                new Vector3(2500, 200, i), new Vector3(0, 90, 0), 12));
            }

            AssemblyLanes.Add(new AssemblyLane(contentMan,
                new Vector3(2500, 225, 4650), new Vector3(0, 90, 0), 13));

            //environment.Add(new AssemblyLane(contentMan, 
            //    new Vector3(2500, 0, -10000), new Vector3(0, 90, 0), 10));
            //environment.Add(new AssemblyLane(contentMan,
            //    new Vector3(2500, 0, -9000), new Vector3(0, 90, 0), 10));
            //environment.Add(new AssemblyLane(contentMan,
            //    new Vector3(2500, 0, -8000), new Vector3(0, 90, 0), 10));
            //environment.Add(new AssemblyLane(contentMan,
            //    new Vector3(2500, 0, -7000), new Vector3(0, 90, 0), 10));
            //environment.Add(new AssemblyLane(contentMan,
            //    new Vector3(2500, 0, -6000), new Vector3(0, 90, 0), 10));
            //environment.Add(new AssemblyLane(contentMan,
            //    new Vector3(2500, 0, -5000), new Vector3(0, 90, 0), 10));
            //environment.Add(new AssemblyLane(contentMan,
            //    new Vector3(2500, 0, -4000), new Vector3(0, 90, 0), 10));
        }

        public void Update(float deltaTime)
        {
            lineEnd.UpdatePosition(Game.player.Cam.Position);
        }

        public void Draw(Camera camera, Effect effect)
        {
            foreach (IEnvironmentObject lane in AssemblyLanes)
            {
                lane.Draw(camera, effect);
           }
            lineEnd.Draw(camera, effect);
        }

        public void OnEnteringArea()
        {
            
        }

    }
    
    public class AssemblyLane : EnvironmentObject
    {
        public AssemblyLane(ContentManager content, Vector3 position, Vector3 rotation, float scale)
            : base(@"Models\AssemblyLane", content, position, rotation, scale)
        {
            FogEnd = 10000;
            emitter = new AudioEmitter();
            emitter.Position = position;
        }

        public override void OnEnteringArea()
        {
            //play some crazy trippy sound lasting for the duration player is being moved
        }

        public void Update(Vector3 playerPos, float deltaTime)
        {
            
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
                    _effect.SpecularColor = new Vector3(0, 0, 0.1f);
                    _effect.SpecularPower = 1000;
                    _effect.LightingEnabled = true;
                    _effect.DirectionalLight0.DiffuseColor = new Vector3(1, 1, 1);
                    _effect.DirectionalLight0.SpecularColor = new Vector3(1, 1, 1);
                    _effect.DirectionalLight0.Direction = camera.ViewDirection;

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
