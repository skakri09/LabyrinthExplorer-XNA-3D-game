using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

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

        #endregion

        public World()
        {
            walls = new List<SolidWall>();
            ceilings = new List<NormalMappedCeiling>();
            floors = new List<NormalMappedFloor>();
        }

        public void Update(float deltaTime)
        {

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
            walls.Add(new SolidWall(device,
                new Vector3(200, 0, 0), new Vector3(250, 0, 0),
                new Vector3(250, 0, -500), new Vector3(200, 0, -500), 256));

            floors.Add(new NormalMappedFloor(device,
                new Vector3(-5000, 0, 5000), new Vector3(5000, 0, 5000),
                new Vector3(5000, 0, -5000), new Vector3(-5000, 0, -5000), Vector3.Up));

            ceilings.Add(new NormalMappedCeiling(device,
                new Vector3(-5000, GameConstants.WALL_HEIGHT, 5000), new Vector3(5000, GameConstants.WALL_HEIGHT, 5000),
                new Vector3(5000, GameConstants.WALL_HEIGHT, -5000), new Vector3(-5000, GameConstants.WALL_HEIGHT, -5000), Vector3.Down));
        }

        public List<SolidWall> Walls
        {
            get { return walls; }
        }
    }
}
