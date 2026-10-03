using System;
using Microsoft.Xna.Framework;
namespace UndertaleEngine.Game1
{
    public class ColorCore
    {
        public float R, G, B;
        public float H, S, V;

        public ColorCore(Color color, float brightness = 1f)
        {
            R = color.R / 255f;
            G = color.G / 255f;
            B = color.B / 255f;

            RGBtoHSV(R, G, B, out H, out S, out V);

            V *= brightness;
            ClampHSV();
        }

        public void SetBrightness(float brightness)
        {
            V = MathHelper.Clamp(brightness, 0f, 1f);
        }

        public Color ToColor()
        {
            HSVtoRGB(H, S, V, out float r, out float g, out float b);
            return new Color(r, g, b);
        }

        private void ClampHSV()
        {
            H = MathHelper.Clamp(H, 0f, 1f);
            S = MathHelper.Clamp(S, 0f, 1f);
            V = MathHelper.Clamp(V, 0f, 1f);
        }

        private static void RGBtoHSV(float r, float g, float b, out float h, out float s, out float v)
        {
            float max = Math.Max(r, Math.Max(g, b));
            float min = Math.Min(r, Math.Min(g, b));
            v = max;

            float delta = max - min;
            if (max == 0f)
                s = 0f;
            else
                s = delta / max;

            if (delta == 0f)
                h = 0f;
            else if (max == r)
                h = (g - b) / delta;
            else if (max == g)
                h = 2f + (b - r) / delta;
            else
                h = 4f + (r - g) / delta;

            h /= 6f;
            if (h < 0f) h += 1f;
        }

        private static void HSVtoRGB(float h, float s, float v, out float r, out float g, out float b)
        {
            if (s == 0f)
            {
                r = g = b = v;
                return;
            }

            h = (h - (float)System.Math.Floor(h)) * 6f;
            int i = (int)h;
            float f = h - i;
            float p = v * (1f - s);
            float q = v * (1f - s * f);
            float t = v * (1f - s * (1f - f));

            switch (i)
            {
                case 0: r = v; g = t; b = p; break;
                case 1: r = q; g = v; b = p; break;
                case 2: r = p; g = v; b = t; break;
                case 3: r = p; g = q; b = v; break;
                case 4: r = t; g = p; b = v; break;
                default: r = v; g = p; b = q; break;
            }
        }
    }
}