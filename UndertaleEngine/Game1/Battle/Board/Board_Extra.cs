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

        /// <summary>
        /// Constrain vị trí player vào các board.
        /// - Nếu nằm trong board nào → cho đứng đó, activeBoard = board đó.
        /// - Nếu không nằm trong board nào → kẹp vào cạnh gần nhất của tất cả board.
        /// </summary>
        public Vector2 ConstrainPlayer(Vector2 desired)
        {
            // HỆ THỐNG CŨ - GIỮ NGUYÊN
            foreach (var b in Boards)
            {
                if (b.ContainsPoint(desired)) // hàm ContainsPoint dùng polygon main
                {
                    ActiveBoard = b;
                    return desired;
                }
            }

            // snap về board gần nhất
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
            // Step 1: clamp vào board chính
            Vector2 pos = ConstrainPlayer(desiredPos);

            // Step 2: xử lý cover
            foreach (var cover in CoverBoards)
            {
                if (cover.ContainsPointCover(pos))
                {
                    // đẩy ra khỏi cover
                    pos = cover.PushOutsideWorldCover(pos);

                    // Step 3: patch — không cho phép thoát ra ngoài board chính
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
            // Nếu pos đã an toàn → trả về luôn
            if (!IsInsideAnyCover(pos) && ActiveBoard.ContainsPoint(pos))
                return pos;

            const float step = 1f;          // khoảng cách từng lần thử
            const float maxRadius = 120f;   // bán kính tối đa (chỉnh tuỳ board)

            // Ưu tiên thử 8 hướng cơ bản trước (nhanh + rẻ)
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

            // Vòng quét xoay dần ra xa
            for (float r = step; r <= maxRadius; r += step)
            {
                // Thử 8 hướng trước (nhanh nhất)
                foreach (var d in dirs8)
                {
                    Vector2 test = pos + d * r;

                    if (!IsInsideAnyCover(test) && ActiveBoard.ContainsPoint(test))
                        return test;
                }

                // Nếu chưa tìm được → quét theo vòng tròn (độ phân giải 20 hướng)
                for (float ang = 0; ang < MathF.PI * 2f; ang += 0.35f) // 0.35 ~ 20 hướng
                {
                    Vector2 dir = new Vector2(MathF.Cos(ang), MathF.Sin(ang));
                    Vector2 test = pos + dir * r;

                    if (!IsInsideAnyCover(test) && ActiveBoard.ContainsPoint(test))
                        return test;
                }
            }

            // Trường hợp hiếm: không thoát nổi cover (do cover to bằng main board)
            // fallback: clamp main board
            return ActiveBoard.LimitPoint(pos);
        }

    }
}

