using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace UndertaleEngine.Game1
{
    public class Button_Act
    {
        private Texture2D _texture;
        private Vector2 Position;
        public SpriteAnimation IsIdle;
        public SpriteAnimation IsHover;
        public SpriteAnimation CurrentAnimation;
        public OBB Hitbox;
        Act act;
        public Button_Act()
        {
            Position = new Vector2(0, 0);
        }
        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("Sprites/Button/button_act");
            Position = new Vector2(240, 453);
            IsIdle = new SpriteAnimation(_texture, 2, 0, 0, 0f, loop: false);
            IsHover = new SpriteAnimation(_texture, 2, 1, 1, 0f, loop: false);
            CurrentAnimation = IsIdle;
            Hitbox = new OBB(Position, new Vector2(_texture.Width / 2, _texture.Height), 0f);
            act = new Act();
            act.Load(content);
        }
        public void Update(GameTime gameTime)
        {
            CurrentAnimation.Play();
            CurrentAnimation.Update(gameTime);
            act.Update(gameTime);
            if (Menu.slot == 2 && Menu.IsMenu)
            {
                CurrentAnimation = IsHover;
            }
            else CurrentAnimation = IsIdle;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            SpriteAnimation anim;
            anim = CurrentAnimation;
            if (_texture != null)
            {
                spriteBatch.Draw(anim.Texture, Position, anim.GetSourceRect(), Color.White, 0f, new Vector2(_texture.Width / 4, _texture.Height / 2), 1f, SpriteEffects.None, 0f);
                //DebugDraw.DrawOBB(spriteBatch, Hitbox, Color.White * 0.5f);
            }
        }
    }
}
