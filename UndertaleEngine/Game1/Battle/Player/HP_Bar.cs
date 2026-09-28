using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace UndertaleEngine.Game1
{
    public class HP_Bar
    {
        private Texture2D _pixel;
        private Vector2 _position;
        private int _width;
        private int _height;
        public bool damaged = false;
        public float _realHP;
        public float _maxHP;
        public float RealHP => _realHP;
        public float MaxHP => _maxHP;
        private Color _hpFrontColor;
        private Color _hpBackColor;
        //CustomColor.SolidHPYellow;
        //CustomColor.SolidHPRed;
        private SoundEffect _hurt;

        public HP_Bar(GraphicsDevice device, Vector2 position, int width, int height, float maxHP, Color frontcolor, Color backcolor)
        {
            _pixel = new Texture2D(device, 1, 1);
            _pixel.SetData(new[] { Color.White });
            _position = position;
            _width = width;
            _height = height;
            _maxHP = maxHP;
            _realHP = maxHP;
            _hpFrontColor = frontcolor;
            _hpBackColor = backcolor;
        }
        public void LoadContent(ContentManager content)
        {
            _hurt = content.Load<SoundEffect>("Sounds/snd_hurt");
        }
        public void Damage(float amount)
        {
            _realHP = MathHelper.Max(0, _realHP - amount);
            _hurt.Play(1f, 0f, 0f);
            System.Diagnostics.Debug.WriteLine($"HP:{_realHP}/{_maxHP}");
        } 

        public void Heal(float amount)
        {
            _realHP = MathHelper.Min(_maxHP, _realHP + amount);
        }
        public void Update(GameTime gameTime)
        {

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            
            int frontWidth = (int)(_width * (_realHP / _maxHP));
            spriteBatch.Draw(_pixel, new Rectangle((int)_position.X, (int)_position.Y, _width, _height), _hpBackColor);
            spriteBatch.Draw(_pixel, new Rectangle((int)_position.X, (int)_position.Y, frontWidth, _height), _hpFrontColor);
        }
    }
}
