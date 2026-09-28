using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UndertaleEngine
{
    public abstract class UIElement
    {
        public int Slot = -1;
        public int row = -1;
        public int colum = -1;

        public Rectangle Bounds;
        public bool visible = true;
        public bool enabled = true;

        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(SpriteBatch spriteBatch) { }
    }
}
