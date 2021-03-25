using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NetBoxes.Input
{
    public class KeyboardInput
    {
        private KeyboardState _keyboardState;
        private KeyboardState _oldKeyboardState;

        public KeyboardInput()
        {
            _keyboardState = Keyboard.GetState();
            _oldKeyboardState = Keyboard.GetState();
        }

        public bool IsKeyPressed(Keys key)
        {
            return _keyboardState.IsKeyDown(key) && _oldKeyboardState.IsKeyUp(key);
        }

        public bool IsKeyDown(Keys key)
        {
            return _keyboardState.IsKeyDown(key) && _oldKeyboardState.IsKeyDown(key);
        }

        public void Update()
        {
            _oldKeyboardState = _keyboardState;
            _keyboardState = Keyboard.GetState();
        }
    }
}
