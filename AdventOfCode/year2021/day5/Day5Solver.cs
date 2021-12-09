using AdventOfCode.shared.dataStructures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.day5
{
    public class Day5Solver
    {
        private const string inputLocation = "D:/CSharpProjects/AdventOfCode/AdventOfCode/AdventOfCode/year2021/day5/day5Input.txt";

        //Complexity of this solution went way overboard. Didn't realize in time that every line is horizontal, vertical or diagonal. 
        //Now it determines whether a point falls on any line in 2D-Euclidean space, which is a total overkill for this problem. Running the program makes the computer cry.

        public int SolvePart1()
        {
            var lines = ParseLinesFromFile(inputLocation);
            //Only use horizontal and vertical lines
            var horizontalOrVerticalLines = lines.Where(x => x.IsHorizontalLine() || x.IsVerticalLine()).ToList();

            //Calculate the size of the grid on which the lines fall
            var gridWidth = horizontalOrVerticalLines.Max(x => x.GetMaxXCoordinate());
            var gridHeight = horizontalOrVerticalLines.Max(x => x.GetMaxYCoordinate());

            //Now for each point on the grid, calculate whether the point falls on multiple lines
            var pointsOnMultipleLines = 0;
            for (int x=0; x<=gridWidth; x++)
            {
                for (int y=0; y<=gridHeight; y++)
                {
                    var point = new Vector2(x, y);
                    if (this.FallsOnMultipleLines(point, horizontalOrVerticalLines))
                    {
                        pointsOnMultipleLines++;
                    }
                }
            }

            return pointsOnMultipleLines;
        }

        public int SolvePart2()
        {
            var lines = ParseLinesFromFile(inputLocation);
            //Only use horizontal and vertical lines
            var horizontalOrVerticalLines = lines.Where(x => x.IsHorizontalLine() || x.IsVerticalLine() || x.IsDiagonalLine()).ToList();

            //Calculate the size of the grid on which the lines fall
            var gridWidth = horizontalOrVerticalLines.Max(x => x.GetMaxXCoordinate());
            var gridHeight = horizontalOrVerticalLines.Max(x => x.GetMaxYCoordinate());

            //Now for each point on the grid, calculate whether the point falls on multiple lines
            var pointsOnMultipleLines = 0;
            for (int x = 0; x <= gridWidth; x++)
            {
                for (int y = 0; y <= gridHeight; y++)
                {
                    var point = new Vector2(x, y);
                    if (this.FallsOnMultipleLines(point, horizontalOrVerticalLines))
                    {
                        pointsOnMultipleLines++;
                    }
                }
            }

            return pointsOnMultipleLines;
        }

        private bool FallsOnMultipleLines(Vector2 point, List<Line2> lines)
        {
            var lineCount = 0;
            foreach (var line in lines)
            {
                if (line.FallsOnLine(point))
                {
                    lineCount++;
                    if (lineCount > 1)
                    {
                        break;
                    }
                }
            }
            return lineCount > 1;
        }

        private List<Line2> ParseLinesFromFile(string fileLocation)
        {
            var result = new List<Line2>();
            var allText = System.IO.File.ReadAllText(fileLocation);

            string[] lines = allText.Split(
                new string[] { Environment.NewLine },
                StringSplitOptions.None
            );

            foreach (var line in lines)
            {
                var linePoints = line.Split(new string[] { "->" }, StringSplitOptions.None);
                var pointA = ParseFromString(linePoints[0]);
                var pointB = ParseFromString(linePoints[1]);
                result.Add(new Line2(pointA, pointB));
            }

            return result;
        }


        private Vector2 ParseFromString(string vec2String)
        {
            var numbers = vec2String.Split(',');
            var x = int.Parse(numbers[0]);
            var y = int.Parse(numbers[1]);
            return new Vector2(x, y);
        }

    }
}
