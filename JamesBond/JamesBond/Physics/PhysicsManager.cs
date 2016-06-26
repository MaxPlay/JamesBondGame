using JamesBond.Levels;
using JamesBond.Physics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace JamesBond.Physics
{
    public static class PhysicsManager
    {
        private static Vector2 gravity;

        public static Vector2 Gravity
        {
            get { return gravity; }
            set { gravity = value; }
        }

        static List<Rigidbody> rigidbodies;

        static PhysicsManager()
        {
            rigidbodies = new List<Rigidbody>();
            gravity = new Vector2(0, 9.81f);
        }

        public static void Update(GameTime gameTime, Level currentLevel)
        {
            for (int i = 0; i < rigidbodies.Count; i++)
            {
                rigidbodies[i].UpdatePhysics(currentLevel, gameTime);
            }
        }

        public static void Register(Rigidbody rigidbody)
        {
            rigidbodies.Add(rigidbody);
        }
    }
}