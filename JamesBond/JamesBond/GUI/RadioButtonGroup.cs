using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JamesBond.GUI
{
    class RadioButtonGroup
    {
        private List<RadioButton> buttons;

        public RadioButtonGroup()
        {
            buttons = new List<RadioButton>();
        }

        public void AddRadioButton(RadioButton button)
        {
            buttons.Add(button);
            button.Click += Button_Click;
        }

        private void Button_Click(object sender, EventArgs e)
        {
            foreach (RadioButton button in buttons)
            {
                button.Active = button == sender;
                if (button.Active)
                    OnSelectionChanged(button.Text);
            }
        }

        public event EventHandler<RadioButtonEventArgs> SelectionChanged;

        private void OnSelectionChanged(string text)
        {
            SelectionChanged?.Invoke(this, new RadioButtonEventArgs(text));
        }

        public void Update()
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].Update();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].Draw(spriteBatch);
            }
        }
    }
}
