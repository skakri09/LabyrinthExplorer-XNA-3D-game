using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabyrinthExplorer
{
    static class GameConstants
    {
        public static readonly int windowWidth = 1280;
        public static readonly int windowHeight = 720;

        public static float rotationSpeed = 30.0f;
        public static float moveSpeed = 300.0f;

        #region Camera
        private const float TORCH_SCALE = 0.03f;
        private const float TORCH_X_OFFSET = 0.45f;
        private const float TORCH_Y_OFFSET = -0.75f;
        private const float TORCH_Z_OFFSET = 1.65f;

        private const float CAMERA_FOVX = 85.0f;
        private const float CAMERA_ZNEAR = 0.01f;
        private const float CAMERA_ZFAR = 1000;
        private const float CAMERA_PLAYER_EYE_HEIGHT = 110.0f;
        private const float CAMERA_ACCELERATION_X = 800.0f;
        private const float CAMERA_ACCELERATION_Y = 800.0f;
        private const float CAMERA_ACCELERATION_Z = 800.0f;
        private const float CAMERA_VELOCITY_X = 200.0f;
        private const float CAMERA_VELOCITY_Y = 200.0f;
        private const float CAMERA_VELOCITY_Z = 200.0f;
        private const float CAMERA_RUNNING_MULTIPLIER = 2.0f;
        #endregion
    }
}
