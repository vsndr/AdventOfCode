using AdventOfCode.shared.dataStructures;
using System.Collections.Generic;

namespace AdventOfCode.year2021.day15
{
    public class Path
    {
        private readonly int[,] map;
        private int totalCost = 0;
        private List<Point> visitedPoints = new List<Point>();

        public Path(int[,] map)
        {
            this.map = map;
        }

        public Path Clone()
        {
            var clone = new Path(this.GetMap());
            var points = this.GetVisitedPoints();
            foreach (var point in points)
            {
                clone.AddPoint(new Point(point.a, point.b));
            }
            return clone;
        }

        public void AddPoint(Point point)
        {
            this.visitedPoints.Add(point);
            this.totalCost += map[point.a, point.b];
        }

        public List<Point> GetVisitedPoints()
        {
            return this.visitedPoints;
        }

        public int GetCost()
        {
            return this.totalCost;
        }

        public int[,] GetMap()
        {
            return this.map;
        }
    }
}
