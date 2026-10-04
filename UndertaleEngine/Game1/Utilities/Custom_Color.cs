
using Microsoft.Xna.Framework;

namespace UndertaleEngine.Game1
{
    public static class CustomColor
    {
        public static readonly Color
            SolidWhite = Color.FromNonPremultiplied(255, 255, 255, 255);
        public static readonly Color
            SolidHPYellow = Color.FromNonPremultiplied(255, 255, 0, 255);
        public static readonly Color
            DarkGray = Color.FromNonPremultiplied(64, 64, 64, 255);
        public static readonly Color SolidHPRed = new ColorCore(new Color(192, 0, 0), brightness: 1f).ToColor();
    }
}