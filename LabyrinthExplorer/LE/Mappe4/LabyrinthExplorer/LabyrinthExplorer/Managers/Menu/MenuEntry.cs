using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LabyrinthExplorer
{
    class MenuEntry
    {
        private bool isSelected;
        private string entryText;
        public MenuEntry(string entryText)
        {
            isSelected = false;
            this.entryText = entryText;
        }

        public void SetSelected()
        {
            isSelected = true;
        }

        public void SetUnselected()
        {
            isSelected = false;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font, Vector2 entryPosition, float scale)
        {
            if(isSelected)
                spriteBatch.DrawString(font, entryText, entryPosition, Color.Red, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            else
                spriteBatch.DrawString(font, entryText, entryPosition, Color.Orange, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
        }

        public void OnEntrySelected()
        {
            Selected(null, null);
        }

        public event EventHandler Selected;
    }
}
