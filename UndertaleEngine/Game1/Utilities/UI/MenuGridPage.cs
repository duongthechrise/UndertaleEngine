using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace UndertaleEngine.Game1
{
    abstract class MenuGridPage : UIPage
    {
        protected InputBuffer input;
        protected SpriteFont font;

        protected int rows;
        protected int cols;

        protected int row;
        protected int col;

        protected Vector2 origin;
        protected Vector2 spacing;

        public MenuGridPage(
            InputBuffer input,
            SpriteFont font,
            int rows,
            int cols,
            Vector2 origin,
            Vector2 spacing)
        {
            this.input = input;
            this.font = font;
            this.rows = rows;
            this.cols = cols;
            this.origin = origin;
            this.spacing = spacing;
        }

        public override void Update(GameTime gameTime)
        {
            if (input.Up()) row--;
            if (input.Down()) row++;
            if (input.Left()) col--;
            if (input.Right()) col++;

            row = Math.Clamp(row, 0, rows - 1);
            col = Math.Clamp(col, 0, cols - 1);

            if (input.Confirm())
                OnConfirm(row, col);

            if (input.Cancel())
                OnCancel();
        }

        protected Vector2 GetPosition(int r, int c)
        {
            return origin + new Vector2(c * spacing.X, r * spacing.Y);
        }

        protected abstract void OnConfirm(int row, int col);
        protected virtual void OnCancel() { }
    }

}
