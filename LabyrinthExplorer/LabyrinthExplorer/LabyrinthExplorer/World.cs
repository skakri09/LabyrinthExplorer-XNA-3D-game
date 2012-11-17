using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using LabyrinthExplorer.EasyWalls;

namespace LabyrinthExplorer
{
    public class World
    {
      
        #region textures
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
        private Effect effect;
        #endregion

        #region environment
        private List<SolidWall> walls;
        private List<NormalMappedCeiling> ceilings;
        private List<NormalMappedFloor> floors;
        private List<Light> lights;

        private Light light;

        private Material material;
        private Color globalAmbient;
        private Vector2 scaleBias;
        #endregion

        private bool enableParallax;
        private Camera camera;

        public World(Camera camera)
        {
            this.camera = camera;
            enableParallax = true;
            walls = new List<SolidWall>();
            ceilings = new List<NormalMappedCeiling>();
            floors = new List<NormalMappedFloor>();
            lights = new List<Light>();
        }

        public void Update(float deltaTime)
        {
            light.Position = camera.Position;

            UpdateEffect();
        }

        public void UpdateEffect()
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

        public void LoadContent(GraphicsDevice device, ContentManager contentMan)
        {
            effect = contentMan.Load<Effect>(@"Effects\parallax_normal_mapping");
            effect.CurrentTechnique = effect.Techniques["ParallaxNormalMappingPointLighting"];

            brickColorMap = contentMan.Load<Texture2D>(@"Textures\brick_color_map");
            brickNormalMap = contentMan.Load<Texture2D>(@"Textures\brick_normal_map");
            brickHeightMap = contentMan.Load<Texture2D>(@"Textures\brick_height_map");

            stoneColorMap = contentMan.Load<Texture2D>(@"Textures\stone_color_map");
            stoneNormalMap = contentMan.Load<Texture2D>(@"Textures\stone_normal_map");
            stoneHeightMap = contentMan.Load<Texture2D>(@"Textures\stone_height_map");

            woodColorMap = contentMan.Load<Texture2D>(@"Textures\wood_color_map");
            woodNormalMap = contentMan.Load<Texture2D>(@"Textures\wood_normal_map");
            woodHeightMap = contentMan.Load<Texture2D>(@"Textures\wood_height_map");

            scaleBias = new Vector2(0.04f, -0.03f);

            GenerateWorld(device); ;
        }

        public void Draw(GraphicsDevice graphicsDevice)
        {
            //           ------ Available assets ------                     
            //  "colorMapTexture", "normalMapTexture", "heightMapTexture",
            //  brickColorMap,      brickNormalMap,     brickHeightMap,
            //  stoneColorMap,      stoneNormalMap,     stoneHeightMap,
            //  woodColorMap,       woodNormalMap,      woodHeightMap
            foreach (SolidWall wall in walls)
            {
                wall.Draw(graphicsDevice, effect, "colorMapTexture",
                        "normalMapTexture", "heightMapTexture",
                        stoneColorMap, stoneNormalMap, stoneHeightMap);
            }
            foreach (NormalMappedCeiling ceiling in ceilings)
            {
                ceiling.Draw(graphicsDevice, effect, "colorMapTexture",
                        "normalMapTexture", "heightMapTexture",
                        woodColorMap, woodNormalMap, woodHeightMap);
            }
            foreach (NormalMappedFloor floor in floors)
            {
                floor.Draw(graphicsDevice, effect, "colorMapTexture",
                            "normalMapTexture", "heightMapTexture",
                            stoneColorMap, stoneNormalMap, stoneHeightMap);
            }
        }

        private void GenerateWorld(GraphicsDevice device)
        {
            GenerateWalls(device);

            GenerateFloorAndCeiling(device);

            GenerateLights();

            GenerateMaterials();

            globalAmbient = GameConstants.CurrentAmbientLight;
        }

