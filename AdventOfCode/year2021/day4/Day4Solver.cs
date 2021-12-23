using AdventOfCode.day4;
using System;
using System.Collections.Generic;

public class Day4Solver
{
    private const string bingoBoardsTextLocation = "D:/CSharpProjects/AdventOfCode/AdventOfCode/AdventOfCode/year2021/day4/day4InputBoards.txt";
    private const string bingoBoardNumbersLocation = "D:/CSharpProjects/AdventOfCode/AdventOfCode/AdventOfCode/year2021/day4/day4InputNumbers.txt";

    public int SolvePart1()
    {
        var boards = this.ParseBoardsFromFile(bingoBoardsTextLocation);
        var numbers = this.ParseNumbersFromFile(bingoBoardNumbersLocation);

        var winningBoard = this.DetermineWinner(boards, numbers);
        return winningBoard.CalculateScore();
    }

    public int SolvePart2()
    {
        var boards = this.ParseBoardsFromFile(bingoBoardsTextLocation);
        var numbers = this.ParseNumbersFromFile(bingoBoardNumbersLocation);

        var superLoserBoard = this.DetermineTheMegaLoser(boards, numbers);
        return superLoserBoard.CalculateScore();
    }

    private BingoBoard DetermineTheMegaLoser(List<BingoBoard> boards, int[] numbers)
    {
        BingoBoard lastToHaveBingo = null;
        foreach (var number in numbers)
        {
            foreach (var board in boards)
            {
                board.MarkNumber(number);
                if (board.HasBingo())
                {
                    lastToHaveBingo = board;

                    //Check if all boards have bingo now
                    if (boards.TrueForAll(x => x.HasBingo()))
                    {
                        return lastToHaveBingo;
                    }
                }
            }
        }

        throw new Exception("Boo! No mega loser could be determined");
    }

    private BingoBoard DetermineWinner(List<BingoBoard> boards, int[] numbers)
    {
        foreach (var number in numbers)
        {
            foreach (var board in boards)
            {
                board.MarkNumber(number);
                if(board.HasBingo())
                {
                    return board;
                }
            }
        }

        throw new Exception("Boo! No winner could be determined");
    }


    private List<BingoBoard> ParseBoardsFromFile(string fileLocation)
    {
        var allBingoBoardsText = System.IO.File.ReadAllText(fileLocation);

        //Split the boardsText on empty line
        string[] boardsText = allBingoBoardsText.Split(new string[] { "\r\n\r\n" },
                            StringSplitOptions.RemoveEmptyEntries);

        //Create boards
        var bingoBoards = new List<BingoBoard>();
        foreach (var boardText in boardsText)
        {
            bingoBoards.Add(BingoBoard.ParseFrom(boardText));
        }

        return bingoBoards;
    }


    private int[] ParseNumbersFromFile(string fileLocation)
    {
        var bingoNumbersText = System.IO.File.ReadAllText(fileLocation);

        //Get the numbers from the text file
        var bingoNumbersAsString = bingoNumbersText.Split(',');
        var bingoNumbers = new int[bingoNumbersAsString.Length];
        for (int i = 0; i < bingoNumbersAsString.Length; i++)
        {
            bingoNumbers[i] = int.Parse(bingoNumbersAsString[i]);
        }

        return bingoNumbers;
    }

}
