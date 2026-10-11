
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1.Effects;
using System;
using System.Collections.Generic;
namespace UndertaleEngine.Game1
{
    public class TypeText
    {
        public string Sentence = "";
        public float Speed = 20f;

        SpriteFont font;
        Vector2 position;

        List<char> chars = new List<char>();
        List<Color> colors = new List<Color>();
        List<float> speeds = new List<float>();
        List<float> waits = new List<float>();
        List<bool> shakes = new List<bool>();

        int shown = 0;
        float timer = 0f;
        float Scale = 2f;
        float waitTimer = 0f;

        public bool Shake = false;
        public float ShakeAmount = 2f;
        public float ShakeSpeed = 20f;
        float shakeTimer = 0f;
        public bool Finished => shown >= chars.Count;
        public TypeText(SpriteFont spriteFont, Vector2 pos)
        {
            font = spriteFont;
            position = pos;
        }

        public void Show(string sentence)
        {
            Sentence = sentence;
            waitTimer = 0f;

            waits.Clear();
            chars.Clear();
            colors.Clear();
            speeds.Clear();
            shakes.Clear();
            shakeTimer = 0f;
            bool currentShake = false;

            shown = 0;
            timer = 0f;

            Color currentColor = Color.White;
            float currentSpeed = Speed;

            for (int i = 0; i < sentence.Length;)
            {
                if (sentence[i] == '~')
                {
                    int end = sentence.IndexOf('~', i + 1);

                    if (end != -1)
                    {
                        string cmd = sentence.Substring(i + 1, end - i - 1);

                        if (cmd.StartsWith("c"))
                        {
                            string name = cmd.Substring(1);

                            switch (name.ToLower())
                            {
                                case "red": currentColor = Color.Red; break;
                                case "blue": currentColor = Color.Blue; break;
                                case "green": currentColor = Color.Green; break;
                                case "yellow": currentColor = Color.Yellow; break;
                                case "white": currentColor = Color.White; break;
                                case "black": currentColor = Color.Black; break;
                                case "orange": currentColor = Color.Orange; break;

                                case "crimson": currentColor = Color.Crimson; break;

                            }

                            i = end + 1;
                            continue;
                        }

                        if (cmd.StartsWith("speed"))
                        {
                            if (float.TryParse(
                                cmd.Substring(5),
                                System.Globalization.NumberStyles.Float,
                                System.Globalization.CultureInfo.InvariantCulture,
                                out float newSpeed))
                            {
                                currentSpeed = Math.Max(1f, newSpeed);
                            }

                            i = end + 1;
                            continue;
                        }
                        if (cmd.StartsWith("wait"))
                        {
                            if (float.TryParse(
                                cmd.Substring(4),
                                System.Globalization.NumberStyles.Float,
                                System.Globalization.CultureInfo.InvariantCulture,
                                out float waitTime))
                            {
                                chars.Add('\0');
                                colors.Add(currentColor);
                                speeds.Add(currentSpeed);
                                waits.Add(Math.Max(0f, waitTime));
                                shakes.Add(currentShake);
                            }

                            i = end + 1;
                            continue;
                        }
                        if (cmd == "shake")
                        {
                            currentShake = true;
                            i = end + 1;
                            continue;
                        }

                        if (cmd == "shakeoff")
                        {
                            currentShake = false;
                            i = end + 1;
                            continue;
                        }
                    }
                }

                chars.Add(sentence[i]);
                colors.Add(currentColor);
                speeds.Add(currentSpeed);
                waits.Add(0f);
                shakes.Add(currentShake);

                i++;
            }
        }


        public void Update(GameTime gameTime)
        {
            shakeTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Finished)
                return;

            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (waitTimer > 0f)
            {
                waitTimer -= delta;
                return;
            }

            timer += delta;

            while (!Finished)
            {
                // Lệnh wait
                if (chars[shown] == '\0')
                {
                    waitTimer = waits[shown];
                    shown++;

                    if (waitTimer > 0f)
                        break;

                    continue;
                }

                float wait = 1f / speeds[shown];

                if (timer < wait)
                    break;

                timer -= wait;
                shown++;
            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            Vector2 drawPos = position;
            float lineHeight = font.LineSpacing * Scale;
            float line = 0f;

            for (int i = 0; i < shown; i++)
            {
                char c = chars[i];

                if(c == '\n')
                {
                    drawPos.X = position.X;
                    drawPos.Y += lineHeight;
                    line += 1;
                    continue;
                }
                if (c == '\r')
                    continue;
                if (c == '\0')
                    continue;

                Vector2 offset = Vector2.Zero;

                if (shakes[i])
                {
                    offset.X = (float)Math.Sin(shakeTimer * ShakeSpeed + i * 12.3f) * ShakeAmount;
                    offset.Y = (float)Math.Cos(shakeTimer * ShakeSpeed + i * 7.1f) * ShakeAmount;
                }
                spritebatch.DrawString(font, c.ToString(), drawPos + offset + Vector2.One, Color.MidnightBlue, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);

                spritebatch.DrawString(font, c.ToString(), drawPos + offset, colors[i], 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);

                drawPos.X += font.MeasureString(c.ToString()).X * Scale;
            }
            for(int a = 0; a <= line; a++)
            {
                spritebatch.DrawString(font, "*", new Vector2(position.X - 34, position.Y + 32 * a), Color.White, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
            }
        }
    }
}