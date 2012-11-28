using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;


using Microsoft.Xna.Framework;

namespace LabyrinthExplorer
{
    public class InputManager : IInputService
    {
        private KeyboardState keyState;
        private KeyboardState prevKeyState;
        private MouseState mouseState;
        private MouseState prevMouseState;

        private Point mousePos;

        private Game game;

        public InputManager(Game game)
        {
            this.game = game;

            keyState = Keyboard.GetState();
            prevKeyState = keyState;
            
            mouseState = Mouse.GetState();
            prevMouseState = mouseState;

            mousePos.X = mouseState.X;
            mousePos.Y = mouseState.Y;
        }

        public void Update()
        {
            prevKeyState = keyState;
            keyState = Keyboard.GetState();

            prevMouseState = mouseState;
            mouseState = Mouse.GetState();
            
            mousePos.X = mouseState.X;
            mousePos.Y = mouseState.Y;
        }

        /// <summary>
        /// Returns true if the key is currently held down
        /// </summary>
        /// <param name="key">Example: Keys.X</param>
        /// <returns></returns>
        public bool IsKeyDown(Keys key)
        {
            return keyState.IsKeyDown(key);
        }

        /// <summary>
        /// Returns true if the key is currently not pressed
        /// </summary>
        /// <param name="key">Example: Keys.X</param>
        /// <returns></returns>
        public bool IsKeyUp(Keys key)
        {
            return keyState.IsKeyUp(key);
        }

        /// <summary>
        /// Returns true if the key is down this frame, but was
        /// up previous frame
        /// </summary>
        /// <param name="key">Example: Keys.X</param>
        /// <returns></returns>
        public bool IsKeyDownOnce(Keys key)
        {
            if (keyState.IsKeyDown(key))
            {
                if (prevKeyState.IsKeyDown(key))
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns true if the key is up this frame, but was down
        /// the previous frame
        /// </summary>
        /// <param name="key">Example: Keys.X</param>
        /// <returns></returns>
        public bool IsKeyUpOnce(Keys key)
        {
            if (keyState.IsKeyUp(key))
            {
                if (prevKeyState.IsKeyUp(key))
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns true if the left Mouse button is pressed
        /// </summary>
        /// <returns></returns>
        public bool LeftMouseDown()
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
                return true;
            return false;
        }

        /// <summary>
        /// Returns true if the middle Mouse button is pressed
        /// </summary>
        /// <returns></returns>
        public bool MiddleMouseDown()
        {
            if (mouseState.MiddleButton == ButtonState.Pressed)
                return true;
            return false;
        }

        /// <summary>
        /// Returns true if the right Mouse button is pressed
        /// </summary>
        /// <returns></returns>
        public bool RightMouseDown()
        {
            if (mouseState.RightButton == ButtonState.Pressed)
                return true;
            return false;
        }

        /// <summary>
        /// Returns true if the left Mouse button is released
        /// </summary>
        /// <returns></returns>
        public bool LeftMouseUp()
        {
            if (mouseState.LeftButton == ButtonState.Released)
                return true;
            return false;
        }

        /// <summary>
        /// Returns true if the middle Mouse button is released
        /// </summary>
        /// <returns></returns>
        public bool MiddleMouseUp()
        {
            if (mouseState.MiddleButton == ButtonState.Released)
                return true;
            return false;
        }

        /// <summary>
        /// Returns true if the right Mouse button is release
        /// </summary>
        /// <returns></returns>
        public bool RightMouseUp()
        {
            if (mouseState.RightButton == ButtonState.Released)
                return true;
            return false;
        }

        /// <summary>
        /// Returns true if the left Mouse button is pressed once
        /// </summary>
        /// <returns></returns>
        public bool LeftMouseDownOnce()
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
                if(prevMouseState.LeftButton == ButtonState.Released)
                    return true;
            return false;
        }

        /// <summary>
        /// Returns true if the middle Mouse button is pressed once
        /// </summary>
        /// <returns></returns>
        public bool MiddleMouseDownOnce()
        {
            if (mouseState.MiddleButton == ButtonState.Pressed)
                if (prevMouseState.MiddleButton == ButtonState.Released)
                    return true;
            return false;
        }

        /// <summary>
        /// Returns true if the right Mouse button is pressed once
        /// </summary>
        /// <returns></returns>
        public bool RightMouseDownOnce()
        {
            if (mouseState.RightButton == ButtonState.Pressed)
                if (prevMouseState.RightButton == ButtonState.Released)
                    return true;
            return false;
        }

        /// <summary>
        /// Returns true if the left Mouse button is released once
        /// </summary>
        /// <returns></returns>
        public bool LeftMouseUpOnce()
        {
            if (mouseState.LeftButton == ButtonState.Released)
                if (prevMouseState.LeftButton == ButtonState.Pressed)
                    return true;
            return false;
        }

        /// <summary>
        /// Returns true if the middle Mouse button is released once
        /// </summary>
        /// <returns></returns>
        public bool MiddleMouseUpOnce()
        {
            if (mouseState.MiddleButton == ButtonState.Released)
                if (prevMouseState.MiddleButton == ButtonState.Pressed)
                    return true;
            return false;
        }

        /// <summary>
        /// Returns true if the right Mouse button is release once
        /// </summary>
        /// <returns></returns>
        public bool RightMouseUpOnce()
        {
            if (mouseState.RightButton == ButtonState.Released)
                if (prevMouseState.RightButton == ButtonState.Pressed)
                    return true;
            return false;
        }

        public Point MousePos { get { return mousePos; } }

        public int MouseX { get { return mousePos.X; } }
        public int MouseY { get { return mousePos.Y; } }

        public void SetMouseVisible(bool mouseVisible)
        {
            game.IsMouseVisible = mouseVisible;
        }
    }
}
