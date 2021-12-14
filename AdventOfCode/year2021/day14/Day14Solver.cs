using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.year2021.day11
{
    public class Day14Solver
    {
        private const string inputLocation = "D:/CSharpProjects/AdventOfCode/AdventOfCode/AdventOfCode/year2021/day14/day14Input.txt";

        //TODO: This code was rushed and needs some rewriting.

        public long SolvePart1()
        {
            (var polymer, var insertions) = ParseFromFile(inputLocation);
            var pairsAfterInsertion = PerformSteps(polymer, insertions, 10);
            var charCounts = CountCharactersAfterInsertions(pairsAfterInsertion).OrderByDescending(x => x.Value);
            return charCounts.First().Value - charCounts.Last().Value;      //TODO: Sometimes there's some rounding error occuring here that's giving an answer that's 1 too high
        }

        public long SolvePart2()
        {
            (var polymer, var insertions) = ParseFromFile(inputLocation);
            var pairsAfterInsertion = PerformSteps(polymer, insertions, 40);
            var charCounts = CountCharactersAfterInsertions(pairsAfterInsertion).OrderByDescending(x => x.Value);
            return charCounts.First().Value - charCounts.Last().Value;      //TODO: Sometimes there's some rounding error occuring here that's giving an answer that's 1 too high
        }

        private Dictionary<char, long> CountCharactersAfterInsertions(Dictionary<(char left, char right), long> allPairs)
        {
            var result = new Dictionary<char, long>();
            foreach (var pair in allPairs)
            {
                if (result.TryGetValue(pair.Key.left, out var amount))
                {
                    result[pair.Key.left] = amount + pair.Value;
                }
                else
                {
                    result.Add(pair.Key.left, pair.Value);
                }

                if (result.TryGetValue(pair.Key.right, out amount))
                {
                    result[pair.Key.right] = amount + pair.Value;
                }
                else
                {
                    result.Add(pair.Key.right, pair.Value);
                }
            }
            //Everything is counted twice, except the starting and the ending character, which is counted twice 1 less time
            var keys = new List<char>(result.Keys);
            foreach (var key in keys)
            {
                var amount = result[key];
                if (key == 'N' || key == 'B')
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
                pairs = GetPairsAfterInsertions(pairs, insertionRules);
            }
            return pairs;
        }

        private Dictionary<(char left, char right), long> GetPairsAfterInsertions(Dictionary<(char left, char right), long> pairs, List<(char left, char right, char middle)> insertionRules)
        {
            var insertionRulesDuplicate = new List<(char left, char right, char middle)>(insertionRules);
            var pairsToAdd = new Dictionary<(char left, char right), long>();
            var pairsToRemove = new HashSet<(char left, char right)>();
            for (int i= insertionRulesDuplicate.Count-1; i>=0; i--)
            {
                var insertionRule = insertionRulesDuplicate[i];
                var pair = (insertionRule.left, insertionRule.right);
                if (pairs.TryGetValue(pair, out var amount))
                {
                    pairsToRemove.Add(pair);
                    AddPair(pairsToAdd, (pair.left, insertionRule.middle), amount);
                    AddPair(pairsToAdd, (insertionRule.middle, pair.right), amount);
                }
                insertionRulesDuplicate.RemoveAt(i);
            }

            foreach (var pair in pairsToRemove)
            {
                pairs.Remove(pair);
            }

            foreach (var pair in pairsToAdd)
            {
                AddPair(pairs, pair.Key, pair.Value);
            }

            return pairs;
        }

        private void AddPair(Dictionary<(char left, char right), long> pairs, (char left, char right) newPair, long amount)
        {
            if (pairs.TryGetValue(newPair, out var oldAmount))
            {
                pairs[newPair] = (oldAmount + amount);
            }
            else
            {
                pairs.Add(newPair, amount);
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
