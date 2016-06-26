using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using System.IO;

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

        public SpriteSheet(Texture2D texture, string file)
        {
            tiles = new List<Sprite>();
            textureSize = texture.Bounds;
            this.texture = texture;
            defaultSprite = new Sprite(texture.Bounds, 0, 0);
            LoadTiles(file);
        }

        private void LoadTiles(string file)
        {
            string filename = "Content/" + file + ".tex";

            if (!File.Exists(filename))
                return;

            using (FileStream stream = File.OpenRead(filename))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        string[] input = reader.ReadLine().Split(' ');

                        tiles.Add(new Sprite(new Rectangle(
                            int.Parse(input[0]),
                            int.Parse(input[1]),
                            int.Parse(input[2]),
                            int.Parse(input[3])
                            ),
                            float.Parse(input[4], System.Globalization.CultureInfo.InvariantCulture),
                            float.Parse(input[5], System.Globalization.CultureInfo.InvariantCulture)
                            ));
                    }
                }
            }
        }

        public SpriteSheet(Texture2D texture, int x, int y)
        {
            tiles = new List<Sprite>();
            textureSize = texture.Bounds;
            this.texture = texture;
            this.tilesize = new Point(x, y);
            defaultSprite = new Sprite(texture.Bounds, 0, 0);
            GenerateTiles();
        }

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; textureSize = texture.Bounds; GenerateTiles(); }
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
                    rect.X = tilesize.X * x;
                    rect.Y = tilesize.Y * y;
                    rect.Width = tilesize.X;
                    rect.Height = tilesize.Y;

                    tiles.Add(new Sprite(rect, 0, 0));
                }
            }
        }

        protected List<Sprite> tiles;
        private Rectangle textureSize;
        private Sprite defaultSprite;

        public Sprite this[int index]
        {
            get
            {
                if (tiles.Count > index && index >= 0)
                    return tiles[index];
                return defaultSprite;
            }
        }


        public class Sprite
        {
            private Rectangle spriteRect;
            public Rectangle SpriteRectangle { get { return spriteRect; } }
            private float offset;
            private float offsetFlipped;

            public float Offset { get { return offset; } }
            public float OffsetFlipped { get { return offsetFlipped; } }

            public Sprite(Rectangle rectangle, float offset, float offsetFlipped)
            {
                this.spriteRect = rectangle;
                this.offset = offset;
                this.offsetFlipped = offsetFlipped;
            }
        }
    }
}