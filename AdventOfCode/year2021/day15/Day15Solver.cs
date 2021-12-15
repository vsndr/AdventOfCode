using AdventOfCode.shared.dataStructures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.year2021.day11
{
    public class Day15Solver
    {
        private const string inputLocation = "D:/CSharpProjects/AdventOfCode/AdventOfCode/AdventOfCode/year2021/day15/day15Input.txt";

        public int SolvePart1()
        {
            var map = this.ParseFromFile(inputLocation);
            (_, var cost) = this.FindShortestPath(map, new Point(0, 0), new Point(map.GetLength(0)-1, map.GetLength(1)-1));
            return cost;
        }

        public int SolvePart2()
        {
            var map = this.ParseFromFile(inputLocation);
            var enlargedMap = this.duplicateMap(map, 5);
            (_, var cost) = this.FindShortestPath(enlargedMap, new Point(0, 0), new Point(enlargedMap.GetLength(0) - 1, enlargedMap.GetLength(1) - 1));
            return cost;
        }


        private (List<Point> path, int cost) FindShortestPath(int[,] map, Point start, Point target)
        {
            var rows = map.GetLength(0);
            var columns = map.GetLength(1);
            var spatialSet = new Dictionary<Point, (List<Point> points, int distance)>() { { start, (new List<Point> { start }, 0) } };
            var undiscoveredPoints = new HashSet<Point> { start };

            while (undiscoveredPoints.Any())
            {
                var newDiscoveredPoints = new HashSet<Point>();
                //Discover new points
                foreach (var point in undiscoveredPoints)
                {
                    (var visitedPoints, var distanceToPoint) = spatialSet[point];

                    //Left
                    if (point.b - 1 >= 0)
                    {
                        var leftPoint = new Point(point.a, point.b-1);
                        var distanceToLeftPoint = distanceToPoint + map[leftPoint.a, leftPoint.b];
                        if (!spatialSet.TryGetValue(leftPoint, out var currentDistance) || currentDistance.distance > distanceToLeftPoint)
                        {
                            spatialSet[leftPoint] = (new List<Point>(visitedPoints), distanceToLeftPoint);
                            newDiscoveredPoints.Add(leftPoint);
                        }
                    }

                    //Right
                    if (point.b +1 < columns)
                    {
                        var rightPoint = new Point(point.a, point.b + 1);
                        var distanceToLeftPoint = distanceToPoint + map[rightPoint.a, rightPoint.b];
                        if (!spatialSet.TryGetValue(rightPoint, out var currentDistance) || currentDistance.distance > distanceToLeftPoint)
                        {
                            spatialSet[rightPoint] = (new List<Point>(visitedPoints), distanceToLeftPoint);
                            newDiscoveredPoints.Add(rightPoint);
                        }
                    }

                    //Top
                    if (point.a - 1 >= 0)
                    {
                        var topPoint = new Point(point.a - 1, point.b);
                        var distanceToLeftPoint = distanceToPoint + map[topPoint.a, topPoint.b];
                        if (!spatialSet.TryGetValue(topPoint, out var currentDistance) || currentDistance.distance > distanceToLeftPoint)
                        {
                            spatialSet[topPoint] = (new List<Point>(visitedPoints), distanceToLeftPoint);
                            newDiscoveredPoints.Add(topPoint);
                        }
                    }

                    //Bottom
                    if (point.a + 1 < rows)
                    {
                        var bottomPoint = new Point(point.a + 1, point.b);
                        var distanceToLeftPoint = distanceToPoint + map[bottomPoint.a, bottomPoint.b];
                        if (!spatialSet.TryGetValue(bottomPoint, out var currentDistance) || currentDistance.distance > distanceToLeftPoint)
                        {
                            spatialSet[bottomPoint] = (new List<Point>(visitedPoints), distanceToLeftPoint);
                            newDiscoveredPoints.Add(bottomPoint);
                        }
                    }

                }
                undiscoveredPoints = newDiscoveredPoints;
            }

            return spatialSet[target];
        }

        private int[,] duplicateMap(int[,] map, int duplicationCount)
        {
            var rows = map.GetLength(0);
            var columns = map.GetLength(1);
            var result = new int[duplicationCount * rows, duplicationCount * columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    for (int r = 0; r < duplicationCount; r++)
                    {
                        for (int c =0; c < duplicationCount; c++)
                        {
                            var cost = map[i, j] + r + c;
                            if (cost > 9)
                            {
                                cost -= 9;
                            }
                            result[i + r * rows, j + c * columns] = cost;
                        }
                    }
                }
            }
            return result;
        }


        private int[,] ParseFromFile(string fileLocation)
        {
            var allText = System.IO.File.ReadAllText(fileLocation);

            string[] lines = allText.Split(
                new string[] { Environment.NewLine },
                StringSplitOptions.None
            );

            var rows = lines.Length;
            var columns = lines[0].Length;
            var result = new int[rows, columns];
            for(int i=0; i< rows; i++)
            {
                for (int j=0; j< columns; j++)
                {
                    result[i, j] = (int) char.GetNumericValue(lines[i][j]);
                }
            }

            return result;
        }

    }
}
