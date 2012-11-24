using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace LabyrinthExplorer
{
    public class Pedistal : EnvironmentObject, IInteractableObject
    {

        private string attatchedGem;
        private Vector3 color;
        private bool haveSound;

        public Pedistal(ContentManager content,
                     Vector3 position, Vector3 rotation,
                    float scale,
                    string attachedGemName, 
                    bool addSound = false)
            :base(@"Models\Environment\GemPedistal", content, position, rotation, scale)
        {
            this.haveSound = addSound;
            this.attatchedGem = attachedGemName;

            emitter = new AudioEmitter();
            emitter.Position = base.position;

            CreateUseAABB(Vector3.Zero, position, 150, 0);
            Interactables.AddInteractable(this);
            GenPilarColor(attatchedGem);
        }

        public override void OnEnteringArea()
        {
            if(haveSound)
                Game.SoundManager.PlaySound("PedistalSound", this, -1);
        }

        public override void Draw(Camera camera, Microsoft.Xna.Framework.Graphics.Effect effect)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect _effect in mesh.Effects)
                {
                    _effect.EnableDefaultLighting();
                    _effect.EmissiveColor = color;

                    if (attatchedGem == "GemBlue")
                    {
                        _effect.DiffuseColor = new Vector3(0, 0, 1);
                    }
                    //_effect.AmbientLightColor = color;
                    //_effect.SpecularColor = color;
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
        
        public void Use(AABB interactingParty)
        {
            if (interactingParty is Player)
            {
                Player player = (Player)interactingParty;
                if (player.inv.HaveItemOfType(attatchedGem))
                {
                    if (attatchedGem == "GemRed")
                    {
                        Game.SoundManager.PlaySound("RedGemEntered", this);
                    }
                    else if (attatchedGem == "GemYellow")
                    {
                        Game.SoundManager.PlaySound("YellowGemEntered", this);
                    }
                    else if (attatchedGem == "GemBlue")
                    {
                        Game.SoundManager.PlaySound("BlueGemEntered", this);
                    }
                    Gem gem = (Gem)player.inv.GetAndRemoveItem(attatchedGem);
                    if (gem != null)
                    {
                        gem.AttatchToPedistal(new Vector3(position.X, 150, position.Z));
                        World.currentArea.AddEnvironmentObjectToEnvironment(gem);
                    }
                    else
                        throw new Exception("Player did not have the gem after all hurpdurp");
                }
            }
        }

        private void GenPilarColor(string gemType)
        {
            if (gemType == "GemRed")
            {
                color = new Vector3(1, 0, 0);
            }
            else if (gemType == "GemYellow")
            {
                color = new Vector3(1, 1, 0);
            }
            else if (gemType == "GemBlue")
            {
                color = new Vector3(0, 0, 1);
            }
        }

        public void UsedCallback()
        {
            
        }
    }
}
