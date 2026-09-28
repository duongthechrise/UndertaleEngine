using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

public static class DebugDraw
{
    // Pixel trắng 1x1 dùng chung
    private static Texture2D _pixel;

    public static void Init(GraphicsDevice graphicsDevice)
    {
        _pixel = new Texture2D(graphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });
    }

    /// <summary>
    /// Vẽ 1 đường thẳng từ p1 đến p2.
    /// </summary>
    public static void DrawLine(SpriteBatch spriteBatch, Vector2 p1, Vector2 p2, Color color)
    {
        Vector2 delta = p2 - p1;
        float length = delta.Length();
        if (length <= 0.0001f)
            return;

        float angle = MathF.Atan2(delta.Y, delta.X);

        spriteBatch.Draw(
            _pixel,
            p1,
            null,
            color,
            angle,
            Vector2.Zero,
            new Vector2(length, 1f),
            SpriteEffects.None,
            0f
        );
    }

    /// <summary>
    /// Vẽ outline của OBB.
    /// </summary>
    public static void DrawOBB(SpriteBatch spriteBatch, OBB obb, Color color)
    {
        var c = obb.Corners;

        DrawLine(spriteBatch, c[0], c[1], color);
        DrawLine(spriteBatch, c[1], c[2], color);
        DrawLine(spriteBatch, c[2], c[3], color);
        DrawLine(spriteBatch, c[3], c[0], color);
    }
}

