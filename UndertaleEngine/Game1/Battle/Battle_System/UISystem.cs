using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace UndertaleEngine.Game1
{
    public class UISystem
    {
        public Button_Fight _buttonfight;
        public Button_Act _buttonact;
        public Button_Item _buttonitem;
        public Button_Mercy _buttonmercy;
        private PlayerHP _HP;
        private SpriteFont _MarsFont;
        private DisplayText _LV;
        private DisplayText _HPstat;
        private DisplayText _name;
        private HP_Bar _hpbar;
        public void Load(ContentManager content, GraphicsDevice graphic)
        {
            // Load button textures and initialize buttons

            _buttonfight = new Button_Fight();
            _buttonfight.LoadContent(content, graphic);
            _buttonact = new Button_Act();
            _buttonact.LoadContent(content);
            _buttonitem = new Button_Item();
            _buttonitem.LoadContent(content);
            _buttonmercy = new Button_Mercy();
            _buttonmercy.LoadContent(content);
            _HP = new PlayerHP();
            _HP.LoadContent(content);
            _MarsFont = content.Load<SpriteFont>("Fonts/Mars_Needs_Cunnilingus");
            _LV = new DisplayText(_MarsFont, $"LV {19}", new Vector2(132, 400), Color.White, 1.5f);
            _HPstat = new DisplayText(_MarsFont, "", new Vector2(404, 400), Color.White, 1.5f);
            _name = new DisplayText(_MarsFont, "Frisk", new Vector2(30, 400), Color.White, 1.5f);
            _hpbar = new HP_Bar(graphic, new Vector2(275, 400), 115, 21, 92f, CustomColor.SolidHPYellow, CustomColor.SolidHPRed); //((x, y), width, height, maxHP) // [default] ((275,400), 115, 21, maxHP)
            _hpbar.LoadContent(content);
        }
        public void Update(GameTime gametime)
        {
            _buttonfight.Update(gametime);
            _buttonact.Update(gametime);
            _buttonitem.Update(gametime);
            _buttonmercy.Update(gametime);
            _HPstat.SetText($"{ _hpbar.RealHP} / { _hpbar.MaxHP}");
        }
        public void Draw(SpriteBatch spritebatch)
        {
            _buttonfight.Draw(spritebatch);
            _buttonact.Draw(spritebatch);
            _buttonitem.Draw(spritebatch);
            _buttonmercy.Draw(spritebatch);

            _LV.Draw(spritebatch);
            _HPstat.Draw(spritebatch);
            _HP.Draw(spritebatch);
            _name.Draw(spritebatch);

            _hpbar.Draw(spritebatch);
        }
    }
}
