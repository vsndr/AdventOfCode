using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.year2021.day11
{
    public class Day14Solver
    {
        private const string inputLocation = "D:/CSharpProjects/AdventOfCode/AdventOfCode/AdventOfCode/year2021/day14/day14Input.txt";

        public long SolvePart1()
        {
            (var polymer, var insertions) = ParseFromFile(inputLocation);
            var pairsAfterInsertion = PerformSteps(polymer, insertions, 10);
            var charCounts = CountCharacterOccurences(polymer, pairsAfterInsertion).OrderByDescending(x => x.Value);
            return charCounts.First().Value - charCounts.Last().Value;
        }

        public long SolvePart2()
        {
            (var polymer, var insertions) = ParseFromFile(inputLocation);
            var pairsAfterInsertion = PerformSteps(polymer, insertions, 40);
            var charCounts = CountCharacterOccurences(polymer, pairsAfterInsertion).OrderByDescending(x => x.Value);
            return charCounts.First().Value - charCounts.Last().Value;
        }

        private Dictionary<char, long> CountCharacterOccurences(string polymer, Dictionary<(char left, char right), long> allPairs)
        {
            var result = new Dictionary<char, long>();
            foreach (var pair in allPairs)
            {
                this.AddOrIncrementPair(result, pair.Key.left, pair.Value);
                this.AddOrIncrementPair(result, pair.Key.right, pair.Value);
            }
            //Everything is counted twice, except the starting and the ending character, which is counted twice 1 less time
            var keys = new List<char>(result.Keys);
            foreach (var key in keys)
            {
                var amount = result[key];
                if (key == polymer[0] || key == polymer[polymer.Length - 1])
                {
                    amount++;
                }

                result[key] = amount / 2;
            }

            return result;
        }

        private Dictionary<(char left, char right), long> PerformSteps(string polymer, List<(char left, char right, char middle)> insertionRules, int steps)
        {
            var pairs = GetPairs(polymer);
            for(int i=0; i<steps; i++)
            {
                ApplyInsertionRules(pairs, insertionRules);
            }
            return pairs;
        }

        private void ApplyInsertionRules(Dictionary<(char left, char right), long> pairs, List<(char left, char right, char middle)> insertionRules)
        {
            var pairsToAdd = new Dictionary<(char left, char right), long>();
            var pairsToRemove = new HashSet<(char left, char right)>();
            foreach (var insertionRule in insertionRules)
            {
                var pair = (insertionRule.left, insertionRule.right);
                if (pairs.TryGetValue(pair, out var amount))
                {
                    pairsToRemove.Add(pair);
                    AddOrIncrementPair(pairsToAdd, (pair.left, insertionRule.middle), amount);
                    AddOrIncrementPair(pairsToAdd, (insertionRule.middle, pair.right), amount);
                }
            }

            foreach (var pair in pairsToRemove)
            {
                pairs.Remove(pair);
            }

            foreach (var pair in pairsToAdd)
            {
                AddOrIncrementPair(pairs, pair.Key, pair.Value);
            }
        }

        private void AddOrIncrementPair<T>(Dictionary<T, long> pairs, T key, long increment) where T : struct
        {
            if (pairs.TryGetValue(key, out var oldAmount))
            {
                pairs[key] = oldAmount + increment;
            }
            else
            {
                pairs.Add(key, increment);
            }
        }

        private Dictionary<(char left, char right), long> GetPairs(string polymer)
        {
            var result = new Dictionary<(char left, char right), long>();
            for (int i=1; i<polymer.Length; i++)
            {
                var pair = (polymer[i-1], polymer[i]);
                if (!result.TryGetValue(pair, out var amount))
                {
                    result.Add(pair, 1);
                }
                else
                {
                    result[pair] = ++amount;
                }
            }
            return result;
        }

        private (string polymer, List<(char left, char right, char middle)> insertions) ParseFromFile(string fileLocation)
        {
            var allText = System.IO.File.ReadAllText(fileLocation);

            string[] lines = allText.Split(
                new string[] { Environment.NewLine },
                StringSplitOptions.None
            );

            var polymer = lines[0].Trim();
            var insertions = new List<(char left, char right, char middle)>();
            for (int i=1; i<lines.Count(); i++)
            {
                if (string.IsNullOrEmpty(lines[i]))
                {
                    continue;
                }
                var pairInsertion = lines[i].Split(new string[] { "->" }, StringSplitOptions.None);
                var pair = pairInsertion[0].Trim();
                var insertion = pairInsertion[1].Trim();
                insertions.Add((pair[0], pair[1], insertion[0]));
            }

            return (polymer, insertions);
        }

    }
}
