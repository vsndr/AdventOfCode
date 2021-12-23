using System;
using System.Collections.Generic;
using System.Linq;
public class Day2Solver
{
    private const string inputLocation = "D:/CSharpProjects/AdventOfCode/AdventOfCode/AdventOfCode/year2021/day2/day2Input.txt";

    public int SolvePart1()
    {
        var commands = ParseFromFile(inputLocation);

        var totalUp = commands.Where(x => x.direction == Direction.UP).Sum(x => x.value);
        var totalDown = commands.Where(x => x.direction == Direction.DOWN).Sum(x => x.value);
        var totalForward = commands.Where(x => x.direction == Direction.FORWARD).Sum(x => x.value);

        return totalForward * (totalDown - totalUp);
    }

    public int SolvePart2()
    {
        var commands = ParseFromFile(inputLocation);
        var forwardVal = 0;
        var depth = 0;
        var aim = 0;

        foreach (var command in commands)
        {
            if (command.direction == Direction.UP)
            {
                aim -= command.value;
            }
            else if (command.direction == Direction.DOWN)
            {
                aim += command.value;
            }
            else
            {
                forwardVal += command.value;
                depth += (aim * command.value);
            }
        }

        return forwardVal * depth;
    }


    private List<Command> ParseFromFile(string fileLocation)
    {
        var text = System.IO.File.ReadAllText(fileLocation);
        string[] lines = text.Split(
            new string[] { Environment.NewLine },
            StringSplitOptions.None
        );

        return lines.Select(x => Command.ParseFrom(x)).ToList();
    }

    private class Command
    {

        public readonly Direction direction;
        public readonly int value;

        private Command(Direction direction, int value)
        {
            this.direction = direction;
            this.value = value;
        }

        public static Command ParseFrom(string commandString)
        {
            var commandParams = commandString.Split(' ');
            string direction = commandParams[0];
            string value = commandParams[1];

            return new Command(ParseDirection(direction), int.Parse(value));
        }

        private static Direction ParseDirection(string direction)
        {
            switch (direction)
            {
                case "forward":
                    return Direction.FORWARD;
                case "down":
                    return Direction.DOWN;
                case "up":
                    return Direction.UP;
                default:
                    throw new Exception("Boo!");
            }
        }
    }

    private enum Direction
    {
        UP,
        DOWN,
        FORWARD,
    }

}
