using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace UndertaleEngine.Game1
{
    public abstract class TextBase
    {
        protected SpriteFont _font;
        protected string _text;
        protected Vector2 _position;
        protected Color _color;
        protected float _scale;
        protected bool _shadow;

        public TextBase(SpriteFont font, string text, Vector2 position, Color color, float scale = 1f, bool shadow = false)
        {
            _font = font;
            _text = text;
            _position = position;
            _color = color;
            _scale = scale;
            _shadow = shadow;
        }
        public void SetText(string newText)
        {
            _text = newText;
        }
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}

