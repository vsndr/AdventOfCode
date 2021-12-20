using AdventOfCode.shared.dataStructures;
using AdventOfCode.year2021.day19;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.year2021.day11
{
    public class Day19Solver
    {
        private const string inputLocation = "D:/CSharpProjects/AdventOfCode/AdventOfCode/AdventOfCode/year2021/day19/day19Input.txt";

        public int SolvePart1()
        {
            var scannerGroup = ParseFromFile(inputLocation);
            var allBeacons = scannerGroup.GetAllBeacons();
            return allBeacons.Count();
        }

        public int SolvePart2()
        {
            var scannerGroup = ParseFromFile(inputLocation);
            var scannerPositions = scannerGroup.GetMatchedScanners().Select(x => x.GetScannerPosition()).ToList();
            var biggestDistance = 0;
            for (int i= scannerPositions.Count-1; i>=0; i--)
            {
                for (int j=0; j< scannerPositions.Count; j++)
                {
                    if (i != j)
                    {
                        var from = scannerPositions[i];
                        var to = scannerPositions[j];
                        biggestDistance = Math.Max(biggestDistance, GetManhattenDistance(from, to));
                    }
                }
                scannerPositions.RemoveAt(i);
            }

            return biggestDistance;
        }

        private static int GetManhattenDistance(Vector3 from, Vector3 to)
        {
            var distance = from - to;
            return (int) (Math.Abs(distance.x) + Math.Abs(distance.y) + Math.Abs(distance.z));
        }

        private ScannerGroup ParseFromFile(string fileLocation)
        {
            var allText = System.IO.File.ReadAllText(fileLocation);

            string[] lines = allText.Split(
                new string[] { Environment.NewLine },
                StringSplitOptions.None
            );

            var allScanners = new List<Scanner>();
            var currentBeacons = new HashSet<Vector3>();
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    allScanners.Add(new Scanner(new HashSet<Vector3>(currentBeacons)));
                    currentBeacons = new HashSet<Vector3>();
                }
                else if (line.Contains("scanner"))
                {
                    continue;
                }
                else
                {
                    var vectorComponents = line.Split(',');
                    currentBeacons.Add(new Vector3(int.Parse(vectorComponents[0]), int.Parse(vectorComponents[1]), int.Parse(vectorComponents[2])));
                }
            }
            if (currentBeacons.Any())
            {
                allScanners.Add(new Scanner(new HashSet<Vector3>(currentBeacons)));
            }

            return new ScannerGroup(allScanners);
        }

    }
}
