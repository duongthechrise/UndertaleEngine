using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace UndertaleEngine.Game1
{
    public class BattleBoardSystem
    {
        public readonly List<BattleBoard> Boards = new();
        public readonly List<BattleBoard> CoverBoards = new();
        public BattleBoard ActiveBoard { get; private set; }

        public void AddBoard(BattleBoard board)
        {
            if (!Boards.Contains(board))
                Boards.Add(board);

            if (ActiveBoard == null)
                ActiveBoard = board;
        }
        public void AddCoverBoard(BattleBoard board)
        {
            CoverBoards.Add(board);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var b in Boards)
                b.Update(gameTime);
        }

        public Vector2 ConstrainPlayer(Vector2 desired)
        {
            foreach (var b in Boards)
            {
                if (b.ContainsPoint(desired))
                {
                    ActiveBoard = b;
                    return desired;
                }
            }

            float bestDist = float.MaxValue;
            Vector2 best = desired;
            BattleBoard bestBoard = ActiveBoard;

            foreach (var b in Boards)
            {
                Vector2 clamped = b.ClampInsideWorldMain(desired);
                float d = Vector2.DistanceSquared(desired, clamped);
                if (d < bestDist)
                {
                    bestDist = d;
                    best = clamped;
                    bestBoard = b;
                }
            }

            ActiveBoard = bestBoard;
            return best;
        }
        public Vector2 ConstrainPlayerWithCover(Vector2 desiredPos)
        {
            Vector2 pos = ConstrainPlayer(desiredPos);

            foreach (var cover in CoverBoards)
            {
                if (cover.ContainsPointCover(pos))
                {
                    pos = cover.PushOutsideWorldCover(pos);

                    if (!ActiveBoard.ContainsPoint(pos))
                    {
                        pos = ActiveBoard.LimitPoint(pos);
                    }
                }
            }

            return pos;
        }
        public bool IsInsideAnyCover(Vector2 pos)
        {
            foreach (var cover in CoverBoards)
            {
                if (cover.ContainsPointCoverCheck(pos))
                    return true;
            }
            return false;
        }
        public Vector2 FindNearestSafePoint(Vector2 pos)
        {
            if (!IsInsideAnyCover(pos) && ActiveBoard.ContainsPoint(pos))
                return pos;

            const float step = 1f;         
            const float maxRadius = 120f;  

            Vector2[] dirs8 =
            {
                new Vector2( 1,  0),
                new Vector2(-1,  0),
                new Vector2( 0,  1),
                new Vector2( 0, -1),
                new Vector2( 1,  1),
                new Vector2( 1, -1),
                new Vector2(-1,  1),
                new Vector2(-1, -1)
            };

            for (float r = step; r <= maxRadius; r += step)
            {
                foreach (var d in dirs8)
                {
                    Vector2 test = pos + d * r;

                    if (!IsInsideAnyCover(test) && ActiveBoard.ContainsPoint(test))
                        return test;
                }

                for (float ang = 0; ang < MathF.PI * 2f; ang += 0.35f) 
                {
                    Vector2 dir = new Vector2(MathF.Cos(ang), MathF.Sin(ang));
                    Vector2 test = pos + dir * r;

                    if (!IsInsideAnyCover(test) && ActiveBoard.ContainsPoint(test))
                        return test;
                }
            }

            return ActiveBoard.LimitPoint(pos);
        }

    }
}

