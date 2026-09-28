using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace UndertaleEngine
{
    public class Item
    {
        SoundEffect confirm;
        KeyboardState prevkey;
        public void Load(ContentManager content)
        {
            confirm = content.Load<SoundEffect>("Sounds/snd_menu_confirm");
        }
        public void Update(GameTime gametime)
        {

        }
        public void Draw(SpriteBatch spritebatch)
        {

        }
    }
}
