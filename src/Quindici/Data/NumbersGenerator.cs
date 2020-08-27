using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Quindici.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace G2048.Data
{
    enum Direction
    {
        Up,
        Down,
        Right,
        Left
    }

    public class NumbersGenerator
    {
        private static int MaxNumOfNumbers = 16;
        private static int MaxNumOfRow = 4;
        private static int MaxNumOfColumn = 4;
        private List<Number> NumbersList = null;
        public bool Done = false;

        public List<Number> GenerateNumbers()
        {
            if (NumbersList == null)
            {
                NumbersList = new List<Number>();

                Random rnd = new Random();

                var FirstItem = new Number();
                FirstItem.Row = rnd.Next(0, MaxNumOfRow - 1);
                FirstItem.Column = rnd.Next(0, MaxNumOfColumn - 1);

                FirstItem.number = GetNewNumber(rnd);
                FirstItem.backgroundColor = GetBackgroundColor(FirstItem.number);

                NumbersList.Add(FirstItem);

                var SecondItem = new Number();

                do
                {
                    SecondItem.Row = rnd.Next(0, MaxNumOfRow - 1);
                    SecondItem.Column = rnd.Next(0, MaxNumOfColumn - 1);
                }
                while (SecondItem.Row == FirstItem.Row && SecondItem.Column == FirstItem.Column);

                SecondItem.number = GetNewNumber(rnd);
                SecondItem.backgroundColor = GetBackgroundColor(SecondItem.number);

                NumbersList.Add(SecondItem);

                // Riempimento matrice
                int row = 0;
                int column = 0;
                int nextRow = 3;
                for (int index = 1; index <= MaxNumOfNumbers; index++)
                {
                    if (!((row == FirstItem.Row && column == FirstItem.Column) || (row == SecondItem.Row && column == SecondItem.Column)))
                    {
                        var item = new Number();

                        item.Row = row;
                        item.Column = column;
                        item.number = 0;
                        item.backgroundColor = GetBackgroundColor(item.number);

                        NumbersList.Add(item);
                    }

                    column += 1;
                    if (column > nextRow)
                    {
                        column = 0;
                        row += 1;
                    }
                }
            }

            return GetAllNumbers();
        }

        private string GetBackgroundColor(int value)
        {
            return "g2048BG_" + value.ToString();
        }

        private int GetNewNumber(Random rnd)
        {
            // Possibili valori: 2 e 4.
            // 80% probabilità per il 2, 20% per il 4.
            return rnd.Next(0, 100) < 80 ? 2 : 4;
        }

        private List<Number> GetAllNumbers()
        {
            List<Number> SortedList = new List<Number>();

            SortedList = NumbersList.OrderBy(n => n.Row).ThenBy(n => n.Column).ToList();

            return SortedList;
        }

        public List<Number> TryMoveNumber(int direzione)
        {
            switch (direzione)
            {
                case 1: // UP
                    // Per ogni colonna
                    for (int indexColumn = 0; indexColumn < MaxNumOfColumn; indexColumn++)
                    {
                        bool squashed = false;

                        // Prima avvicino tutte le celle
                        for (int indexRow = 1; indexRow < MaxNumOfRow - 1; indexRow++)
                        {
                            // Confronto la cella corrente con quella precedente
                            Number ThisNumber = NumbersList.Where(n => n.Row == indexRow && n.Column == indexColumn).First();
                            Number PreviousNumber = NumbersList.Where(n => n.Row == indexRow - 1 && n.Column == indexColumn).First();

                            if (ThisNumber.number != 0 && PreviousNumber.number == 0)
                            {
                                for (int indexRowToSlide = indexRow; indexRowToSlide < MaxNumOfRow; indexRowToSlide++)
                                {
                                    MoveNumber(indexRowToSlide, indexColumn, Direction.Up);
                                }
                                //indexRow = 0;
                            }
                        }

                        // Per ogni cella
                        for (int indexRow = 1; indexRow < MaxNumOfRow; indexRow++)
                        {
                            // Confronto la cella corrente con quella precedente
                            Number ThisNumber = NumbersList.Where(n => n.Row == indexRow && n.Column == indexColumn).First();
                            Number PreviousNumber = NumbersList.Where(n => n.Row == indexRow - 1 && n.Column == indexColumn).First();

                            if (ThisNumber.number != 0)
                            {
                                // Stesso numero -> Squash
                                if (ThisNumber.number == PreviousNumber.number)
                                {
                                    SquashNumber(indexRow, indexColumn, Direction.Up);
                                    squashed = true;
                                }
                                else if (PreviousNumber.number == 0)
                                {
                                    MoveNumber(indexRow, indexColumn, Direction.Up);
                                    if (!squashed)
                                        indexRow--;
                                    else
                                        squashed = false;
                                }
                            }
                        }
                    }
                    break;
            }
            //if (row > 0
            //    && NumbersList.Where(n => (n.Row == (row - 1) && n.Column == column && n.number == 0)).Any())
            //{
            //    MoveNumber(row, column, Direction.Up);
            //}   // DOWN
            //else if (row < 3
            //    && NumbersList.Where(n => (n.Row == (row + 1) && n.Column == column && n.number == 0)).Any())
            //{
            //    MoveNumber(row, column, Direction.Down);
            //}   // LEFT
            //else if (column > 0
            //    && NumbersList.Where(n => (n.Row == row && n.Column == (column - 1) && n.number == 0)).Any())
            //{
            //    MoveNumber(row, column, Direction.Left);
            //}   // RIGHT
            //else if (column < 3
            //    && NumbersList.Where(n => (n.Row == row && n.Column == (column + 1) && n.number == 0)).Any())
            //{
            //    MoveNumber(row, column, Direction.Right);
            //}

            return GetAllNumbers();
        }

        //private void MoveNumber(int row, int column, Direction direction)
        //{
        //    Number tmpFrom = NumbersList.Where(n => n.Row == row && n.Column == column).FirstOrDefault();
        //    Number tmpTo = new Number();
        //    Number tmp = new Number();

        //    switch (direction)
        //    {
        //        case Direction.Up:
        //            tmpTo = NumbersList.Where(n => n.Row == (row - 1) && n.Column == column).FirstOrDefault();
        //            break;

        //        case Direction.Down:
        //            tmpTo = NumbersList.Where(n => n.Row == (row + 1) && n.Column == column).FirstOrDefault();
        //            break;

        //        case Direction.Left:
        //            tmpTo = NumbersList.Where(n => n.Row == row && n.Column == (column - 1)).FirstOrDefault();
        //            break;

        //        case Direction.Right:
        //            tmpTo = NumbersList.Where(n => n.Row == row && n.Column == (column + 1)).FirstOrDefault();
        //            break;
        //    }

        //    SwapTiles(tmpFrom, tmpTo);

        //    this.Done = CheckSolution();
        //}

        private void SquashNumber(int row, int column, Direction direction)
        {
            Number tmpFrom = NumbersList.Where(n => n.Row == row && n.Column == column).FirstOrDefault();
            Number tmpTo = new Number();
            Number tmp = new Number();

            switch (direction)
            {
                case Direction.Up:
                    tmpTo = NumbersList.Where(n => n.Row == (row - 1) && n.Column == column).FirstOrDefault();
                    break;

                case Direction.Down:
                    tmpTo = NumbersList.Where(n => n.Row == (row + 1) && n.Column == column).FirstOrDefault();
                    break;

                case Direction.Left:
                    tmpTo = NumbersList.Where(n => n.Row == row && n.Column == (column - 1)).FirstOrDefault();
                    break;

                case Direction.Right:
                    tmpTo = NumbersList.Where(n => n.Row == row && n.Column == (column + 1)).FirstOrDefault();
                    break;
            }

            SquashTiles(tmpFrom, tmpTo);

            this.Done = CheckSolution();
        }

        private void MoveNumber(int row, int column, Direction direction)
        {
            Number tmpFrom = NumbersList.Where(n => n.Row == row && n.Column == column).FirstOrDefault();
            Number tmpTo = new Number();
            Number tmp = new Number();

            switch (direction)
            {
                case Direction.Up:
                    tmpTo = NumbersList.Where(n => n.Row == (row - 1) && n.Column == column).FirstOrDefault();
                    break;

                case Direction.Down:
                    tmpTo = NumbersList.Where(n => n.Row == (row + 1) && n.Column == column).FirstOrDefault();
                    break;

                case Direction.Left:
                    tmpTo = NumbersList.Where(n => n.Row == row && n.Column == (column - 1)).FirstOrDefault();
                    break;

                case Direction.Right:
                    tmpTo = NumbersList.Where(n => n.Row == row && n.Column == (column + 1)).FirstOrDefault();
                    break;
            }

            MoveTiles(tmpFrom, tmpTo);

            this.Done = CheckSolution();
        }

        /// <summary>
        /// Checks is the puzzle has been successfully solved
        /// </summary>
        /// <returns></returns>
        private bool CheckSolution()
        {
            return false;
            //int num = 1;
            //int counter = 1;

            //for (int row = 0; row < 4; row++)
            //{
            //    for (int column = 0; column < 4; column++)
            //    {
            //        num = NumbersList.Where(n => n.Row == row && n.Column == column).FirstOrDefault().number;
            //        if (num != counter++)
            //            return false;

            //        if (counter > 15)
            //            counter = 0;
            //    }
            //}

            return true;
        }

        public List<Number> Restart()
        {
            Done = false;
            NumbersList = null;
            return GenerateNumbers();
        }

        private void SquashTiles(Number From, Number To)
        {
            int value = From.number;
            string background = From.backgroundColor;
            From.number = 0;
            To.number *= 2;
            From.backgroundColor = GetBackgroundColor(From.number);
            To.backgroundColor = GetBackgroundColor(To.number);
        }

        private void MoveTiles(Number From, Number To)
        {
            int value = From.number;
            string background = From.backgroundColor;
            To.number = From.number;
            From.number = 0;
            From.backgroundColor = GetBackgroundColor(From.number);
            To.backgroundColor = GetBackgroundColor(To.number);
        }
    }
}

