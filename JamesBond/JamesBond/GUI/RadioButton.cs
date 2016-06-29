using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JamesBond.GUI
{
    class RadioButton : Button
    {
        protected bool active;
        private Texture2D activeTexture;

        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        protected Color activeTextColor;

        public Color ActiveTextColor
        {
            get { return activeTextColor; }
            set { activeTextColor = value; }
        }


        public RadioButton(Texture2D texture, Texture2D hoverTexture, Texture2D activeTexture, string text, Point position) : base(texture, hoverTexture, text, position)
        {
            this.activeTexture = activeTexture;
        }

        public new void Draw(SpriteBatch spriteBatch)
        {
            if (hover)
            {
                if (hoverTexture != null)
                    spriteBatch.Draw(hoverTexture, boundingBox, Color.White);
                spriteBatch.DrawString(font, text, new Vector2(boundingBox.Center.X, boundingBox.Center.Y) - (font.MeasureString(text) / 2f), hoverTextColor);
            }
            else
            {
                if (active)
                {
                    if (activeTexture != null)
                        spriteBatch.Draw(activeTexture, boundingBox, Color.White);
                    spriteBatch.DrawString(font, text, new Vector2(boundingBox.Center.X, boundingBox.Center.Y) - (font.MeasureString(text) / 2f), activeTextColor);
                }
                else
                {
                    if (texture != null)
                        spriteBatch.Draw(texture, boundingBox, Color.White);
                    spriteBatch.DrawString(font, text, new Vector2(boundingBox.Center.X, boundingBox.Center.Y) - (font.MeasureString(text) / 2f), textColor);
                }
            }
        }
    }
}
