using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JamesBond.Statemachine
{
    public class StateMachine
    {
        private bool exiting;

        public bool Exiting
        {
            get { return exiting; }
        }


        Dictionary<string, State> states;
        private string targetState;
        private string currentState;
        private bool running;

        public void SetCurrentState(string name)
        {
            if (states.ContainsKey(targetState))
                targetState = name;
        }

        public StateMachine()
        {
            states = new Dictionary<string, State>();
        }

        public void AddState(string name, State state)
        {
            if (states.ContainsKey(name))
                return;

            states.Add(name, state);
            if (states.Count == 1)
            {
                currentState = name;
                targetState = name;
            }
        }

        public void Start()
        {
            running = true;
            states[currentState].Initialize();
        }

        public void Stop()
        {
            running = false;
            states[currentState].Unload();
        }

        public void Update(GameTime gameTime)
        {
            if (states.Count == 0 || !running)
                return;

            if (targetState != currentState)
            {
                states[targetState].Initialize();
                states[currentState].Unload();
                currentState = targetState;
            }
            states[targetState].Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            states[currentState].Draw(spriteBatch);
        }

        public void ExitGame()
        {
            exiting = true;
        }
    }
}
