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
            (_, var cost) = this.FindShortestPathASstar(map, new Point(0, 0), new Point(map.GetLength(0)-1, map.GetLength(1)-1));
            return cost;
        }

        public int SolvePart2()
        {
            var map = this.ParseFromFile(inputLocation);
            var enlargedMap = this.duplicateMap(map, 5);
            (_, var cost) = this.FindShortestPathASstar(enlargedMap, new Point(0, 0), new Point(enlargedMap.GetLength(0) - 1, enlargedMap.GetLength(1) - 1));
            return cost;
        }

        private (List<Point> path, int cost) FindShortestPathASstar(int[,] map, Point start, Point target)
        {
            var rows = map.GetLength(0);
            var columns = map.GetLength(1);
            var spatialMap = new Dictionary<Point, (List<Point> points, int distance)>() { { start, (new List<Point> { start }, 0) } };
            var undiscoveredPoints = new PriorityQueue<Point>();
            undiscoveredPoints.Add(start, 0);

            while (undiscoveredPoints.TryPop(out var undiscoveredPoint))
            {
                //Stop if we've reached the target point
                if (undiscoveredPoint.Value.a == target.a && undiscoveredPoint.Value.b == target.b)
                {
                    break;
                }

                //Get the path to the currentpoint that we're going to expand
                (var visitedPoints, var distanceToPoint) = spatialMap[undiscoveredPoint.Value];

                //Discover new points
                var neighborPoints = new Point[] { 
                    new Point(undiscoveredPoint.Value.a - 1, undiscoveredPoint.Value.b),
                    new Point(undiscoveredPoint.Value.a + 1, undiscoveredPoint.Value.b),
                    new Point(undiscoveredPoint.Value.a, undiscoveredPoint.Value.b - 1),
                    new Point(undiscoveredPoint.Value.a, undiscoveredPoint.Value.b + 1)
                };

                foreach (var neighborPoint in neighborPoints)
                {
                    //Check if the point actually falls on the grid
                    if (neighborPoint.a >= 0 && neighborPoint.a < rows && neighborPoint.b >= 0 && neighborPoint.b < columns)
                    {
                        var distanceToNeighborPoint = distanceToPoint + map[neighborPoint.a, neighborPoint.b];
                        //If the spatial map does not contain the distance to the newly discovered point yet
                        //OR the distance to the newly discovered point is smaller than the path that was already discovered to that point
                        if (!spatialMap.TryGetValue(neighborPoint, out var currentDistance) || currentDistance.distance > distanceToNeighborPoint)
                        {
                            //Update the spatial map
                            var path = new List<Point>(visitedPoints);
                            path.Add(neighborPoint);
                            spatialMap[neighborPoint] = (visitedPoints, distanceToNeighborPoint);

                            //Add the newly discovered point to the priority queue such that new points can be discovered from that point
                            var priority = CalculateEstimatedCost(neighborPoint, distanceToNeighborPoint, target);
                            undiscoveredPoints.Add(neighborPoint, priority);
                        }
                    }
                }
            }

            return spatialMap[target];
        }

        private int CalculateEstimatedCost(Point point, int pathCost, Point target)
        {
            return pathCost + (target.a + target.b - point.a - point.b);
        }
        

        private (List<Point> path, int cost) FindShortestPathDijkstra(int[,] map, Point start, Point target)
        {
            var rows = map.GetLength(0);
            var columns = map.GetLength(1);
            var spatialMap = new Dictionary<Point, (List<Point> points, int distance)>() { { start, (new List<Point> { start }, 0) } };
            var undiscoveredPoints = new HashSet<Point> { start };

            while (undiscoveredPoints.Any())
            {
                var newDiscoveredPoints = new HashSet<Point>();
                //Discover new points
                foreach (var point in undiscoveredPoints)
                {
                    (var visitedPoints, var distanceToPoint) = spatialMap[point];


                    //Left
                    if (point.b - 1 >= 0)
                    {
                        var leftPoint = new Point(point.a, point.b-1);
                        var distanceToLeftPoint = distanceToPoint + map[leftPoint.a, leftPoint.b];
                        if (!spatialMap.TryGetValue(leftPoint, out var currentDistance) || currentDistance.distance > distanceToLeftPoint)
                        {
                            spatialMap[leftPoint] = (new List<Point>(visitedPoints), distanceToLeftPoint);
                            newDiscoveredPoints.Add(leftPoint);
                        }
                    }

                    //Right
                    if (point.b +1 < columns)
                    {
                        var rightPoint = new Point(point.a, point.b + 1);
                        var distanceToLeftPoint = distanceToPoint + map[rightPoint.a, rightPoint.b];
                        if (!spatialMap.TryGetValue(rightPoint, out var currentDistance) || currentDistance.distance > distanceToLeftPoint)
                        {
                            spatialMap[rightPoint] = (new List<Point>(visitedPoints), distanceToLeftPoint);
                            newDiscoveredPoints.Add(rightPoint);
                        }
                    }

                    //Top
                    if (point.a - 1 >= 0)
                    {
                        var topPoint = new Point(point.a - 1, point.b);
                        var distanceToLeftPoint = distanceToPoint + map[topPoint.a, topPoint.b];
                        if (!spatialMap.TryGetValue(topPoint, out var currentDistance) || currentDistance.distance > distanceToLeftPoint)
                        {
                            spatialMap[topPoint] = (new List<Point>(visitedPoints), distanceToLeftPoint);
                            newDiscoveredPoints.Add(topPoint);
                        }
                    }

                    //Bottom
                    if (point.a + 1 < rows)
                    {
                        var bottomPoint = new Point(point.a + 1, point.b);
                        var distanceToLeftPoint = distanceToPoint + map[bottomPoint.a, bottomPoint.b];
                        if (!spatialMap.TryGetValue(bottomPoint, out var currentDistance) || currentDistance.distance > distanceToLeftPoint)
                        {
                            spatialMap[bottomPoint] = (new List<Point>(visitedPoints), distanceToLeftPoint);
                            newDiscoveredPoints.Add(bottomPoint);
                        }
                    }

                }
                undiscoveredPoints = newDiscoveredPoints;
            }

            return spatialMap[target];
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
