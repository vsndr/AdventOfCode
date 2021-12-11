using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.year2021.day10
{
    public class Day10Solver
    {
        private const string inputLocation = "D:/CSharpProjects/AdventOfCode/AdventOfCode/AdventOfCode/year2021/day10/day10Input.txt";

        public int SolvePart1()
        {
            var lines = this.ParseFromFile(inputLocation);
            var firstSyntaxErrorPerLine = lines.Select(x => this.GetSyntaxErrors(x).FirstOrDefault());
            var characterScores = this.GetCharacterScoresPart1();
            return firstSyntaxErrorPerLine.Where(x => x != null).Sum(x => characterScores[x.Value.actual]);
        }

        public long SolvePart2()
        {
            var lines = this.ParseFromFile(inputLocation);
            var uncorruptedLines = lines.Where(x => this.GetSyntaxErrors(x).Count == 0);
            var missingCharacters = uncorruptedLines.Select(x => this.GetMissingCharacters(x));
            var scores = missingCharacters.Select(x => CalculateScorePart2(x)).ToList();
            return scores.OrderBy(x => x).ToList()[scores.Count() / 2];
        }

        private List<(int position, char actual, char expected)?> GetSyntaxErrors(string line)
        {
            var result = new List<(int position, char actual, char expected)?>();
            var stack = new Stack<char>();
            var pairs = this.GetPairs();
            for (int i=0; i<line.Length; i++)
            {
                var currChar = line[i];
                if (currChar == '(' || currChar == '[' || currChar == '{' || currChar == '<')
                {
                    stack.Push(currChar);
                }
                else
                {
                    if (stack.Count == 0)
                    {
                        result.Add((i, currChar, ' '));     //Dunno what to expect when the stack is empty
                    }
                    else
                    {
                        var expectedChar = pairs[stack.Pop()];
                        if (expectedChar != currChar)
                        {
                            result.Add((i, currChar, expectedChar));
                        }
                    }
                }
            }
            return result;
        }

        private char[] GetMissingCharacters(string line)
        {
            var stack = new Stack<char>();
            for (int i = 0; i < line.Length; i++)
            {
                var currChar = line[i];
                if (currChar == '(' || currChar == '[' || currChar == '{' || currChar == '<')
                {
                    stack.Push(currChar);
                }
                else
                {
                    stack.Pop();
                }
            }
            var pairs = this.GetPairs();
            return stack.Select(x => pairs[x]).ToArray();
        }

        private long CalculateScorePart2(char[] missingCharacters)
        {
            var characterScores = this.GetCharacterScoresPart2();
            long totalScore = 0;
            foreach (var missingCharacter in missingCharacters)
            {
                totalScore = totalScore * 5 + characterScores[missingCharacter];
            }
            return totalScore;
        }

        private Dictionary<char, int> GetCharacterScoresPart1()
        {
            return new Dictionary<char, int>
            {
                { ')', 3 },
                { ']', 57 },
                { '}', 1197 },
                { '>', 25137 },
            };
        }

        private Dictionary<char, int> GetCharacterScoresPart2()
        {
            return new Dictionary<char, int>
            {
                { ')', 1 },
                { ']', 2 },
                { '}', 3 },
                { '>', 4 },
            };
        }

        private Dictionary<char, char> GetPairs()
        {
            return new Dictionary<char, char>
            {
                { '(', ')' },
                { '[', ']' },
                { '{', '}' },
                { '<', '>' },
            };
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
