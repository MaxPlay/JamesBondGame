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
using JamesBond.Items;
using JamesBond.Rendering;
using System.IO;

namespace JamesBond
{
    class Gameplay : State
    {
        Dictionary<Point, Level> levels;
        Bond player;
        Point currentLevel;
        Point targetLevel;
        SpectreRing ring;
        private float transitionTimer;

        void SetNextLevel(Directions targetDirection)
        {
            Rectangle b = player.BoundingBox;
            switch (targetDirection)
            {
                case Directions.east:
                    currentLevel = new Point(currentLevel.X + 1, currentLevel.Y);
                    b.X -= Level.Tilesize * 10;
                    break;
                case Directions.south:
                    currentLevel = new Point(currentLevel.X, currentLevel.Y + 1);
                    b.Y -= Level.Tilesize * 10;
                    break;
                case Directions.west:
                    currentLevel = new Point(currentLevel.X - 1, currentLevel.Y);
                    b.X += Level.Tilesize * 10;
                    break;
                case Directions.north:
                    currentLevel = new Point(currentLevel.X, currentLevel.Y - 1);
                    b.Y += Level.Tilesize * 10;
                    break;
            }
            player.BoundingBox = b;
            transitionTimer = 0;
        }

        public Gameplay(string name, StateMachine statemachine)
            : base(name, statemachine)
        {
            levels = new Dictionary<Point, Level>();
            LoadLevels();
            ring = new SpectreRing();
            ring.BoundingBox = new Rectangle(100, 100, ring.BoundingBox.Width, ring.BoundingBox.Height);
            player = new Bond();
            player.BoundingBox = new Rectangle(100, 100, 20, 40);
            /*levels.Add(new Point(0, 0), new Level());
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
            */
        }

        private void LoadLevels()
        {
            string[] levelfiles = Directory.EnumerateFiles("Content/Levels/").ToArray();

            for (int i = 0; i < levelfiles.Length; i++)
            {
                string[] content = File.ReadAllLines(levelfiles[i]);
                levels.Add(new Point(int.Parse(content[0]), int.Parse(content[1])), new Level(content));
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, Camera.Main.TransformMatrix);
            spriteBatch.Begin();
            for (int y = 0; y < levels[currentLevel].Dimension.Y; y++)
            {
                for (int x = 0; x < levels[currentLevel].Dimension.X; x++)
                {
                    spriteBatch.Draw(Game1.Pixel,
                        new Rectangle(
                            x * Level.Tilesize,
                            y * Level.Tilesize,
                            Level.Tilesize,
                            Level.Tilesize),
                        levels[currentLevel][x, y] > 0 ? Color.Red : Color.Transparent);
                }
            }
            //levels[currentLevel].Draw(spriteBatch);
            //if (currentLevel != targetLevel)
            //levels[targetLevel].Draw(spriteBatch);
            player.Draw(spriteBatch);
            ring.Draw(spriteBatch);
            spriteBatch.End();
        }

        public override void Initialize()
        {
            currentLevel = new Point(2, 5);
            targetLevel = new Point(2, 5);
        }

        public override void Unload()
        {

        }

        public override void Update(GameTime gameTime)
        {
            PhysicsManager.Update(gameTime, levels[currentLevel]);

            if (InputManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Space) && player.Grounded)
                player.Velocity += Vector2.UnitY * -6;

            if (InputManager.CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
                player.Velocity += Vector2.UnitX * 1;

            if (InputManager.CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
                player.Velocity += Vector2.UnitX * -1;

            player.Update(gameTime);

            if (player.BoundingBox.Center.X > 32 * 10)
                SetNextLevel(Directions.east);

            if (player.BoundingBox.Center.X < 0)
                SetNextLevel(Directions.west);

            if (player.BoundingBox.Center.Y > 32 * 10)
                SetNextLevel(Directions.south);

            if (player.BoundingBox.Center.Y < 0)
                SetNextLevel(Directions.north);
        }
    }
}
