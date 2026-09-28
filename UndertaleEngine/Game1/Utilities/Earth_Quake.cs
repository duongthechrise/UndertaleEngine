using Microsoft.Xna.Framework;
using System;

namespace UndertaleEngine.Game1
{
    public class PixelShake
    {
        private float timer;
        private float duration;
        private int strength;   // pixel intensity

        private Random rand = new Random();
        public Vector2 Offset { get; private set; }

        public void Shake(int strength, float duration)
        {
            this.strength = strength;
            this.duration = duration;
            timer = duration;
        }

        public void Update(GameTime gameTime)
        {
            if (timer <= 0)
            {
                Offset = Vector2.Zero;
                return;
            }

            timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            // rung theo bước pixel, không mượt, đổi hướng liên tục
            int x = rand.Next(-strength, strength + 1);
            int y = rand.Next(-strength, strength + 1);

            Offset = new Vector2(x, y);
        }
    }

}
