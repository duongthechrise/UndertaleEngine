using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic; //rotspeed Color.White DrawMaskPolygon ClampInsideLocal ContainsLocal

namespace UndertaleEngine.Game1
{
    public class BattleBoard
    {
        public Vector2 Position;

        public float RotSpeed;

        public double rotD;
        public float Rotation => (float)rotD;

        public List<Vector2> VerticesMask = new(); 
        public List<Vector2> VerticesVisual = new();
        public List<Vector2> VerticesCollision = new();
        public List<Vector2> VerticesCollisionCover = new();
        public List<Vector2> VerticesCollisionCoverFixed = new();
        public BoardRect CurrentRect;
        public BoardRect StartRect;
        public BoardRect TargetRect;

        // ====== Resize State ======
        private float resizeTimer = 0f;
        private float resizeDuration = 0f;         
        private Func<float, float> easeFunc = Ease.Linear;
        private bool _isResizing = false;        
        public bool IsResizing => _isResizing;

        // ===== MOVE TO =====
        private Vector2 moveStartPos;
        private Vector2 moveTargetPos;
        private float moveTimer = 1f;
        private float moveDuration = 1f;
        private Func<float, float> moveEase = Ease.Linear;
        public bool IsMoving => moveTimer < moveDuration;

        // ===== ROTATE TO =====
        private float rotStart;
        private float rotTarget;
        private float rotTimer = 1f;
        private float rotDuration = 1f;
        private Func<float, float> rotEase = Ease.Linear;
        public bool IsRotating => rotTimer < rotDuration;

        public bool arcActive;
        public Vector2 arcCenter;
        public float arcStartAngle;
        public float arcEndAngle;
        public float arcRadius;
        public float arcTimer;
        public float arcDuration;
        public Func<float, float> arcEase;


        public BattleBoard()
        {
        }

        public void SetRectangle(float left, float up, float right, float down)
        {
            // Visual 
            VerticesVisual.Clear();
            VerticesVisual.Add(new Vector2(-left, -up));   
            VerticesVisual.Add(new Vector2(right, -up));   
            VerticesVisual.Add(new Vector2(right, down));  
            VerticesVisual.Add(new Vector2(-left, down));  
            VerticesVisual.Add(new Vector2(-left, -up));   

            // Collision (thu vào n px mỗi cạnh)
            float pad = 13f;
            VerticesCollision.Clear();
            VerticesCollision.Add(new Vector2(-(left - pad), -(up - pad)));
            VerticesCollision.Add(new Vector2(right - (pad + 0), -(up - pad)));
            VerticesCollision.Add(new Vector2(right - (pad + 0), down - (pad + 0)));
            VerticesCollision.Add(new Vector2(-(left - pad), down - (pad + 0)));

            //dùng cho cover board
            // ----- Mask (nền đen bên trong) thu vào ~5px để overlap xoá viền -----
            float inset = 5f;
            VerticesMask.Clear();
            VerticesMask.Add(new Vector2(-(left - inset), -(up - inset)));
            VerticesMask.Add(new Vector2(right - inset, -(up - inset)));
            VerticesMask.Add(new Vector2(right - inset, down - inset));
            VerticesMask.Add(new Vector2(-(left - inset), down - inset));

            float outset = -8f;
            VerticesCollisionCover.Clear();
            VerticesCollisionCover.Add(new Vector2(-(left - outset), -(up - outset)));
            VerticesCollisionCover.Add(new Vector2(right - outset, -(up - outset)));
            VerticesCollisionCover.Add(new Vector2(right - outset, down - outset));
            VerticesCollisionCover.Add(new Vector2(-(left - outset), down - outset));

            float insetfixed = -0f;
            VerticesCollisionCoverFixed.Clear();
            VerticesCollisionCoverFixed.Add(new Vector2(-(left - insetfixed), -(up - insetfixed)));
            VerticesCollisionCoverFixed.Add(new Vector2(right - insetfixed, -(up - insetfixed)));
            VerticesCollisionCoverFixed.Add(new Vector2(right - insetfixed, down - insetfixed));
            VerticesCollisionCoverFixed.Add(new Vector2(-(left - insetfixed), down - insetfixed));

            CurrentRect = new BoardRect(left, up, right, down);
        }


        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            rotD += RotSpeed * dt; 

