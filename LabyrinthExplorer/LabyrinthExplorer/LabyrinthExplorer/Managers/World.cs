using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using LabyrinthExplorer;

namespace LabyrinthExplorer
{
    public class World
    {
        public static Effect WallsEffect;

        public static Light PlayerLight;
        private Material material;
        private Color globalAmbient;
        private Vector2 scaleBias;
        private Camera camera;

        public static AreaContent currentArea;
        private static Dictionary<string, AreaContent> GameAreas
            = new Dictionary<string, AreaContent>();
        Skybox skybox;

        protected Texture2D brickColorMap;
        protected Texture2D brickNormalMap;
        protected Texture2D brickHeightMap;
        protected Texture2D stoneColorMap;
        protected Texture2D stoneNormalMap;
        protected Texture2D stoneHeightMap;
        protected Texture2D woodColorMap;
        protected Texture2D woodNormalMap;
        protected Texture2D woodHeightMap;

        private bool enableParallax;

        public World(Camera camera)
        {
            this.camera = camera;
            enableParallax = true;

            DrawSkybox = true;
           
            GameAreas.Add("area1", new Area1Content(camera));
            GameAreas.Add("area2", new Area2Content(camera));
            GameAreas.Add("area3", new Area3Content(camera));
            GameAreas.Add("area4", new Area4Content(camera));
        }

        public static void ChangeArea(string targetArea, Vector3 playerTargetPos)
        {
            if (GameAreas.ContainsKey(targetArea))
            {
                Game.SoundManager.StopAllSounds();
                Game.SoundManager.StopSong();
                currentArea = GameAreas[targetArea];
                currentArea.OnEnteringArea();
                Game.player.Cam.Position = playerTargetPos;
            }
        }

        public void Update(GameTime gameTime)
        {
            UpdateEffect();

            currentArea.Update(gameTime, camera);
        }

        public void UpdateEffect()
        {
            
            if (enableParallax)
                WallsEffect.CurrentTechnique = WallsEffect.Techniques["ParallaxNormalMappingPointLighting"];
            else
                WallsEffect.CurrentTechnique = WallsEffect.Techniques["NormalMappingPointLighting"];

            PlayerLight.Position = camera.Position;
            PlayerLight.Direction = camera.ViewDirection;

            WallsEffect.Parameters["worldMatrix"].SetValue(Matrix.Identity);
            WallsEffect.Parameters["worldInverseTransposeMatrix"].SetValue(Matrix.Identity);
            WallsEffect.Parameters["worldViewProjectionMatrix"].SetValue(camera.ViewMatrix * camera.ProjectionMatrix);

            WallsEffect.Parameters["cameraPos"].SetValue(camera.Position);
            WallsEffect.Parameters["globalAmbient"].SetValue(globalAmbient.ToVector4());
            WallsEffect.Parameters["scaleBias"].SetValue(scaleBias);

            WallsEffect.Parameters["light"].StructureMembers["dir"].SetValue(PlayerLight.Direction);
            WallsEffect.Parameters["light"].StructureMembers["pos"].SetValue(PlayerLight.Position);
            WallsEffect.Parameters["light"].StructureMembers["ambient"].SetValue(PlayerLight.Ambient.ToVector4());
            WallsEffect.Parameters["light"].StructureMembers["diffuse"].SetValue(PlayerLight.Diffuse.ToVector4());
            WallsEffect.Parameters["light"].StructureMembers["specular"].SetValue(PlayerLight.Specular.ToVector4());
            WallsEffect.Parameters["light"].StructureMembers["spotInnerCone"].SetValue(PlayerLight.SpotInnerConeRadians);
            WallsEffect.Parameters["light"].StructureMembers["spotOuterCone"].SetValue(PlayerLight.SpotOuterConeRadians);
            WallsEffect.Parameters["light"].StructureMembers["radius"].SetValue(PlayerLight.Radius);

            WallsEffect.Parameters["material"].StructureMembers["ambient"].SetValue(material.Ambient.ToVector4());
            WallsEffect.Parameters["material"].StructureMembers["diffuse"].SetValue(material.Diffuse.ToVector4());
            WallsEffect.Parameters["material"].StructureMembers["emissive"].SetValue(material.Emissive.ToVector4());
            WallsEffect.Parameters["material"].StructureMembers["specular"].SetValue(material.Specular.ToVector4());
            WallsEffect.Parameters["material"].StructureMembers["shininess"].SetValue(material.Shininess);
        }

