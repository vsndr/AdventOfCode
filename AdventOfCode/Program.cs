using AdventOfCode.day1;
using AdventOfCode.day2;
using AdventOfCode.day3;
using AdventOfCode.day4;
using AdventOfCode.day5;
using AdventOfCode.day6;
using AdventOfCode.day7;
using AdventOfCode.day8;
using AdventOfCode.shared.dataStructures;
using AdventOfCode.year2021.day10;
using AdventOfCode.year2021.day11;
using AdventOfCode.year2021.day9;

namespace AdventOfCode
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var solver = new Day17Solver();
            var solution = solver.SolvePart1();

            System.Diagnostics.Debug.WriteLine(solution);
        }
    }
}
