using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
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
        float MissAnimationT_Stage1 = 0f;
        float MissAnimationT_Wait = 0f;
        float MissAnimationT_Stage2 = 0f;
        float BGAlpha = 1f;
        float MissAlphaT_Stage1 = 0f;
        float MissAlphaT_Stage2 = 0f;

        bool closingBackground = false;
        public bool startaiming = false;

        //dealing
        KeyboardState prevkey;
        Vector2 BGscale = new Vector2(1f, 1f);
        float MissAlpha = 0f;
        SpriteAnimation aiming;
        SpriteAnimation dealing;
        SpriteAnimation CurrentAnimation;
        Texture2D fightaim;
        Texture2D fightknife;
        Texture2D miss;
        Vector2 missposition;
        SpriteAnimation knifeslice;
        String Damage = Menu.Damage.ToString();
        Texture2D DamageNumber;
        SpriteAnimation[] DamageFont = new SpriteAnimation[10];
        Vector2 DamagePosition;
        Vector2 ShiftedPosition;
        int Count;
        float percentage;
        public Anim_Fight(Vector2 BGPosition, Vector2 AimPosition)
        {
            this.BGPosition = BGPosition;
            this.AimPosition = AimPosition;
            knifeposition = new Vector2(300, 70);
            missposition = new Vector2(320, 60);
            DamagePosition = new Vector2(305, 160);
            //(320,230)
            //(320,160)
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
        }
        public void Update(GameTime gametime)
        {
            foreach (var Damage in DamageFont)
            {
                Damage.Play();
                Damage.Update(gametime);
            }
            Count = Damage.Length;

            var k = Keyboard.GetState();
            if (k.IsKeyDown(Keys.Q) && !prevkey.IsKeyDown(Keys.Q))
            {
                Debug.WriteLine($"bgAnimationT: {bgAnimationT} " +
                    $"TotalBGAnimationT:{TotalBGAnimationT} " +
                    $"TotalAimingT:{TotalAimingT} " +
                    $"TotalSlashingT:{TotalSlashingT} " +
                    $"MissAnimationT_Stage1:{MissAnimationT_Stage1} " +
                    $"MissAnimationT_Wait:{MissAnimationT_Wait} " +
                    $"MissAnimationT_Stage2:{MissAnimationT_Stage2} " +
                    $"MissAlphaT_Stage1:{MissAlphaT_Stage1} " +
                    $"MissAlphaT_Stage2:{MissAlphaT_Stage2} " +
                    $"BGAlpha:{BGAlpha} " +
                    $"closingBackground:{closingBackground} " +
                    $"startaiming:{startaiming} " +
                    $"MissAlpha:{MissAlpha} " +
                    $"targetingEnemy1:{Menu.targetingEnemy1} " +
                    $"attacking:{Menu.attacking} " +
                    $"performingattack:{Menu.performingattack} ");
            }
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
                        percentage = 2 * (0.5f - Math.Abs(TotalBGAnimationT - 0.5f));
                    Menu.Damage = percentage * (Math.Pow(Menu.PlayerATK, 2) / (Menu.PlayerATK + Menu.EnemyDef));

                    //attacking, begin animation
                    if (Menu.attacking)
                    {
                        TotalAimingT += dt2 / 1.5f;
                        if (TotalSlashingT <= 1f)
                            TotalSlashingT += dt2 / 0.75f;
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
                        knifeslice.Pause();
                        knifeslice.Reset();
                    }
                    //begin miss animation, stage 1: move up and change alpha
                    if (Menu.Miss && MissAnimationT_Wait == 0f)
                    {
                        MissAnimationT_Stage1 += dt2 / 0.25f;
                        MissAlphaT_Stage1 += dt2 / 0.25f;
                        float Ease = MissAnimationT_Stage1 * (2f - MissAnimationT_Stage1);
                        if (MissAnimationT_Stage1 <= 1f)
                        {
                            missposition = Vector2.Lerp(new Vector2(320, 160), new Vector2(320, 120), Ease);
                            MissAlpha = MathHelper.Lerp(0f, 1f, MissAlphaT_Stage1);
                        }
                    }
                    //miss animation delay, wait for a moment before stage 2
                    if (MissAnimationT_Stage1 >= 1f && MissAnimationT_Stage2 == 0f)
                    {
                        MissAnimationT_Wait += dt2 / 0.25f;
                    }
                    //delay finished, begin stage 2: move up and change alpha again
                    if (MissAnimationT_Wait >= 1f && MissAnimationT_Stage2 <= 1f)
                    {
                        MissAnimationT_Stage2 += dt2 / 0.25f;
                        MissAlphaT_Stage2 += dt2 / 0.25f;
                        missposition = Vector2.Lerp(new Vector2(320, 120), new Vector2(320, 60), MissAnimationT_Stage2 * MissAnimationT_Stage2);
                        MissAlpha = MathHelper.Lerp(1f, 0f, MissAlphaT_Stage2);
                    }
                    //finishing miss animation, reset variables
                    if (MissAnimationT_Stage2 >= 1f)
                    {
                        Menu.Miss = false;
                        MissAnimationT_Stage1 = 0f;
                        MissAnimationT_Stage2 = 0f;
                        MissAnimationT_Wait = 0f;
                        MissAlphaT_Stage1 = 0f;
                        MissAlphaT_Stage2 = 0f;
                        TotalSlashingT = 0f;
                        MissAlpha = 1f;
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
            prevkey = k;
            Debug.WriteLine(percentage);
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
            if (Menu.slice)
                spritebatch.Draw(knifeslice.Texture, knifeposition, knifeslice.GetSourceRect(), Color.White);
            if (MissAnimationT_Stage1 > 0f)
                spritebatch.Draw(miss, missposition, null, Color.White * MissAlpha, 0f, new Vector2(miss.Width / 2, miss.Height / 2), 1f, SpriteEffects.None, 0f);
            //spritebatch.Draw(test.Texture, testposition, test.GetSourceRect(), Color.White);
            for (int i = 0; i < Count; i++)
            {
                char c = Damage[i];
                DamagePosition = new Vector2(305 - 16 * (Count - 1), 160);
                ShiftedPosition = new Vector2(DamagePosition.X + i * 32, DamagePosition.Y);
                switch (c)
                {
                    case '0':
                        spritebatch.Draw(DamageFont[0].Texture, ShiftedPosition, DamageFont[0].GetSourceRect(), Color.White);
                        break;
                    case '1':
                        spritebatch.Draw(DamageFont[1].Texture, ShiftedPosition, DamageFont[1].GetSourceRect(), Color.White);
                        break;
                    case '2':
                        spritebatch.Draw(DamageFont[2].Texture, ShiftedPosition, DamageFont[2].GetSourceRect(), Color.White);
                        break;
                }

            }
            spritebatch.Draw(DamageFont[1].Texture, new Vector2(305, 120), DamageFont[1].GetSourceRect(), Color.White);
        }
    }
}
