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
        private bool onladder;

        public Vector2 LimitVelocity
        {
            get { return limitVelocity; }
            set { limitVelocity = value; }
        }

        public bool OnLadder
        {
            get { return onladder; }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Was lustiges rein.
        /// </summary>
        /// <param name="level">Der Level</param>
        /// <param name="gameTime">Der GameTime</param>
        public void UpdatePhysics(Level level, GameTime gameTime, bool ringCollected, bool keyCollected)
        {
            if (!awake)
                return;

            //Apply Gravity
            if (!onladder)
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
            List<Tuple<Rectangle, int>> colliders = GetPossibleColliders(level, movedboundingBox, ringCollected, keyCollected);

            //Point Left = new Point(finalBoundingBox.Left, boundingBox.Center.Y);
            //Point Right = new Point(finalBoundingBox.Right, boundingBox.Center.Y);

            grounded = false;
            onladder = false;
            foreach (Tuple<Rectangle, int> collider in colliders)
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

                if (collider.Item2 == 1)
                {
                    if (collider.Item1.Contains(Top))
                    {
                        distanceCorrection.Y = collider.Item1.Bottom - Top.Y;
                        velocity.Y = 0;
                    }

                    if (collider.Item1.Contains(Bottom))
                    {
                        distanceCorrection.Y = collider.Item1.Top - Bottom.Y;
                        velocity.Y = 0;
                        velocity.X *= 0.9f;
                        grounded = true;
                    }

                    if (collider.Item1.Contains(TopLeft) || collider.Item1.Contains(BottomLeft))
                    {
                        distanceCorrection.X = collider.Item1.Right - TopLeft.X;
                        velocity.X = 0;
                    }

                    if (collider.Item1.Contains(TopRight) || collider.Item1.Contains(BottomRight))
                    {
                        distanceCorrection.X = collider.Item1.Left - TopRight.X;
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
                else if (collider.Item2 == 2)
                {
                    if (finalBoundingBox.Intersects(collider.Item1))
                        if (velocity.Y != 0)
                        {
                            velocity.Y = 0;
                            velocity.X = velocity.X * 0.1f;
                            onladder = true;
                        }
                }
            }


            boundingBox.X = (int)Math.Round(position.X);
            boundingBox.Y = (int)Math.Round(position.Y);
        }

        private static List<Tuple<Rectangle, int>> GetPossibleColliders(Level level, Rectangle movedboundingBox, bool ringCollected, bool keyCollected)
        {
            //Generate a list of all the colliders that are overlapping this area

            List<Tuple<Rectangle, int>> colliders = new List<Tuple<Rectangle, int>>();

            int Top = (movedboundingBox.Top / Level.Tilesize - 1);
            int Bottom = (movedboundingBox.Bottom / Level.Tilesize + 2);
            int Left = (movedboundingBox.Left / Level.Tilesize - 1);
            int Right = (movedboundingBox.Right / Level.Tilesize + 2);

            //Debug.DrawRectangle(new Rectangle(Left * Level.Tilesize, Top * Level.Tilesize, (Right - Left) * Level.Tilesize, (Bottom - Top) * Level.Tilesize), Color.Yellow * 0.2f);

            for (int x = Left; x < Right; x++)
            {
                for (int y = Top; y < Bottom; y++)
                {
                    int type = level[x, y];

                    if (!ringCollected && type == 3)
                        type = 1;

                    if (!keyCollected && type == 5)
                        type = 1;

                    if (type > 0)
                        colliders.Add(Tuple.Create<Rectangle, int>(new Rectangle(x * Level.Tilesize, y * Level.Tilesize, Level.Tilesize, Level.Tilesize), type));
                }
            }

            return colliders;
        }

        #endregion Public Methods

    }
}