using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace JamesBond.Rendering
{
    public class SpriteSheet
    {
        private Texture2D texture;
        private Point tilesize;

        public Point TileSize
        {
            get { return tilesize; }
            set { tilesize = value; GenerateTiles(); }
        }

        public SpriteSheet(Texture2D texture, int x, int y)
        {
            textureSize = texture.Bounds;
            this.texture = texture;
            this.tilesize = new Point(x, y);
            GenerateTiles();
        }

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; textureSize = texture.Bounds; GenerateTiles();  }
        }

        private void GenerateTiles()
        {
            Rectangle rect;
            int CountX = textureSize.Width / tilesize.X;
            int CountY = textureSize.Height / tilesize.Y;
            for (int y = 0; y < CountY; y++)
            {
                for (int x = 0; x < CountX; x++)
                {
                    rect.X = textureSize.Width * x;
                    rect.Y = textureSize.Height * y;
                    rect.Width = textureSize.Width;
                    rect.Height = textureSize.Height;

                    tiles.Add(rect);
                }
            }
        }

        protected List<Rectangle> tiles;
        private Rectangle textureSize;

        public Rectangle this[int index]
        {
            get
            {
                if (tiles.Count > index && index > 0) return tiles[index];
                return textureSize;
            }
        }
    }
}