using JamesBond.Statemachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using JamesBond.GUI;

namespace JamesBond
{
    public class SettingsMenu : State
    {
        private SpriteFont font;
        RadioButtonGroup group;
        Button backButton;
        Button timerVisibleButton;

        public SettingsMenu(string name, StateMachine statemachine) : base(name, statemachine)
        {
            font = Game1.ContentManager.Load<SpriteFont>("Default");
            group = new RadioButtonGroup();
            for (int i = 0; i < Localization.ShortLanguages.Length; i++)
            {
                RadioButton newRadioButton = new RadioButton(null, null, null, Localization.Languages[i], new Point());
                newRadioButton.Font = font;
                newRadioButton.TextColor = Color.DarkGray;
                newRadioButton.HoverTextColor = Color.White;
                newRadioButton.ActiveTextColor = Color.Yellow;
                newRadioButton.BoundingBox = new Rectangle(50, 200 + i * 40, 220, 24);
                if (Localization.ShortLanguages[i] == Settings.Language)
                    newRadioButton.Active = true;
                group.AddRadioButton(newRadioButton);
            }
            group.SelectionChanged += Group_SelectionChanged;

            backButton = new Button(null, null, Localization.GetString(Settings.Language, "back"), new Point());
            backButton.Font = font;
            backButton.TextColor = Color.DarkGray;
            backButton.HoverTextColor = Color.White;
            backButton.BoundingBox = new Rectangle(200, 320, 100, 24);
            backButton.Click += BackButton_Click;

            //timerVisibleButton = new Button(null, null, Localization.GetString(Settings.Language, "timeractive"), new Point());
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            statemachine.SetCurrentState("MainMenu");
            Settings.Save();
        }

        private void Group_SelectionChanged(object sender, RadioButtonEventArgs e)
        {
            for (int i = 0; i < Localization.Languages.Length; i++)
            {
                if (Localization.Languages[i] == e.Language)
                    Settings.Language = Localization.ShortLanguages[i];
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            group.Draw(spriteBatch);
            backButton.Draw(spriteBatch);
            spriteBatch.End();
        }

        public override void Initialize()
        {

        }

        public override void Unload()
        {

        }

        public override void Update(GameTime gameTime)
        {
            group.Update();
            backButton.Update();
        }
    }
}
