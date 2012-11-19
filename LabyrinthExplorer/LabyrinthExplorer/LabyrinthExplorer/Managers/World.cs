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
        Model ctrlRoom;
        Skybox skybox;
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
            ctrlRoom = contentMan.Load<Model>(@"Models\ControlRoom");
            scaleBias = new Vector2(0.04f, -0.03f);
            skybox = new Skybox(contentMan);
            GenerateWorld(device);
        }

        public void Draw(GraphicsDevice graphicsDevice)
        {
            //           ------ Available assets ------                     
            //  "colorMapTexture", "normalMapTexture", "heightMapTexture",
            //  brickColorMap,      brickNormalMap,     brickHeightMap,
            //  stoneColorMap,      stoneNormalMap,     stoneHeightMap,
            //  woodColorMap,       woodNormalMap,      woodHeightMap
            skybox.Draw(camera, graphicsDevice);

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
                        stoneColorMap, stoneNormalMap, stoneHeightMap);
            }
            foreach (NormalMappedFloor floor in floors)
            {
                floor.Draw(graphicsDevice, effect, "colorMapTexture",
                            "normalMapTexture", "heightMapTexture",
                            stoneColorMap, stoneNormalMap, stoneHeightMap);
            }

            Matrix[] transforms = new Matrix[ctrlRoom.Bones.Count];
            ctrlRoom.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in ctrlRoom.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = Matrix.Identity
                        * transforms[mesh.ParentBone.Index]
                        //* Matrix.CreateRotationX(MathHelper.ToRadians(rotation.X))
                        //* Matrix.CreateRotationY(MathHelper.ToRadians(rotation.Y))
                        //* Matrix.CreateRotationZ(MathHelper.ToRadians(rotation.Z))
                        * Matrix.CreateScale(1)
                        * Matrix.CreateTranslation(new Vector3(-5000, -100, 0));
                    effect.View = camera.ViewMatrix;
                    effect.Projection = camera.ProjectionMatrix;

                    effect.EnableDefaultLighting();
                }
                mesh.Draw();
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

           // MapLoader.LoadWalls(@"Levels\lvl1");

            #region outer wall

            //+Z outer wall
            walls.Add(new SolidWall(device,
                new Vector2(0, 5050), new Vector2(5000, 5050),
                new Vector2(5000, 5000), new Vector2(0, 5000), GameConstants.WALL_HEIGHT));

            //-Z outer wall
            walls.Add(new SolidWall(device,
                 new Vector2(0, 0), new Vector2(5000, 0),
                 new Vector2(5000, -50), new Vector2(0, -50), GameConstants.WALL_HEIGHT));

            //+X outer wall

            walls.Add(new SolidWall(device,
                new Vector2(5000, 5000), new Vector2(5050, 5000),
                new Vector2(5050, -0), new Vector2(5000, 0), GameConstants.WALL_HEIGHT));
            //-X outer wall
            walls.Add(new SolidWall(device,
                 new Vector2(-50, 5000), new Vector2(0, 5000),
                 new Vector2(0, 0), new Vector2(-50, 0), GameConstants.WALL_HEIGHT));
           

            //XWallNegZ = grows out on negative Z
            //XWallPosZ = grows out on positive Z
            //ZWallNegX = grows out on negative X
            //ZWallPosX = grows out on positive X

            //Z Walls - ZWallPosX
            //X Walls - XWallNegZAl
            #endregion

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

            //walls.Add(new XWallPosZ(device, new Vector2(3200, 4200), new Vector2(4200, 4200)));//11

            #endregion

            #region area 2
            walls.Add(new XWallPosZ(device, new Vector2(0, 4100), new Vector2(1500, 4100)));//1
            walls.Add(new XWallPosZ(device, new Vector2(400, 2850), new Vector2(1000, 2850)));//2

            walls.Add(new ZWallPosX(device, new Vector2(1000, 3800), new Vector2(1000, 2850)));//3
            #endregion

            #region area 3
            walls.Add(new XWallPosZ(device, new Vector2(800, 2400), new Vector2(1600, 2400)));//1
            walls.Add(new XWallPosZ(device, new Vector2(2000, 2700), new Vector2(3050, 2700)));//2
            walls.Add(new XWallPosZ(device, new Vector2(2000, 1800), new Vector2(3000, 1800)));//3
            walls.Add(new XWallPosZ(device, new Vector2(1400, 1150), new Vector2(2650, 1150)));//4
            walls.Add(new XWallPosZ(device, new Vector2(1000, 2000), new Vector2(1450, 2000)));//5
            walls.Add(new XWallPosZ(device, new Vector2(1000, 750), new Vector2(3000, 750)));//6

            walls.Add(new ZWallPosX(device, new Vector2(2000, 2700), new Vector2(2000, 1850)));//7
            walls.Add(new ZWallPosX(device, new Vector2(3000, 2700), new Vector2(3000, 2050)));//8
            walls.Add(new ZWallPosX(device, new Vector2(2650, 2300), new Vector2(2650, 1150)));//9
            walls.Add(new ZWallPosX(device, new Vector2(1400, 2000), new Vector2(1400, 1200)));//10
            walls.Add(new ZWallPosX(device, new Vector2(1000, 2000), new Vector2(1000, 800)));//11
            walls.Add(new ZWallPosX(device, new Vector2(3000, 1400), new Vector2(3000, 750)));//12
            #endregion

            #region area 4
            walls.Add(new ZWallPosX(device, new Vector2(4700, 3500), new Vector2(4700, 1900)));//1
            walls.Add(new ZWallPosX(device, new Vector2(3700, 3100), new Vector2(3700, 750)));//2
            walls.Add(new ZWallPosX(device, new Vector2(4000, 1900), new Vector2(4000, 400)));//3
            walls.Add(new ZWallPosX(device, new Vector2(3600, 400), new Vector2(3600, 0)));//4

            walls.Add(new XWallPosZ(device, new Vector2(3750, 2400), new Vector2(4700, 2400)));//5
            walls.Add(new XWallPosZ(device, new Vector2(4000, 1900), new Vector2(4700, 1900)));//6
            walls.Add(new XWallPosZ(device, new Vector2(3600, 400), new Vector2(4000, 400)));//7
            #endregion

            #region area 5
            walls.Add(new XWallPosZ(device, new Vector2(500, 400), new Vector2(1450, 400)));//1
            walls.Add(new XWallPosZ(device, new Vector2(1800, 400), new Vector2(3350, 400)));//2

            walls.Add(new ZWallPosX(device, new Vector2(3300, 400), new Vector2(3300, 0)));//3
            walls.Add(new ZWallPosX(device, new Vector2(1800, 400), new Vector2(1800, 0)));//4
            walls.Add(new ZWallPosX(device, new Vector2(500, 400), new Vector2(500, 0)));//5
            walls.Add(new ZWallPosX(device, new Vector2(1400, 400), new Vector2(1400, 0)));//6
            #endregion

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
