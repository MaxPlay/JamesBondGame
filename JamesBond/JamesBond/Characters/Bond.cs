using JamesBond.Animations;
using JamesBond.Levels;
using JamesBond.Physics;
using JamesBond.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JamesBond.Characters
{
    public class Bond : Rigidbody
    {
        enum BondStates
        {
            idleRight,
            idleLeft,
            walkRight,
            walkLeft,
            shootRight,
            shootLeft,
            jumping,
            falling
        }

        BondStates state;
        Animator animator;
        SpriteSheet texture;
        private Directions facing;

        public Bond() : base()
        {
            facing = Directions.east;
            state = BondStates.idleRight;
            animator = new Animator();
            awake = true;
            texture = new SpriteSheet(Game1.ContentManager.Load<Texture2D>("Bond"), "Bond");

            limitVelocity = new Vector2(2, 10);

            animator.AddAnimation("idleLeft", new Animation(0, SpriteEffects.FlipHorizontally, 0));
            animator.AddAnimation("idleRight", new Animation(0, 0));
            animator.AddAnimation("walkLeft", new Animation(0.1f, SpriteEffects.FlipHorizontally, 1, 2, 3, 4, 5, 6, 7, 8));
            animator.AddAnimation("walkRight", new Animation(0.1f, 1, 2, 3, 4, 5, 6, 7, 8));
            animator.AddAnimation("shootLeft", new Animation(0.1f, SpriteEffects.FlipHorizontally, 9, 10, 11, 12, 13));
            animator.AddAnimation("shootRight", new Animation(0.1f, 9, 10, 11, 12, 13));
            animator.PlayAnimation("walkLeft");
            animator.Start();
        }

        public void Update(GameTime gameTime)
        {
            Console.WriteLine("{1} {0} {2}", velocity, state, boundingBox);
            switch (state)
            {
                case BondStates.idleRight:
                    animator.PlayAnimation("idleRight");
                    GenericMovement();

                    break;
                case BondStates.idleLeft:
                    animator.PlayAnimation("idleLeft");
                    GenericMovement();

                    break;
                case BondStates.walkRight:
                    animator.PlayAnimation("walkRight");
                    GenericMovement();
                    break;
                case BondStates.walkLeft:
                    animator.PlayAnimation("walkLeft");
                    GenericMovement();
                    break;
                case BondStates.shootRight:
                    animator.PlayAnimation("shootRight");
                    break;
                case BondStates.shootLeft:
                    animator.PlayAnimation("shootLeft");
                    break;
                case BondStates.jumping:
                    if (velocity.Y > -0.1f)
                        state = facing == Directions.east ? BondStates.idleRight : BondStates.idleLeft;
                    if (velocity.Y > 0.2f)
                        state = BondStates.falling;
                    break;

                case BondStates.falling:
                    if (velocity.Y > -0.1f && velocity.Y < 0.2f)
                        state = facing == Directions.east ? BondStates.idleRight : BondStates.idleLeft;
                    break;
            }

            animator.Update(gameTime);
        }

        private void GenericMovement()
        {
            if (velocity.X <= 0.1f && velocity.X >= -0.1f)
            {
                state = facing == Directions.east ? BondStates.idleRight : BondStates.idleLeft;
            }

            if (velocity.X > 0.1f)
            {
                state = BondStates.walkRight;
                facing = Directions.east;
            }

            if (velocity.X < -0.1f)
            {
                state = BondStates.walkLeft;
                facing = Directions.west;
            }

            if (velocity.Y > 0.2f)
                state = BondStates.falling;

            if (velocity.Y < -0.2f)
                state = BondStates.jumping;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(Game1.Pixel, new Rectangle(0, 0, 720, 720), Color.White);
            spriteBatch.Draw(Game1.Pixel, BoundingBox, Color.Blue);
            spriteBatch.Draw(texture.Texture, new Vector2((animator.CurrentEffect == SpriteEffects.None ? texture[animator.CurrentFrame].Offset : texture[animator.CurrentFrame].OffsetFlipped) + this.boundingBox.X, boundingBox.Top), texture[animator.CurrentFrame].SpriteRectangle, Color.White, 0, Vector2.Zero, 1, animator.CurrentEffect, 0);
            //Console.WriteLine("{0}, {1}", new Vector2((animator.CurrentEffect == SpriteEffects.None ? texture[animator.CurrentFrame].Offset : texture[animator.CurrentFrame].OffsetFlipped) + this.boundingBox.X, boundingBox.Top), animator.CurrentFrame);
        }
    }
}
