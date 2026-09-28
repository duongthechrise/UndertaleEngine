using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace UndertaleEngine.Game1
{
    public class Anim_Fight
    {
        Texture2D fightbg;
        Vector2 Position;
        DisplayText test;
        float duration = 0.25f;
        float duration2 = 0.015625f;
        float t = 0f;
        float t2 = 1f;
        bool isplaying = false;
        bool isplaying2 = false;

        KeyboardState prevkey;
        Vector2 scale = new Vector2(1f, 1f);
        float alpha = 1f;
        public Anim_Fight(Vector2 position)
        {
            Position = position;
        }
        public void Load(ContentManager content, GraphicsDevice graphics)
        {
            fightbg = content.Load<Texture2D>("Sprites/Fight/fight_bg");
        }
        public void Update(GameTime gametime)
        {
            if (Menu.targetingEnemy1)
            {
                var k = Keyboard.GetState();
                if (k.IsKeyDown(Keys.Q) && !prevkey.IsKeyDown(Keys.Q))
                {
                    t = 0f;
                    isplaying = true;
                    t2 = 1f;
                    isplaying2 = true;
                }
                if (isplaying)
                {
                    var dt = (float)gametime.ElapsedGameTime.TotalSeconds;
                    t += dt / duration;
                    t2 -= dt / duration2;
                    scale = Vector2.Lerp(new Vector2(1f, 1f), new Vector2(0.5f, 1f), t);
                    alpha = MathHelper.Lerp(1f, 0f, t);
                    if (t >= 1f)
                    {
                        isplaying = false;

                    }
                    if (t <= 0f)
                        isplaying = false;
                }
                if (k.IsKeyDown(Keys.Space) && !prevkey.IsKeyDown(Keys.Space) && isplaying == false)
                {
                    scale = new Vector2(1f, 1f);
                    alpha = 1f;
                }
                prevkey = k;
            }
        }
        public void Draw(SpriteBatch spritebatch)
        {
            if(Menu.targetingEnemy1 == true)
            spritebatch.Draw(fightbg, Position, null, Color.White * alpha, 0f, new Vector2(fightbg.Width / 2, fightbg.Height / 2), scale, SpriteEffects.None, 0f);
        }
    }
}
