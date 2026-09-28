
using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework;

namespace UndertaleEngine
{
    public class Button : UIElement
    {
        Texture2D texture;
        Action onClick;
        bool isHover;
        bool wasPressedLastFrame;
        public Button(Rectangle bounds, Texture2D texture, Action onClick)
        {
            Bounds = bounds;
            this.texture = texture;
            this.onClick = onClick;
        }
        public override void Update(GameTime gameTime)
        {
            if (!enabled) return;
        }
    }
}