            if (_isResizing && resizeDuration > 0f)
            {
                resizeTimer += dt;
                float t = Math.Clamp(resizeTimer / resizeDuration, 0f, 1f);
                float e = easeFunc(t);

                float L = MathHelper.Lerp(StartRect.Left, TargetRect.Left, e);
                float R = MathHelper.Lerp(StartRect.Right, TargetRect.Right, e);
                float U = MathHelper.Lerp(StartRect.Up, TargetRect.Up, e);
                float D = MathHelper.Lerp(StartRect.Down, TargetRect.Down, e);

                SetRectangle(L, U, R, D);

                if (t >= 1f)
                {
                    _isResizing = false;
                    resizeDuration = 0f;
                    CurrentRect = TargetRect;
                }
            }
            if (moveTimer < moveDuration)
            {
                moveTimer += dt;
                float t = Math.Clamp(moveTimer / moveDuration, 0f, 1f);
                float e = moveEase(t);

                Position = Vector2.Lerp(moveStartPos, moveTargetPos, e);
            }

            if (rotTimer < rotDuration)
            {
                rotTimer += dt;
                float t = Math.Clamp(rotTimer / rotDuration, 0f, 1f);
                float e = rotEase(t);

                rotD = MathHelper.Lerp(rotStart, rotTarget, e);
            }
            if (arcActive)
            {
                arcTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                float t = Math.Clamp(arcTimer / arcDuration, 0f, 1f);
                float e = arcEase(t);
                float angle = MathHelper.Lerp(arcStartAngle, arcEndAngle, e);
                Position = new Vector2(arcCenter.X + arcRadius * MathF.Cos(angle),
                                       arcCenter.Y + arcRadius * MathF.Sin(angle)
                                       );
                if (t >= 1f)
                    arcActive = false;
            }
        }

        public Vector2 WorldToLocal(Vector2 p)
        {
            Vector2 offset = p - Position;
            float ang = -Rotation; 

            float c = (float)Math.Cos(ang);
            float s = (float)Math.Sin(ang);

            return new Vector2(
                offset.X * c - offset.Y * s,
                offset.X * s + offset.Y * c
            );
        }

        public Vector2 LocalToWorld(Vector2 p)
        {
            float ang = Rotation;

            float c = (float)Math.Cos(ang);
            float s = (float)Math.Sin(ang);

            return new Vector2(
                p.X * c - p.Y * s,
                p.X * s + p.Y * c
            ) + Position;
        }

        public bool ContainsPoint(Vector2 worldPoint)
        {
            Vector2 p = WorldToLocal(worldPoint);
            return ContainsLocalMain(p);
        }

        public bool ContainsLocalMain(Vector2 p)
        {
            bool inside = false;
            int count = VerticesCollision.Count;

            for (int i = 0, j = count - 1; i < count; j = i++)
            {
                Vector2 vi = VerticesCollision[i];
                Vector2 vj = VerticesCollision[j];

                bool intersect =
                    ((vi.Y > p.Y) != (vj.Y > p.Y)) &&
                    (p.X < (vj.X - vi.X) * (p.Y - vi.Y) / (vj.Y - vi.Y + float.Epsilon) + vi.X);

                if (intersect)
                    inside = !inside;
            }

            return inside;
        }

        public Vector2 LimitPoint(Vector2 worldPoint)
        {
            Vector2 local = WorldToLocal(worldPoint);

            if (ContainsLocalMain(local))
                return worldPoint; 

            int count = VerticesCollision.Count;
            if (count == 0)
                return worldPoint;

            float bestDist = float.MaxValue;
            Vector2 best = local;

            for (int i = 0; i < count; i++)
            {
                Vector2 a = VerticesCollision[i];
                Vector2 b = VerticesCollision[(i + 1) % count];

                Vector2 closest = ClosestPointOnSegment(a, b, local);
                float d = Vector2.DistanceSquared(local, closest);
                if (d < bestDist)
                {
                    bestDist = d;
                    best = closest;
                }
            }

            return LocalToWorld(best);
        }

        private Vector2 ClosestPointOnSegment(Vector2 a, Vector2 b, Vector2 p)
        {
            Vector2 ab = b - a;
            float lenSq = ab.LengthSquared();
            if (lenSq <= float.Epsilon)
                return a;

            float t = Vector2.Dot(p - a, ab) / lenSq;
            t = MathHelper.Clamp(t, 0f, 1f);
            return a + t * ab;
        }

