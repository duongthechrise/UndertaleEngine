using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace UndertaleEngine.Game1
{
    public class Anim_Fight
    {
        Texture2D fightbg;
        Vector2 BGPosition;
        Vector2 AimPosition;
        public Vector2 knifeposition;
        float bgAnimationT = 0f;
        float TotalBGAnimationT = 0f;
        float TotalAimingT = 0f;
        float TotalSlashingT = 0f;
        float IndicatorAnimationT_Stage1 = 0f;
        float IndicatorAnimationT_Wait = 0f;
        float IndicatorAnimationT_Stage2 = 0f;
        float BGAlpha = 1f;
        float IndicatorAlphaT_Stage1 = 0f;
        float IndicatorAlphaT_Stage2 = 0f;

        bool closingBackground = false;
        public bool startaiming = false;

        //dealing
        Vector2 BGscale = new Vector2(1f, 1f);
        float IndicatorAlpha = 0f;
        SpriteAnimation aiming;
        SpriteAnimation dealing;
        SpriteAnimation CurrentAnimation;
        Texture2D fightaim;
        Texture2D fightknife;
        Texture2D miss;
        Vector2 missposition;
        SpriteAnimation knifeslice;
        String Damage;
        Texture2D DamageNumber;
        SpriteAnimation[] DamageFont = new SpriteAnimation[10];
        Vector2 DamagePosition;
        Vector2 ShiftedPosition;
        int Count;
        float percentage;
        SoundEffect damage;
        float DamageSoundT = 0f;
        HP_Bar hpbar;
        Texture2D enemy;
        PixelShake enemyshake;
        
        public Anim_Fight(Vector2 BGPosition, Vector2 AimPosition)
        {
            this.BGPosition = BGPosition;
            this.AimPosition = AimPosition;
            knifeposition = new Vector2(320, 120);
            missposition = new Vector2(320, 60);
            DamagePosition = new Vector2(305, 160);
        }
        public void Load(ContentManager content, GraphicsDevice graphics)
        {
            miss = content.Load<Texture2D>("Sprites/Fight/fight_miss");
            fightbg = content.Load<Texture2D>("Sprites/Fight/fight_bg");
            fightaim = content.Load<Texture2D>("Sprites/Fight/fight_aim");
            fightknife = content.Load<Texture2D>("Sprites/Fight/fight_knife");
            knifeslice = new SpriteAnimation(fightknife, 6, 0, 5, 0.125f, loop: true);
            aiming = new SpriteAnimation(fightaim, 2, 0, 0, 0f, loop: false);
            dealing = new SpriteAnimation(fightaim, 2, 0, 1, 0.125f, loop: true);
            CurrentAnimation = aiming;
            DamageNumber = content.Load<Texture2D>("Sprites/Fight/fight_damage");
            damage = content.Load<SoundEffect>("Sounds/snd_damage");
            DamageFont[1] = new SpriteAnimation(DamageNumber, 10, 0, 0, 0f, loop: false);
            DamageFont[2] = new SpriteAnimation(DamageNumber, 10, 1, 1, 0f, loop: false);
            DamageFont[3] = new SpriteAnimation(DamageNumber, 10, 2, 2, 0f, loop: false);
            DamageFont[4] = new SpriteAnimation(DamageNumber, 10, 3, 3, 0f, loop: false);
            DamageFont[5] = new SpriteAnimation(DamageNumber, 10, 4, 4, 0f, loop: false);
            DamageFont[6] = new SpriteAnimation(DamageNumber, 10, 5, 5, 0f, loop: false);
            DamageFont[7] = new SpriteAnimation(DamageNumber, 10, 6, 6, 0f, loop: false);
            DamageFont[8] = new SpriteAnimation(DamageNumber, 10, 7, 7, 0f, loop: false);
            DamageFont[9] = new SpriteAnimation(DamageNumber, 10, 8, 8, 0f, loop: false);
            DamageFont[0] = new SpriteAnimation(DamageNumber, 10, 9, 9, 0f, loop: false);
            hpbar = new HP_Bar(graphics, new Vector2(240, 30), 163, 13, 1050, Color.Lime, CustomColor.DarkGray, animation: true, outline: true);
            hpbar.LoadContent(content);
            enemy = content.Load<Texture2D>("Sprites/Enemy/enemy");
            enemyshake = new PixelShake();

        }
        public void Update(GameTime gametime)
        {
            foreach (var Damage in DamageFont)
            {
                Damage.Play();
                Damage.Update(gametime);
            }
            Damage = Menu.Damage.ToString();
            Count = Damage.Length;

            var k = Keyboard.GetState();
            CurrentAnimation.Play();
            CurrentAnimation.Update(gametime); ;
            if (Menu.targetingEnemy1)
            {//performingattack
                if (Menu.attacking == false)
                    CurrentAnimation = aiming;
                if (Menu.attacking)
                    CurrentAnimation = dealing;
                if (startaiming)
                {
                    var dt2 = (float)gametime.ElapsedGameTime.TotalSeconds;
                    if (Menu.attacking == false && Menu.slice == false)
                        TotalBGAnimationT += dt2 / 1.5f;
                    if (Menu.attacking && Menu.slice)
                        percentage = MathF.Pow((2 * (0.5f - Math.Abs(TotalBGAnimationT - 0.5f))), 1.5f);
                    Menu.Damage = Math.Round(percentage * (30 - Menu.EnemyDef / Menu.PlayerATK)) + Math.Round(percentage * (Math.Pow(Menu.PlayerATK, 3) / (Menu.PlayerATK + Math.Pow(Menu.EnemyDef, 3))), MidpointRounding.AwayFromZero);

                    //attacking, begin animation
                    if (Menu.attacking)
                    {
                        TotalAimingT += dt2 / 1.5f;
                        if (TotalSlashingT <= 1f)
                        {
                            TotalSlashingT += dt2 / 0.75f;
                            DamageSoundT += dt2 / 0.75f;
                        }
                        knifeslice.Play();
                        knifeslice.Update(gametime);
                    }
                    //if not attack, next turn
                    if (TotalAimingT >= 1)
                    {
                        TotalAimingT = 0f;
                        TotalBGAnimationT = 0f;
                        startaiming = false;
                        closingBackground = true;
                        Menu.targetingEnemy1 = false;
                        Menu.turnfinish = true;
                        Menu.attacking = false;
                    }
                    //finishing slashing animation
                    if (TotalSlashingT >= 1)
                    {
                        Menu.slice = false;
                        if (Menu.dodge)
                            Menu.Miss = true;
                        else Menu.Hit = true;
                        knifeslice.Pause();
                        knifeslice.Reset();
                    }
                    //begin miss animation, stage 1: move up and change alpha
                    if (Menu.Miss && IndicatorAnimationT_Wait == 0f)
                    {
                        IndicatorAnimationT_Stage1 += dt2 / 0.25f;
                        IndicatorAlphaT_Stage1 += dt2 / 0.25f;
                        
                        float Ease = IndicatorAnimationT_Stage1 * (2f - IndicatorAnimationT_Stage1);
                        if (IndicatorAnimationT_Stage1 <= 1f)
                        {
                            missposition = Vector2.Lerp(new Vector2(320, 160), new Vector2(320, 120), Ease);
                            IndicatorAlpha = MathHelper.Lerp(0f, 1f, IndicatorAlphaT_Stage1);
                        }
                    }
                    else if (Menu.Hit && IndicatorAnimationT_Wait == 0f)
                    {
                        IndicatorAnimationT_Stage1 += dt2 / 0.25f;
                        IndicatorAlphaT_Stage1 += dt2 / 0.25f;
                        float Ease = IndicatorAnimationT_Stage1 * (2f - IndicatorAnimationT_Stage1);
                        if (IndicatorAnimationT_Stage1 <= 1f)
                        {
                            DamagePosition = Vector2.Lerp(new Vector2(DamagePosition.X, 160), new Vector2(DamagePosition.X, 120), Ease);
                            IndicatorAlpha = MathHelper.Lerp(0f, 1f, IndicatorAlphaT_Stage1);
                        }
                    }
                    //miss animation delay, wait for a moment before stage 2
                    if (IndicatorAnimationT_Stage1 >= 1f && IndicatorAnimationT_Stage2 == 0f)
                    {
                        IndicatorAnimationT_Wait += dt2 / 0.25f;
                    }
                    //delay finished, begin stage 2: move up and change alpha again
                    if (IndicatorAnimationT_Wait >= 1f && IndicatorAnimationT_Stage2 <= 1f && Menu.Miss)
                    {
                        IndicatorAnimationT_Stage2 += dt2 / 0.25f;
                        IndicatorAlphaT_Stage2 += dt2 / 0.25f;
                        missposition = Vector2.Lerp(new Vector2(320, 120), new Vector2(320, 60), IndicatorAnimationT_Stage2 * IndicatorAnimationT_Stage2);
                        IndicatorAlpha = MathHelper.Lerp(1f, 0f, IndicatorAlphaT_Stage2);
                    }
                    else if (IndicatorAnimationT_Wait >= 1f && IndicatorAnimationT_Stage2 <= 1f && Menu.Hit)
                    {
                        IndicatorAnimationT_Stage2 += dt2 / 0.25f;
                        IndicatorAlphaT_Stage2 += dt2 / 0.25f;
                        DamagePosition = Vector2.Lerp(new Vector2(DamagePosition.X, 120), new Vector2(DamagePosition.X, 60), IndicatorAnimationT_Stage2 * IndicatorAnimationT_Stage2);
                        IndicatorAlpha = MathHelper.Lerp(1f, 0f, IndicatorAlphaT_Stage2);
                    }
                    //finishing miss animation, reset variables
                    if (IndicatorAnimationT_Stage2 >= 1f)
                    {
                        Menu.Miss = false;
                        Menu.Hit = false;
                        IndicatorAnimationT_Stage1 = 0f;
                        IndicatorAnimationT_Stage2 = 0f;
                        IndicatorAnimationT_Wait = 0f;
                        IndicatorAlphaT_Stage1 = 0f;
                        IndicatorAlphaT_Stage2 = 0f;
                        TotalSlashingT = 0f;
                        IndicatorAlpha = 1f;
                    }

                    if (Menu.direction == 1)
                        AimPosition = Vector2.Lerp(new Vector2(23, 256), new Vector2(603, 256), TotalBGAnimationT);
                    if (Menu.direction == 2)
                        AimPosition = Vector2.Lerp(new Vector2(603, 256), new Vector2(20, 256), TotalBGAnimationT);
                    //finishing slashing animation, closing background and next turn
                    if (TotalBGAnimationT >= 1f)
                    {
                        TotalBGAnimationT = 0f;
                        startaiming = false;
                        closingBackground = true;
                        Menu.targetingEnemy1 = false;
                        Menu.turnfinish = true;
                    }
                }
            }
            if (closingBackground)
            {
                var dt = (float)gametime.ElapsedGameTime.TotalSeconds;
                bgAnimationT += dt / 0.25f;
                BGscale = Vector2.Lerp(new Vector2(1f, 1f), new Vector2(0.5f, 1f), bgAnimationT);
                BGAlpha = MathHelper.Lerp(1f, 0f, bgAnimationT);
                if (bgAnimationT >= 1f)
                {
                    bgAnimationT = 0f;
                    Menu.performingattack = false;
                    closingBackground = false;
                }
            }
            if (closingBackground == false)
            {
                BGscale = new Vector2(1f, 1f);
                BGAlpha = 1f;
            }
            if (DamageSoundT >= 1f)
            {
                Menu.DealingDamage = true;
                DamageSoundT = 0f;
            }
            if(Menu.DealingDamage)
            {
                damage.Play(0.65f, 0, 0);
                hpbar.Damage((float)Menu.Damage);
                enemyshake.Shake(1, 0.25f);
                Menu.DealingDamage = false;
            }
            hpbar.Update(gametime);
            enemyshake.Update(gametime);
        }
        public void Draw(SpriteBatch spritebatch)
        {
            if (Menu.performingattack)
            {
                spritebatch.Draw(fightbg, BGPosition, null, Color.White * BGAlpha, 0f, new Vector2(fightbg.Width / 2, fightbg.Height / 2), BGscale, SpriteEffects.None, 0f);
            }
        }
        public void DrawIn(SpriteBatch spritebatch)
        {
            if (Menu.targetingEnemy1)
                spritebatch.Draw(CurrentAnimation.Texture, AimPosition, CurrentAnimation.GetSourceRect(), Color.White);
        }
        public void DrawOut(SpriteBatch spritebatch)
        {
            spritebatch.Draw(enemy, new Vector2(320, 170) + enemyshake.Offset, null, Color.White, 0f, new Vector2(enemy.Width / 2, enemy.Height / 2), 2f, SpriteEffects.None, 0f);
            if (Menu.slice)
                spritebatch.Draw(knifeslice.Texture, knifeposition, knifeslice.GetSourceRect(), Color.White, 0f, new Vector2(knifeslice.Texture.Width / 12, knifeslice.Texture.Height / 12), 1f, SpriteEffects.None, 0f);
            if (IndicatorAnimationT_Stage1 > 0f && Menu.Miss)
            {
                spritebatch.Draw(miss, missposition, null, Color.White * IndicatorAlpha, 0f, new Vector2(miss.Width / 2, miss.Height / 2), 1f, SpriteEffects.None, 0f);
            }
            //spritebatch.Draw(test.Texture, testposition, test.GetSourceRect(), Color.White);
            if (IndicatorAnimationT_Stage1 > 0f && Menu.Hit)
            {
                for (int i = 0; i < Count; i++)
                {
                    char c = Damage[i];
                    DamagePosition = new Vector2(305 - 16 * (Count - 1), DamagePosition.Y);
                    ShiftedPosition = new Vector2(DamagePosition.X + i * 32, DamagePosition.Y);
                    hpbar.alpha = IndicatorAlpha;
                    hpbar.Draw(spritebatch);
                    switch (c)
                    {
                        case '0':
                            spritebatch.Draw(DamageFont[0].Texture, ShiftedPosition, DamageFont[0].GetSourceRect(), Color.White * IndicatorAlpha);
                            break;
                        case '1':
                            spritebatch.Draw(DamageFont[1].Texture, ShiftedPosition, DamageFont[1].GetSourceRect(), Color.White * IndicatorAlpha);
                            break;
                        case '2':
                            spritebatch.Draw(DamageFont[2].Texture, ShiftedPosition, DamageFont[2].GetSourceRect(), Color.White * IndicatorAlpha);
                            break;
                        case '3':
                            spritebatch.Draw(DamageFont[3].Texture, ShiftedPosition, DamageFont[3].GetSourceRect(), Color.White * IndicatorAlpha);
                            break;
                        case '4':
                            spritebatch.Draw(DamageFont[4].Texture, ShiftedPosition, DamageFont[4].GetSourceRect(), Color.White * IndicatorAlpha);
                            break;
                        case '5':
                            spritebatch.Draw(DamageFont[5].Texture, ShiftedPosition, DamageFont[5].GetSourceRect(), Color.White * IndicatorAlpha);
                            break;
                        case '6':
                            spritebatch.Draw(DamageFont[6].Texture, ShiftedPosition, DamageFont[6].GetSourceRect(), Color.White * IndicatorAlpha);
                            break;
                        case '7':
                            spritebatch.Draw(DamageFont[7].Texture, ShiftedPosition, DamageFont[7].GetSourceRect(), Color.White * IndicatorAlpha);
                            break;
                        case '8':
                            spritebatch.Draw(DamageFont[8].Texture, ShiftedPosition, DamageFont[8].GetSourceRect(), Color.White * IndicatorAlpha);
                            break;
                        case '9':
                            spritebatch.Draw(DamageFont[9].Texture, ShiftedPosition, DamageFont[9].GetSourceRect(), Color.White * IndicatorAlpha);
                            break;
                    }
                }
            }
        }
    }
}
