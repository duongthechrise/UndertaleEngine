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
        public TypeText dialogtext;
        
        public Dialogs(Option_Act act)
        {
            this.act = act;
        }
        public void Load(ContentManager content)
        {
            determono = content.Load<SpriteFont>("Fonts/Determination_Mono");
            dialogtext = new TypeText(determono, new Vector2(62, 270));
        }
        public void Update(GameTime gametime)
        {
            if(Menu.confirmActionevoke && Menu.confirmAction)
            {
                switch(act.Option[Menu.PlayerActSlot - 1])
                {
                    case "Check":
                        if (Menu.next == 0 && Menu.confirmActionevoke)
                        {
                            dialogtext.Show($"* {Menu.Enemy1Name} - ATK 8 DEF 4\n* Has been following you around.\n~wait0.25~* Must want to be friends!");
                            Menu.confirmActionevoke = false;
                        }
                        if(Menu.next == 1 && Menu.confirmActionevoke)
                        {
                            dialogtext.Show("* Yes, this is the second page.\n* what are you waiting for?\n~wait0.5~* another page?");
                            Menu.confirmActionevoke = false;
                        }
                        if(Menu.next == 2 && Menu.confirmActionevoke)
                            End();
                        break;
                    case "Talk":
                        if (Menu.next == 0 && Menu.confirmActionevoke)
                        {
                            dialogtext.Show("* You talk to Red about your self.\n~wait~* He seems don't care.\n~wait~* He is such emotionless.");
                            Menu.confirmActionevoke = false;
                        }
                        if (Menu.next == 1 && Menu.confirmActionevoke)
                            End();
                        break;
                    case "Flirt":
                        if (Menu.next == 0 && Menu.confirmActionevoke)
                        {
                            dialogtext.Show("* You flirt Red with all your\n  loves.\n* He is such nonchalant.");
                            Menu.confirmActionevoke = false;
                        }
                        if (Menu.next == 1 && Menu.confirmActionevoke)
                            End();
                        break;

                    case "Joke":
                        if(Menu.next == 0 && Menu.confirmActionevoke)
                        {
                            dialogtext.Show("* You make bad puns, or even\n  stupid jokes you know somewhere.");
                            Menu.confirmActionevoke = false;
                        }
                        if(Menu.next == 1 && Menu.confirmActionevoke)
                        {
                            dialogtext.Show("* But Red is too stupid to\n  understand.");
                            Menu.confirmActionevoke = false;
                        }
                        if (Menu.next == 2 && Menu.confirmActionevoke)
                            End();
                        break;
                }
            }
            dialogtext.Update(gametime);
        }
        void End()
        {
            Menu.ActEnemyColumn = 1;
            Menu.ActOptsColumn = 1;
            Menu.ActOptsRow = 1;
            Menu.next = 0;
            Menu.confirmAction = false;
            Menu.confirmActionevoke = false;
            Menu.selectingActopts = false;
            Menu.actingonEnemy1 = false;
            Menu.turnfinish = true;
        }
        public void Draw(SpriteBatch spritebatch)
        {
            if(Menu.confirmAction)
            {
                if (dialogtext != null)
                dialogtext.Draw(spritebatch);
            }
        }
    }
}
