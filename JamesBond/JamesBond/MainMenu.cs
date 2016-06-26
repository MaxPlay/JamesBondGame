using System;
using JamesBond.Statemachine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using JamesBond.GUI;

namespace JamesBond
{
    internal class MainMenu : State
    {
        private float introAnim;
        Texture2D logo;
        Button btnStart;
        private SpriteFont font;
        private Button btnSettings;
        private Button btnCredits;
        private Button btnQuit;

        public MainMenu(string name, StateMachine statemachine)
            : base(name, statemachine)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(logo, new Vector2(Game1.graphics.PreferredBackBufferWidth / 2, Game1.graphics.PreferredBackBufferHeight / 2 - 50 - 50 * (float)Math.Sin(introAnim)), null, Color.White, 0, new Vector2(logo.Bounds.Center.X, logo.Bounds.Center.Y), 1, SpriteEffects.None, 0);

            if (introAnim >= Math.PI / 2)
            {
                btnStart.Draw(spriteBatch);
                btnSettings.Draw(spriteBatch);
                btnCredits.Draw(spriteBatch);
                btnQuit.Draw(spriteBatch);
            }

            spriteBatch.End();
        }

        public override void Initialize()
        {
            logo = Game1.ContentManager.Load<Texture2D>("SpectreLogo");
            font = Game1.ContentManager.Load<SpriteFont>("Default");
            btnStart = new Button(null, null, "Start", new Point(50, 190));
            btnStart.Font = font;
            btnStart.TextColor = Color.DarkGray;
            btnStart.HoverTextColor = Color.White;
            btnStart.BoundingBox = new Rectangle(btnStart.BoundingBox.X, btnStart.BoundingBox.Y, 220, 24);
            btnStart.Click += BtnStart_Click;

            btnSettings = new Button(null, null, "Settings", new Point(50, 220));
            btnSettings.Font = font;
            btnSettings.TextColor = Color.DarkGray;
            btnSettings.HoverTextColor = Color.White;
            btnSettings.BoundingBox = new Rectangle(btnSettings.BoundingBox.X, btnSettings.BoundingBox.Y, 220, 24);
            btnSettings.Click += BtnSettings_Click;

            btnCredits = new Button(null, null, "Credits", new Point(50, 250));
            btnCredits.Font = font;
            btnCredits.TextColor = Color.DarkGray;
            btnCredits.HoverTextColor = Color.White;
            btnCredits.BoundingBox = new Rectangle(btnCredits.BoundingBox.X, btnCredits.BoundingBox.Y, 220, 24);
            btnCredits.Click += BtnCredits_Click;

            btnQuit = new Button(null, null, "Quit", new Point(50, 280));
            btnQuit.Font = font;
            btnQuit.TextColor = Color.DarkGray;
            btnQuit.HoverTextColor = Color.White;
            btnQuit.BoundingBox = new Rectangle(btnQuit.BoundingBox.X, btnQuit.BoundingBox.Y, 220, 24);
            btnQuit.Click += BtnQuit_Click;
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            statemachine.SetCurrentState("Gameplay");
        }

        private void BtnSettings_Click(object sender, EventArgs e)
        {
            statemachine.SetCurrentState("Settings");
        }

        private void BtnCredits_Click(object sender, EventArgs e)
        {
            statemachine.SetCurrentState("Credits");
        }

        private void BtnQuit_Click(object sender, EventArgs e)
        {
            statemachine.ExitGame();
        }

        public override void Unload()
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (introAnim < Math.PI / 2)
                introAnim += 0.02f;

            btnStart.Update();
            btnSettings.Update();
            btnCredits.Update();
            btnQuit.Update();
        }
    }
}