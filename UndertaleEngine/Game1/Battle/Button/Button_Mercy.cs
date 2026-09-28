using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;


namespace UndertaleEngine.Game1
{
    public class Button_Mercy
    {
        private Texture2D _texture;
        public Vector2 Position;
        public SpriteAnimation IsHover;
        public SpriteAnimation IsIdle;
        public SpriteAnimation CurrentAnimation;
        public OBB Hitbox;
        SoundEffect Confirm;

        KeyboardState prevkey;
        public Button_Mercy()
        {
            Position = new Vector2(0, 0);
        }
        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("Sprites/Button/button_mercy");
            Position = new Vector2(555, 453);
            IsHover = new SpriteAnimation(_texture, 2, 1, 1, 0f, false);
            IsIdle = new SpriteAnimation(_texture, 2, 0, 0, 0f, false);
            CurrentAnimation = IsIdle;
            Hitbox = new OBB(Position, new Vector2(_texture.Width / 2, _texture.Height), 0f);
            Confirm = content.Load<SoundEffect>("Sounds/snd_menu_confirm");
        }
        public void Update(GameTime gameTime)
        {
            CurrentAnimation.Play();
            CurrentAnimation.Update(gameTime);
            if (Menu.slot == 4 && Menu.IsMenu)
            {
                CurrentAnimation = IsHover;
            }
            else CurrentAnimation = IsIdle;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            SpriteAnimation anim = CurrentAnimation;
            spriteBatch.Draw(anim.Texture, Position, anim.GetSourceRect(), Color.White, 0f, new Vector2(_texture.Width / 4, _texture.Height / 2), 1f, SpriteEffects.None, 0f);
            //DebugDraw.DrawOBB(spriteBatch, Hitbox, Color.White * 0.5f);
        }
    }
}
