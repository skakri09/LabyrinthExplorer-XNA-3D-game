using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace LabyrinthExplorer
{
    public class Player : AABB
    {
        private Matrix[] lightStickTransforms;
        private Matrix lightStickWorldMatrix;
        private Model lightStick;

        public static Inventory inventory;

        private static Camera camera;

        public Game game;

        private static AudioListener playerListener;
        public static AudioListener PlayerListener
        {
            get
            {
                AudioListener newListener = new AudioListener();
                newListener.Position = camera.Position;
                newListener.Up = Vector3.Up;
                newListener.Forward = camera.ViewDirection;
                return newListener;
            }
        }
        private float walkSpeedSteps = 0.45f;
        private float runSpeedSteps = 0.35f;
        private float stepsTimer = 0.0f;

        public bool MakeFootstepSound
        {
            set;
            get;
        }

        public Player(Game game, Vector3 position)
            : base(Vector3.Zero, GameConstants.CAM_BOUNDS_PADDING)
        {
            this.game = game;
            camera = new Camera(game);
            game.Components.Add(camera);
            InitializeLightStick(game.Content);
            SetCameraProperties();
            EnableColorMap = true;
            PerformPlayerCollision = true;
            playerListener = new AudioListener();
            playerListener.Position = camera.Position;
            playerListener.Forward = camera.ViewDirection;
            playerListener.Up = Vector3.Up;
            inventory = new Inventory();
            PlayerAbleToMove = true;
            MakeFootstepSound = true;
        }

        public void HandlePlayerInput(InputManager input)
        {
            if (input.IsKeyDownOnce(Keys.C))
                PerformPlayerCollision = !PerformPlayerCollision;

            if (input.IsKeyDownOnce(Keys.E))
            {
                List<AABB> interactables = Interactables.GetInteractablesInRange(this);
                foreach (AABB aabb in interactables)
                {
                    if (aabb is IInteractableObject)
                    {
                        IInteractableObject obj = (IInteractableObject)aabb;
                        obj.Use(this);
                    }
                }
            }
        }

        public void Update(float deltaTime)
        {
            HandleCollision();
            HandleFootseps(deltaTime);
            UpdateAABB(camera.Position);
            playerListener.Position = camera.Position;
            playerListener.Forward = camera.ViewDirection;
            playerListener.Up = Vector3.Up;
           
            lightStick.CopyAbsoluteBoneTransformsTo(lightStickTransforms);

            lightStickWorldMatrix = camera.WeaponWorldMatrix(GameConstants.CANDLE_X_OFFSET,
                GameConstants.CANDLE_Y_OFFSET, GameConstants.CANDLE_Z_OFFSET, GameConstants.CANDLE_SCALE);
            inv.Update(deltaTime, camera);
        }

        public void Draw(GraphicsDevice device)
        {
            inv.DrawInventory(device, camera);

            //Drawing the lightstick
            //foreach (ModelMesh m in lightStick.Meshes)
            //{
            //    foreach (BasicEffect e in m.Effects)
            //    {
            //        e.EnableDefaultLighting();
            //        //e.DiffuseColor = new Vector3(0.8f, 0.8f, 0.8f);
            //        //e.AmbientLightColor = new Vector3(0.8f, 0.8f, 0.8f);
            //        //e.SpecularColor = new Vector3(0.8f, 0.8f, 0.8f);

            //        e.World = lightStickTransforms[m.ParentBone.Index] * lightStickWorldMatrix;
            //        e.View = camera.ViewMatrix;
            //        e.Projection = camera.ProjectionMatrix;
            //    }
            //    m.Draw();
            //}
        }

        private void HandleCollision()
        {
            Vector3 newPos = camera.Position;

            //if (camera.Position.Y > GameConstants.CAMERA_BOUNDS_MAX_Y)
            //    newPos.Y = GameConstants.CAMERA_BOUNDS_MAX_Y;

            if (camera.Position.Y < GameConstants.CAMERA_BOUNDS_MIN_Y)
                newPos.Y = GameConstants.CAMERA_BOUNDS_MIN_Y;

            World world = (World)game.Services.GetService(typeof(World));

            if (PerformPlayerCollision)
            {
                
                foreach (AABB aabb in world.EnvironmentCollidables())
                {
                    Vector3 collision = PlayerAABB.CheckCollision(aabb);
                    if (collision != Vector3.Zero)
                    {
                        newPos += collision;
                    }
                }
            }
            camera.Position = newPos;
        }

        private void InitializeLightStick(ContentManager content)
        {
            lightStick = content.Load<Model>(@"Models\LightStick");
            lightStickTransforms = new Matrix[lightStick.Bones.Count];
            lightStickWorldMatrix = Matrix.Identity;

        }
        
        private void SetCameraProperties()
        {
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
        }

        private void HandleFootseps(float deltaTime)
        {
            if (MakeFootstepSound)
            {
                stepsTimer += deltaTime;

                if (camera.FootMode == Camera.FootstepsMode.RUN)
                {
                    if (stepsTimer >= runSpeedSteps)
                    {
                        Game.SoundManager.PlaySound("footsteps", 0.9f);
                        stepsTimer -= runSpeedSteps;
                    }
                }
                else if (camera.FootMode == Camera.FootstepsMode.WALK)
                {
                    if (stepsTimer >= walkSpeedSteps)
                    {
                        Game.SoundManager.PlaySound("footsteps", 0.9f);
                        stepsTimer -= walkSpeedSteps;
                    }
                }
                else
                {
                    stepsTimer = 0;
                }
            }
        }

        public Camera Cam { get { return camera; } }
        public Inventory inv { get { return inventory; } }


        public bool PlayerAbleToMove
        {
            get { return camera.CanMoveWithControlKeys; }
            set { camera.CanMoveWithControlKeys = value; }
        }

        public bool EnableColorMap { get; set; }

        public AABB PlayerAABB
        {
            get { return this; }
        }

        public bool PerformPlayerCollision { get; set; }

    }
}
