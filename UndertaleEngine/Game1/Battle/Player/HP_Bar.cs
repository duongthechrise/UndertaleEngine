using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

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
        bool Animation;
        float damageamount;
        float decreasedHP;
        float currentHP;
        public float t = 0;
        bool startanimation = false;
        public float alpha = 1f;

        public HP_Bar(GraphicsDevice device, Vector2 position, int width, int height, float maxHP, Color frontcolor, Color backcolor, bool animation = false)
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
            Animation = animation;
            currentHP = _realHP;
        }
        public void LoadContent(ContentManager content)
        {
            _hurt = content.Load<SoundEffect>("Sounds/snd_hurt");
        }
        public void Damage(float amount)
        {
            damageamount = amount;
            if (Animation)
            {
                startanimation = true;
                currentHP = _realHP;
            }
            else
            {
                _realHP = MathHelper.Max(0, _realHP - amount);
                _hurt.Play(1f, 0f, 0f);
            }
            
        } 

        public void Heal(float amount)
        {
            _realHP = MathHelper.Min(_maxHP, _realHP + amount);
        }
        public void Update(GameTime gameTime)
        {
            if (startanimation)
            {
                t += (float)gameTime.ElapsedGameTime.TotalSeconds / 0.25f;
                t = MathHelper.Clamp(t, 0, 1);
                if (t >= 1f)
                {
                    startanimation = false;
                    t = 0f;
                    currentHP = _realHP;
                }
                _realHP = MathF.Round(MathHelper.Lerp(currentHP, currentHP - damageamount, t));
                _realHP = MathHelper.Max(0, _realHP);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            
            int frontWidth = (int)(_width * (_realHP / _maxHP));
            spriteBatch.Draw(_pixel, new Rectangle((int)_position.X - 1, (int)_position.Y - 1, _width + 2, _height + 2), Color.Black * alpha);
            spriteBatch.Draw(_pixel, new Rectangle((int)_position.X, (int)_position.Y, _width, _height), _hpBackColor * alpha);
            spriteBatch.Draw(_pixel, new Rectangle((int)_position.X, (int)_position.Y, frontWidth, _height), _hpFrontColor * alpha);
            
        }
    }
}
