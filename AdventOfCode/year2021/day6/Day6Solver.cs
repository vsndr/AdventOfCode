using AdventOfCode.day6;
using System.Collections.Generic;
using System.Linq;

public class Day6Solver
{

    private const string inputLocation = "D:/CSharpProjects/AdventOfCode/AdventOfCode/AdventOfCode/year2021/day6/day6Input.txt";

    public long SolvePart1()
    {
        var lanternFish = this.ParseLinesFromFile(inputLocation);
        var ageGroups = this.DivideIntoGroups(lanternFish);
        return this.GetPopulationSizeAfterDays(ageGroups, 80);
    }

    public long SolvePart2()
    {
        var lanternFish = this.ParseLinesFromFile(inputLocation);
        var ageGroups = this.DivideIntoGroups(lanternFish);
        return this.GetPopulationSizeAfterDays(ageGroups, 256);
    }

    private List<AgeGroup> DivideIntoGroups(List<int> numbers)
    {
        return numbers.GroupBy(x => x)
                        .Select(x => new AgeGroup(x.Key, x.Count()))
                        .ToList();
    }

    private long GetPopulationSizeAfterDays(List<AgeGroup> ageGroups, int daysToSimulate)
    {
        for (int i = 0; i < daysToSimulate; i++)
        {
            AgeGroup reproducingGroup = null;
            foreach (var ageGroup in ageGroups)
            {
                ageGroup.Tick();
                if (ageGroup.IsReproducing())
                {
                    reproducingGroup = ageGroup;
                }
            }

            if (reproducingGroup != null)
            {
                var groupSize = reproducingGroup.GetGroupSize();
                ageGroups.Add(new AgeGroup(8, groupSize));
                //lantern fish that have reproduced and have died are reincarnated with 6 new days to live
                var aged6Group = ageGroups.SingleOrDefault(x => x.GetAge() == 6);
                if (aged6Group != null)
                {
                    aged6Group.AddToGroup(groupSize);
                }
                else
                {
                    ageGroups.Add(new AgeGroup(6, groupSize));
                }

                ageGroups.Remove(reproducingGroup);
            }
        }

        return ageGroups.Sum(x => x.GetGroupSize());
    }


    private List<int> ParseLinesFromFile(string fileLocation)
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
