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
        private Point dimension;

        public Point Dimension
        {
            get { return dimension; }
            set { dimension = value; }
        }

        private int tilesize;
        
        public int Tilesize
        {
            get { return tilesize; }
            set { tilesize = value; }
        }


        public Level()
        {
            dimension = new Point(20, 20);
            tiles = new int[20 * 20];
        }

        public int this[int x, int y]
        {
            get
            {
                if (x >= 0 && x < dimension.X &&
                    y >= 0 && y < dimension.Y)
                    return tiles[x + y * dimension.X];
                return 0;
            }

            set
            {
                if (x >= 0 && x < dimension.X &&
                    y >= 0 && y < dimension.Y)
                    tiles[x + y * dimension.X] = value;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < dimension.Y; y++)
            {
                for (int x = 0; x < dimension.X; x++)
                {
                    if (this[x, y] > -1)
                        spriteBatch.Draw(tileset.Texture, new Vector2(x * tileset.TileSize.X, y * tileset.TileSize.Y), tileset[this[x, y]], Color.White);
                }
            }
        }
    }
}
