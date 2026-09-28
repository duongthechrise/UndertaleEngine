
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace UndertaleEngine
{
    public abstract class  UIPage
    {
        protected List<UIElement> elements = new();

        public virtual void OnEnter() { }
        public virtual void OnExit() { }
        public virtual void Update(GameTime gameTime)
        {
            foreach (var e in elements)
            {
                e.Update(gameTime);
            }
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach(var e in elements)
            {
                e.Draw(spriteBatch);
            }
        }
    }
}
