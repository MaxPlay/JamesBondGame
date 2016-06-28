using JamesBond.Animations;
using JamesBond.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JamesBond.Items
{
    public abstract class Collectable
    {
        protected bool collected;
        protected SpriteSheet sprites;
        protected Animator animator;

        public event EventHandler<EventArgs> Collected;
        
        protected void OnCollect()
        {
            Collected?.Invoke(this, new EventArgs());
        }

        protected Rectangle boundingBox;

        public Rectangle BoundingBox
        {
            get { return boundingBox; }
            set { boundingBox = value; }
        }
        
        public abstract void Draw(SpriteBatch spriteBatch);

        public abstract void Update(GameTime gameTime);

        public void Collect()
        {
            OnCollect();
            collected = true;
        }
    }
}
