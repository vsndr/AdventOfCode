using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.day7
{
    public class Day7Solver
    {
        private const string inputLocation = "D:/CSharpProjects/AdventOfCode2021/AdventOfCode/AdventOfCode/year2021/day7/day7Input.txt";

        public int SolvePart1()
        {
            var input = ParseFromFile(inputLocation);
            var median = this.CalculateMedian(input);
            return GetTotalDistanceTo(input, median);
        }

        public int SolvePart2()
        {
            var input = ParseFromFile(inputLocation);
            var average = this.CalculateAverage(input);
            return GetTotalAccumulativeDistanceTo(input, average);
        }

        private int GetTotalDistanceTo(List<int> numbers, int position)
        {
            return numbers.Sum(x => Math.Abs(x - position));
        }

        private int GetTotalAccumulativeDistanceTo(List<int> numbers, int position)
        {
            return numbers.Sum(x => {
                var distance = Math.Abs(x - position);
                return ((distance + 1) * distance) / 2;
            });
        }

        private int CalculateMedian(List<int> numbers)
        {
            var orderedNumbers = numbers.OrderBy(x => x).ToList();
            var middleOfListIndex = (int) Math.Ceiling((double) orderedNumbers.Count / 2);
            //If the length of the list is even, the median is calculated by taking the average of the middle 2 numbers
            if (orderedNumbers.Count % 2 == 0)
            {
                var middleNumber1 = orderedNumbers[middleOfListIndex];
                var middleNumber2 = orderedNumbers[middleOfListIndex-1];
                return (middleNumber1 + middleNumber2) / 2;
            }
            else
            {
                return orderedNumbers[middleOfListIndex];
            }
        }

        private int CalculateAverage(List<int> numbers)
        {
            return numbers.Sum(x => x) / numbers.Count;
        }

        private List<int> ParseFromFile(string fileLocation)
        {
            var result = new List<int>();
            var allText = System.IO.File.ReadAllText(fileLocation);

            string[] numbers = allText.Split(',');

            foreach (var num in numbers)
            {
                result.Add(int.Parse(num));
            }

            return result;
        }

    }
}
