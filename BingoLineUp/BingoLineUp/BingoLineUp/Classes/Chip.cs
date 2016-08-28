using System;
using Microsoft.Xna.Framework;

namespace BingoLineUp.Classes
{
    class Chip
    {
        /// <summary>
        /// Возвращает true, если ходил первый игрок. Используется для цвета
        /// </summary>
        public bool IsFirstPlayer { get; private set; }
        /// <summary>
        /// Возвращает true, если фишка уже упала
        /// </summary>
        public bool IsFallen { get; private set; }
        /// <summary>
        /// Возвращает индекс вертикали
        /// </summary>
        public int IndexColumn { get; private set; }
        /// <summary>
        /// Возвращает количество фишек в вертикали, включая текущую фишку
        /// </summary>
        public int WeightColumn { get; private set; }
        /// <summary></summary>
        /// <param name="x">Координата x верхнего левого угла</param>
        /// <param name="y">Координата y верхнего левого угла</param>
        /// <param name="step">true, если ходил первый игрок. Используется для цвета в дальнейшем</param>
        /// <param name="weightColumn">Количество фишек в вертикали, включая текущую фишку</param>
        /// <param name="indexColumn">Индекс вертикали</param>
        public Chip(int x, int y, bool step, int weightColumn, int indexColumn)
        {
            IsFirstPlayer = step;
            IsFallen = false;
            WeightColumn = weightColumn;
            IndexColumn = indexColumn;
            int newx = (IndexColumn == 0) ? x : 10 + IndexColumn * 70;
            RectChip = new Rectangle(newx, y, 60, 60);
        }
        /// <summary>
        /// Перемещает фишку в вертикали вниз. Используется в Анимации.
        /// </summary>
        public void DeposeDown()
        {
            int h = (int)Math.Round(414 - (WeightColumn - 1) * 67.5);
            RectChip = new Rectangle(RectChip.X, (int)MathHelper.Min((float)(RectChip.Y + 10), (float)h), RectChip.Width, RectChip.Height);

            if (RectChip.Y == h) { IsFallen = true; }
        }
        public Rectangle RectChip { get; private set; }
    }
}
