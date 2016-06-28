using JamesBond.Items;
using JamesBond.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JamesBond.Levels
{
    public class Level
    {
        SpriteSheet tileset;
        int[] tiles;
        int[] logic;
        private int background;
        Collectable collectable;

        public int Background
        {
            get { return background; }
            set { background = value; }
        }

        private Point dimension;

        public Point Dimension
        {
            get { return dimension; }
            set { dimension = value; }
        }

        private static int tilesize;

        public static int Tilesize
        {
            get { return tilesize; }
            set { tilesize = value; }
        }

        public Level()
        {
            dimension = new Point(10, 10);
            tiles = new int[10 * 10];
            logic = new int[10 * 10];
            tilesize = 32;
        }

        public Collectable HasCollectable()
        {
            return collectable;
        }

        public Level(string[] content)
        {
            dimension = new Point(10, 10);
            tiles = new int[10 * 10];
            logic = new int[10 * 10];
            tilesize = 32;

            for (int i = 2; i < content.Length; i++)
            {
                string[] row = content[i].Split(',');
                for (int j = 0; j < row.Length; j++)
                {
                    if (i < 12)
                        tiles[(i - 2) * 10 + j] = int.Parse(row[j]);
                    else
                    {
                        logic[(i - 12) * 10 + j] = int.Parse(row[j]);

                        if (logic[(i - 12) * 10 + j] == 4)
                        {
                            collectable = new SpectreRing();
                            collectable.BoundingBox = new Rectangle(j * 32, (i - 12) * 32, tilesize, tilesize);
                        }
                        if (logic[(i - 12) * 10 + j] == 6)
                        {
                            collectable = new Key();
                            collectable.BoundingBox = new Rectangle(j * 32, (i - 12) * 32, tilesize, tilesize);
                        }
                    }
                }
            }
        }

        public int this[int x, int y]
        {
            get
            {
                if (x >= 0 && x < dimension.X &&
                    y >= 0 && y < dimension.Y)
                    return logic[x + y * dimension.X];
                return 0;
            }

            set
            {
                if (x >= 0 && x < dimension.X &&
                    y >= 0 && y < dimension.Y)
                    logic[x + y * dimension.X] = value;
            }
        }

        public void Update(GameTime gameTime, Rectangle player)
        {
            if (collectable != null)
            {
                collectable.Update(gameTime);
                if (player.Intersects(collectable.BoundingBox))
                    collectable.Collect();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < dimension.Y; y++)
            {
                for (int x = 0; x < dimension.X; x++)
                {
                    if (this[x, y] > -1)
                        spriteBatch.Draw(tileset.Texture, new Vector2(x * tileset.TileSize.X, y * tileset.TileSize.Y), tileset[this[x, y]].SpriteRectangle, Color.White);
                }
            }

            if (collectable != null)
                collectable.Draw(spriteBatch);
        }
    }
}
