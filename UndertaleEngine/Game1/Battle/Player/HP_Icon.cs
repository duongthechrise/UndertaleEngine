using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace UndertaleEngine.Game1
{
    public class PlayerHP
    {
        Texture2D _texture;
        Vector2 _position;
        float scale = 1;

        public PlayerHP()
        {
            _position = new Vector2(0, 0);
        }
        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("Sprites/Player/HP_Icon");
            _position = new Vector2(244, 405);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }


    }
}
