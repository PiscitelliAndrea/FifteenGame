using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Quindici.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace Quindici.Data
{
    enum Direction
    {
        Up,
        Down,
        Right,
        Left
    }

    public class TilesGenerator
    {
        private static int MaxNumOfTiles = 16;
        private List<Tile> TilesList = null;
        public bool Done = false;

        public List<Tile> GenerateTiles()
        {
            int row = 0;
            int column = 0;
            int nextRow = 3;

            if (TilesList == null)
            {
                Random rnd = new Random();
                int nextNum = 0;
                TilesList = new List<Tile>();

                for (int index = 1; index < MaxNumOfTiles; index++)
                {
                    var item = new Tile();
                    item.Row = row;
                    item.Column = column;

                    do
                    {
                        nextNum = rnd.Next(1, MaxNumOfTiles);
                    }
                    while (TilesList.Where(n => n.number == nextNum).Any());

                    item.number = nextNum;
                    item.backgroundColor = ((nextNum % 2) == 0) ? "darkBackground" : "lightBackground";

                    TilesList.Add(item);

                    column += 1;
                    if (column > nextRow)
                    {
                        column = 0;
                        row += 1;
                    }
                }

                var emptyTile = new Tile();
                emptyTile.Row = emptyTile.Column = 3;
                emptyTile.number = 0;
                emptyTile.backgroundColor = "blackBackground";

                TilesList.Add(emptyTile);

            }

            return GetAllTiles();
        }

        private List<Tile> GetAllTiles()
        {
            List<Tile> SortedList = new List<Tile>();

            SortedList = TilesList.OrderBy(n => n.Row).ThenBy(n => n.Column).ToList();

            return SortedList;
        }

        public List<Tile> TryMoveTile(int row, int column)
        {
            // UP
            if (row > 0
                && TilesList.Where(n => (n.Row == (row - 1) && n.Column == column && n.number == 0)).Any())
            {
                MoveTile(row, column, Direction.Up);
            }   // DOWN
            else if (row < 3
                && TilesList.Where(n => (n.Row == (row + 1) && n.Column == column && n.number == 0)).Any())
            {
                MoveTile(row, column, Direction.Down);
            }   // LEFT
            else if (column > 0
                && TilesList.Where(n => (n.Row == row && n.Column == (column - 1) && n.number == 0)).Any())
            {
                MoveTile(row, column, Direction.Left);
            }   // RIGHT
            else if (column < 3
                && TilesList.Where(n => (n.Row == row && n.Column == (column + 1) && n.number == 0)).Any())
            {
                MoveTile(row, column, Direction.Right);
            }

            return GetAllTiles();
        }

        private void MoveTile(int row, int column, Direction direction)
        {
            Tile tmpFrom = TilesList.Where(n => n.Row == row && n.Column == column).FirstOrDefault();
            Tile tmpTo = new Tile();
            Tile tmp = new Tile();

            switch (direction)
            {
                case Direction.Up:
                    tmpTo = TilesList.Where(n => n.Row == (row - 1) && n.Column == column).FirstOrDefault();
                    break;

                case Direction.Down:
                    tmpTo = TilesList.Where(n => n.Row == (row + 1) && n.Column == column).FirstOrDefault();
                    break;

                case Direction.Left:
                    tmpTo = TilesList.Where(n => n.Row == row && n.Column == (column - 1)).FirstOrDefault();
                    break;

                case Direction.Right:
                    tmpTo = TilesList.Where(n => n.Row == row && n.Column == (column + 1)).FirstOrDefault();
                    break;
            }

            SwapTiles(tmpFrom, tmpTo);

            this.Done = CheckSolution();
        }

        /// <summary>
        /// Checks is the puzzle has been successfully solved
        /// </summary>
        /// <returns></returns>
        private bool CheckSolution()
        {
            int num = 1;
            int counter = 1;

            for (int row = 0; row < 4; row++)
            {
                for (int column = 0; column < 4; column++)
                {
                    num = TilesList.Where(n => n.Row == row && n.Column == column).FirstOrDefault().number;
                    if (num != counter++)
                        return false;

                    if (counter > 15)
                        counter = 0;
                }
            }

            return true;
        }

        public List<Tile> Restart()
        {
            Done = false;
            TilesList = null;
            return GenerateTiles();
        }

        private void SwapTiles(Tile From, Tile To)
        {
            int value = From.number;
            string background = From.backgroundColor;
            From.number = 0;
            From.backgroundColor = "blackBackground";
            To.number = value;
            To.backgroundColor = background;
        }
    }
}

