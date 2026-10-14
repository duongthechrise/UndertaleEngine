
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace UndertaleEngine.Game1
{
    public static class Fonts
    {
        public static SpriteFont Determono;
        public static void Load(ContentManager content)
        {
            Determono = content.Load<SpriteFont>("Fonts/Determination_Mono");
        }
    }
}
