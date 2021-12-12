using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.year2021.day11
{
    public class Day12Solver
    {
        private const string inputLocation = "D:/CSharpProjects/AdventOfCode/AdventOfCode/AdventOfCode/year2021/day12/day12Input.txt";

        public int SolvePart1()
        {
            var nodes = ParseFromFile(inputLocation);
            var paths = this.GetPaths(nodes, "start", new List<string>(), false);
            return paths.Count();
        }

        public int SolvePart2()
        {
            var nodes = ParseFromFile(inputLocation);
            var paths = this.GetPaths(nodes, "start", new List<string>(), true);
            return paths.Count();
        }

        private List<List<string>> GetPaths(Dictionary<string, List<string>> allNodes, string currentNode, List<string> visitedNodes, bool canVisitSmallNodeTwice)
        {
            visitedNodes.Add(currentNode);

            if (currentNode == "end")
            {
                return new List<List<string>> { visitedNodes };
            }

            var result = new List<List<string>>();
            var connections = allNodes[currentNode];
            foreach (var connection in connections)
            {
                //if it's a large node or a small node that hasn't been visited before
                if (this.IsLargeNode(connection) || !visitedNodes.Contains(connection))
                {
                    result.AddRange(this.GetPaths(allNodes, connection, new List<string>(visitedNodes), canVisitSmallNodeTwice));
                }
                //If it's allowed to visit the node a second time
                else if (canVisitSmallNodeTwice && connection != "start")
                {
                    result.AddRange(this.GetPaths(allNodes, connection, new List<string>(visitedNodes), false));
                }
            }

            return result;
        }

        private bool IsLargeNode(string node)
        {
            return node.All(c => char.IsUpper(c));
        }

        private Dictionary<string, List<string>> ParseFromFile(string fileLocation)
        {
            var allText = System.IO.File.ReadAllText(fileLocation);

            string[] lines = allText.Split(
                new string[] { Environment.NewLine },
                StringSplitOptions.None
            );

            var result = new Dictionary<string, List<string>>();
            foreach (var line in lines)
            {
                var fromTo = line.Split('-');
                var from = fromTo[0];
                var to = fromTo[1];
                //Every connection is bi-directional. Therefore, add connections A -> B as well as B -> A
                if (!result.TryGetValue(from, out var connectionsTo))
                {
                    connectionsTo = new List<string>();
                    result.Add(from, connectionsTo);
                }
                connectionsTo.Add(to);

                if (!result.TryGetValue(to, out var connectionsFrom))
                {
                    connectionsFrom = new List<string>();
                    result.Add(to, connectionsFrom);
                }
                connectionsFrom.Add(from);
            }

            return result;
        }

    }
}