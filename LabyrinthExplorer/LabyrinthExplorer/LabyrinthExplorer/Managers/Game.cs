
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
        private Vector2 fontPos;
        private int frames;
        private int framesPerSecond;
        private TimeSpan elapsedTime = TimeSpan.Zero;
        private bool displayHelp;

        //The world object, handles all environment and enemy mobs etc
        World world;

        //Player object, holds the player, the camera etc
        public static Player player;

        public static AudioManager SoundManager;

        //public static Vector3 PlayerPosition { get{return }
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

            Window.Title = "Labyrinth Survival. You can make it!";
            Window.Title = Window.Title.PadRight(200);
            Window.Title += "....hahaha not!";

            IsFixedTimeStep = GameConstants.verticalSyncOn;
            
            SoundManager = new AudioManager(this);
            Components.Add(SoundManager);
        }

        protected override void Initialize()
        {
            base.Initialize();
            
             // Setup frame buffer.
            graphics.SynchronizeWithVerticalRetrace = false;
            graphics.PreferredBackBufferWidth = GameConstants.windowWidth;
            graphics.PreferredBackBufferHeight = GameConstants.windowHeight;
            graphics.PreferMultiSampling = true;
            graphics.ApplyChanges();

            player = new Player(this, GameConstants.PLAYER_START_POS);

            // Initial position for text rendering.
            fontPos = new Vector2(1.0f, 1.0f);

            world = new World(player.Cam);
            world.LoadContent(GraphicsDevice, Content);
            Services.AddService(typeof(World), world);
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = Content.Load<SpriteFont>(@"Fonts\Chiller");
            
            //weapon = Content.Load<Model>(@"Models\LightStick");
            SoundManager.LoadContent();
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        private void HandleInput()
        {
            if (input.IsKeyDown(Keys.Escape))
            {
                SoundManager.StopAllSounds();
                this.Exit();
            }

            if (input.IsKeyDownOnce(Keys.H))
                displayHelp = !displayHelp;

            if (input.IsKeyDownOnce(Keys.M))
                player.Cam.EnableMouseSmoothing = !player.Cam.EnableMouseSmoothing;

            if (input.IsKeyDownOnce(Keys.P))
                world.EnableParallax = !world.EnableParallax;

            if (input.IsKeyDownOnce(Keys.T))
                player.EnableColorMap = !player.EnableColorMap;

            if (input.IsKeyDownOnce(Keys.Add))
            {
                player.Cam.RotationSpeed += 0.01f;
                if (player.Cam.RotationSpeed > 1.0f)
                    player.Cam.RotationSpeed = 1.0f;
            }
            if (input.IsKeyDownOnce(Keys.Subtract))
            {
                player.Cam.RotationSpeed -= 0.01f;

                if (player.Cam.RotationSpeed <= 0.0f)
                    player.Cam.RotationSpeed = 0.01f;
            }
            if (input.IsKeyDown(Keys.LeftAlt) || input.IsKeyDown(Keys.RightAlt))
            {
                if (input.IsKeyDownOnce(Keys.Enter))
                    ToggleFullScreen();
            }
            if (input.IsKeyDownOnce(Keys.F))
            {
                world.DrawSkybox = !world.DrawSkybox;
            }
            if (input.IsKeyDownOnce(Keys.G))
            {
                GameConstants.RenderOnScreenText = !GameConstants.RenderOnScreenText;
            }

            player.HandlePlayerInput(input);
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

            player.Cam.Perspective(GameConstants.CAMERA_FOVX, aspectRatio, GameConstants.CAMERA_ZNEAR, GameConstants.CAMERA_ZFAR);
        }

        protected override void Update(GameTime gameTime)
        {
            //if(!GameConstants.UpdateWhenTabbed)
                if (!this.IsActive)
                    return;

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);

            input.Update();
            HandleInput();
            player.Update(deltaTime);
            world.Update(gameTime);
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
                    (player.Cam.EnableMouseSmoothing ? "on" : "off"));
                buffer.Append("Camera:\n");
                buffer.AppendFormat("  Position: x:{0} y:{1} z:{2}\n",
                    player.Cam.Position.X.ToString("f2"),
                    player.Cam.Position.Y.ToString("f2"),
                    player.Cam.Position.Z.ToString("f2"));
                buffer.AppendFormat("  Orientation: heading:{0} pitch:{1}\n",
                    player.Cam.HeadingDegrees.ToString("f2"),
                    player.Cam.PitchDegrees.ToString("f2"));
                buffer.AppendFormat("  Velocity: x:{0} y:{1} z:{2}\n",
                    player.Cam.CurrentVelocity.X.ToString("f2"),
                    player.Cam.CurrentVelocity.Y.ToString("f2"),
                    player.Cam.CurrentVelocity.Z.ToString("f2"));
                buffer.AppendFormat("  Rotation speed: {0}\n",
                    player.Cam.RotationSpeed.ToString("f2"));
                buffer.Append("\nPress H to display help");
            }

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            spriteBatch.DrawString(spriteFont, buffer.ToString(), fontPos, Color.Yellow);
            spriteBatch.End();
        }

        protected override void Draw(GameTime gameTime)
        {
            if (!GameConstants.UpdateWhenTabbed)
            {
                if (!this.IsActive)
                {
                    return;
                }
            }
            GraphicsDevice.Clear(Color.Black);

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            GraphicsDevice.SamplerStates[1] = SamplerState.LinearWrap;
            GraphicsDevice.SamplerStates[2] = SamplerState.LinearWrap;

            //Must draw world first, as the skybox is part of it, and skybox has to be drawn first
            world.Draw(GraphicsDevice);

            if(GameConstants.RenderOnScreenText)
                DrawText();

            base.Draw(gameTime);
            IncrementFrameCounter();
        }
    }
}
