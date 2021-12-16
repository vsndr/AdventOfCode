using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.year2021.day11
{
    public class Day17Solver
    {
        private const string inputLocation = "D:/CSharpProjects/AdventOfCode/AdventOfCode/AdventOfCode/year2021/day16/day16Input.txt";

        public int SolvePart1()
        {
            return -1;
        }

        public int SolvePart2()
        {
            return -1;
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
}
