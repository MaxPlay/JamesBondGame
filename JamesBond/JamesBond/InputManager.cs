using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JamesBond
{
    public static class InputManager
    {
        public static Point MousePosition { get { return new Point(currentMouseState.X, currentMouseState.Y); } }
        public static Point PreviousMousePosition { get { return new Point(previousMouseState.X, previousMouseState.Y); } }

        private static KeyboardState currentKeyboardState;
        private static KeyboardState previousKeyboardState;
        private static MouseState currentMouseState;
        private static MouseState previousMouseState;
        private static GamePadState[] currentGamePadState;
        private static GamePadState[] previousGamePadState;

        public static KeyboardState CurrentKeyboardState { get { return currentKeyboardState; } }
        public static KeyboardState PreviousKeyboardState { get { return previousKeyboardState; } }

        public static MouseState CurrentMouseState { get { return currentMouseState; } }
        public static MouseState PreviousMouseState { get { return previousMouseState; } }

        public static GamePadState[] CurrentGamePadState { get { return currentGamePadState; } }
        public static GamePadState[] PreviousGamePadState { get { return previousGamePadState; } }

        static InputManager()
        {
            //The other fields don't need initialization.
            currentGamePadState = new GamePadState[4];
            previousGamePadState = new GamePadState[4];
        }

        public static void Update()
        {
            previousMouseState = currentMouseState;
            previousKeyboardState = currentKeyboardState;
            for (int i = 0; i < 4; i++)
            {
                previousGamePadState[i] = currentGamePadState[i];
            }

            currentMouseState = Mouse.GetState();
            currentKeyboardState = Keyboard.GetState();
            for (int i = 0; i < 4; i++)
            {
                currentGamePadState[i] = GamePad.GetState((PlayerIndex)i);
            }
        }

        public static bool MouseLeftClicked()
        {
            return currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released;
        }

        public static bool MouseRightClicked()
        {
            return currentMouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Released;
        }

        public static bool MouseMiddleClicked()
        {
            return currentMouseState.MiddleButton == ButtonState.Pressed && previousMouseState.MiddleButton == ButtonState.Released;
        }

        public static int Scrollwheelvalue()
        {
            return (int)((currentMouseState.ScrollWheelValue - previousMouseState.ScrollWheelValue) / 120f);
        }

        public static bool KeyPressed(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyUp(key);
        }

        public static bool KeyReleased(Keys key)
        {
            return currentKeyboardState.IsKeyUp(key) && previousKeyboardState.IsKeyDown(key);
        }

        public static bool ButtonPressed(PlayerIndex playerIndex, Buttons buttons)
        {
            return currentGamePadState[(int)playerIndex].IsButtonDown(buttons) && previousGamePadState[(int)playerIndex].IsButtonUp(buttons);
        }

        public static bool SetVibration(PlayerIndex index, float leftMotor, float rightMotor)
        {
            return GamePad.SetVibration(index, leftMotor, rightMotor);
        }
    }
}
