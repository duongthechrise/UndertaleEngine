using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace UndertaleEngine
{
    public class Act
    {
        SoundEffect Confirm;
        KeyboardState prevkey;
        public void Load(ContentManager content)
        {
            Confirm = content.Load<SoundEffect>("Sounds/snd_menu_confirm");
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
                        Menu.selectingActopts = true;
                        Menu.IsInMenu = false;
                        Confirm.Play(0.65f, 0, 0);
                    }
                }
            }
            if (Menu.IsInMenu == false)
            {
                if (Menu.slot == 2)
                {
                    if (k.IsKeyDown(Keys.X) && !prevkey.IsKeyDown(Keys.X))
                    {
                        Menu.selectingActopts = false;
                        Menu.IsInMenu = true;
                        Menu.ActOptsColumn = 1;
                        Menu.ActOptsRow = 1;
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
        }
        public void Draw(SpriteBatch spritebatch)
        {

        }
    }
}
