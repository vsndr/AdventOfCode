using AdventOfCode.shared.dataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.year2021.day11
{
    public class Day13Solver
    {
        private const string inputLocation = "D:/CSharpProjects/AdventOfCode/AdventOfCode/AdventOfCode/year2021/day13/day13Input.txt";

        public int SolvePart1()
        {
            (var points, var foldingLines) = ParseFromFile(inputLocation);
            return Fold(points, foldingLines.First()).Count();
        }

        public string SolvePart2()
        {
            (var points, var foldingLines) = ParseFromFile(inputLocation);
            foldingLines.ForEach(x => points = Fold(points, x).ToList());
            return GetVisualisation(points);
        }

        private IEnumerable<Point> Fold(List<Point> points, Point foldingLine)
        {
            if (foldingLine.x != 0)
            {
                return FoldVertically(points, foldingLine.x);
            }
            else
            {
                return FoldHorizontally(points, foldingLine.y);
            }
        }

        private IEnumerable<Point> FoldHorizontally(List<Point> points, int axisValue)
        {
            var result = new List<Point>();
            foreach (var point in points)
            {
                if (point.y > axisValue)
                {
                    result.Add(new Point(point.x, axisValue - (point.y - axisValue)));
                }
                else
                {
                    result.Add(point);
                }
            }
            return result.Distinct();
        }

        private IEnumerable<Point> FoldVertically(List<Point> points, int axisValue)
        {
            var result = new List<Point>();
            foreach (var point in points)
            {
                if (point.x > axisValue)
                {
                    result.Add(new Point(axisValue - (point.x - axisValue), point.y));
                }
                else
                {
                    result.Add(point);
                }
            }
            return result.Distinct();
        }

        private string GetVisualisation(List<Point> points)
        {
            var rows = points.Max(p => p.y) + 1;
            var columns = points.Max(p => p.x) + 1;
            var lines = new StringBuilder[rows];
            for (int i=0; i<rows; i++)
            {
                var sb = new StringBuilder();
                sb.Append(new string('.', columns));
                lines[i] = sb;
            }

            foreach (var point in points)
            {
                var line = lines[point.y];
                line[point.x] = '#';
            }

            return string.Join("\n", lines.Select(x => x.ToString()));
        }

        private (List<Point> points, List<Point> foldingLines) ParseFromFile(string fileLocation)
        {
            var allText = System.IO.File.ReadAllText(fileLocation);

            string[] lines = allText.Split(
                new string[] { Environment.NewLine },
                StringSplitOptions.None
            );

            var points = new List<Point>();
            var foldingLines = new List<Point>();

            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                if (line.StartsWith("fold along"))
                {
                    var cleanedLine = line.Replace("fold along", string.Empty);
                    var foldingLineData = cleanedLine.Split('=');
                    var axis = foldingLineData[0];
                    var value = int.Parse(foldingLineData[1]);
                    if (axis.Trim() == "x")
                    {
                        foldingLines.Add(new Point(value, 0));
                    }
                    else
                    {
                        foldingLines.Add(new Point(0, value));
                    }
                }
                else
                {
                    var dotData = line.Split(',');
                    var dot = new Point(int.Parse(dotData[0]), int.Parse(dotData[1]));
                    points.Add(dot);
                }
            }

            return (points, foldingLines);
        }

    }
}
