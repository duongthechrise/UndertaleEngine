using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

public class OBB
{
    public Vector2 Position;   
    public Vector2 HalfSize;  
    public float Rotation;     
    public Vector2[] Corners = new Vector2[4];
    public Vector2 AxisX;
    public Vector2 AxisY;
    public HashSet<OBB> CurrentCollisions = new HashSet<OBB>();
    public HashSet<OBB> PreviousCollisions = new HashSet<OBB>();

    public OBB(Vector2 position, Vector2 size, float rotation = 0f)
    {
        Position = position;
        HalfSize = size * 0.5f;
        Rotation = rotation;
        Update();
    }

    public void Update()
    {
        AxisX = new Vector2(MathF.Cos(Rotation), MathF.Sin(Rotation));
        AxisY = new Vector2(-AxisX.Y, AxisX.X);

        Vector2 hx = AxisX * HalfSize.X;
        Vector2 hy = AxisY * HalfSize.Y;

        Corners[0] = Position + hx + hy;
        Corners[1] = Position - hx + hy;
        Corners[2] = Position - hx - hy;
        Corners[3] = Position + hx - hy;
    }

    public bool Intersects(OBB other)
    {
        Vector2[] axes = new Vector2[4]
        {
            this.AxisX,
            this.AxisY,
            other.AxisX,
            other.AxisY
        };

        for (int i = 0; i < axes.Length; i++)
        {
            Vector2 axis = axes[i];

            ProjectOntoAxis(this, axis, out float minA, out float maxA);
            ProjectOntoAxis(other, axis, out float minB, out float maxB);

            if (maxA < minB || maxB < minA)
            {
                return false;
            }
        }

        return true; 
    }

    private static void ProjectOntoAxis(OBB obb, Vector2 axis, out float min, out float max)
    {
        float dot = Vector2.Dot(obb.Corners[0], axis);
        min = max = dot;

        for (int i = 1; i < 4; i++)
        {
            dot = Vector2.Dot(obb.Corners[i], axis);
            if (dot < min) min = dot;
            if (dot > max) max = dot;
        }
    }
}

