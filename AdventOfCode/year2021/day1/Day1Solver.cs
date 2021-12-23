using System;
using System.Collections.Generic;
using System.Linq;

public class Day1Solver
{

    private const string inputLocation = "D:/CSharpProjects/AdventOfCode/AdventOfCode/AdventOfCode/year2021/day1/day1Input.txt";

    public int SolvePart1()
    {
        var input = this.ParseFromFile(inputLocation);
        return CountElementsLargerThanPrevious(input);
    }

    public int SolvePart2()
    {
        var input = this.ParseFromFile(inputLocation);
        var slidingWindowsSums = this.GenerateSlidingWindowSumList(input, 3);
        return this.CountElementsLargerThanPrevious(slidingWindowsSums);
    }

    private int CountElementsLargerThanPrevious(List<int> items)
    {
        int result = 0;

        int current = items[0];
        for (int i=1; i<items.Count; i++)
        {
            if (items[i] > current)
            {
                result++;
            }
            current = items[i];
        }

        return result;
    }


    private List<int> GenerateSlidingWindowSumList(List<int> items, int slidingWindowSize)
    {
        var result = new List<int>();

        for (int i=0; i<items.Count- (slidingWindowSize-1); i++)
        {
            var slidingWindowSum = items.GetRange(i, slidingWindowSize).Sum();
            result.Add(slidingWindowSum);
        }

        return result;
    }


    private List<int> ParseFromFile(string fileLocation)
    {
        var text = System.IO.File.ReadAllText(fileLocation);
        string[] lines = text.Split(
            new string[] { Environment.NewLine },
            StringSplitOptions.None
        );

        return lines.AsEnumerable().Select(x => int.Parse(x)).ToList();
    }

}
