using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JamesBond.Animations
{
    public class Animator
    {
        Dictionary<string, Animation> animations;
        private string defaultAnimation;
        private bool running;
        private Queue<string> targetanimation;
        private string currentAnimation;

        public string CurrentAnimation { get { return currentAnimation; } }

        public int CurrentFrame { get { return animations[currentAnimation].Current; } }

        public SpriteEffects CurrentEffect { get { return animations[currentAnimation].Effects; } }

        public Animator()
        {
            animations = new Dictionary<string, Animation>();
            targetanimation = new Queue<string>();
        }

        public void AddAnimation(string name, Animation animation)
        {
            if (!animations.ContainsKey(name))
                animations.Add(name, animation);

            if (animations.Count == 1)
            {
                SetDefaultAnimation(name);
                currentAnimation = name;
            }
        }

        public void PlayAnimation(string name)
        {
            targetanimation.Enqueue(name);
        }

        public void SetDefaultAnimation(string name)
        {
            defaultAnimation = name;
        }

        public void Stop()
        {
            running = false;
        }

        public void Start()
        {
            running = true;
        }

        public void Update(GameTime gameTime)
        {
            if (!running)
                return;

            if (animations[currentAnimation].Update(gameTime) && targetanimation.Count != 0)
            {
                if (targetanimation.Peek() != currentAnimation)
                {
                    animations[targetanimation.Peek()].Reset();
                    currentAnimation = targetanimation.Dequeue();
                }
                else
                    targetanimation.Dequeue();
            }
        }
    }
}
