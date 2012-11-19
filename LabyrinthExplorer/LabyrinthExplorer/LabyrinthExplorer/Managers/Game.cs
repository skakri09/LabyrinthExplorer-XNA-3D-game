
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
using System.Text;

namespace LabyrinthExplorer
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        InputManager input;

#region startup
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;
        private Camera camera;
        private Model weapon;
        private Matrix[] weaponTransforms;
        private Matrix weaponWorldMatrix;
        private Vector2 fontPos;
        private int frames;
        private int framesPerSecond;
        private TimeSpan elapsedTime = TimeSpan.Zero;
        private bool enableColorMap;
        private bool displayHelp;

        World world;

        private AudioManager soundMan;
        Enemy spider;
#endregion  

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = GameConstants.windowWidth;
            graphics.PreferredBackBufferHeight = GameConstants.windowHeight;
            Content.RootDirectory = "Content";

            input = new InputManager(this);
            input.SetMouseVisible(GameConstants.mouseVisible);
            Services.AddService(typeof(IInputService), input);

            camera = new Camera(this);
            Components.Add(camera);

            Window.Title = "Labyrinth Survival. You can make it!";
            Window.Title = Window.Title.PadRight(200);
            Window.Title += "....hahaha not!";

            IsFixedTimeStep = GameConstants.verticalSyncOn;
            
            soundMan = new AudioManager(this);  
                                                     
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
            
             // Setup frame buffer.
            graphics.SynchronizeWithVerticalRetrace = false;
            graphics.PreferredBackBufferWidth = GameConstants.windowWidth;
            graphics.PreferredBackBufferHeight = GameConstants.windowHeight;
            graphics.PreferMultiSampling = true;
            graphics.ApplyChanges();

            // Initially enable the diffuse color map texture.
            enableColorMap = true;

            // Initial position for text rendering.
            fontPos = new Vector2(1.0f, 1.0f);

            world = new World(camera);
            world.LoadContent(GraphicsDevice, Content);
            Services.AddService(typeof(World), world);
            // Setup the camera.
            camera.EyeHeightStanding = GameConstants.CAMERA_PLAYER_EYE_HEIGHT;
            camera.Acceleration = new Vector3(
                GameConstants.CAMERA_ACCELERATION_X,
                GameConstants.CAMERA_ACCELERATION_Y,
                GameConstants.CAMERA_ACCELERATION_Z);
            camera.VelocityWalking = new Vector3(
                GameConstants.CAMERA_VELOCITY_X,
                GameConstants.CAMERA_VELOCITY_Y,
                GameConstants.CAMERA_VELOCITY_Z);
            camera.VelocityRunning = new Vector3(
                camera.VelocityWalking.X * GameConstants.CAMERA_RUNNING_MULTIPLIER,
                camera.VelocityWalking.Y * GameConstants.CAMERA_RUNNING_JUMP_MULTIPLIER,
                camera.VelocityWalking.Z * GameConstants.CAMERA_RUNNING_MULTIPLIER);
            camera.Perspective(
                GameConstants.CAMERA_FOVX,
                (float)GameConstants.windowWidth / (float)GameConstants.windowHeight,
                GameConstants.CAMERA_ZNEAR, GameConstants.CAMERA_ZFAR);

            // Initialize the weapon matrices.
            weaponTransforms = new Matrix[weapon.Bones.Count];
            weaponWorldMatrix = Matrix.Identity;

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = Content.Load<SpriteFont>(@"Fonts\Chiller");
            
            weapon = Content.Load<Model>(@"Models\LightStick");
            soundMan.LoadContent();
            //soundMan.PlaySong("LOD",true);
            spider = new Enemy("dude");
            spider.LoadContent(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        private void HandleInput()
        {
            if (input.IsKeyDown(Keys.Escape))
                this.Exit();

            if (input.IsKeyDownOnce(Keys.H))
                displayHelp = !displayHelp;

            if (input.IsKeyDownOnce(Keys.M))
                camera.EnableMouseSmoothing = !camera.EnableMouseSmoothing;

            if (input.IsKeyDownOnce(Keys.P))
                world.EnableParallax = !world.EnableParallax;

            if (input.IsKeyDownOnce(Keys.T))
                enableColorMap = !enableColorMap;

            if (input.IsKeyDownOnce(Keys.Add))
            {
                camera.RotationSpeed += 0.01f;
                if (camera.RotationSpeed > 1.0f)
                    camera.RotationSpeed = 1.0f;
            }
            if (input.IsKeyDownOnce(Keys.Subtract))
            {
                camera.RotationSpeed -= 0.01f;

                if (camera.RotationSpeed <= 0.0f)
                    camera.RotationSpeed = 0.01f;
            }
            if (input.IsKeyDownOnce(Keys.C))
                camera.PerformPlayerCollision = !camera.PerformPlayerCollision;

            if (input.IsKeyDown(Keys.LeftAlt) || input.IsKeyDown(Keys.RightAlt))
            {
                if (input.IsKeyDownOnce(Keys.Enter))
                    ToggleFullScreen();
            }
        }
        
        private void IncrementFrameCounter()
        {
            ++frames;
        }

        private void UpdateFrameRate(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                framesPerSecond = frames;
                frames = 0;
            }
        }

        private void UpdateWeapon()
        {
            weapon.CopyAbsoluteBoneTransformsTo(weaponTransforms);

            weaponWorldMatrix = camera.WeaponWorldMatrix(GameConstants.CANDLE_X_OFFSET,
                GameConstants.CANDLE_Y_OFFSET, GameConstants.CANDLE_Z_OFFSET, GameConstants.CANDLE_SCALE);
        }

        private void ToggleFullScreen()
        {
            int newWidth = 0;
            int newHeight = 0;

            graphics.IsFullScreen = !graphics.IsFullScreen;

            if (graphics.IsFullScreen)
            {
                newWidth = GraphicsDevice.DisplayMode.Width;
                newHeight = GraphicsDevice.DisplayMode.Height;
            }
            else
            {
                newWidth = GameConstants.windowWidth;
                newHeight = GameConstants.windowHeight;
            }

            graphics.PreferredBackBufferWidth = newWidth;
            graphics.PreferredBackBufferHeight = newHeight;
            graphics.PreferMultiSampling = true;
            graphics.ApplyChanges();

            float aspectRatio = (float)newWidth / (float)newHeight;

            camera.Perspective(GameConstants.CAMERA_FOVX, aspectRatio, GameConstants.CAMERA_ZNEAR, GameConstants.CAMERA_ZFAR);
        }
        
        /// <summary>
        /// Very simple camera collision detection logic to prevent the camera
        /// from moving below the floor and from moving outside the bounds of
        /// the room. This is basically the player collision detection
        /// </summary>
        private void PerformCameraCollisionDetection()
        {
            Vector3 newPos = camera.Position;

            //if (camera.Position.Y > GameConstants.CAMERA_BOUNDS_MAX_Y)
            //    newPos.Y = GameConstants.CAMERA_BOUNDS_MAX_Y;

            if (camera.Position.Y < GameConstants.CAMERA_BOUNDS_MIN_Y)
                newPos.Y = GameConstants.CAMERA_BOUNDS_MIN_Y;

            camera.Position = newPos;

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //We dont update the game if the game window is not active/in focus
            if (!this.IsActive)
                return;

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);

            input.Update();
            HandleInput();
            PerformCameraCollisionDetection();
            UpdateWeapon();
            spider.Update(gameTime.ElapsedGameTime, deltaTime);
            world.Update(deltaTime);
            UpdateFrameRate(gameTime);
        }
        
        private void DrawText()
        {
            StringBuilder buffer = new StringBuilder();

            if (displayHelp)
            {
                buffer.AppendLine("Move mouse to free look");
                buffer.AppendLine();
                buffer.AppendLine("Press W and S to move forwards and backwards");
                buffer.AppendLine("Press A and D to strafe left and right");
                buffer.AppendLine("Press SPACE to jump");
                buffer.AppendLine("Press and hold LEFT CTRL to crouch");
                buffer.AppendLine("Press and hold LEFT SHIFT to run");
                buffer.AppendLine();
                buffer.AppendLine("Press M to toggle mouse smoothing");
                buffer.AppendLine("Press P to toggle between parallax normal mapping and normal mapping");
                buffer.AppendLine("Press NUMPAD +/- to change camera rotation speed");
                buffer.AppendLine("Press ALT + ENTER to toggle full screen");
                buffer.AppendLine();
                buffer.AppendLine("Press H to hide help");
            }
            else
            {
                buffer.AppendFormat("FPS: {0}\n", framesPerSecond);
                buffer.AppendFormat("Technique: {0}\n",
                    (world.EnableParallax ? "Parallax normal mapping" : "Normal mapping"));
                buffer.AppendFormat("Mouse smoothing: {0}\n\n",
                    (camera.EnableMouseSmoothing ? "on" : "off"));
                buffer.Append("Camera:\n");
                buffer.AppendFormat("  Position: x:{0} y:{1} z:{2}\n",
                    camera.Position.X.ToString("f2"),
                    camera.Position.Y.ToString("f2"),
                    camera.Position.Z.ToString("f2"));
                buffer.AppendFormat("  Orientation: heading:{0} pitch:{1}\n",
                    camera.HeadingDegrees.ToString("f2"),
                    camera.PitchDegrees.ToString("f2"));
                buffer.AppendFormat("  Velocity: x:{0} y:{1} z:{2}\n",
                    camera.CurrentVelocity.X.ToString("f2"),
                    camera.CurrentVelocity.Y.ToString("f2"),
                    camera.CurrentVelocity.Z.ToString("f2"));
                buffer.AppendFormat("  Rotation speed: {0}\n",
                    camera.RotationSpeed.ToString("f2"));
                buffer.Append("\nPress H to display help");
            }

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            spriteBatch.DrawString(spriteFont, buffer.ToString(), fontPos, Color.Yellow);
            spriteBatch.End();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (!this.IsActive)
                return;

            GraphicsDevice.Clear(Color.CornflowerBlue);

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            GraphicsDevice.SamplerStates[1] = SamplerState.LinearWrap;
            GraphicsDevice.SamplerStates[2] = SamplerState.LinearWrap;

            world.Draw(GraphicsDevice);

            //Draw the weapon.
            foreach (ModelMesh m in weapon.Meshes)
            {
                foreach (BasicEffect e in m.Effects)
                {
                    e.TextureEnabled = enableColorMap;
                    e.EnableDefaultLighting();
                    e.World = weaponTransforms[m.ParentBone.Index] * weaponWorldMatrix;
                    e.View = camera.ViewMatrix;
                    e.Projection = camera.ProjectionMatrix;
                }

                m.Draw();
            }
            spider.Draw(camera);
            DrawText();

            base.Draw(gameTime);
            IncrementFrameCounter();
        }
    }
}