using System;
using Microsoft.Xna.Framework;
using System.Threading.Tasks;

namespace BingoLineUp.Classes
{
    /// <summary>
    /// Тип победителя
    /// </summary>
    enum WinnerType { FirstPlayer, SecondPlayer, StandOff, None }
    class PlayGround
    {
        public Chip[,] Chips { get; private set; }
        public bool IsFirstPlayer { get; private set; }
        public int[] HorisontalWeight { get; private set; }
        public int LenghtHorisontal { get; private set; }
        public int LenghtVertical { get; private set; }
        /// <summary>
        /// Добавляет фишку в указанную вертикаль
        /// </summary>
        /// <param name="Column">Индекс вертикали. Индексация начинается с 0</param>
        public Chip Add(int Column)
        {
            Chip newChip = new Chip(10, 6, IsFirstPlayer, HorisontalWeight[Column] + 1, Column);
            Chips[HorisontalWeight[Column], Column] = newChip;
            HorisontalWeight[Column]++;
            IsFirstPlayer = !IsFirstPlayer;
            return newChip;
        }
        /// <summary></summary>
        /// <param name="CountHorisontal">Количество фишек по горизонтали</param>
        /// <param name="CountVertical">Количество фишек по вертикали</param>
        public PlayGround(int CountVertical, int CountHorisontal)
        {
            Chips = new Chip[CountVertical, CountHorisontal];
            LenghtHorisontal = CountHorisontal;
            LenghtVertical = CountVertical;
            HorisontalWeight = new int[CountHorisontal];
            IsFirstPlayer = true;
            rectBasis = new Rectangle(0, 75, 500, 400);
            mas = new bool?[LenghtVertical, LenghtHorisontal];
        }
        public Rectangle rectBasis { get; private set; }


        private bool?[,] mas;
        /// <summary>
        /// Определяет победителя
        /// </summary>
        /// <returns>Возвращает WinnerType</returns>
        public WinnerType HasWinner()
        {
            ConvertChipsToBoolMassive();
            bool isFirst = false, isSecond = false;

            isFirst = Test(mas, 4, true);
            isSecond = Test(mas, 4, false);

            if (isFirst) return WinnerType.FirstPlayer;
            else if (isSecond) return WinnerType.SecondPlayer;
            else return WinnerType.None;
            //Добавить ничью
        }
        private void ConvertChipsToBoolMassive()
        {
            Parallel.Invoke(() =>
            {
                for (int i = 0; i < LenghtVertical; i++)
                {
                    for (int j = 0; j < LenghtHorisontal; j++)
                    {
                        if (Chips[i, j] != null)
                        {
                            if (Chips[i, j].IsFirstPlayer) mas[i, j] = true;
                            else mas[i, j] = false;
                        }
                        else mas[i, j] = null;
                    }
                }
            });
        }
        private bool Test(bool?[,] mas, int count, bool isGreen)
        {
            for (int i = 0; i < mas.GetLength(0); i++)
            {
                for (int j = 0; j < mas.GetLength(1); j++)
                {
                    if (isGreen == mas[i, j])
                        if (TestHorisontal(mas, i, j, count, isGreen) || TestVertical(mas, i, j, count, isGreen) || TestDiagonals(mas, i, j, count, isGreen)) return true;
                }
            }
            return false;
        }
        private bool TestHorisontal(bool?[,] mas, int i, int j, int count, bool isGreen)
        {
            int a = 0;
            for (; j < mas.GetLength(1); j++)
            {
                if (isGreen == mas[i, j]) a++;
                else return false;
                if (a == count) return true;
            }
            return false;
        }
        private bool TestVertical(bool?[,] mas, int i, int j, int count, bool isGreen)
        {
            int a = 0;
            for (; i < mas.GetLength(0); i++)
            {
                if (isGreen == mas[i, j]) a++;
                else return false;
                if (a == count) return true;
            }
            return false;
        }
        private bool TestDiagonals(bool?[,] mas, int i, int j, int count, bool isGreen)
        {
            int a = 0;
            int i1 = i;
            int j1 = j;
            // левая диагональ
            while (i1 < mas.GetLength(0) && j1 >= 0)
            {
                if (mas[i1, j1] == isGreen) a++;
                else return false;
                if (a == count) return true;
                i1++;
                j1--;
            }
            a = 0;
            // правая диагональ
            while (i < mas.GetLength(0) && j < mas.GetLength(1))
            {
                if (mas[i, j] == isGreen) a++;
                else return false;
                if (a == count) return true;
                i++;
                j++;
            }
            return false;
        }
    }
}
