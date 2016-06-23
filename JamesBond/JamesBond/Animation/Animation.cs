using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JamesBond.Animation
{
    public class Animation
    {
        List<int> frames;
        private float speed;

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

        public Animation(float speed, bool playonce, params int[] frames)
        {
            this.playonce = playonce;
            this.speed = speed;
            this.frames = new List<int>(frames);
        }

        public Animation(float speed, params int[] frames)
        {
            this.playonce = false;
            this.speed = speed;
            this.frames = new List<int>(frames);
        }

        public bool Update(GameTime gameTime)
        {
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
            return false;
        }

        public void Reset()
        {
            current = 0;
        }
    }
}