        public void LoadContent(GraphicsDevice device, ContentManager contentMan)
        {
            WallsEffect = contentMan.Load<Effect>(@"Effects\parallax_normal_mapping");
            WallsEffect.CurrentTechnique = WallsEffect.Techniques["ParallaxNormalMappingPointLighting"];
            LoadMaps(contentMan);

            scaleBias = new Vector2(0.04f, -0.03f);
            
            skybox = new Skybox(contentMan);
            
            GenPlayerLight();

            GenerateMaterials();

            globalAmbient = GameConstants.CurrentAmbientLight;

            foreach (AreaContent area in GameAreas.Values)
            {
                currentArea = area;
                area.LoadContent(device, contentMan);
            }
            
            //ChangeArea("area1", new Vector3(3750, 150, 4500));
            //ChangeArea("area2", new Vector3(400, 150, 650));
            //ChangeArea("area3", new Vector3(2500, 150, 8000));
            ChangeArea("area3", new Vector3(2500, GameConstants.CAMERA_PLAYER_EYE_HEIGHT, 250));
        }

        public void Draw(GraphicsDevice graphicsDevice)
        {
            currentArea.Draw(graphicsDevice, WallsEffect, brickColorMap, brickNormalMap, brickHeightMap,
                stoneColorMap, stoneNormalMap, stoneHeightMap, woodColorMap, woodNormalMap, woodHeightMap);
        }

        public void DrawTheSkybox(GraphicsDevice graphicsDevice)
        {
            if (DrawSkybox)
                skybox.Draw(camera, graphicsDevice);
        }

        private void GenPlayerLight()
        {
            PlayerLight.Type = LightType.DirectionalLight;
            PlayerLight.Direction = camera.ViewDirection;
            PlayerLight.Position = GameConstants.PLAYER_START_POS;
            PlayerLight.Ambient = GameConstants.ambient;
            PlayerLight.Diffuse = GameConstants.diffuse;
            PlayerLight.Specular = GameConstants.specular;
            PlayerLight.SpotInnerConeRadians = GameConstants.SpotInnerConeRadians;
            PlayerLight.SpotOuterConeRadians = GameConstants.SpotOuterConeRadians;
            PlayerLight.Radius = GameConstants.Radius;

        }
       
        private void GenerateMaterials()
        {
            material.Ambient = new Color(new Vector4(0.2f, 0.2f, 0.2f, 1.0f));
            material.Diffuse = new Color(new Vector4(0.2f, 0.2f, 0.2f, 1.0f));
            material.Emissive = Color.Yellow;
            material.Specular = Color.LightYellow;
            material.Shininess = 0.3f;
        }

        public List<AABB> EnvironmentCollidables()
        {
            return currentArea.EnvironmentCollidables();
        }

        public bool EnableParallax
        {
            get { return enableParallax; }
            set { enableParallax = value; }
        }

        public bool DrawSkybox
        {
            get;
            set;
        }

        protected virtual void LoadMaps(ContentManager contentMan)
        {
            brickColorMap = contentMan.Load<Texture2D>(@"Textures\brick_color_map");
            brickNormalMap = contentMan.Load<Texture2D>(@"Textures\brick_normal_map");
            brickHeightMap = contentMan.Load<Texture2D>(@"Textures\brick_height_map");

            stoneColorMap = contentMan.Load<Texture2D>(@"Textures\stone_color_map");
            stoneNormalMap = contentMan.Load<Texture2D>(@"Textures\stone_normal_map");
            stoneHeightMap = contentMan.Load<Texture2D>(@"Textures\stone_height_map");

            //stoneColorMap = contentMan.Load<Texture2D>(@"Textures\tile1a");
            //stoneNormalMap = contentMan.Load<Texture2D>(@"Textures\tile1a_nm");
            //stoneHeightMap = contentMan.Load<Texture2D>(@"Textures\stone_height_map");


            woodColorMap = contentMan.Load<Texture2D>(@"Textures\wood_color_map");
            woodNormalMap = contentMan.Load<Texture2D>(@"Textures\wood_normal_map");
            woodHeightMap = contentMan.Load<Texture2D>(@"Textures\wood_height_map");
        }
    }
}
