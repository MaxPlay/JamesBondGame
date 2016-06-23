using JamesBond.Levels;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace JamesBond.Physics
{
    public abstract class Rigidbody
    {
        public Rigidbody()
        {
            PhysicsManager.Register(this);
        }

        #region Protected Fields

        protected bool applyFriction;
        protected bool awake;
        protected Rectangle boundingBox;
        protected Vector2 position;
        protected Vector2 velocity;

        #endregion Protected Fields

        #region Public Properties

        public bool ApplyFriction
        {
            get { return applyFriction; }
            set { applyFriction = value; }
        }

        public bool Awake
        {
            get { return awake; }
            set { awake = value; }
        }

        public Rectangle BoundingBox
        {
            get { return boundingBox; }
            set { boundingBox = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        #endregion Public Properties

        #region Public Methods

        public void UpdatePhysics(Level level, GameTime gameTime)
        {
            this.velocity += PhysicsManager.Gravity * (gameTime.ElapsedGameTime.Milliseconds / 1000f);

            Vector2 position = new Vector2(boundingBox.X, boundingBox.Y);
            Vector2 oldposition = position;
            position += velocity;

            Rectangle movedboundingBox = Rectangle.Union(
                new Rectangle((int)position.X, (int)position.Y, boundingBox.Width, boundingBox.Height),
                new Rectangle((int)oldposition.X, (int)oldposition.Y, boundingBox.Width, boundingBox.Height)
                );

            List<Rectangle> colliders = new List<Rectangle>();

            int Top = (movedboundingBox.Top / level.Tilesize);
            int Bottom = (movedboundingBox.Bottom / level.Tilesize + 1);
            int Left = (movedboundingBox.Left / level.Tilesize);
            int Right = (movedboundingBox.Right / level.Tilesize + 1);

            for (int x = Left; x < Right; x++)
            {
                for (int y = Top; y < Bottom; y++)
                {
                    if (level[x, y] > 0)
                        colliders.Add(new Rectangle(x * level.Tilesize, y * level.Tilesize, level.Tilesize, level.Tilesize));
                }
            }

            for (int i = 0; i < colliders.Count; i++)
            {
                if (movedboundingBox.Intersects(colliders[i]))
                {
                    Vector2 majorAxis = GetMajorAxis(colliders[i], boundingBox);
                    Vector2 collisionNormal = GetAxialDistance(majorAxis, colliders[i], boundingBox);
                    collisionNormal.Normalize();

                    Rectangle positionedBoundingBox = new Rectangle((int)position.X, (int)position.Y, boundingBox.Width, boundingBox.Height);

                    for (int j = 0; j < velocity.Length(); j++)
                    {
                        if (colliders[i].Intersects(positionedBoundingBox))
                        {
                            positionedBoundingBox.X += (int)collisionNormal.X;
                            positionedBoundingBox.Y += (int)collisionNormal.Y;
                        }
                        else
                            break;
                    }

                    if (collisionNormal.X != 0)
                        velocity.X = 0;
                    if (collisionNormal.Y != 0)
                        velocity.Y = 0;

                    position.X = positionedBoundingBox.X;
                    position.Y = positionedBoundingBox.Y;

                    Console.Clear();
                    Console.WriteLine(Velocity);
                    Console.WriteLine(majorAxis);
                }
            }

            boundingBox.X = (int)position.X;
            boundingBox.Y = (int)position.Y;
        }

        private Vector2 GetAxialDistance(Vector2 majorAxis, Rectangle boundingBox1, Rectangle boundingBox2)
        {
            if (majorAxis.X == 0)
                return Vector2.UnitY * (majorAxis.Y - boundingBox1.Height / 2f - boundingBox2.Height / 2f);
            else
                return Vector2.UnitX * (majorAxis.X - boundingBox1.Width / 2f - boundingBox2.Width / 2f);
        }

        private Vector2 GetMajorAxis(Rectangle otherBounds, Rectangle ownBounds)
        {
            Vector2 distance = new Vector2();

            otherBounds.Inflate(ownBounds.Height / 2, ownBounds.Width / 2);
            Vector2 center = new Vector2(ownBounds.Center.X, ownBounds.Center.Y);
            float dx = Math.Max(Math.Abs(center.X - otherBounds.Center.X) - otherBounds.Width / 2, 0);
            float dy = Math.Max(Math.Abs(center.Y - otherBounds.Center.Y) - otherBounds.Height / 2, 0);
            distance = new Vector2(dx, dy);

            if (Math.Abs(distance.X) > Math.Abs(distance.Y))
            {
                return Vector2.UnitX * distance.X;
            }
            else
            {
                return Vector2.UnitY * distance.Y;
            }
        }

        #endregion Public Methods
    }
}