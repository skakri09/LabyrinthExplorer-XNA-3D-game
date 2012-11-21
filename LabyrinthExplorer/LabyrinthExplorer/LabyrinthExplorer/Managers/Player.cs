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
    public class Player
    {
        private Matrix[] lightStickTransforms;
        private Matrix lightStickWorldMatrix;
        private Model lightStick;

        private Camera camera;

        private AABB playerAABB;

        private Game game;

        public static AudioListener playerListener;

        private float walkSpeedSteps = 0.45f;
        private float runSpeedSteps = 0.35f;
        private float stepsTimer = 0.0f;

        public Player(Game game, Vector3 position)
        {
             this.game = game;

            camera = new Camera(game);
            game.Components.Add(camera);
            InitializeLightStick(game.Content);
            SetCameraProperties();
            EnableColorMap = true;
            PerformPlayerCollision = true;
            playerAABB = new AABB(Vector3.Zero, GameConstants.CAM_BOUNDS_PADDING);
            playerListener = new AudioListener();
            playerListener.Position = camera.Position;
            playerListener.Forward = camera.ViewDirection;
            playerListener.Up = Vector3.Up;
        }

        public void HandlePlayerInput(InputManager input)
        {
            if (input.IsKeyDownOnce(Keys.C))
                PerformPlayerCollision = !PerformPlayerCollision;

            if (input.IsKeyDownOnce(Keys.E))
            {
                List<AABB> interactables = Interactables.GetInteractablesInRange(playerAABB);
                foreach (AABB aabb in interactables)
                {
                    if (aabb is IInteractableObject)
                    {
                        IInteractableObject obj = (IInteractableObject)aabb;
                        obj.Use(playerAABB);
                    }
                }
            }
            if (input.IsKeyDownOnce(Keys.B))
            {
                Game.SoundManager.PlaySound("footsteps", null, 10);
            }
        }

        public void Update(float deltaTime)
        {
            HandleCollision();
            HandleFootseps(deltaTime);
            playerAABB.UpdateAABB(camera.Position);

            playerListener.Position = camera.Position;
            playerListener.Forward = camera.ViewDirection;
            playerListener.Up = Vector3.Up;
           
            lightStick.CopyAbsoluteBoneTransformsTo(lightStickTransforms);

            lightStickWorldMatrix = camera.WeaponWorldMatrix(GameConstants.CANDLE_X_OFFSET,
                GameConstants.CANDLE_Y_OFFSET, GameConstants.CANDLE_Z_OFFSET, GameConstants.CANDLE_SCALE);
        }

        public void Draw(GraphicsDevice device)
        {
            //Drawing the lightstick
            foreach (ModelMesh m in lightStick.Meshes)
            {
                foreach (BasicEffect e in m.Effects)
                {
                    e.TextureEnabled = EnableColorMap;
                    e.EnableDefaultLighting();
                    e.World = lightStickTransforms[m.ParentBone.Index] * lightStickWorldMatrix;
                    e.View = camera.ViewMatrix;
                    e.Projection = camera.ProjectionMatrix;
                }
                m.Draw();
            }
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
            stepsTimer += deltaTime;

            if (camera.FootMode == Camera.FootstepsMode.RUN)
            {
                if (stepsTimer >= runSpeedSteps)
                {
                    Game.SoundManager.PlaySound("footsteps", 0.5f);
                    stepsTimer -= runSpeedSteps;
                }
            }
            else if (camera.FootMode == Camera.FootstepsMode.WALK)
            {
                if (stepsTimer >= walkSpeedSteps)
                {
                    Game.SoundManager.PlaySound("footsteps", 0.5f);
                    stepsTimer -= walkSpeedSteps;
                }
            }
            else
            {
                stepsTimer = 0;
            }
        }

        public Camera Cam { get { return camera; } }
        public bool EnableColorMap { get; set; }

        public AABB PlayerAABB
        {
            get { return playerAABB; }
        }

        public bool PerformPlayerCollision { get; set; }

    }
}
