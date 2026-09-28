
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UndertaleEngine
{
    public class UIManager
    {
        UIPage currentPage;
        public void SetPage(UIPage page)
        {
            currentPage?.OnExit();
            currentPage = page;
            currentPage?.OnEnter();
        }
        public void Update(GameTime gameTime)
        {
            currentPage?.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            currentPage?.Draw(spriteBatch);
        }
    }
}
