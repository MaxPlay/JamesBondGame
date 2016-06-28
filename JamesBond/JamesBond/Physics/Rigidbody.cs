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
        protected bool grounded;

        public bool Grounded
        {
            get { return grounded; }
            set { grounded = value; }
        }


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

        protected Vector2 limitVelocity;

        public Vector2 LimitVelocity
        {
            get { return limitVelocity; }
            set { limitVelocity = value; }
        }


        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Was lustiges rein.
        /// </summary>
        /// <param name="level">Der Level</param>
        /// <param name="gameTime">Der GameTime</param>
        public void UpdatePhysics(Level level, GameTime gameTime)
        {
            if (!awake)
                return;

            //Apply Gravity
            this.velocity += PhysicsManager.Gravity * (gameTime.ElapsedGameTime.Milliseconds / 1000f);

            //Limit the velocity
            if (Math.Abs(velocity.X) > Math.Abs(limitVelocity.X))
            {
                velocity.X = Math.Sign(velocity.X) != Math.Sign(limitVelocity.X) ? velocity.X = limitVelocity.X * -1 : velocity.X = limitVelocity.X;
            }

            if (Math.Abs(velocity.Y) > Math.Abs(limitVelocity.Y))
            {
                velocity.Y = Math.Sign(velocity.Y) != Math.Sign(limitVelocity.Y) ? velocity.Y = limitVelocity.Y * -1 : velocity.Y = limitVelocity.Y;
            }

            //Apply velocity
            Vector2 position = new Vector2(boundingBox.X, boundingBox.Y);
            Vector2 oldposition = position;
            position += velocity;

            //Generate a boundingbox around the area that the object passes through

            Rectangle movedboundingBox = Rectangle.Union(
                new Rectangle((int)position.X, (int)position.Y, boundingBox.Width, boundingBox.Height),
                new Rectangle((int)oldposition.X, (int)oldposition.Y, boundingBox.Width, boundingBox.Height)
                );
            List<Rectangle> colliders = GetPossibleColliders(level, movedboundingBox);

            //Point Left = new Point(finalBoundingBox.Left, boundingBox.Center.Y);
            //Point Right = new Point(finalBoundingBox.Right, boundingBox.Center.Y);

            grounded = false;
            foreach (Rectangle collider in colliders)
            {
                Rectangle finalBoundingBox = boundingBox;
                finalBoundingBox.X = (int)position.X;
                finalBoundingBox.Y = (int)position.Y;

                Point BottomLeft = new Point(finalBoundingBox.Left, finalBoundingBox.Bottom - (int)(finalBoundingBox.Height * 0.2f));
                Point BottomRight = new Point(finalBoundingBox.Right, finalBoundingBox.Bottom - (int)(finalBoundingBox.Height * 0.2f));
                Point TopLeft = new Point(finalBoundingBox.Left, finalBoundingBox.Top + (int)(finalBoundingBox.Height * 0.1f));
                Point TopRight = new Point(finalBoundingBox.Right, finalBoundingBox.Top + (int)(finalBoundingBox.Height * 0.1f));

                Point Top = new Point(finalBoundingBox.X + finalBoundingBox.Width / 2, finalBoundingBox.Top);
                Point Bottom = new Point(finalBoundingBox.X + finalBoundingBox.Width / 2, finalBoundingBox.Bottom);

                Vector2 distanceCorrection = new Vector2();

                if (collider.Contains(Top))
                {
                    distanceCorrection.Y = collider.Bottom - Top.Y;
                    velocity.Y = 0;
                }

                if (collider.Contains(Bottom))
                {
                    distanceCorrection.Y = collider.Top - Bottom.Y;
                    velocity.Y = 0;
                    velocity.X *= 0.9f;
                    grounded = true;
                }

                if (collider.Contains(TopLeft) || collider.Contains(BottomLeft))
                {
                    distanceCorrection.X = collider.Right - TopLeft.X;
                    velocity.X = 0;
                }

                if (collider.Contains(TopRight) || collider.Contains(BottomRight))
                {
                    distanceCorrection.X = collider.Left - TopRight.X;
                    velocity.X = 0;
                }

                if (Math.Abs(velocity.X) < 0.1f)
                    velocity.X = 0;
                if (Math.Abs(velocity.Y) < 0.1f)
                    velocity.Y = 0;

                position += distanceCorrection;
                //Debug.DrawRectangle(new Rectangle(Top.X, Top.Y, 1, 1));
                //Debug.DrawRectangle(new Rectangle(TopLeft.X, TopLeft.Y, 1, 1));
                //Debug.DrawRectangle(new Rectangle(BottomLeft.X, BottomLeft.Y, 1, 1));
                //Debug.DrawRectangle(new Rectangle(Bottom.X, Bottom.Y, 1, 1));
                //Debug.DrawRectangle(new Rectangle(TopRight.X, TopRight.Y, 1, 1));
                //Debug.DrawRectangle(new Rectangle(BottomRight.X, BottomRight.Y, 1, 1));
            }


            boundingBox.X = (int)Math.Round(position.X);
            boundingBox.Y = (int)Math.Round(position.Y);
        }

        private static List<Rectangle> GetPossibleColliders(Level level, Rectangle movedboundingBox)
        {
            //Generate a list of all the colliders that are overlapping this area

            List<Rectangle> colliders = new List<Rectangle>();

            int Top = (movedboundingBox.Top / Level.Tilesize - 1);
            int Bottom = (movedboundingBox.Bottom / Level.Tilesize + 2);
            int Left = (movedboundingBox.Left / Level.Tilesize - 1);
            int Right = (movedboundingBox.Right / Level.Tilesize + 2);

            //Debug.DrawRectangle(new Rectangle(Left * Level.Tilesize, Top * Level.Tilesize, (Right - Left) * Level.Tilesize, (Bottom - Top) * Level.Tilesize), Color.Yellow * 0.2f);

            for (int x = Left; x < Right; x++)
            {
                for (int y = Top; y < Bottom; y++)
                {
                    if (level[x, y] > 0)
                        colliders.Add(new Rectangle(x * Level.Tilesize, y * Level.Tilesize, Level.Tilesize, Level.Tilesize));
                }
            }

            return colliders;
        }

        #endregion Public Methods

    }
}