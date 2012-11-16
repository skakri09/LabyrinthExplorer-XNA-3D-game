using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LabyrinthExplorer
{
    class Camera
    {
        private Game game;
        private InputManager input;

        Vector3 cameraPosition = new Vector3(130, 30, -50);
        float leftrightRot = MathHelper.PiOver2;
        float updownRot = -MathHelper.Pi / 10.0f;

        float moveVel = GameConstants.moveSpeed;
        float rotVel = GameConstants.rotationSpeed;

        Matrix viewMatrix;
        public Camera(Game game)
        {
            this.game = game;
            input = (InputManager)game.Services.GetService(typeof(IInputService));
        }

        public void Update(float deltaTime)
        {
            ProcessInput(deltaTime);
        }

        private void UpdateViewMatrix()
        {
            Matrix cameraRotation = Matrix.CreateRotationX(updownRot) * Matrix.CreateRotationY(leftrightRot);

            Vector3 cameraOriginalTarget = new Vector3(0, 0, -1);
            Vector3 cameraRotatedTarget = Vector3.Transform(cameraOriginalTarget, cameraRotation);
            Vector3 cameraFinalTarget = cameraPosition + cameraRotatedTarget;

            Vector3 cameraOriginalUpVector = new Vector3(0, 1, 0);
            Vector3 cameraRotatedUpVector = Vector3.Transform(cameraOriginalUpVector, cameraRotation);

            viewMatrix = Matrix.CreateLookAt(cameraPosition, cameraFinalTarget, cameraRotatedUpVector);
        }

        private void ProcessInput(float deltaTime)
        {
            Vector3 moveVector = new Vector3(0, 0, 0);

            if (input.IsKeyDown(Keys.Up) || input.IsKeyDown(Keys.W))
                moveVector += new Vector3(0, 0, -1);
            if (input.IsKeyDown(Keys.Down) || input.IsKeyDown(Keys.S))
                moveVector += new Vector3(0, 0, 1);
            if (input.IsKeyDown(Keys.Right) || input.IsKeyDown(Keys.D))
                moveVector += new Vector3(1, 0, 0);
            if (input.IsKeyDown(Keys.Left) || input.IsKeyDown(Keys.A))
                moveVector += new Vector3(-1, 0, 0);
            if (input.IsKeyDown(Keys.Q))
                moveVector += new Vector3(0, 1, 0);
            if (input.IsKeyDown(Keys.Z))
                moveVector += new Vector3(0, -1, 0);

            AddToCameraPosition(moveVector * deltaTime);
        }

        private void AddToCameraPosition(Vector3 vectorToAdd)
        {
            Matrix cameraRotation = Matrix.CreateRotationX(updownRot) * Matrix.CreateRotationY(leftrightRot);
            Vector3 rotatedVector = Vector3.Transform(vectorToAdd, cameraRotation);
            cameraPosition += moveVel * rotatedVector;
            UpdateViewMatrix();
        }


    }
}
