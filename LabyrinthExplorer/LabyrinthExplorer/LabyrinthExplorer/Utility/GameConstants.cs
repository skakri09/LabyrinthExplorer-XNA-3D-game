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

        public static bool verticalSyncOn = true;

        public static bool mouseVisible = false;

        public const float MAP_SCALE = 1.0f;

        public static bool UpdateWhenTabbed = true;

        public static bool RenderOnScreenText = true;

        #region light
        public static Color GlobalAmbientGame = new Color(new Vector4(0.1f, 0.1f, 0.1f, 0.01f));
        public static Color GlobalAmbientDebug = new Color(new Vector4(1.00f, 1.00f, 1.00f, 1.00f));
        public static Color CurrentAmbientLight = GlobalAmbientDebug;

        public static Color ambient = new Color(new Vector4(0.2f, 0.2f, 0.2f, 0.5f));
        public static Color diffuse = new Color(new Vector4(0.2f, 0.2f, 0.2f, 0.5f));
        public static Color specular = new Color(new Vector4(0.2f, 0.2f, 0.2f, 0.5f));
        public static float SpotInnerConeRadians = MathHelper.ToRadians(100.0f);
        public static float SpotOuterConeRadians = MathHelper.ToRadians(10000.0f);
        public static float Radius = 1000;
        #endregion
        
        #region Camera
        public const float CAMERA_FOVX = 90.0f;
        public const float CAMERA_ZNEAR = 5.0f;
        public const float CAMERA_ZFAR = 15000*MAP_SCALE; ///Reduce when done !
        public const float CAMERA_BOUNDS_MIN_X = -FLOOR_PLANE_SIZE / 2.0f + CAM_BOUNDS_PADDING;
        public const float CAMERA_BOUNDS_MAX_X = FLOOR_PLANE_SIZE / 2.0f - CAM_BOUNDS_PADDING;
        public const float CAMERA_BOUNDS_MIN_Y = 0.0f;
        public const float CAMERA_BOUNDS_MAX_Y = WALL_HEIGHT;
        public const float CAMERA_BOUNDS_MIN_Z = -FLOOR_PLANE_SIZE / 2.0f + CAM_BOUNDS_PADDING;
        public const float CAMERA_BOUNDS_MAX_Z = FLOOR_PLANE_SIZE / 2.0f - CAM_BOUNDS_PADDING;
        #endregion

        #region Player
        public const float CAM_BOUNDS_PADDING = 30.0f;
        public const float CAMERA_PLAYER_EYE_HEIGHT = 150;
        
        //public const float CAMERA_ACCELERATION_X = 800.0f;
        //public const float CAMERA_ACCELERATION_Y = 800.0f;
        //public const float CAMERA_ACCELERATION_Z = 800.0f;
        //public const float CAMERA_VELOCITY_X = 150.0f;
        //public const float CAMERA_VELOCITY_Y = 200.0f;
        //public const float CAMERA_VELOCITY_Z = 300.0f;

        //public static Vector3 PLAYER_START_POS = new Vector3(8000, 0, 8750);
        public static Vector3 PLAYER_START_POS = new Vector3(4100, 0, 4500);
        public const float CAMERA_ACCELERATION_X = 8000.0f;
        public const float CAMERA_ACCELERATION_Y = 5000.0f;
        public const float CAMERA_ACCELERATION_Z = 8000.0f;
        public const float CAMERA_VELOCITY_X = 1000.0f;
        public const float CAMERA_VELOCITY_Y = 1000.0f;
        public const float CAMERA_VELOCITY_Z = 1000.0f;

        public const float CAMERA_RUNNING_MULTIPLIER = 4.0f;
        public const float CAMERA_RUNNING_JUMP_MULTIPLIER = 1.5f;

        public const float CANDLE_SCALE = 0.5f;
        public const float CANDLE_X_OFFSET = 0.45f;
        public const float CANDLE_Y_OFFSET = -0.30f;
        public const float CANDLE_Z_OFFSET = 1.65f;

        #endregion
        
        #region tile factors
        private const float foo = WALL_HEIGHT / 256;
        public const float WallTileFactorNormalX = 0.3f;
        public const float WallTileFactorNormalY = 1.5f * foo;

        public const float CEILING_TILE_FACTOR = 8.0f;
        public const float FLOOR_PLANE_SIZE = 2024.0f;
        public const float FLOOR_CLIP_BOUNDS = FLOOR_PLANE_SIZE * 0.5f - 30.0f;
        public const float WALL_HEIGHT = 500.0f;

       

        public const float FLOOR_TILE_FACTOR_SMALL = 1.2f;
        public const float FLOOR_TILE_FACTOR_NORMAL = 0.4f;
        public const float FLOOR_TILE_FACTOR_BIG = 0.3f;
        #endregion
    }
}
 