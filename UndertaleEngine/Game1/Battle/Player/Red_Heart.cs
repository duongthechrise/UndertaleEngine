using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace UndertaleEngine.Game1
{
    public class Red_heart 
    {

        private Texture2D _texture;
        public Vector2 Position;

        public SpriteAnimation IsBlinking;
        public SpriteAnimation IsIdle;
        public SpriteAnimation CurrentAnimation;
        private Texture2D _pixel;
        KeyboardState _prevkeyboard;
        KeyboardState _prevkeyboard2;
        KeyboardState prevkey3;


        public float blinkTimer = 0f;
        private float blinkDuration = 0.6f;

        private float iFrameDuration = 0.6f;
        public float iFrameTimer = 0f;

        public bool IsInvincible => iFrameTimer > 0f;

        SoundEffect Switch;
        SoundEffect Confirm;

        public Red_heart(Vector2 pos)
        {
            Position = pos;
        }
        public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice)
        {
            _texture = content.Load<Texture2D>("Sprites/Player/heart");
            IsIdle = new SpriteAnimation(_texture, 2, 0, 0, 0, loop: false);
            IsBlinking = new SpriteAnimation(_texture, 2, 0, 1, 0.1f, loop: true);
            CurrentAnimation = IsIdle;
            _pixel = new Texture2D(graphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });
            Switch = content.Load<SoundEffect>("Sounds/snd_menu_switch");
            Confirm = content.Load<SoundEffect>("Sounds/snd_menu_confirm");
            
        }
        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (iFrameTimer > 0)
            {
                iFrameTimer -= dt;
                if (iFrameTimer <= 0)
                {
                    CurrentAnimation = IsIdle;
                    CurrentAnimation.Reset();
                    CurrentAnimation.Play();
                }
            }
            if (blinkTimer > 0)
            {
                blinkTimer -= dt;
            }
            CurrentAnimation.Update(gameTime);
            var k = Keyboard.GetState();
            if (k.IsKeyDown(Keys.R) && !_prevkeyboard.IsKeyDown(Keys.R))
            {
                Menu.IsMenu = !Menu.IsMenu;
            }
            _prevkeyboard = k;
            if (Menu.IsInMenu == true)
            {
                var k2 = Keyboard.GetState();
                if (k2.IsKeyDown(Keys.Left) && !_prevkeyboard2.IsKeyDown(Keys.Left))
                {
                    Menu.slot -= 1;
                    Switch.Play(0.65f, 0, 0);
                }
                if (k2.IsKeyDown(Keys.Right) && !_prevkeyboard2.IsKeyDown(Keys.Right))
                {
                    Menu.slot += 1;
                    Switch.Play(0.65f, 0, 0);
                }
                    _prevkeyboard2 = k2;

                if (Menu.slot <= 0)
                    Menu.slot = 4;
                if (Menu.slot >= 5)
                    Menu.slot = 1;

                if (Menu.slot == 1)
                    Position = new Vector2(48, 454);//-39,+1
                if (Menu.slot == 2)
                    Position = new Vector2(201, 454);
                if (Menu.slot == 3)
                    Position = new Vector2(361, 454);
                if (Menu.slot == 4)
                    Position = new Vector2(516, 454);
            }
            if(Menu.selectingFightopts == true)
            {
                var k4 = Keyboard.GetState();
                if(k4.IsKeyDown(Keys.Up) && !prevkey3.IsKeyDown(Keys.Up))
                {
                    if(Menu.FightOptsColumn > 1)
                    {
                        Menu.FightOptsColumn -= 1;
                        Switch.Play(0.65f, 0, 0);
                    }
                }
                if(k4.IsKeyDown(Keys.Down) && !prevkey3.IsKeyDown(Keys.Down))
                {
                    if(Menu.FightOptsColumn != 3 && Menu.FightSlots == 3 || 
                       Menu.FightOptsColumn == 1 && Menu.FightSlots == 2)
                    {
                        Menu.FightOptsColumn += 1;
                        Switch.Play(0.65f, 0, 0);
                    }
                }
                prevkey3 = k4;
            }
            if (Menu.selectingActopts == true)
            {
                var k3 = Keyboard.GetState();
                if (k3.IsKeyDown(Keys.Up) && !prevkey3.IsKeyDown(Keys.Up))
                {
                    if (Menu.ActOptsColumn == 2)
                    {
                        Menu.ActOptsColumn -= 1;
                        Switch.Play(0.65f, 0, 0);
                    }
                }
                if (k3.IsKeyDown(Keys.Down) && !prevkey3.IsKeyDown(Keys.Down))
                {
                    if (Menu.ActOptsRow == 1 && Menu.ActSlots == 4 || 
                        Menu.ActSlots == 3 && Menu.ActOptsColumn == 1 && Menu.ActOptsRow == 1)
                    {
                        Menu.ActOptsColumn += 1;
                        Switch.Play(0.65f, 0, 0);
                    }
                }
                if (k3.IsKeyDown(Keys.Left) && !prevkey3.IsKeyDown(Keys.Left))
                {
                    if (Menu.ActOptsRow == 2)
                    {
                        Menu.ActOptsRow -= 1;
                        Switch.Play(0.65f, 0, 0);
                    }
                }
                if (k3.IsKeyDown(Keys.Right) && !prevkey3.IsKeyDown(Keys.Right))
                {
                    if (Menu.ActSlots == 2 && Menu.ActOptsRow == 1 ||
                        Menu.ActSlots == 4 && Menu.ActOptsRow == 1 ||
                        Menu.ActSlots == 3 && Menu.ActOptsColumn == 1 && Menu.ActOptsRow == 1)
                    {
                        Menu.ActOptsRow += 1;
                        Switch.Play(0.65f, 0, 0);
                    }
                }
                //if(k3.IsKeyDown(Keys.Q) && !prevkey3.IsKeyDown(Keys.Q))
                //{
                //    Debug.WriteLine(Menu.ActOptsRow);
                //    Debug.WriteLine(Menu.ActOptsColumn);
                //    Debug.WriteLine(Menu.ActSlots);
                //}
                prevkey3 = k3;
            }
            if (Menu.selectingActopts)
            {
                if(Menu.ActOptsRow == 1 && Menu.ActOptsColumn == 1 && Menu.ActSlots >= 1)
                    Position = new Vector2(72, 286);
                if (Menu.ActOptsRow == 2 && Menu.ActOptsColumn == 1 && Menu.ActSlots >= 2)
                    Position = new Vector2(328, 286);
                if (Menu.ActOptsRow == 1 && Menu.ActOptsColumn == 2 && Menu.ActSlots >= 3)
                    Position = new Vector2(72, 318);
                if (Menu.ActOptsRow == 2 && Menu.ActOptsColumn == 2 && Menu.ActSlots == 4)
                    Position = new Vector2(328, 318);
            }
            if(Menu.selectingFightopts)
            {
                if (Menu.FightOptsColumn == 1)
                    Position = new Vector2(72, 286);
                if (Menu.FightOptsColumn == 2)
                    Position = new Vector2(72, 318);
                if (Menu.FightOptsColumn == 3)
                    Position = new Vector2(72, 350);
            }
            if(Menu.targetingEnemy1)
            {
                Position = new Vector2(-500, -500);
            }
        }
        public void TakeDamage()
        {
            if (IsInvincible)
                return;
            iFrameTimer = iFrameDuration;
            blinkTimer = blinkDuration;
            CurrentAnimation = IsBlinking;
            CurrentAnimation.Reset();
            CurrentAnimation.Play();
        }
        public Vector2 GetMovement(float speed)
        {
            KeyboardState k = Keyboard.GetState();
            Vector2 move = Vector2.Zero;

            if (k.IsKeyDown(Keys.Left) || k.IsKeyDown(Keys.A))
                move.X -= 1;
            if (k.IsKeyDown(Keys.Right) || k.IsKeyDown(Keys.D))
                move.X += 1;
            if (k.IsKeyDown(Keys.Up) || k.IsKeyDown(Keys.W))
                move.Y -= 1;
            if (k.IsKeyDown(Keys.Down) || k.IsKeyDown(Keys.S))
                move.Y += 1;

            if (move.LengthSquared() > 0)
                move.Normalize();

            return move * speed;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (_texture != null)
            {
                SpriteAnimation anim;
                anim = CurrentAnimation;
                spriteBatch.Draw(anim.Texture, Position, anim.GetSourceRect(), Color.Red, 0f, new Vector2(_texture.Width / 4, _texture.Height / 2), 1f, SpriteEffects.None, 0f);
            }
        }

    }
}