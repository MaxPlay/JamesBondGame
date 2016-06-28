using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using JamesBond.Rendering;
using JamesBond.Animations;

namespace JamesBond.Items
{
    class Key : Collectable
    {
        public Key()
        {
            sprites = new SpriteSheet(Game1.ContentManager.Load<Texture2D>("SpectreRing"), 32, 32);
            animator = new Animator();
            animator.AddAnimation("Rotate", new Animation(0.1f, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9));
            animator.Start();
            boundingBox = new Rectangle(0, 0, 32, 32);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprites.Texture, boundingBox, sprites[animator.CurrentFrame].SpriteRectangle, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            animator.Update(gameTime);
        }
    }
}
