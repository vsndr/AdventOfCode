using System;
using System.Collections.Generic;
using System.Linq;

public class Day18Solver
{
    private const string inputLocation = "D:/CSharpProjects/AdventOfCode/AdventOfCode/AdventOfCode/year2021/day18/day18Input.txt";

    public int SolvePart1()
    {
        var lines = this.ParseFromFile(inputLocation);
        var snailNumbers = lines.Select(x => this.ToSnailNumber(x)).ToList();
        var additionResult = snailNumbers.Aggregate((first, second) => first = this.AddAndReduce(first, second));
        return this.GetMagnitude(additionResult);
    }

    public int SolvePart2()
    {
        var lines = this.ParseFromFile(inputLocation);
        var snailNumbers = lines.Select(x => this.ToSnailNumber(x)).ToList();
        var biggestMagnitude = 0;
        while (snailNumbers.Count > 1)
        {
            var toCompare = snailNumbers[0];
            for (int i = 1; i < snailNumbers.Count; i++)
            {
                var additionResult = this.AddAndReduce(toCompare, snailNumbers[i]);
                biggestMagnitude = Math.Max(biggestMagnitude, this.GetMagnitude(additionResult));

                var additionResult2 = this.AddAndReduce(snailNumbers[i], toCompare);
                biggestMagnitude = Math.Max(biggestMagnitude, this.GetMagnitude(additionResult2));
            }
            snailNumbers.RemoveAt(0);
        }

        return biggestMagnitude;
    }

    private int GetMagnitude(List<(int number, int depth)> snailNumber)
    {
        var snailNumberCopy = new List<(int number, int depth)>(snailNumber);
        var maxDepth = snailNumberCopy.Max(x => x.depth);
        for (int i=maxDepth; i>=0; i--)
        {
            int? lastNumber = null;
            for (int j=snailNumberCopy.Count-1; j>=0 ; j--)
            {
                var current = snailNumberCopy[j];
                if (current.depth == i)
                {
                    if (!lastNumber.HasValue)
                    {
                        lastNumber = current.number;
                    }
                    else
                    {
                        snailNumberCopy[j] = (current.number * 3 + lastNumber.Value * 2, --current.depth);
                        snailNumberCopy.RemoveAt(j+1);
                        lastNumber = null;
                    }
                }
            }
        }

        return snailNumberCopy.Single().number;
    }

    private void PrintSnailNumber(List<(int number, int depth)> snailNumber)
    {
        List<(string pair, int depth)> snailNumberCopy = snailNumber.Select(x => (x.number.ToString(), x.depth)).ToList();
        var maxDepth = snailNumberCopy.Max(x => x.depth);
        for (int i = maxDepth; i >= 0; i--)
        {
            string lastPair = null;
            for (int j = snailNumberCopy.Count - 1; j >= 0; j--)
            {
                var current = snailNumberCopy[j];
                if (current.depth == i)
                {
                    if (string.IsNullOrEmpty(lastPair))
                    {
                        lastPair = current.pair;
                    }
                    else
                    {
                        snailNumberCopy[j] = ($"[{current.pair},{lastPair}]", --current.depth);
                        snailNumberCopy.RemoveAt(j + 1);
                        lastPair = null;
                    }
                }
            }
        }
        var result = snailNumberCopy.Single().pair;
        Console.WriteLine(result);
    }

    private void Reduce(List<(int number, int depth)> snailNumber)
    {
        var needsExplosion = true;
        while (needsExplosion)
        {
            while (TryExplode(snailNumber)) ;
            needsExplosion = TrySplit(snailNumber);
        }
    }

    private bool TryExplode(List<(int number, int depth)> snailNumber)
    {
        var lastDepth = -1;
        for (int i=0; i<snailNumber.Count; i++)
        {
            var current = snailNumber[i];
            if (current.depth == lastDepth && current.depth > 4)
            {
                //Add the number at index i to i+1 if exists
                if (i+1 < snailNumber.Count)
                {
                    var toIncrement = snailNumber[i + 1];
                    snailNumber[i + 1] = (toIncrement.number + snailNumber[i].number, toIncrement.depth);
                }
                snailNumber.RemoveAt(i);

                //Add the number at i-1 to i-2 if exists
                if (i - 2 >= 0)
                {
                    var toIncrement = snailNumber[i - 2];
                    snailNumber[i - 2] = (toIncrement.number + snailNumber[i - 1].number, toIncrement.depth);
                }

                snailNumber[i - 1] = (0, --current.depth);

                return true;
            }

            lastDepth = current.depth;
        }

        return false;
    }

    private bool TrySplit(List<(int number, int depth)> snailNumber)
    {
        for (int i=0; i< snailNumber.Count; i++)
        {
            var current = snailNumber[i];
            if (current.number >= 10)
            {
                var floor = (int) Math.Floor((double)current.number / 2);
                snailNumber[i] = (floor, current.depth + 1);
                var ceiling = (int)Math.Ceiling((double)current.number / 2);
                snailNumber.Insert(i+1, (ceiling, current.depth + 1));
                return true;
            }
        }
        return false;
    }

    private List<(int number, int depth)> AddAndReduce(List<(int number, int depth)> first, List<(int number, int depth)> second)
    {
        var addResult = this.Add(first, second);
        this.Reduce(addResult);
        return addResult;
    }

    private List<(int number, int depth)> Add(List<(int number, int depth)> first, List<(int number, int depth)> second)
    {
        var addResult = new List<(int number, int depth)>(first);
        addResult.AddRange(second);
        return addResult.Select(x => (x.number, x.depth + 1)).ToList();
    }

    private List<(int number, int depth)> ToSnailNumber(string line)
    {
        var result = new List<(int number, int depth)>();
        var depthStack = new Stack<char>();
        foreach (var c in line)
        {
            if (c == '[')
            {
                depthStack.Push(c);
            }
            else if (c == ']')
            {
                depthStack.Pop();
            }
            else if (c == ',')
            {
                continue;
            }
            else
            {
                result.Add(((int) char.GetNumericValue(c), depthStack.Count));
            }
        }

        return result;
    }

    private List<string> ParseFromFile(string fileLocation)
    {
        var allText = System.IO.File.ReadAllText(fileLocation);

        string[] lines = allText.Split(
            new string[] { Environment.NewLine },
            StringSplitOptions.None
        );

        return lines.ToList();
    }

}
