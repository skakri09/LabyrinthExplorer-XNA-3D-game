
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
    public enum GameStates { MainMenu, GAME, PAUSE }

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        public static GameStates currentGameState;
        public static bool quitGame = false;

        private Menu menu;

        InputManager input;

        #region startup
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteFont gameFont;
        private SpriteFont menuFont;
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

            menu = new Menu(Content);
            menu.EnterMenu(GameStates.MainMenu, GameStates.MainMenu);
            currentGameState = GameStates.MainMenu;

             // Setup frame buffer.
            graphics.SynchronizeWithVerticalRetrace = false;
            graphics.PreferredBackBufferWidth = GameConstants.windowWidth;
            graphics.PreferredBackBufferHeight = GameConstants.windowHeight;
            graphics.PreferMultiSampling = true;
            graphics.ApplyChanges();

            player = new Player(this, GameConstants.PLAYER_START_POS);

            // Initial position for text rendering.
            fontPos = new Vector2(5.0f, -20.0f);

            world = new World(player.Cam);
            world.LoadContent(GraphicsDevice, Content);
            Services.AddService(typeof(World), world);
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            gameFont = Content.Load<SpriteFont>(@"Fonts\Chiller");
            menuFont = Content.Load<SpriteFont>(@"Fonts\ChillerMenu");

            SoundManager.LoadContent();
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        private void HandleGameInput()
        {
            if (input.IsKeyDown(Keys.Escape))
            {
                currentGameState = GameStates.PAUSE;
                menu.EnterMenu(GameStates.GAME, GameStates.PAUSE);
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
            if (input.IsKeyDownOnce(Keys.Tab))
            {
                player.Cam.ToggleMapMode();
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
            if (quitGame)
            {
                SoundManager.StopAllSounds();
                this.Exit();
            }
            
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            input.Update();
            switch (currentGameState)
            {
                case GameStates.GAME:
                    base.Update(gameTime);
                    
                    HandleGameInput();
                    player.Update(deltaTime);
                    world.Update(gameTime);
                    break;
                case GameStates.MainMenu:
                    menu.UpdateMenu(input);
                    break;
                case GameStates.PAUSE:
                    menu.UpdateMenu(input);
                    break;
            }

            UpdateFrameRate(gameTime);
        }

        private void DrawText()
        {
            StringBuilder buffer = new StringBuilder();

            if (displayHelp)
            {
                buffer.AppendLine();
                buffer.AppendFormat("FPS: {0}\n", framesPerSecond);
                buffer.AppendLine();
                buffer.AppendLine("Press WASD to move");
                buffer.AppendLine("Press E to use/loot/inspect item or environment");
                buffer.AppendLine("Press and hold LEFT CTRL to crouch");
                buffer.AppendLine("Press and hold LEFT SHIFT to run");
                buffer.AppendLine();
                buffer.AppendLine("Press M to toggle mouse smoothing");
                buffer.AppendLine("Press P to toggle between parallax normal mapping and normal mapping");
                buffer.AppendLine("Press NUMPAD +/- to change camera rotation speed");
                buffer.AppendLine("Press ALT + ENTER to toggle full screen");
                buffer.AppendLine();
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

                buffer.AppendLine("Press H to hide help");
            }
            else
            {
                buffer.Append("\nPress H to display help");
            }

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            spriteBatch.DrawString(gameFont, buffer.ToString(), fontPos, Color.Beige);
            spriteBatch.End();
        }

        private void DrawGame(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            GraphicsDevice.SamplerStates[1] = SamplerState.LinearWrap;
            GraphicsDevice.SamplerStates[2] = SamplerState.LinearWrap;

            DepthStencilState dss = new DepthStencilState();
            dss.DepthBufferEnable = false;
            GraphicsDevice.DepthStencilState = dss;


            world.DrawTheSkybox(GraphicsDevice);

            if (World.currentArea.TestCenters != null)
                foreach (Testcenter center in World.currentArea.TestCenters)
                    center.Draw(player.Cam, null);

            dss = new DepthStencilState();
            dss.DepthBufferEnable = true;
            GraphicsDevice.DepthStencilState = dss;


            world.Draw(GraphicsDevice);

            if (GameConstants.RenderOnScreenText)
                DrawText();
            player.Draw(GraphicsDevice);
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

            switch (currentGameState)
            {
                case GameStates.GAME:
                    DrawGame(gameTime);
                    break;
                case GameStates.MainMenu:
                    menu.DrawMenu(spriteBatch, menuFont);
                    break;
                case GameStates.PAUSE:
                    menu.DrawMenu(spriteBatch, menuFont);
                    break;
            }

            base.Draw(gameTime);
            IncrementFrameCounter();
        }
    }
}
