using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JamesBond.Animations
{
    public class Animation
    {
        List<int> frames;
        private float speed;
        private SpriteEffects effects;

        public SpriteEffects Effects
        {
            get { return effects; }
            set { effects = value; }
        }

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        private int current;

        public int Current
        {
            get { return frames[current]; }
        }

        float timer;
        bool playonce;

        public Animation(float speed, bool playonce, SpriteEffects effects, params int[] frames)
        {
            this.effects = effects;
            this.playonce = playonce;
            this.speed = speed;
            this.frames = new List<int>(frames);
        }

        public Animation(float speed, bool playonce, params int[] frames)
        {
            this.effects = SpriteEffects.None;
            this.playonce = playonce;
            this.speed = speed;
            this.frames = new List<int>(frames);
        }

        public Animation(float speed, SpriteEffects effects, params int[] frames)
        {
            this.effects = effects;
            this.playonce = false;
            this.speed = speed;
            this.frames = new List<int>(frames);
        }

        public Animation(float speed, params int[] frames)
        {
            this.effects = SpriteEffects.None;
            this.playonce = false;
            this.speed = speed;
            this.frames = new List<int>(frames);
        }

        public bool Update(GameTime gameTime)
        {
            if (speed == 0)
                return true;

            timer -= gameTime.ElapsedGameTime.Milliseconds / 1000f;

            while (timer <= 0)
            {
                current++;
                timer += speed;

                if (current >= frames.Count)
                {
                    if (playonce)
                    {
                        current = frames.Count - 1;
                        return true;
                    }
                    else
                        Reset();
                }
            }
            return !playonce;
        }

        public void Reset()
        {
            current = 0;
        }
    }
}
