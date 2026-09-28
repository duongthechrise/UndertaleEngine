using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace UndertaleEngine.Game1
{
    public interface IScene
    {
        void Load(ContentManager content, GraphicsDevice graphics);
        void Update(GameTime gameTime);
        void Draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch);
        void Unload();
    }
}
