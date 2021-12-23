using AdventOfCode.year2021.day22;
using System;
using System.Collections.Generic;
using System.Linq;

public class Day22Solver
{
    private const string inputLocation = "D:/CSharpProjects/AdventOfCode/AdventOfCode/AdventOfCode/year2021/day22/day22Input.txt";

    public long SolvePart1()
    {
        var boxCommands = this.ParseFromFile(inputLocation);
        var resultingBoxes = this.CombineBoxes(boxCommands);
        return resultingBoxes.Sum(x => x.GetSize((-50, 51), (-50, 51), (-50, 51)));     //51 because the range is exclusive
    }

    public long SolvePart2()
    {
        var boxCommands = this.ParseFromFile(inputLocation);
        var resultingBoxes = this.CombineBoxes(boxCommands);
        return resultingBoxes.Sum(x => x.GetSize());
    }

    private List<Box3D> CombineBoxes(List<(Box3D box, bool turnOn)> boxCommands)
    {
        var boxSplits = new List<Box3D>();
        boxSplits.Add(boxCommands[0].box);
        for (int i=1; i<boxCommands.Count; i++)
        {
            var boxCommand = boxCommands[i];
            if (boxCommand.turnOn)
            {
                boxSplits = boxSplits.SelectMany(x => x.RemoveFromBox(boxCommand.box)).ToList();
                boxSplits.Add(boxCommand.box);
            }
            else
            {
                boxSplits = boxSplits.SelectMany(x => x.RemoveFromBox(boxCommand.box)).ToList();
            }
        }

        return boxSplits;
    }


    private List<(Box3D box, bool turnOn)> ParseFromFile(string fileLocation)
    {
        var allText = System.IO.File.ReadAllText(fileLocation);

        string[] lines = allText.Split(
            new string[] { Environment.NewLine },
            StringSplitOptions.None
        );

        var result = new List<(Box3D box, bool turnOn)>();
        foreach (var line in lines)
        {
            var splitLine = line.Split(' ');
            var turnOn = splitLine[0] == "on";
            var coordinateRanges = splitLine[1].Split(',');
            var xRange = coordinateRanges[0].Split('=')[1];
            var yRange = coordinateRanges[1].Split('=')[1];
            var zRange = coordinateRanges[2].Split('=')[1];

            var xFromTo = xRange.Split(new string[] { ".." }, StringSplitOptions.None);
            var yFromTo = yRange.Split(new string[] { ".." }, StringSplitOptions.None);
            var zFromTo = zRange.Split(new string[] { ".." }, StringSplitOptions.None);

            //The range is exclusive, so +1
            var rebootStep = new Box3D((int.Parse(xFromTo[0]), int.Parse(xFromTo[1]) + 1), (int.Parse(yFromTo[0]), int.Parse(yFromTo[1]) + 1), (int.Parse(zFromTo[0]), int.Parse(zFromTo[1]) + 1));
            result.Add((rebootStep, turnOn));
        }

        return result;
    }

}

