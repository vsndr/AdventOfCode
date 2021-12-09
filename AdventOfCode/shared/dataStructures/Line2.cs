using System;

namespace AdventOfCode.shared.dataStructures
{
    public class Line2
    {
        private readonly Vector2 begin;
        private readonly Vector2 end;
        private readonly Vector2 direction;

        public Line2(Vector2 begin, Vector2 end)
        {
            this.begin = begin;
            this.end = end;

            this.direction = (end - begin) / Math.Max(Math.Abs(this.direction.x), Math.Abs(this.direction.y));
        }

        public bool FallsOnLine(Vector2 point)
        {
            if (this.FallsOutsideLineRange(point))
            {
                return false;
            }

            //Check: whether the direction is a multiple of (point - begin)
            //Really wonky way to do this to avoid NaN and divide by zero hell
            var diff = point - begin;
            var bla = diff / direction;
            var multiplyer = bla.GetMaxOfXandY();
            var fallsOnLine = (direction * multiplyer) == diff;
            return fallsOnLine;
        }

        private bool FallsOutsideLineRange(Vector2 point)
        {
            return (point.x < begin.x && point.x < end.x) || (point.x > begin.x && point.x > end.x) ||
                (point.y < begin.y && point.y < end.y) || (point.y > begin.y && point.y > end.y);
        }


        public bool IsHorizontalLine()
        {
            return this.direction.y == 0;
        }

        public bool IsVerticalLine()
        {
            return this.direction.x == 0;
        }

        public bool IsDiagonalLine()
        {
            return Math.Abs(this.direction.x) == Math.Abs(this.direction.y);
        }

        public int GetMaxXCoordinate()
        {
            return (int) Math.Max(begin.x, end.x);
        }

        public int GetMaxYCoordinate()
        {
            return (int)Math.Max(begin.y, end.y);
        }

    }
}
