using System;
using NetBoxes.Input;

namespace NetBoxes
{
    public static class Context
    {
        private static Main _game;

        public static Main Game
        {
            get
            {
                return _game;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                _game = value;
            }
        }

        public static KeyboardInput Keyboard { get; private set; }

        static Context()
        {
            Keyboard = new KeyboardInput();
        }

        public static void Update()
        {
            Keyboard.Update();
        }
    }
}