        private void GenerateWalls(GraphicsDevice device)
        {
            #region outer wall
            //+Z outer wall
            walls.Add(new SolidWall(device,
                new Vector2(-5000, 5050), new Vector2(5000, 5050),
                new Vector2(5000, 5000), new Vector2(-5000, 5000), GameConstants.WALL_HEIGHT));

            //-Z outer wall
            walls.Add(new SolidWall(device,
                 new Vector2(-5000, -5000), new Vector2(5000, -5000),
                 new Vector2(5000, -5050), new Vector2(-5000, -5050), GameConstants.WALL_HEIGHT));
            
            //+X outer wall

            walls.Add(new SolidWall(device,
                new Vector2(5000, 5000), new Vector2(5050, 5000),
                new Vector2(5050, -5000), new Vector2(5000, -5000), GameConstants.WALL_HEIGHT));
            //-X outer wall
            walls.Add(new SolidWall(device,
                 new Vector2(-5050, 5000), new Vector2(-5000, 5000),
                 new Vector2(-5000, -5000), new Vector2(-5050, -5000), GameConstants.WALL_HEIGHT));

            #endregion

            //XWallNegZ = grows out on negative Z
            //XWallPosZ = grows out on positive Z
            //ZWallNegX = grows out on negative X
            //ZWallPosX = grows out on positive X

            //Z Walls - ZWallPosX
            //X Walls - XWallNegZAl
            
            #region area 1
            walls.Add(new ZWallPosX(device, new Vector2(4200, 4600), new Vector2(4200, 4200)));//1
            walls.Add(new ZWallPosX(device, new Vector2(2600, 4600), new Vector2(2600, 3900)));//2
            walls.Add(new ZWallPosX(device, new Vector2(2000, 4000), new Vector2(2000, 3500)));//3
            walls.Add(new ZWallPosX(device, new Vector2(3200, 4200), new Vector2(3200, 3500)));//4


            walls.Add(new XWallPosZ(device, new Vector2(1500, 4300), new Vector2(2600, 4300)));//5
            walls.Add(new XWallPosZ(device, new Vector2(2050, 3500), new Vector2(3200, 3500)));//6
            walls.Add(new XWallPosZ(device, new Vector2(2600, 4600), new Vector2(4200, 4600)));//7
            walls.Add(new XWallPosZ(device, new Vector2(3200, 4200), new Vector2(4200, 4200)));//8

            walls.Add(new ZWallPosX(device, new Vector2(1500, 4300), new Vector2(1500, 3800)));//9
            walls.Add(new ZWallPosX(device, new Vector2(3700, 4200), new Vector2(3700, 3500)));//10

            walls.Add(new XWallPosZ(device, new Vector2(3200, 4200), new Vector2(4200, 4200)));//11

            #endregion


            //walls.Add(new ZWallNegX(device, new Vector2(200, 0), new Vector2(200, -5000)));

            //walls.Add(new SolidWall(device,
            //    new Vector2(0, 0), new Vector2(50, 0),
            //    new Vector2(50,-5000), new Vector2(0, -5000), 256));

        }

        private void GenerateFloorAndCeiling(GraphicsDevice device)
        {
            floors.Add(new NormalMappedFloor(device,
                new Vector3(-5000, 0, 5000), new Vector3(5000, 0, 5000),
                new Vector3(5000, 0, -5000), new Vector3(-5000, 0, -5000), Vector3.Up));

            ceilings.Add(new NormalMappedCeiling(device,
                new Vector3(-5000, GameConstants.WALL_HEIGHT, 5000), new Vector3(5000, GameConstants.WALL_HEIGHT, 5000),
                new Vector3(5000, GameConstants.WALL_HEIGHT, -5000), new Vector3(-5000, GameConstants.WALL_HEIGHT, -5000), Vector3.Down));

        }

        private void GenerateLights()
        {
            light.Type = LightType.DirectionalLight;
            light.Direction = camera.ViewDirection;
            light.Position = new Vector3(0.0f, GameConstants.WALL_HEIGHT - (0.25f * GameConstants.WALL_HEIGHT), 0.0f);
            light.Ambient = GameConstants.ambient;
            light.Diffuse = GameConstants.diffuse;
            light.Specular = GameConstants.specular;
            light.SpotInnerConeRadians = GameConstants.SpotInnerConeRadians;
            light.SpotOuterConeRadians = GameConstants.SpotOuterConeRadians;
            light.Radius = GameConstants.Radius;

        }
       
        private void GenerateMaterials()
        {
            material.Ambient = new Color(new Vector4(0.2f, 0.2f, 0.2f, 1.0f));
            material.Diffuse = new Color(new Vector4(0.8f, 0.8f, 0.8f, 1.0f));
            material.Emissive = Color.Black;
            material.Specular = Color.White;
            material.Shininess = 0.0f;
        }

        public List<SolidWall> Walls
        {
            get { return walls; }
        }

        public bool EnableParallax
        {
            get { return enableParallax; }
            set { enableParallax = value; }
        }
    }
}
