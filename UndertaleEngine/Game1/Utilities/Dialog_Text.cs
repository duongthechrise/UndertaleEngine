
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1.Effects;

namespace UndertaleEngine.Game1
{
    public class DialogText : TextBase
    {
        private float _charInterval;
        private float _timer;
        private int _visibleChars;
        private bool _isFinished;

        public DialogText(SpriteFont font, string text, Vector2 position, Color color, float scale = 1f, float charsPerSecond = 30f, bool shadow = false) : base(font, text, position, color, scale, shadow)
        {
            _charInterval = 1f / charsPerSecond;
            _timer = 0f;
            _visibleChars = 0;
            _isFinished = false;
        }


        public override void Update(GameTime gameTime)
        {
            if (_isFinished)
                return;
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            while (_timer >= _charInterval && _visibleChars < _text.Length)
            {
                _visibleChars++;
                _timer -= _charInterval;
            }
            if (_visibleChars >= _text.Length)
                _isFinished = true;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            string visibleText = _text.Substring(0, _visibleChars);
            if (_shadow)
            {
                spriteBatch.DrawString(_font, visibleText, _position + Vector2.One, Color.MidnightBlue, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
            }
            spriteBatch.DrawString(_font, visibleText, _position, _color, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
        }
    }
}
