using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace LabyrinthExplorer
{
    public interface IGameLevel
    {
        void LoadContent(GraphicsDevice device, ContentManager contentMan);

        void Update(GameTime gameTime, Camera camera);

        void Draw(GraphicsDevice graphicsDevice, Effect effect);

        List<AABB> EnvironmentCollidables();

    }
}