        public Vector2 ClampInsideLocalMain(Vector2 p)
        {
            if (ContainsLocalMain(p))
                return p;

            int count = VerticesCollision.Count;
            float bestDist = float.MaxValue;
            Vector2 best = p;

            for (int i = 0; i < count; i++)
            {
                Vector2 a = VerticesCollision[i];
                Vector2 b = VerticesCollision[(i + 1) % count];

                Vector2 closest = ClosestPointOnSegment(a, b, p);
                float d = Vector2.DistanceSquared(p, closest);
                if (d < bestDist)
                {
                    bestDist = d;
                    best = closest;
                }
            }

            return best;
        }

        public Vector2 ClampInsideWorldMain(Vector2 world)
        {
            var local = WorldToLocal(world);
            local = ClampInsideLocalMain(local);
            return LocalToWorld(local);
        }
        public bool ContainsLocalCover(Vector2 p)
        {
            int count = VerticesCollisionCover.Count;
            if (count < 3) return false;

            bool inside = false;
            for (int i = 0, j = count - 1; i < count; j = i++)
            {
                Vector2 vi = VerticesCollisionCover[i];
                Vector2 vj = VerticesCollisionCover[j];

                bool intersect =
                    ((vi.Y > p.Y) != (vj.Y > p.Y)) &&
                    (p.X < (vj.X - vi.X) * (p.Y - vi.Y) / (vj.Y - vi.Y + float.Epsilon) + vi.X);

                if (intersect)
                    inside = !inside;
            }
            return inside;
        }

        public bool ContainsPointCover(Vector2 worldPoint)
        {
            Vector2 local = WorldToLocal(worldPoint);
            return ContainsLocalCover(local);
        }

        public Vector2 PushOutsideWorldCover(Vector2 worldPoint)
        {
            Vector2 local = WorldToLocal(worldPoint);

            if (!ContainsLocalCover(local))
                return worldPoint;

            float bestDist = float.MaxValue;
            Vector2 best = local;

            int count = VerticesCollisionCover.Count;
            for (int i = 0; i < count; i++)
            {
                Vector2 a = VerticesCollisionCover[i];
                Vector2 b = VerticesCollisionCover[(i + 1) % count];

                Vector2 closest = ClosestPointOnSegment(a, b, local);
                float d = Vector2.DistanceSquared(local, closest);

                if (d < bestDist)
                {
                    bestDist = d;
                    best = closest;
                }
            }

            return LocalToWorld(best);
        }
        public bool ContainsPointCoverCheck(Vector2 worldPoint, float shrink = 0f)
        {
            Vector2 p = WorldToLocal(worldPoint);

            Vector2 center = Vector2.Zero;
            foreach (var v in VerticesCollisionCoverFixed)
                center += v;
            center /= VerticesCollisionCoverFixed.Count;

            Vector2 dir = center - p;

            float len = dir.Length();
            if (len > 0.0001f)
            {
                dir /= len;
                p += dir * shrink; 
            }

            return ContainsLocalCoverRaw(p);
        }
        private bool ContainsLocalCoverRaw(Vector2 p)
        {
            int count = VerticesCollisionCoverFixed.Count;
            if (count < 3) return false;

            bool inside = false;

            for (int i = 0, j = count - 1; i < count; j = i++)
            {
                Vector2 vi = VerticesCollisionCoverFixed[i];
                Vector2 vj = VerticesCollisionCoverFixed[j];

                bool intersect =
                    ((vi.Y > p.Y) != (vj.Y > p.Y)) &&
                    (p.X < (vj.X - vi.X) * (p.Y - vi.Y) / ((vj.Y - vi.Y) + float.Epsilon) + vi.X);

                if (intersect)
                    inside = !inside;
            }

            return inside;
        }

