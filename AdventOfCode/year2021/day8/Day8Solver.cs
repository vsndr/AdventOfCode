using AdventOfCode.shared.utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.day8
{
    public class Day8Solver
    {

        private const string inputLocation = "D:/CSharpProjects/AdventOfCode/AdventOfCode/AdventOfCode/year2021/day8/day8Input.txt";

        public int SolvePart1()
        {
            //1 has length 2, 4 has length 4, 7 has length 3, 8 has length 7
            //So the question becomes: how many strings are there with length 2, 3, 4 or 7 in the output? 
            var displays = ParseFromFile(inputLocation);
            return displays.SelectMany(x => x.outputs)
                            .Where(x => x.Length == 2 || x.Length == 3 || x.Length == 4 || x.Length == 7)
                            .Count();
        }

        public int SolvePart2()
        {
            var displays = ParseFromFile(inputLocation);

            var decryptedDigits = new List<int>();
            foreach ((var digits, var outputs) in displays)
            {
                //First grab each digit for which we know for certain what the "encryption" is
                var one = digits.RemoveFirstAndGet(x => x.Count() == 2);
                var four = digits.RemoveFirstAndGet(x => x.Count() == 4);
                var seven = digits.RemoveFirstAndGet(x => x.Count() == 3);
                var eight = digits.RemoveFirstAndGet(x => x.Count() == 7);
                //Now we can determine what the top segment is by subtracting one from seven
                var topSegment = seven.Except(one).Single();
                //There's only 1 digit left which has all segments of four and that has the top segment, namely 9
                var fourPlusTop = four + topSegment;
                var nine = digits.RemoveFirstAndGet(x => fourPlusTop.Except(x).Count() == 0);
                //Now we can determine the bottom left segment, which is eight minus nine
                var bottomLeftSegment = eight.Except(nine).Single();
                //Now there's only 1 digit left which contains the bottomLeftSegment as well as all segments in seven, namely zero
                var sevenPlusBottomLeft = seven + bottomLeftSegment;
                var zero = digits.RemoveFirstAndGet(x => sevenPlusBottomLeft.Except(x).Count() == 0);
                //Now there's only 1 digit left which has a difference of 1 segment from 8, namely six
                var six = digits.RemoveFirstAndGet(x => eight.Except(x).Count() == 1);
                //Now there's only 1 digit left which contains the bottom left segment, namely two
                var two = digits.RemoveFirstAndGet(x => x.Contains(bottomLeftSegment));
                //Now there's only 1 digit left which overlaps completely with 1, namely three
                var three = digits.RemoveFirstAndGet(x => one.Except(x).Count() == 0);
                //Now there's only 1 digit remaining, namely five
                var five = digits.Single();

                var patternToNum = new Dictionary<string, int>
                {
                    { new string(zero.OrderBy(x => x).ToArray()), 0 },
                    { new string(one.OrderBy(x => x).ToArray()), 1 },
                    { new string(two.OrderBy(x => x).ToArray()), 2 },
                    { new string(three.OrderBy(x => x).ToArray()), 3 },
                    { new string(four.OrderBy(x => x).ToArray()), 4 },
                    { new string(five.OrderBy(x => x).ToArray()), 5 },
                    { new string(six.OrderBy(x => x).ToArray()), 6 },
                    { new string(seven.OrderBy(x => x).ToArray()), 7 },
                    { new string(eight.OrderBy(x => x).ToArray()), 8 },
                    { new string(nine.OrderBy(x => x).ToArray()), 9 },
                };

                var fourDigitNumber = 0;
                for (int i=0; i< outputs.Count; i++)
                {
                    //Get the string which needs to be decoded to a number. 
                    //Sort the string on alphabetical order to align it with the dictionary above
                    var encryptedDigit = new string(outputs[i].OrderBy(x => x).ToArray());
                    var decryptedDigit = patternToNum[encryptedDigit];
                    //The first digit gets multiplied by 1000, the second by 100, the third by 10, the fourth by 1.
                    fourDigitNumber += (int) Math.Pow(10, 3-i) * decryptedDigit;
                }
                decryptedDigits.Add(fourDigitNumber);
            }

            return decryptedDigits.Sum();
        }




        private List<(List<string> digits, List<string> outputs)> ParseFromFile(string fileLocation)
        {
            var result = new List<(List<string> digits, List<string> outputs)>();       //Internal screeching q.q

            var allText = System.IO.File.ReadAllText(fileLocation);

            string[] lines = allText.Split(
                new string[] { Environment.NewLine },
                StringSplitOptions.None
            );

            foreach (var line in lines)
            {
                string[] splitLine = line.Split('|');
                var digitsString = splitLine[0];
                var outputString = splitLine[1];

                var digits = digitsString.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                var outputs = outputString.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

                result.Add((new List<string>(digits), new List<string>(outputs)));
            }

            return result;
        }

    }
}
