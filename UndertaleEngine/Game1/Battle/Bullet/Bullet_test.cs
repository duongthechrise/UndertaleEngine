using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace UndertaleEngine.Game1
{
    public class Bullet_test
    {
        public Texture2D _texture;
        public Vector2 _position;

        KeyboardState k;
        public Bullet_test()
        {
            _position = new Vector2(0, 0);
        }
        public void LoadContent(ContentManager content, GraphicsDevice gd)
        {
            _texture = content.Load<Texture2D>("Sprites/Button/button_act");
            _position = new Vector2(0, 0);
        }
        public void Update(GameTime gameTime)
        {
            var m = Mouse.GetState();
            if (k.IsKeyDown(Keys.M))
            {
                _position = new Vector2(m.X, m.Y);
                
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, Color.White, 0f, new Vector2(_texture.Width / 2f, _texture.Height / 2f), 1f, SpriteEffects.None, 0f);
        }
    }
}

