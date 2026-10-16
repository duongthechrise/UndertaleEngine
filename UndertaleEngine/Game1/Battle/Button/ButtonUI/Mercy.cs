using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace UndertaleEngine
{
    public class Mercy
    {
        SoundEffect confirm;
        KeyboardState prevkey;
        public void Load(ContentManager content)
        {
            confirm = content.Load<SoundEffect>("Sounds/snd_menu_confirm");
        }
        public void Update(GameTime gametime)
        {
            var k = Keyboard.GetState();
            if (Menu.IsMenu)
            {
                if (Menu.slot == 4)
                {
                    if (k.IsKeyDown(Keys.Z) && !prevkey.IsKeyDown(Keys.Z) ||
                        k.IsKeyDown(Keys.Enter) && !prevkey.IsKeyDown(Keys.Enter))
                    {
                        if (!Menu.choosingMercyOption)
                        {
                            Menu.IsInMenu = false;
                            confirm.Play(0.65f, 0, 0);
                            Menu.choosingMercyOption = true;
                        }
                        else if (!Menu.mercyingonEnemy1)
                        {
                            confirm.Play(0.65f, 0, 0);
                        }
                    }
                }
            }
        }
        public void Draw(SpriteBatch spritebatch)
        {

        }
    }
}

