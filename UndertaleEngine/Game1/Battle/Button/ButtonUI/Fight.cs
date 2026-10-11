using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace UndertaleEngine.Game1
{
    public class Fight
    {
        SoundEffect Confirm;
        KeyboardState prevkey;
        SpriteFont determono;
        DisplayText[] target = new DisplayText[3];
        DisplayText[] star = new DisplayText[3];
        HP_Bar hpbar;
        public Anim_Fight Anim;
        Random random = new Random();
        SoundEffect slice;
        DisplayText test;
        public void Load(ContentManager content, GraphicsDevice graphics)
        {
            Confirm = content.Load<SoundEffect>("Sounds/snd_menu_confirm");
            slice = content.Load<SoundEffect>("Sounds/snd_slice");
            determono = content.Load<SpriteFont>("Fonts/Determination_Mono");
            target[0] = new DisplayText(determono, $"{Menu.Enemy1Name}", new Vector2(134, 270), Color.White, 2, true);
            target[1] = new DisplayText(determono, $"{Menu.Enemy2Name}", new Vector2(134, 302), Color.White, 2, true);
            target[2] = new DisplayText(determono, $"{Menu.Enemy3Name}", new Vector2(134, 334), Color.White, 2, true);
            star[0] = new DisplayText(determono, "*", new Vector2(100, 270), Color.White, 2, true);
            star[1] = new DisplayText(determono, "*", new Vector2(100, 302), Color.White, 2, true);
            star[2] = new DisplayText(determono, "*", new Vector2(100, 334), Color.White, 2, true);
            hpbar = new HP_Bar(graphics, new Vector2(212, 280), 101, 17, Menu.Enemy1HP, Color.Lime, CustomColor.SolidHPRed);
            Anim = new Anim_Fight(new Vector2(320, 320), new Vector2(23,256));
            Anim.Load(content, graphics);
            //(23, 256)
            //(603, 256)
        }
        public void Update(GameTime gametime)
        {
            var k = Keyboard.GetState();
            if (Menu.IsMenu == true)
            {
                if (Menu.slot == 1)
                {
                    if (k.IsKeyDown(Keys.Z) && !prevkey.IsKeyDown(Keys.Z) ||
                        k.IsKeyDown(Keys.Enter) && !prevkey.IsKeyDown(Keys.Enter))
                    {
                        //choosing enemy
                        if (Menu.selectingFightopts == false && Menu.targetingEnemy1 != true)
                        {
                            Menu.selectingFightopts = true;
                            Menu.IsInMenu = false;
                            Confirm.Play(0.65f, 0, 0);
                        }
                        //aiming minigame
                        else if (Menu.selectingFightopts && Menu.targetingEnemy1 == false)
                        {
                            Confirm.Play(0.65f, 0, 0);
                            Menu.direction = random.Next(1, 3);
                            Menu.targetingEnemy1 = true;
                            Menu.performingattack = true;
                            Menu.selectingFightopts = false;
                            Anim.startaiming = true;  
                        }
                        //confirming attack
                        else if(Menu.targetingEnemy1 && Menu.attacking != true)
                        {
                            Menu.attacking = true;
                            Menu.slice = true;
                            slice.Play(0.65f, 0, 0);
                        }
                    }
                }
            }
            if (Menu.IsInMenu == false && Menu.IsMenu)
            {
                if (Menu.slot == 1)
                {
                    if (Menu.targetingEnemy1 != true)
                    {
                        if (k.IsKeyDown(Keys.X) && !prevkey.IsKeyDown(Keys.X) ||
                            k.IsKeyDown(Keys.LeftShift) && !prevkey.IsKeyDown(Keys.LeftShift) ||
                            k.IsKeyDown(Keys.RightShift) && !prevkey.IsKeyDown(Keys.RightShift))
                        {
                            Menu.selectingFightopts = false;
                            Menu.IsInMenu = true;
                            Menu.FightOptsColumn = 1;
                        }
                    }
                }
            }
            prevkey = k;
            if (Menu.FightOptsColumn < 1)
                Menu.FightOptsColumn = 1;
            if (Menu.FightOptsColumn > 3)
                Menu.FightOptsColumn = 3;
            Anim.Update(gametime);
        }
        public void Draw(SpriteBatch spritebatch)
        {
            if(Menu.selectingFightopts)
            {
                target[0].Draw(spritebatch);
                hpbar.Draw(spritebatch);
                star[0].Draw(spritebatch);
                if (Menu.FightSlots >= 2)
                {
                    target[1].Draw(spritebatch);
                    star[1].Draw(spritebatch);
                }
                if (Menu.FightSlots == 3)
                {
                    target[2].Draw(spritebatch);
                    star[2].Draw(spritebatch);
                }
            }
        }
    }
}
