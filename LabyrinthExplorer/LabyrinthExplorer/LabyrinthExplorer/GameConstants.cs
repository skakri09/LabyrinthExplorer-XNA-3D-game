using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace LabyrinthExplorer
{
    static class GameConstants
    {
        public static readonly int windowWidth = 1280;
        public static readonly int windowHeight = 720;

        public static float rotationSpeed = 30.0f;
        public static float moveSpeed = 300.0f;

        public static bool verticalSyncOn = false;

        public static bool mouseVisible = false;

        #region light
        public static Color GlobalAmbientGame = new Color(new Vector4(0.03f, 0.03f, 0.03f, 0.01f));
        public static Color GlobalAmbientDebug = new Color(new Vector4(1.00f, 1.00f, 1.00f, 1.00f));
        public static Color ambient = new Color(new Vector4(0.2f, 0.2f, 0.2f, 0.5f));
        public static Color diffuse = new Color(new Vector4(0.2f, 0.2f, 0.2f, 0.5f));
        public static Color specular = new Color(new Vector4(0.2f, 0.2f, 0.2f, 0.5f));
        public static float SpotInnerConeRadians = MathHelper.ToRadians(10.0f);
        public static float SpotOuterConeRadians = MathHelper.ToRadians(40.0f);
        public static float Radius = 800;
        #endregion
        
        #region Camera
        public const float CAMERA_FOVX = 85.0f;
        public const float CAMERA_ZNEAR = 0.01f;
        public const float CAMERA_ZFAR = FLOOR_PLANE_SIZE * 2.0f; //change to a static value
        public const float CAMERA_BOUNDS_MIN_X = -FLOOR_PLANE_SIZE / 2.0f + CAM_BOUNDS_PADDING;
        public const float CAMERA_BOUNDS_MAX_X = FLOOR_PLANE_SIZE / 2.0f - CAM_BOUNDS_PADDING;
        public const float CAMERA_BOUNDS_MIN_Y = 0.0f;
        public const float CAMERA_BOUNDS_MAX_Y = WALL_HEIGHT;
        public const float CAMERA_BOUNDS_MIN_Z = -FLOOR_PLANE_SIZE / 2.0f + CAM_BOUNDS_PADDING;
        public const float CAMERA_BOUNDS_MAX_Z = FLOOR_PLANE_SIZE / 2.0f - CAM_BOUNDS_PADDING;
        #endregion

        #region Player
        public const float CAM_BOUNDS_PADDING = 30.0f;

        public const float CAMERA_PLAYER_EYE_HEIGHT = 110.0f;
        public const float CAMERA_ACCELERATION_X = 8000.0f;
        public const float CAMERA_ACCELERATION_Y = 800.0f;
        public const float CAMERA_ACCELERATION_Z = 8000.0f;
        public const float CAMERA_VELOCITY_X = 500.0f;
        public const float CAMERA_VELOCITY_Y = 300.0f;
        public const float CAMERA_VELOCITY_Z = 500.0f;
        public const float CAMERA_RUNNING_MULTIPLIER = 2.0f;
        public const float CAMERA_RUNNING_JUMP_MULTIPLIER = 1.5f;

        public const float CANDLE_SCALE = 0.5f;
        public const float CANDLE_X_OFFSET = 0.45f;
        public const float CANDLE_Y_OFFSET = -0.30f;
        public const float CANDLE_Z_OFFSET = 1.65f;
        #endregion
        
        #region tile factors
        public const float WallTileFactorNormalX = 0.6f;
        public const float WallTileFactorNormalY = 2.0f;

        public const float CEILING_TILE_FACTOR = 8.0f;
        public const float FLOOR_PLANE_SIZE = 2024.0f;
        public const float FLOOR_CLIP_BOUNDS = FLOOR_PLANE_SIZE * 0.5f - 30.0f;
        public const float WALL_HEIGHT = 256.0f;

        public const float FLOOR_TILE_FACTOR_SMALL = 1.2f;
        public const float FLOOR_TILE_FACTOR_NORMAL = 0.6f;
        public const float FLOOR_TILE_FACTOR_BIG = 0.3f;
        #endregion
    }
}
