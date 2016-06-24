using JamesBond.Statemachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using JamesBond.Levels;
using JamesBond.Physics;
using JamesBond.Characters;

namespace JamesBond
{
    class Gameplay : State
    {
        Dictionary<int, Level> levels;
        Bond player;
        int currentLevel;

        public Gameplay(string name, StateMachine statemachine)
            : base(name, statemachine)
        {
            player = new Bond();
            player.BoundingBox = new Rectangle(100, 100, 20, 40);
            levels = new Dictionary<int, Level>();
            levels.Add(0, new Level());
            levels[currentLevel].Tilesize = 32;
            Random rand = new Random(DateTime.Now.Millisecond);
            for (int y = 0; y < levels[currentLevel].Dimension.Y; y++)
            {
                for (int x = 0; x < levels[currentLevel].Dimension.X; x++)
                {
                    levels[currentLevel][x, y] = x == 0 || y == 0 || x == levels[currentLevel].Dimension.X - 1 || y == levels[currentLevel].Dimension.Y - 1 ? 1 : 0;
                    if (levels[currentLevel][x, y] == 0)
                        levels[currentLevel][x, y] = rand.Next(0, 100) < 10 ? 1 : 0;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            for (int y = 0; y < levels[currentLevel].Dimension.Y; y++)
            {
                for (int x = 0; x < levels[currentLevel].Dimension.X; x++)
                {
                    spriteBatch.Draw(Game1.Pixel, new Rectangle(x * levels[currentLevel].Tilesize, y * levels[currentLevel].Tilesize, levels[currentLevel].Tilesize, levels[currentLevel].Tilesize), levels[currentLevel][x, y] > 0 ? Color.Red : Color.Transparent);
                }
            }
            player.Draw(spriteBatch);
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
            PhysicsManager.Update(gameTime, levels[currentLevel]);

            if (InputManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Space) && player.Grounded)
                player.Velocity += Vector2.UnitY * -5;

            if (InputManager.CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
                player.Velocity += Vector2.UnitX * 1;

            if (InputManager.CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
                player.Velocity += Vector2.UnitX * -1;
        }
    }
}
