using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JamesBond.Animation
{
    public class Animator
    {
        Dictionary<string, Animation> animations;
        private string defaultAnimation;
        private bool running;
        private string targetanimation;
        private string currentAnimation;

        public Animator()
        {
            animations = new Dictionary<string, Animation>();
        }

        public void AddAnimation(string name, Animation animation)
        {
            if (!animations.ContainsKey(name))
                animations.Add(name, animation);

            if (animations.Count == 0)
                SetDefaultAnimation(name);
        }

        public void PlayAnimation(string name)
        {
            targetanimation = name;
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

            if(animations[currentAnimation].Update(gameTime))
            {
                animations[targetanimation].Reset();
                currentAnimation = targetanimation;
            }
        }
    }
}
