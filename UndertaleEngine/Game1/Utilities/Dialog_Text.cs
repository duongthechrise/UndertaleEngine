
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
        float fixspeed = 1f;
        float charpersecond;

        public DialogText(SpriteFont font, string text, Vector2 position, Color color, float scale = 1f, float charsPerSecond = 30f, bool shadow = false) : base(font, text, position, color, scale, shadow)
        {
            _charInterval = fixspeed / charsPerSecond;
            _timer = 0f;
            _visibleChars = 0;
            _isFinished = false;
            charpersecond = charsPerSecond;
        }


        public override void Update(GameTime gameTime)
        {
            if (_isFinished)
                return;
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            while(_visibleChars < _text.Length)
            {
                if (_text[_visibleChars] == '\\')
                {
                    int end = _text.IndexOf('s', _visibleChars);
                    if(end != -1)
                    {
                        string value = _text.Substring(_visibleChars + 1, end - _visibleChars - 1);
                        if(float.TryParse(value, out float speed))
                        {
                            fixspeed = speed;
                            _charInterval = fixspeed / charpersecond;
                            _visibleChars = end + 1;
                            continue;
                        }
                    }
                }
                if (_timer < _charInterval)
                    break;
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
