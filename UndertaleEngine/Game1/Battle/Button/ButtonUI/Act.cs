using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UndertaleEngine.Game1;

namespace UndertaleEngine
{
    public class Act
    {
        SoundEffect Confirm;
        KeyboardState prevkey;
        Option_Act optionact;
        public void Load(ContentManager content)
        {
            Confirm = content.Load<SoundEffect>("Sounds/snd_menu_confirm");
            optionact = new Option_Act();
            optionact.Load(content);
            optionact.AddOption("Talk");
            optionact.AddOption("Flirt");
            optionact.AddOption("Joke");
        }
        public void Update(GameTime gametime)
        {
            var k = Keyboard.GetState();
            
            if (Menu.IsMenu == true)
            {
                if (Menu.slot == 2)
                {
                    if (k.IsKeyDown(Keys.Z) && !prevkey.IsKeyDown(Keys.Z))
                    {
                        if (!Menu.selectingActopts && !Menu.actingonEnemy1 && !Menu.turnfinish)
                        {
                            Menu.selectingActopts = true;
                            Menu.IsInMenu = false;
                            Confirm.Play(0.65f, 0, 0);
                        }
                        else if(Menu.selectingActopts && !Menu.actingonEnemy1)
                        {
                            Confirm.Play(0.65f, 0, 0);
                            Menu.actingonEnemy1 = true;
                        }
                        else if(Menu.selectingActopts && Menu.actingonEnemy1 && !Menu.confirmAction)
                        {
                            Confirm.Play(0.65f, 0, 0);
                            Menu.confirmAction = true;
                            Menu.confirmActionevoke = true;
                        }
                        else if(Menu.confirmAction)
                        {
                            if(optionact.dialogs.dialogtext.Finished)
                            {
                                Menu.next += 1;
                                Menu.confirmActionevoke = true;
                            }
                        }
                    }
                }
            }
            if (Menu.IsInMenu == false)
            {
                if (Menu.slot == 2)
                {
                    if (k.IsKeyDown(Keys.X) && !prevkey.IsKeyDown(Keys.X))
                    {
                        if (Menu.selectingActopts && !Menu.actingonEnemy1)
                        {
                            Menu.selectingActopts = false;
                            Menu.IsInMenu = true;
                            Menu.ActOptsColumn = 1;
                            Menu.ActOptsRow = 1;
                        }
                        else if (Menu.selectingActopts && Menu.actingonEnemy1 && !Menu.confirmAction)
                        {
                            Menu.actingonEnemy1 = false;
                        }
                        else if (Menu.confirmAction && Menu.selectingActopts && Menu.actingonEnemy1)
                        {
                            optionact.dialogs.dialogtext.skip();
                        }
                    }
                }
            }
            prevkey = k;
            if (Menu.ActOptsRow < 1)
                Menu.ActOptsRow = 1;
            if (Menu.ActOptsColumn < 1)
                Menu.ActOptsColumn = 1;
            if (Menu.ActOptsRow > 2)
                Menu.ActOptsRow = 2;
            if (Menu.ActOptsColumn > 2)
                Menu.ActOptsColumn = 2;
            optionact.Update(gametime);
        }
        public void Draw(SpriteBatch spritebatch)
        {
            optionact.Draw(spritebatch);
        }
    }
}
