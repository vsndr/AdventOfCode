using AdventOfCode.shared.dataStructures;
using System;
using System.Collections.Generic;
using System.Linq;

public class Day11Solver
{
    private const string inputLocation = "D:/CSharpProjects/AdventOfCode/AdventOfCode/AdventOfCode/year2021/day11/day11Input.txt";

    public int SolvePart1()
    {
        var matrix = ParseFromFile(inputLocation);
        return CountFlashes(matrix, 100);
    }

    public int SolvePart2()
    {
        var matrix = ParseFromFile(inputLocation);
        return FindSynchronizedFlashCycle(matrix);
    }

    private int CountFlashes(Matrix2D matrix, int numberOfCycles)
    {
        var result = 0;
        for (int i = 0; i < numberOfCycles; i++)
        {
            result += CountFlashersAfterCycle(matrix);
        }
        return result;
    }

    private int FindSynchronizedFlashCycle(Matrix2D matrix)
    {
        var cycleNumber = 0;
        while (true)
        {
            var numberOfFlashers = CountFlashersAfterCycle(matrix);
            cycleNumber++;

            if (numberOfFlashers == matrix.GetSize())
            {
                break;
            }
        }
        return cycleNumber;
    }

    private int CountFlashersAfterCycle(Matrix2D matrix)
    {
        //Increment everything by 1
        matrix++;

        //Create a set for each octopus that's flashing in this cycle
        var allFlashers = new List<Point>();
        //Get the indices of any octopus that's going to flash
        var currentFlashers = this.GetFlashers(matrix);
        while (currentFlashers.Any())
        {
            foreach (var flasher in currentFlashers)
            {
                //Increment every neighbor of the flashing octopus by 1
                var slice = matrix[$"{flasher.a - 1}:{flasher.a + 1},{flasher.b - 1}:{flasher.b + 1}"];
                slice++;
            }

            allFlashers.AddRange(currentFlashers);
            currentFlashers = this.GetFlashers(matrix).Except(allFlashers).ToList();
        }

        //Reset every flasher to 0
        foreach (var flasher in allFlashers)
        {
            matrix[flasher.a, flasher.b] = 0;
        }

        return allFlashers.Count();
    }

    private List<Point> GetFlashers(Matrix2D matrix)
    {
        return matrix.Where(x => x > 9).ToList();
    }

    private Matrix2D ParseFromFile(string fileLocation)
    {
        var allText = System.IO.File.ReadAllText(fileLocation);

        string[] lines = allText.Split(
            new string[] { Environment.NewLine },
            StringSplitOptions.None
        );

        var matrix = new int[lines.Length, lines[0].Length];
        for (int i=0; i<lines.Length; i++)
        {
            var currentLine = lines[i];
            for (int j=0; j<currentLine.Length; j++)
            {
                matrix[i, j] = (int) char.GetNumericValue(currentLine[j]);
            }
        }

        return new Matrix2D(matrix);
    }

}