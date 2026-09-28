using Microsoft.Xna.Framework;
using System;
public class CameraShake
{
    private float duration;
    private float timer;
    private float intensity;
    private float speed = 20f;

    private float noiseX;
    private float noiseY;

    public Vector2 Offset { get; private set; }

    public void Shake(float intensity, float duration)
    {
        this.intensity = intensity;
        this.duration = duration;
        this.timer = duration;
    }

    public void Update(GameTime gameTime)
    {
        if (timer <= 0)
        {
            Offset = Vector2.Zero;
            return;
        }

        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        timer -= dt;

        float t = timer / duration;
        float falloff = t * t;

        noiseX += dt * speed;
        noiseY += dt * speed;

        float sx = (MathF.Sin(noiseX) * 2f - 1f);
        float sy = (MathF.Sin(noiseY * 0.8f + 10f) * 2f - 1f);

        Offset = new Vector2(sx, sy) * (intensity * falloff);
    }
}
