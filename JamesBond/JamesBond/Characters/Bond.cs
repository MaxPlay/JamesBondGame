using JamesBond.Animation;
using JamesBond.Physics;
using JamesBond.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JamesBond.Characters
{
    public class Bond : Rigidbody
    {
        Animator animator;
        SpriteSheet texture;

        public Bond() : base()
        {
            animator = new Animator();
            awake = true;
            //texture = new SpriteSheet(Game1.ContentManager.Load<Texture2D>("Bond"), 10, 42);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Game1.Pixel, BoundingBox, Color.Blue);
        }
    }
}
