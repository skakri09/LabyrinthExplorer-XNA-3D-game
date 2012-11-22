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

        public static IGameLevel currentLevel;

        Skybox skybox;

        private bool enableParallax;

        public World(Camera camera)
        {
            this.camera = camera;
            enableParallax = true;

            DrawSkybox = true;

            currentLevel = new Level1Content(camera);
        }

        public void Update(GameTime gameTime)
        {
            PlayerLight.Position = camera.Position;

            UpdateEffect();

            currentLevel.Update(gameTime, camera);
            
        }

        public void UpdateEffect()
        {
            
            if (enableParallax)
                WallsEffect.CurrentTechnique = WallsEffect.Techniques["ParallaxNormalMappingPointLighting"];
            else
                WallsEffect.CurrentTechnique = WallsEffect.Techniques["NormalMappingPointLighting"];


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

            scaleBias = new Vector2(0.04f, -0.03f);
            
            skybox = new Skybox(contentMan);
            
            GenPlayerLight();

            GenerateMaterials();

            globalAmbient = GameConstants.CurrentAmbientLight;
            
            currentLevel.LoadContent(device, contentMan);
           // Game.SoundManager.PlaySong("LOD");
            Game.SoundManager.PlaySound("spiderAmbient", 0.7f, null, -1);

        }

        public void Draw(GraphicsDevice graphicsDevice)
        {
            currentLevel.Draw(graphicsDevice, WallsEffect);
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
            PlayerLight.Position = GameConstants.PLAYER_START_POS;// new Vector3(0.0f, GameConstants.WALL_HEIGHT - (0.25f * GameConstants.WALL_HEIGHT), 0.0f);
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
            return currentLevel.EnvironmentCollidables();
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
    }
}
