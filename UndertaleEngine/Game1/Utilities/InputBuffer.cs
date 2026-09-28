
using Microsoft.Xna.Framework.Input;

namespace UndertaleEngine
{
    public class InputBuffer
    {
        KeyboardState current;
        KeyboardState previous;

        public void Update()
        {
            previous = current;
            current = Keyboard.GetState();
        }
        bool Pressed(Keys key) =>
            current.IsKeyDown(key) && previous.IsKeyDown(key);
        public bool Up() => Pressed(Keys.Up);
        public bool Down() => Pressed(Keys.Down);
        public bool Left() => Pressed(Keys.Left);
        public bool Right() => Pressed(Keys.Right);

        public bool Confirm() => Pressed(Keys.Z) || Pressed(Keys.Enter);
        public bool Cancel() => Pressed(Keys.X) || Pressed(Keys.RightShift) || Pressed(Keys.LeftShift);
    }
}
