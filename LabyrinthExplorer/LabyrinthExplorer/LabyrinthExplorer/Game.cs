
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
using System.Diagnostics;

namespace LabyrinthExplorer
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        InputManager input;
        Camera cam;
        Model torch;
        Matrix[] torchTransforms;
        Matrix torchWorldMatrix;

        Model wall;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = GameConstants.windowWidth;
            graphics.PreferredBackBufferHeight = GameConstants.windowHeight;
            
            Content.RootDirectory = "Content";

            input = new InputManager(this);
            input.SetMouseVisible(true);

            cam = new Camera(this);
            Components.Add(cam);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            // These camera settings are application specific.
            // Your values will differ to the ones used here.
            // It's important that your application correctly initializes
            // the camera's EyeHeightStanding, Acceleration, VelocityWalking,
            // and VelocityRunning properties. These properties are used
            // to move the camera about in your virtual 3D world.
            cam.EyeHeightStanding = 100.0f;
            cam.Acceleration = new Vector3(800.0f, 800.0f, 800.0f);
            cam.VelocityWalking = new Vector3(200.0f, 200.0f, 200.0f);
            cam.VelocityRunning = cam.VelocityWalking * 2.0f;

            int width = this.graphics.GraphicsDevice.DisplayMode.Width / 2;
            int height = this.graphics.GraphicsDevice.DisplayMode.Height / 2;
            float aspectRatio = (float)width / (float)height;

            // Setup the camera's perspective projection matrix.
            cam.Perspective(90.0f, aspectRatio, 0.01f, 1000.0f);

            // Initialize the weapon matrices.
            torchTransforms = new Matrix[torch.Bones.Count];
            torchWorldMatrix = Matrix.Identity;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Services.AddService(typeof(IInputService), input);
            wall = Content.Load<Model>("Models\\Wall");
            torch = Content.Load<Model>("Models\\LightStick");

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
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            input.Update();
            if (input.IsKeyDown(Keys.Escape))
                this.Exit();


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //Matrix[] transforms = new Matrix[wall.Bones.Count];
            //wall.CopyAbsoluteBoneTransformsTo(transforms);
            torch.CopyAbsoluteBoneTransformsTo(torchTransforms);
            torchWorldMatrix = cam.WeaponWorldMatrix(0.45f, -0.30f, 1.65f,0.5f);



            foreach (ModelMesh mesh in torch.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = torchTransforms[mesh.ParentBone.Index] * torchWorldMatrix;
                    effect.View = cam.ViewMatrix;
                    effect.Projection = cam.ProjectionMatrix;
                }
                mesh.Draw();
            }

            base.Draw(gameTime);
        }
    }
}
