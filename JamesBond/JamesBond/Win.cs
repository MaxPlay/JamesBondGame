using JamesBond.Statemachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;

namespace JamesBond
{
    public class Win : State
    {
        Video video;
        VideoPlayer player;
        float fadeout;
        private bool videoplayed;
        Rectangle window;
        Texture2D logo;
        private bool waitforInput;
        private float pressStartFadeAnimator;
        private Texture2D pressStart;

        public Win(string name, StateMachine statemachine) : base(name, statemachine)
        {
            player = new VideoPlayer();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            if (player.State == MediaState.Playing)
                spriteBatch.Draw(player.GetTexture(), window, Color.White);

            spriteBatch.Draw(logo, new Vector2(Game1.graphics.PreferredBackBufferWidth / 2, Game1.graphics.PreferredBackBufferHeight / 2), null, Color.White * (1 - fadeout), 0, new Vector2(logo.Bounds.Center.X, logo.Bounds.Center.Y), 1, SpriteEffects.None, 0);

            if (waitforInput)
                spriteBatch.Draw(pressStart, new Vector2(Game1.graphics.PreferredBackBufferWidth / 2, Game1.graphics.PreferredBackBufferHeight / 2 + 100), null, Color.Lerp(Color.White, new Color(0.1f, 0.1f, 0.1f), pressStartFadeAnimator), 0, new Vector2(pressStart.Bounds.Center.X, pressStart.Bounds.Center.Y), 1, SpriteEffects.None, 0);

            spriteBatch.End();
        }

        public override void Initialize()
        {
            video = Game1.ContentManager.Load<Video>("HelloJames");
            logo = Game1.ContentManager.Load<Texture2D>("YouWon_" + Settings.Language);
            pressStart = Game1.ContentManager.Load<Texture2D>("PressStart_" + Settings.Language);
            window = new Rectangle(
                Game1.graphics.PreferredBackBufferWidth / 2 - video.Width / 2,
                Game1.graphics.PreferredBackBufferHeight / 2 - video.Height / 2,
                video.Width,
                video.Height
                );
            fadeout = 1;
        }

        public override void Unload()
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (player.State == MediaState.Stopped && !this.videoplayed)
            {
                player.IsLooped = false;
                player.Play(video);
                videoplayed = true;
            }

            if (((video.Duration.TotalMilliseconds - player.PlayPosition.TotalMilliseconds) / 1000f) < 3)
            {
                fadeout -= 0.02f;
                if (fadeout < 0)
                {
                    fadeout = 0;
                    waitforInput = true;
                }
            }

            if (InputManager.KeyPressed(Keys.Space) && waitforInput)
                this.statemachine.SetCurrentState("MainMenu");

            if (InputManager.KeyPressed(Keys.Space) && player.State == MediaState.Playing)
            {
                player.Stop();
                fadeout = 0;
                waitforInput = true;
            }

            pressStartFadeAnimator = (float)Math.Sin(gameTime.TotalGameTime.TotalMilliseconds / 1000f * 4) / 2 + 0.5f;
        }
    }
}
