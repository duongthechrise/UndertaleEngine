
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1.Effects;

namespace UndertaleEngine.Game1
{
    public class DisplayText : TextBase
    {
        public DisplayText(SpriteFont font, string text, Vector2 position, Color color, float scale = 1f, bool shadow = false) : base(font, text, position, color, scale, shadow)
        {
            for (int i = 0; i < _text.Length; i++)
            {
                if (_text[i] == '~')
                {
                    if (_text[i + 1] == 'R')
                        color = Color.Red;
                }
                i += 2;
            }
        }
        public override void Update(GameTime gametime)
        {
        }
        public void FixedUpdate(string text)
        {
            text = _text;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_shadow)
            {
                spriteBatch.DrawString(_font, _text, _position + Vector2.One, Color.MidnightBlue, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
            }
            spriteBatch.DrawString(_font, _text, _position, _color, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
        }
    }
}
