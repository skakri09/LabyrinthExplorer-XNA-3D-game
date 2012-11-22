using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace LabyrinthExplorer
{
    public interface IGameArea
    {
        void LoadContent(GraphicsDevice device, ContentManager contentMan);

        void Update(GameTime gameTime, Camera camera);

        void Draw(GraphicsDevice graphicsDevice, Effect effect,
        Texture2D brickColorMap, Texture2D brickNormalMap, Texture2D brickHeightMap,
        Texture2D stoneColorMap, Texture2D stoneNormalMap, Texture2D stoneHeightMap,
        Texture2D woodColorMap, Texture2D woodNormalMap, Texture2D woodHeightMap);

        void RemoveEnvironmentItem(IEnvironmentObject item);

        List<AABB> EnvironmentCollidables();
    }
}
