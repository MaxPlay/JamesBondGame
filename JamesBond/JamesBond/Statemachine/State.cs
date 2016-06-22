using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JamesBond.Statemachine
{
    public abstract class State
    {
        StateMachine statemachine;

        public State(string name, StateMachine statemachine)
        {
            this.statemachine = statemachine;
            statemachine.AddState(name, this);
        }

        public abstract void Initialize();

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);

        public abstract void Unload();
    }
}
