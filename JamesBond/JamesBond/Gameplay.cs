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
        Color[] debugcolors = new Color[] { Color.Transparent, Color.Red, Color.White, Color.Orange, Color.DarkOrange, Color.Blue, Color.Turquoise };

        Dictionary<Point, Level> levels;
        Bond player;
        Point currentLevel;
        Point targetLevel;
        private bool ringCollected;
        private bool keyCollected;
        private Texture2D guibase;
        private SpectreRing ring;
        private Key key;
        private TimeSpan timer;
        private SpriteFont lcdFont;
        private int ammo;
        private int health;
        private SpriteSheet ammoTex;
        private SpriteSheet healthTex;

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
        }

        public Gameplay(string name, StateMachine statemachine)
            : base(name, statemachine)
        {
            levels = new Dictionary<Point, Level>();
            player = new Bond();
            guibase = Game1.ContentManager.Load<Texture2D>("UI");
            ring = new SpectreRing();
            ring.BoundingBox = new Rectangle(135, 320, 32, 32);
            key = new Key();
            key.BoundingBox = new Rectangle(175, 320, 32, 32);
            lcdFont = Game1.ContentManager.Load<SpriteFont>("lcd");

            ammoTex = new SpriteSheet(Game1.ContentManager.Load<Texture2D>("ammo"), 29, 21);
            healthTex = new SpriteSheet(Game1.ContentManager.Load<Texture2D>("health"), 27, 27);
        }

        private void LoadLevels()
        {
            string[] levelfiles = Directory.EnumerateFiles("Content/Levels/").ToArray();

            for (int i = 0; i < levelfiles.Length; i++)
            {
                string[] content = File.ReadAllLines(levelfiles[i]);
                Point location = new Point(int.Parse(content[0]), int.Parse(content[1]));
                levels.Add(location, new Level(content));
                if (levels[location].HasCollectable() != null)
                    levels[location].HasCollectable().Collected += Gameplay_Collected;
            }
        }

        private void Gameplay_Collected(object sender, EventArgs e)
        {
            if (sender is SpectreRing)
                ringCollected = true;
            if (sender is Key)
                keyCollected = true;
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
                        debugcolors[levels[currentLevel][x, y]]);
                }
            }
            //levels[currentLevel].Draw(spriteBatch);
            player.Draw(spriteBatch);
            DrawGui(spriteBatch);
            spriteBatch.End();
        }

        private void DrawGui(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(guibase, new Vector2(0, 320), Color.White);
            if (ringCollected)
                ring.Draw(spriteBatch);
            if (keyCollected)
                key.Draw(spriteBatch);

            spriteBatch.DrawString(lcdFont, timer.ToString(@"hh\:mm\:ss"), new Vector2(34, 318), Color.White);

            spriteBatch.Draw(healthTex.Texture, new Rectangle(215, 323, (int)healthTex.TileSize.X, (int)healthTex.TileSize.Y), healthTex[5 - health].SpriteRectangle, Color.White);
            spriteBatch.Draw(ammoTex.Texture, new Rectangle(246, 326, (int)ammoTex.TileSize.X, (int)ammoTex.TileSize.Y), ammoTex[4 - ammo].SpriteRectangle, Color.White);
        }

        public override void Initialize()
        {
            currentLevel = new Point(2, 5);
            targetLevel = new Point(2, 5);
            LoadLevels();
            player.BoundingBox = new Rectangle(100, 200, 20, 40);
            timer = new TimeSpan(1, 0, 0);
            ammo = 4;
            health = 5;
        }

        public override void Unload()
        {
            keyCollected = false;
            ringCollected = false;
            levels.Clear();
        }

        public override void Update(GameTime gameTime)
        {
            UpdateGUI(gameTime);

            PhysicsManager.Update(gameTime, levels[currentLevel], ringCollected, keyCollected);

            //Jump
            if (InputManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Space) && player.Grounded)
                player.Velocity += Vector2.UnitY * -6;

            //Walk
            if (InputManager.CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
                player.Velocity += Vector2.UnitX * 1;

            if (InputManager.CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
                player.Velocity += Vector2.UnitX * -1;

            //LadderLogic (Overwrites Walk)
            if (player.OnLadder)
            {
                if (InputManager.CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Up))
                    player.Velocity = -Vector2.UnitY;
                if (InputManager.CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Down))
                    player.Velocity = Vector2.UnitY;
                if (InputManager.CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
                    player.Velocity = Vector2.UnitX;
                if (InputManager.CurrentKeyboardState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
                    player.Velocity = -Vector2.UnitX;
            }

            player.Update(gameTime);

            levels[currentLevel].Update(gameTime, player.BoundingBox);

            if (player.BoundingBox.Center.X > 32 * 10)
                SetNextLevel(Directions.east);

            if (player.BoundingBox.Center.X < 0)
                SetNextLevel(Directions.west);

            if (player.BoundingBox.Center.Y > 32 * 10)
                SetNextLevel(Directions.south);

            if (player.BoundingBox.Center.Y < 0)
                SetNextLevel(Directions.north);

            if (currentLevel == new Point(15, 4) && player.BoundingBox.X > 85)
                statemachine.SetCurrentState("Win");

            if (InputManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
                statemachine.SetCurrentState("MainMenu");
        }

        private void UpdateGUI(GameTime gameTime)
        {
            timer -= gameTime.ElapsedGameTime;
            ring.Update(gameTime);
            key.Update(gameTime);
        }
    }
}
