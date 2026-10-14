using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace UndertaleEngine.Game1
{
    public class Option_Act
    {
        int[] Page = { 1, 2 };
        public List<string> Option = new List<string>();
        SpriteFont determono;
        DisplayText[] star = new DisplayText[4];
        Vector2[] slot = new Vector2[4];
        DisplayText[] enemy = new DisplayText[3];
        public Dialogs dialogs;
        public Option_Act()
        {
            Option.Add("Check");
            slot[0] = new Vector2(134, 270);
            slot[1] = new Vector2(390, 270);
            slot[2] = new Vector2(134, 302);
            slot[3] = new Vector2(390, 302);
        }

        public void Load(ContentManager content)
        {
            determono = content.Load<SpriteFont>("Fonts/Determination_Mono");
            enemy[0] = new DisplayText(determono, $"{Menu.Enemy1Name}", new Vector2(134, 270), Color.White, 2f, true);
            enemy[1] = new DisplayText(determono, $"{Menu.Enemy1Name}", new Vector2(134, 302), Color.White, 2f, true);
            enemy[2] = new DisplayText(determono, $"{Menu.Enemy1Name}", new Vector2(134, 334), Color.White, 2f, true);
            star[0] = new DisplayText(determono, "*", new Vector2(100, 270), Color.White, 2f, true);
            star[1] = new DisplayText(determono, "*", new Vector2(356, 270), Color.White, 2f, true);
            star[2] = new DisplayText(determono, "*", new Vector2(100, 302), Color.White, 2f, true);
            star[3] = new DisplayText(determono, "*", new Vector2(356, 302), Color.White, 2f, true);
            dialogs = new Dialogs(this);
            dialogs.Load(content);
        }
        public void AddOption(string text)
        {
            Option.Add($"{text}");
        }
        public void RemoveOption(string text)
        {

        }
        public void Update(GameTime gametime)
        {
            Menu.ActSlots = Option.Count;
            dialogs.Update(gametime);
        }
        public void Draw(SpriteBatch spritebatch)
        {
            if (Menu.selectingActopts && !Menu.actingonEnemy1)
            {
                enemy[0].Draw(spritebatch);
                star[0].Draw(spritebatch);
            }
            if (Menu.selectingActopts && Menu.actingonEnemy1 && !Menu.confirmAction)
            {
                for (int i = 0; i < Option.Count; i++)
                {
                    string c = Option[i];
                    spritebatch.DrawString(determono, c, slot[i] + Vector2.One, Color.MidnightBlue, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
                    spritebatch.DrawString(determono, c, slot[i], Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
                    star[i].Draw(spritebatch);
                }
            }
            dialogs.Draw(spritebatch);
        }
    }
}
