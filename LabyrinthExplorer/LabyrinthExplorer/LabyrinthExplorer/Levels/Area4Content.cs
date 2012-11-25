using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LabyrinthExplorer
{
    public class Area4Content : AreaContent
    {

        public Area4Content(Camera camera)
            :base(camera)
        {
            this.camera = camera;

            walls = new List<SolidWall>();
            ceilings = new List<NormalMappedCeiling>();
            floors = new List<NormalMappedFloor>();
            enemies = new List<Enemy>();
            environment = new List<IEnvironmentObject>();
            environmentCollidables = new List<AABB>();
        }

        public override void LoadContent(GraphicsDevice device, ContentManager contentMan)
        {
            base.LoadContent(device, contentMan);

            CreateTestCenters();
            CreateHangar();
            CreateAssemblyLane();
        }

        public override void Update(GameTime gameTime, Camera camera)
        {
            base.Update(gameTime, camera);
        }

        public override void Draw(GraphicsDevice graphicsDevice, Effect effect, Texture2D brickColorMap, Texture2D brickNormalMap, Texture2D brickHeightMap, Texture2D stoneColorMap, Texture2D stoneNormalMap, Texture2D stoneHeightMap, Texture2D woodColorMap, Texture2D woodNormalMap, Texture2D woodHeightMap)
        {
            foreach (IEnvironmentObject obj in environment)
            {
                obj.Draw(camera, effect);
            }
        }

        public override void OnEnteringArea()
        {
            base.OnEnteringArea();
            Game.SoundManager.PlaySong("space", true);//find new crazy trippy tune
        }

        private void CreateHangar()
        {
            environment.Add(new Hangar(contentMan, new Vector3(3800, -30, 15550), new Vector3(0, 90, 0), 15.0f));

            //right corridor collision
            environmentCollidables.Add(new AABB(new Vector2(2650, 4875), new Vector2(2950, 12500)));
            //left corridor collision
            environmentCollidables.Add(new AABB(new Vector2(2000, 4875), new Vector2(2300, 12500)));
            //right positive hangar collision
            environmentCollidables.Add(new AABB(new Vector2(2700, 12000), new Vector2(10000, 12500)));
            //left positive hangar collision
            environmentCollidables.Add(new AABB(new Vector2(0, 12000), new Vector2(2300, 12500)));
            //Back(left) of hangar collision
            environmentCollidables.Add(new AABB(new Vector2(0, 12000), new Vector2(550, 21000)));
            //negative z side hangar collision
            environmentCollidables.Add(new AABB(new Vector2(0, 19700), new Vector2(10000, 21000)));
            //hangar opening collision
            environmentCollidables.Add(new AABB(new Vector2(9000, 10000), new Vector2(10000, 21000)));
        }

        private void CreateAssemblyLane()
        {

        }

        private void CreateTestCenters()
        {
        }

    }
}
