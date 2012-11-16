
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
        private const float CEILING_TILE_FACTOR = 8.0f;
        private const float FLOOR_PLANE_SIZE = 2024.0f;
        private const float FLOOR_TILE_FACTOR = 8.0f;
        private const float FLOOR_CLIP_BOUNDS = FLOOR_PLANE_SIZE * 0.5f - 30.0f;
        private const float WALL_HEIGHT = 256.0f;
        private const float WALL_TILE_FACTOR_X = 8.0f;
        private const float WALL_TILE_FACTOR_Y = 2.0f;

        private const float WEAPON_SCALE = 0.5f;
        private const float WEAPON_X_OFFSET = 0.45f;
        private const float WEAPON_Y_OFFSET = -0.30f;
        private const float WEAPON_Z_OFFSET = 1.65f;

        private const float CAMERA_FOVX = 85.0f;
        private const float CAMERA_ZNEAR = 0.01f;
        private const float CAMERA_ZFAR = FLOOR_PLANE_SIZE * 2.0f;
        private const float CAMERA_PLAYER_EYE_HEIGHT = 110.0f;
        private const float CAMERA_ACCELERATION_X = 800.0f;
        private const float CAMERA_ACCELERATION_Y = 800.0f;
        private const float CAMERA_ACCELERATION_Z = 800.0f;
        private const float CAMERA_VELOCITY_X = 200.0f;
        private const float CAMERA_VELOCITY_Y = 200.0f;
        private const float CAMERA_VELOCITY_Z = 200.0f;
        private const float CAMERA_RUNNING_MULTIPLIER = 2.0f;
        private const float CAMERA_RUNNING_JUMP_MULTIPLIER = 1.5f;
        private const float CAMERA_BOUNDS_PADDING = 30.0f;
        private const float CAMERA_BOUNDS_MIN_X = -FLOOR_PLANE_SIZE / 2.0f + CAMERA_BOUNDS_PADDING;
        private const float CAMERA_BOUNDS_MAX_X = FLOOR_PLANE_SIZE / 2.0f - CAMERA_BOUNDS_PADDING;
        private const float CAMERA_BOUNDS_MIN_Y = 0.0f;
        private const float CAMERA_BOUNDS_MAX_Y = WALL_HEIGHT;
        private const float CAMERA_BOUNDS_MIN_Z = -FLOOR_PLANE_SIZE / 2.0f + CAMERA_BOUNDS_PADDING;
        private const float CAMERA_BOUNDS_MAX_Z = FLOOR_PLANE_SIZE / 2.0f - CAMERA_BOUNDS_PADDING;

        private Texture2D nullTexture;
        private Texture2D brickColorMap;
        private Texture2D brickNormalMap;
        private Texture2D brickHeightMap;
        private Texture2D stoneColorMap;
        private Texture2D stoneNormalMap;
        private Texture2D stoneHeightMap;
        private Texture2D woodColorMap;
        private Texture2D woodNormalMap;
        private Texture2D woodHeightMap;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;
        private Effect effect;
        private KeyboardState currentKeyboardState;
        private KeyboardState prevKeyboardState;
        private Camera camera;
        private NormalMappedRoom room;
        private Model weapon;
        private Matrix[] weaponTransforms;
        private Matrix weaponWorldMatrix;
        private Light light;
        private Material material;
        private Color globalAmbient;
        private Vector2 scaleBias;
        private Vector2 fontPos;
        private int frames;
        private int framesPerSecond;
        private TimeSpan elapsedTime = TimeSpan.Zero;
        private bool enableColorMap;
        private bool enableParallax;
        private bool displayHelp;

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
            Window.Title += "....hahahaha NOT!";

            IsFixedTimeStep = GameConstants.verticalSyncOn;                                                          
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

            // Initially enable parallax mapping.
            enableParallax = true;

            // Initial position for text rendering.
            fontPos = new Vector2(1.0f, 1.0f);

            // Parallax mapping height scale and bias values.
            scaleBias = new Vector2(0.04f, -0.03f);

            // Initialize point lighting for the scene.
            globalAmbient = new Color(new Vector4(0.03f, 0.03f, 0.03f, 0.01f));
            light.Type = LightType.DirectionalLight;
            light.Direction = camera.ViewDirection;
            light.Position = new Vector3(0.0f, WALL_HEIGHT - (0.25f * WALL_HEIGHT), 0.0f);
            light.Ambient = new Color(new Vector4(0.2f, 0.2f, 0.2f, 0.5f));
            light.Diffuse = new Color(new Vector4(0.2f, 0.2f, 0.2f, 0.5f));
            light.Specular = new Color(new Vector4(0.2f, 0.2f, 0.2f, 0.5f));
            light.SpotInnerConeRadians = MathHelper.ToRadians(10.0f);
            light.SpotOuterConeRadians = MathHelper.ToRadians(40.0f);
            light.Radius = 800;

            // Initialize material settings. Just a plain lambert material.
            material.Ambient = new Color(new Vector4(0.2f, 0.2f, 0.2f, 1.0f));
            material.Diffuse = new Color(new Vector4(0.8f, 0.8f, 0.8f, 1.0f));
            material.Emissive = Color.Black;
            material.Specular = Color.White;
            material.Shininess = 0.0f;

            // Create the room.
            room = new NormalMappedRoom(GraphicsDevice,
                    FLOOR_PLANE_SIZE, WALL_HEIGHT, FLOOR_TILE_FACTOR,
                    CEILING_TILE_FACTOR, WALL_TILE_FACTOR_X, WALL_TILE_FACTOR_Y);

            // Setup the camera.
            camera.EyeHeightStanding = CAMERA_PLAYER_EYE_HEIGHT;
            camera.Acceleration = new Vector3(
                CAMERA_ACCELERATION_X,
                CAMERA_ACCELERATION_Y,
                CAMERA_ACCELERATION_Z);
            camera.VelocityWalking = new Vector3(
                CAMERA_VELOCITY_X,
                CAMERA_VELOCITY_Y,
                CAMERA_VELOCITY_Z);
            camera.VelocityRunning = new Vector3(
                camera.VelocityWalking.X * CAMERA_RUNNING_MULTIPLIER,
                camera.VelocityWalking.Y * CAMERA_RUNNING_JUMP_MULTIPLIER,
                camera.VelocityWalking.Z * CAMERA_RUNNING_MULTIPLIER);
            camera.Perspective(
                CAMERA_FOVX,
                (float)GameConstants.windowWidth / (float)GameConstants.windowHeight,
                CAMERA_ZNEAR, CAMERA_ZFAR);

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
            
            effect = Content.Load<Effect>(@"Effects\parallax_normal_mapping");
            effect.CurrentTechnique = effect.Techniques["ParallaxNormalMappingPointLighting"];

            brickColorMap = Content.Load<Texture2D>(@"Textures\brick_color_map");
            brickNormalMap = Content.Load<Texture2D>(@"Textures\brick_normal_map");
            brickHeightMap = Content.Load<Texture2D>(@"Textures\brick_height_map");

            stoneColorMap = Content.Load<Texture2D>(@"Textures\stone_color_map");
            stoneNormalMap = Content.Load<Texture2D>(@"Textures\stone_normal_map");
            stoneHeightMap = Content.Load<Texture2D>(@"Textures\stone_height_map");

            woodColorMap = Content.Load<Texture2D>(@"Textures\wood_color_map");
            woodNormalMap = Content.Load<Texture2D>(@"Textures\wood_normal_map");
            woodHeightMap = Content.Load<Texture2D>(@"Textures\wood_height_map");

            weapon = Content.Load<Model>(@"Models\LightStick");

            // Create an empty white texture. This will be bound to the
            // colorMapTexture shader parameter when the user wants to
            // disable the color map texture. This trick will allow the
            // same shader to be used for when textures are enabled and
            // disabled.

            nullTexture = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);

            Color[] pixels = { Color.White };

            nullTexture.SetData(pixels);
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
                enableParallax = !enableParallax;

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
            if (input.IsKeyDown(Keys.LeftAlt) || input.IsKeyDown(Keys.RightAlt))
            {
                if (input.IsKeyDownOnce(Keys.Enter))
                    ToggleFullScreen();
            }
        }
        
        private void UpdateEffect()
        {
            if (enableParallax)
                effect.CurrentTechnique = effect.Techniques["ParallaxNormalMappingPointLighting"];
            else
                effect.CurrentTechnique = effect.Techniques["NormalMappingPointLighting"];

            effect.Parameters["worldMatrix"].SetValue(Matrix.Identity);
            effect.Parameters["worldInverseTransposeMatrix"].SetValue(Matrix.Identity);
            effect.Parameters["worldViewProjectionMatrix"].SetValue(camera.ViewMatrix * camera.ProjectionMatrix);

            effect.Parameters["cameraPos"].SetValue(camera.Position);
            effect.Parameters["globalAmbient"].SetValue(globalAmbient.ToVector4());
            effect.Parameters["scaleBias"].SetValue(scaleBias);

            effect.Parameters["light"].StructureMembers["dir"].SetValue(light.Direction);
            effect.Parameters["light"].StructureMembers["pos"].SetValue(light.Position);
            effect.Parameters["light"].StructureMembers["ambient"].SetValue(light.Ambient.ToVector4());
            effect.Parameters["light"].StructureMembers["diffuse"].SetValue(light.Diffuse.ToVector4());
            effect.Parameters["light"].StructureMembers["specular"].SetValue(light.Specular.ToVector4());
            effect.Parameters["light"].StructureMembers["spotInnerCone"].SetValue(light.SpotInnerConeRadians);
            effect.Parameters["light"].StructureMembers["spotOuterCone"].SetValue(light.SpotOuterConeRadians);
            effect.Parameters["light"].StructureMembers["radius"].SetValue(light.Radius);

            effect.Parameters["material"].StructureMembers["ambient"].SetValue(material.Ambient.ToVector4());
            effect.Parameters["material"].StructureMembers["diffuse"].SetValue(material.Diffuse.ToVector4());
            effect.Parameters["material"].StructureMembers["emissive"].SetValue(material.Emissive.ToVector4());
            effect.Parameters["material"].StructureMembers["specular"].SetValue(material.Specular.ToVector4());
            effect.Parameters["material"].StructureMembers["shininess"].SetValue(material.Shininess);
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

            weaponWorldMatrix = camera.WeaponWorldMatrix(WEAPON_X_OFFSET,
                WEAPON_Y_OFFSET, WEAPON_Z_OFFSET, WEAPON_SCALE);
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

            camera.Perspective(CAMERA_FOVX, aspectRatio, CAMERA_ZNEAR, CAMERA_ZFAR);
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

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);

            light.Position = camera.Position;

            input.Update();
            HandleInput();
            UpdateWeapon();
            UpdateEffect();
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
                    (enableParallax ? "Parallax normal mapping" : "Normal mapping"));
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

            // Draw the room.
            if (enableColorMap)
            {
                room.Draw(GraphicsDevice, effect,
                    "colorMapTexture", "normalMapTexture", "heightMapTexture",
                    brickColorMap, brickNormalMap, brickHeightMap,
                    stoneColorMap, stoneNormalMap, stoneHeightMap,
                    woodColorMap, woodNormalMap, woodHeightMap);
            }
            else
            {
                room.Draw(GraphicsDevice, effect,
                    "colorMapTexture", "normalMapTexture", "heightMapTexture",
                    nullTexture, brickNormalMap, brickHeightMap,
                    nullTexture, stoneNormalMap, stoneHeightMap,
                    nullTexture, woodNormalMap, woodHeightMap);
            }

            // Draw the weapon.
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

            DrawText();
            base.Draw(gameTime);
            IncrementFrameCounter();
        }
    }
}
