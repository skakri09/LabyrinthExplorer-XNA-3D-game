using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace LabyrinthExplorer
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        Model model;
        float aspectRatio;

        Vector3 modelPos = Vector3.Zero;
        float modelRot = 0.0f;

        #region camera
        Vector3 camPos = new Vector3(0.0f, 50.0f, 500.0f);
        Vector3 camHeadoffset = new Vector3(0, 10, 0);

        //// Set the direction the camera points without rotation.
        //Vector3 cameraReference = new Vector3(0, 0, 10);

        //Matrix rotationMatrix = Matrix.CreateRotationY(avatarYaw);

        //// Create a vector pointing the direction the camera is facing.
        //Vector3 transformedReference = Vector3.Transform(cameraReference, rotationMatrix);

        //// Calculate the position the camera is looking at.
        //Vector3 cameraLookat = cameraPosition + transformedReference;
        
        //view = Matrix.CreateLookAt( cameraPosition, cameraLookat, new Vector3( 0.0f, 1.0f, 0.0f ) );

        #endregion  
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = GameConstants.windowWidth;
            graphics.PreferredBackBufferHeight = GameConstants.windowHeight;
            
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            model = Content.Load<Model>("Models\\Wall");
            aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            modelRot += (float)gameTime.ElapsedGameTime.TotalMilliseconds * MathHelper.ToRadians(0.1f);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] *
                        Matrix.CreateRotationY(modelRot)
                        * Matrix.CreateTranslation(modelPos);

                    effect.View = Matrix.CreateLookAt(camPos,Vector3.Zero, Vector3.Up);

                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(
                        MathHelper.ToRadians(45.0f), aspectRatio,
                        1.0f, 10000.0f);
                }
                mesh.Draw();
            }

            base.Draw(gameTime);
        }
    }
}
