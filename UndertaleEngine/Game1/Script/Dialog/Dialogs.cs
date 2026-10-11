using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
namespace UndertaleEngine.Game1
{
    public class Dialogs
    {
        SpriteFont determono;
        Option_Act act;
        List<string> dialog = new List<string>();
        TypeText[] dialogtext = new TypeText[4];
        
        public Dialogs(Option_Act act)
        {
            this.act = act;
        }
        public void Load(ContentManager content)
        {
            determono = content.Load<SpriteFont>("Fonts/Determination_Mono");
            for (int i = 0; i < act.Option.Count; i++)
            {
                dialogtext[i] = new TypeText(determono, new Vector2(94, 270));
            }
        }
        public void Update(GameTime gametime)
        {
            if(Menu.confirmActionevoke && Menu.confirmAction)
            {
                switch(act.Option[Menu.PlayerActSlot - 1])
                {
                    case "Check":
                        dialogtext[Menu.PlayerActSlot - 1].Show($"{Menu.Enemy1Name} - ATK 8 DEF 4\nHas been following you around.\nMust want to be friends!");
                        break;

                }
                Menu.confirmActionevoke = false;
            }
            dialogtext[Menu.PlayerActSlot - 1].Update(gametime);
        }
        public void Draw(SpriteBatch spritebatch)
        {
            if(Menu.confirmAction)
            {
                dialogtext[Menu.PlayerActSlot - 1].Draw(spritebatch);
            }
        }
    }
}