        public void DrawBorder(SpriteBatch spriteBatch, Texture2D texture, Color color, float thickness)
        {
            int count = VerticesVisual.Count;

            if (count < 2) return;

            for (int i = 0; i < count; i++)
            {
                Vector2 localA = VerticesVisual[i];
                Vector2 localB = VerticesVisual[(i + 1) % count];

                Vector2 worldA = LocalToWorld(localA);
                Vector2 worldB = LocalToWorld(localB);

                Vector2 delta = worldB - worldA;
                float length = delta.Length();
                if (length <= float.Epsilon)
                    continue;

                float angle = (float)Math.Atan2(delta.Y, delta.X);

                spriteBatch.Draw(
                    texture,
                    worldA,
                    null,
                    color,
                    angle,
                    new Vector2(0f, 0f),
                    new Vector2(length, thickness),
                    SpriteEffects.None,
                    0f
                );
            }
        }
        public void DrawMaskPolygon(GraphicsDevice graphics, Effect effect)
        {
            if (VerticesMask.Count < 4)
                return;

            Vector3 v0 = new Vector3(LocalToWorld(VerticesMask[0]), 0);
            Vector3 v1 = new Vector3(LocalToWorld(VerticesMask[1]), 0);
            Vector3 v2 = new Vector3(LocalToWorld(VerticesMask[2]), 0);
            Vector3 v3 = new Vector3(LocalToWorld(VerticesMask[3]), 0);

            VertexPositionColor[] verts = new VertexPositionColor[]
            {
                new VertexPositionColor(v0, Color.Black),
                new VertexPositionColor(v1, Color.Black),
                new VertexPositionColor(v2, Color.Black),
                
                new VertexPositionColor(v2, Color.Black),
                new VertexPositionColor(v3, Color.Black),
                new VertexPositionColor(v0, Color.Black)
            };

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.DrawUserPrimitives(
                    PrimitiveType.TriangleList,
                    verts,
                    0,
                    2              
                );
            }
        }



        // ======= Khu vực cho chuyển động và resize board =======

        public void StartResize(BoardRect newRect, float duration, Func<float, float> ease)
        {
            if (CurrentRect.Left == 0 && CurrentRect.Right == 0 &&
                CurrentRect.Up == 0 && CurrentRect.Down == 0)
            {
                CurrentRect = newRect;
                SetRectangle(newRect.Left, newRect.Up, newRect.Right, newRect.Down);
            }

            StartRect = CurrentRect;
            TargetRect = newRect;

            resizeTimer = 0f;
            resizeDuration = Math.Max(duration, 0.0001f);
            easeFunc = ease ?? Ease.Linear;
            _isResizing = true;
        }

        public void MoveTo(Vector2 newCenter, float duration, Func<float, float> ease)
        {
            moveStartPos = Position;
            moveTargetPos = newCenter;
            moveDuration = Math.Max(0.0001f, duration);
            moveTimer = 0f;
            moveEase = ease;
        }

        public void RotateTo(float targetRadians, float duration, Func<float, float> ease)
        {
            rotStart = Rotation;
            rotTarget = targetRadians;
            rotDuration = Math.Max(0.0001f, duration);
            rotTimer = 0f;
            rotEase = ease;
        }

        public void MoveInArc(BattleBoard board, Vector2 center, float startAngle, float endAngle, float radius, float duration, Func<float, float> ease)
        {
            board.arcActive = true;
            board.arcCenter = center;
            board.arcStartAngle = startAngle;
            board.arcEndAngle = endAngle;
            board.arcRadius = radius;
            board.arcTimer = 0f;
            board.arcDuration = duration;
            board.arcEase = ease;
        }
        public static class Ease
        {
            public static float Linear(float t) => t;

            public static float InQuad(float t) => t * t;
            public static float OutQuad(float t) => 1 - (1 - t) * (1 - t);
            public static float InOutQuad(float t)
                => t < 0.5f ? 2 * t * t : 1 - MathF.Pow(-2 * t + 2, 2) / 2;

            public static float InCubic(float t) => t * t * t;
            public static float OutCubic(float t) => 1 - MathF.Pow(1 - t, 3);

            public static float InOutCubic(float t)
                => t < 0.5f ? 4 * t * t * t
                           : 1 - MathF.Pow(-2 * t + 2, 3) / 2;

            public static float OutBack(float t)
            {
                float c1 = 1.70158f;
                float c3 = c1 + 1;
                return 1 + c3 * MathF.Pow(t - 1, 3) + c1 * MathF.Pow(t - 1, 2);
            }

            public static float OutElastic(float t)
            {
                float c4 = (2 * MathF.PI) / 3;
                return t == 0 ? 0 :
                       t == 1 ? 1 :
                       MathF.Pow(2, -10 * t) * MathF.Sin((t * 10 - 0.75f) * c4) + 1;
            }

            public static float OutBounce(float t)
            {
                const float n1 = 7.5625f;
                const float d1 = 2.75f;

                if (t < 1 / d1)
                    return n1 * t * t;
                else if (t < 2 / d1)
                    return n1 * (t -= 1.5f / d1) * t + 0.75f;
                else if (t < 2.5 / d1)
                    return n1 * (t -= 2.25f / d1) * t + 0.9375f;
                else
                    return n1 * (t -= 2.625f / d1) * t + 0.984375f;
            }
        }

    }
}
