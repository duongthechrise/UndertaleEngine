using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace UndertaleEngine.Game1
{
    public class Button_Item
    {
        private Texture2D _texture;
        private Vector2 Position;
        public SpriteAnimation IsIdle;
        public SpriteAnimation IsHover;
        public SpriteAnimation IsEmpty;
        public SpriteAnimation CurrentAnimation;

        SoundEffect Confirm;

        KeyboardState prevkey;
        public bool Empty = false;
        public OBB Hitbox;
        public Button_Item()
        {
            Position = new Vector2(0, 0);
        }
        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("Sprites/Button/button_item");
            Position = new Vector2(400, 453);
            IsIdle = new SpriteAnimation(_texture, 3, 0, 0, 0f, false);
            IsHover = new SpriteAnimation(_texture, 3, 1, 1, 0f, false);
            IsEmpty = new SpriteAnimation(_texture, 3, 2, 2, 0f, false);
            CurrentAnimation = IsIdle;
            Hitbox = new OBB(Position, new Vector2(_texture.Width / 3, _texture.Height), 0f);
            Confirm = content.Load<SoundEffect>("Sounds/snd_menu_confirm");
        }
        public void Update(GameTime gameTime)
        {
            CurrentAnimation.Play();
            CurrentAnimation.Update(gameTime);
            if (Menu.slot == 3 && Menu.IsMenu)
            {
                CurrentAnimation = IsHover;
            }
            else CurrentAnimation = IsIdle;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            SpriteAnimation anim = CurrentAnimation;
            spriteBatch.Draw(anim.Texture, Position, anim.GetSourceRect(), Color.White, 0f, new Vector2(_texture.Width / 6, _texture.Height / 2), 1f, SpriteEffects.None, 0f);
            //DebugDraw.DrawOBB(spriteBatch, Hitbox, Color.White * 0.5f);
        }
    }
}
