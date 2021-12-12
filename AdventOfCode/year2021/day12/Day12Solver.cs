using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.year2021.day11
{
    public class Day12Solver
    {
        private const string inputLocation = "D:/CSharpProjects/AdventOfCode/AdventOfCode/AdventOfCode/year2021/day12/day12Input.txt";

        private const string nonExistingNode = "afsdflkajsd;fikjhasdlf";

        public int SolvePart1()
        {
            var nodes = ParseFromFile(inputLocation);
            var paths = this.GetPaths(nodes, "start", new List<string>(), nonExistingNode);
            return paths.Count();
        }

        public int SolvePart2()
        {
            var nodes = ParseFromFile(inputLocation);
            var paths = this.GetPaths(nodes, "start", new List<string>(), null);
            return paths.Count();
        }

        private List<List<string>> GetPaths(Dictionary<string, List<string>> allNodes, string currentNode, List<string> visitedNodes, string allowAdditionalVisitNode)
        {
            visitedNodes.Add(currentNode);

            if (currentNode == "end")
            {
                //If we allowed for an additional visit but we haven't used the additional visit, don't return the result as it is a duplicate
                if (allowAdditionalVisitNode == null || allowAdditionalVisitNode == nonExistingNode)
                {
                    return new List<List<string>> { visitedNodes };
                }
            }

            var result = new List<List<string>>();
            var connections = allNodes[currentNode];
            foreach (var connection in connections)
            {
                //Check if the node can be visited
                if (this.IsLargeNode(connection))
                {
                    //Visit it
                    result.AddRange(this.GetPaths(allNodes, connection, new List<string>(visitedNodes), allowAdditionalVisitNode));
                }
                else
                {
                    //If we can still visit the small node, visit it
                    if (!visitedNodes.Contains(connection))
                    {
                        //Don't allow for another visit
                        result.AddRange(this.GetPaths(allNodes, connection, new List<string>(visitedNodes), allowAdditionalVisitNode));
                        //Allow for another visit
                        if (allowAdditionalVisitNode == null && connection != "start" && connection != "end")
                        {
                            result.AddRange(this.GetPaths(allNodes, connection, new List<string>(visitedNodes), string.Copy(connection)));
                        }
                    }
                    //If we can visit the node again
                    else if (connection == allowAdditionalVisitNode)
                    {
                        result.AddRange(this.GetPaths(allNodes, connection, new List<string>(visitedNodes), nonExistingNode));
                    }
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