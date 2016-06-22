﻿using JamesBond.Statemachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using JamesBond.Level;

namespace JamesBond
{
    class Gameplay : State
    {
        Dictionary<int, Level> levels;

        int currentLevel;

        public Gameplay(string name, StateMachine statemachine)
            : base(name, statemachine)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {

        }

        public override void Initialize()
        {

        }

        public override void Unload()
        {

        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}
