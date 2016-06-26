using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JamesBond.GUI
{
    public class Button
    {
        private Texture2D texture;

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        private Texture2D hoverTexture;

        public Texture2D HoverTexture
        {
            get { return hoverTexture; }
            set { hoverTexture = value; }
        }

        public Point Position
        {
            get { return new Point(boundingBox.X, boundingBox.Y); }
            set { boundingBox.X = value.X; boundingBox.Y = value.Y; }
        }

        private string text;

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        private SpriteFont font;

        public SpriteFont Font
        {
            get { return font; }
            set { font = value; }
        }

        private Color textColor;

        public Color TextColor
        {
            get { return textColor; }
            set { textColor = value; }
        }

        private Color hoverTextColor;

        public Color HoverTextColor
        {
            get { return hoverTextColor; }
            set { hoverTextColor = value; }
        }

        private Rectangle boundingBox;
        private bool hover;

        public Rectangle BoundingBox
        {
            get { return boundingBox; }
            set { boundingBox = value; }
        }

        public Button(Texture2D texture, Texture2D hoverTexture, string text, Point position)
        {
            this.texture = texture;
            this.hoverTexture = hoverTexture;
            this.text = text;

            if (texture != null)
                this.boundingBox = texture.Bounds;

            this.boundingBox.X = position.X;
            this.boundingBox.Y = position.Y;
            this.textColor = Color.White;
            this.hoverTextColor = Color.Black;
        }

        public void Update()
        {
            hover = boundingBox.Contains(InputManager.MousePosition);

            if (hover && InputManager.MouseLeftClicked())
                OnClick();
        }

        private void OnClick()
        {
            Click?.Invoke(this, new EventArgs());
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (hover)
            {
                if (hoverTexture != null)
                    spriteBatch.Draw(hoverTexture, boundingBox, Color.White);
                spriteBatch.DrawString(font, text, new Vector2(boundingBox.Center.X, boundingBox.Center.Y) - (font.MeasureString(text) / 2f), hoverTextColor);
            }
            else
            {
                if (texture != null)
                    spriteBatch.Draw(texture, boundingBox, Color.White);
                spriteBatch.DrawString(font, text, new Vector2(boundingBox.Center.X, boundingBox.Center.Y)- (font.MeasureString(text) / 2f), textColor);
            }
        }

        public event EventHandler<EventArgs> Click;
    }
}
