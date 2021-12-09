using AdventOfCode.day4.bingoGame;
using System;

namespace AdventOfCode.day4
{
    public class BingoBoard
    {
        private BingoTile[,] board;
        private int lastNumberMarked;

        public BingoBoard(int[,] tiles)
        {
            this.board = CreateNewBoardFrom(tiles);
        }

        private BingoTile[,] CreateNewBoardFrom(int[,] tiles)
        {
            var boardHeight = tiles.GetLength(0);
            var boardWidth = tiles.GetLength(1);
            var result = new BingoTile[boardHeight, boardWidth];
            for (int i=0; i< boardHeight; i++)
            {
                for (int j=0; j< boardWidth; j++)
                {
                    result[i, j] = new BingoTile(tiles[i,j]);
                }
            }

            return result;
        }

        public void MarkNumber(int number)
        {
            var boardHeight = this.board.GetLength(0);
            var boardWidth = this.board.GetLength(1);
            for (int i = 0; i < boardHeight; i++)
            {
                for (int j = 0; j < boardWidth; j++)
                {
                    if (this.board[i,j].GetNumber() == number)
                    {
                        this.board[i, j].MarkTile();
                        this.lastNumberMarked = number;
                    }
                }
            }
        }

        public bool HasBingo()
        {
            var boardHeight = this.board.GetLength(0);
            var boardWidth = this.board.GetLength(1);
            //Check rows
            for (int i = 0; i < boardHeight; i++)
            {
                var tilesMarkedAtRow = 0;
                for (int j = 0; j < boardWidth; j++)
                {
                    if (this.board[i, j].IsMarked())
                    {
                        tilesMarkedAtRow++;
                    }
                }

                if (tilesMarkedAtRow == boardWidth)
                {
                    return true;
                }
            }

            //Check columns
            for (int j = 0; j < boardWidth; j++)
            {
                var tilesMarkedAtColumn = 0;
                for (int i = 0; i < boardHeight; i++)
                {
                    if (this.board[i, j].IsMarked())
                    {
                        tilesMarkedAtColumn++;
                    }
                }

                if (tilesMarkedAtColumn == boardHeight)
                {
                    return true;
                }
            }

            return false;
        }

        public int CalculateScore()
        {
            //Get the sum of all unmarked numbers
            var unmarkedNumbersSum = 0;
            var boardHeight = this.board.GetLength(0);
            var boardWidth = this.board.GetLength(1);
            for (int i = 0; i < boardHeight; i++)
            {
                for (int j = 0; j < boardWidth; j++)
                {
                    var currentTile = this.board[i, j];
                    if (!currentTile.IsMarked())
                    {
                        unmarkedNumbersSum += currentTile.GetNumber();
                    }
                }
            }

            //Multiply by the last number that was called before the game was won
            return unmarkedNumbersSum * this.lastNumberMarked;
        }

        public static BingoBoard ParseFrom(string bingoBoardText)
        {
            string[] rows = bingoBoardText.Split(
                new string[] { Environment.NewLine },
                StringSplitOptions.None
            );

            var rowCount = rows.Length;
            var colCount = rows[0].Split(new char[0], StringSplitOptions.RemoveEmptyEntries).Length;
            var tiles = new int[rowCount, colCount];

            for (int i=0; i<rowCount; i++)
            {
                var currentRow = rows[i];
                var rowValues = currentRow.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                for (int j=0; j<rowValues.Length; j++)
                {
                    tiles[i, j] = int.Parse(rowValues[j]);
                }
            }

            return new BingoBoard(tiles);
        }

    }
}
