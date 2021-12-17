using AdventOfCode.shared.dataStructures;

namespace AdventOfCode.year2021.day11
{
    public class Day17Solver
    {

        public int SolvePart1()
        {
            (int xMin, int xMax, int yMin, int yMax) targetArea = (281, 311, -74, -54);
            var acceleration = new Vector2(-1, -1);

            var maxY = 0;
            for (int x=1; x<=100; x++)
            {
                for (int y = 1; y<=100; y++)
                {
                    var startingVelocity = new Vector2(x, y);
                    var currentMaxY = 0;
                    var currentPosition = Vector2.Zero;
                    for (int t = 1; t <= 1000; t++)
                    {
                        startingVelocity += acceleration;
                        if (startingVelocity.x < 0)
                        {
                            startingVelocity = new Vector2(0, startingVelocity.y);
                        }
                        currentPosition += startingVelocity;
                        if (currentPosition.y > currentMaxY)
                        {
                            currentMaxY = (int) currentPosition.y;
                        }

                        if (currentPosition.x > targetArea.xMax || currentPosition.y < targetArea.yMin || (currentPosition.x < targetArea.xMin && startingVelocity.x == 0))
                        {
                            break;
                        }

                        if (currentPosition.x >= targetArea.xMin && currentPosition.x <= targetArea.xMax &&
                            currentPosition.y >= targetArea.yMin && currentPosition.y <= targetArea.yMax && currentMaxY > maxY)
                        {
                            maxY = currentMaxY;
                            break;
                        }
                    }
                }
            }

            return maxY;
        }

        public int SolvePart2()
        {
            (int xMin, int xMax, int yMin, int yMax) targetArea = (281, 311, -74, -54);
            var acceleration = new Vector2(-1, -1);

            var distinctVelocities = 0;
            for (int x = 1; x <= 500; x++)
            {
                for (int y = -74; y <= 500; y++)
                {
                    var startingVelocity = new Vector2(x, y);
                    var currentPosition = Vector2.Zero;
                    var meetsTarget = false;
                    for (int t = 1; t <= 3000; t++)
                    {
                        startingVelocity += acceleration;
                        if (startingVelocity.x < 0)
                        {
                            startingVelocity = new Vector2(0, startingVelocity.y);
                        }
                        currentPosition += startingVelocity;

                        if (currentPosition.x > targetArea.xMax || currentPosition.y < targetArea.yMin || (currentPosition.x < targetArea.xMin && startingVelocity.x == 0))
                        {
                            break;
                        }

                        if (currentPosition.x >= targetArea.xMin && currentPosition.x <= targetArea.xMax &&
                            currentPosition.y >= targetArea.yMin && currentPosition.y <= targetArea.yMax)
                        {
                            meetsTarget = true;
                        }
                    }

                    if (meetsTarget)
                    {
                        distinctVelocities++;
                    }
                }
            }

            return distinctVelocities;
        }


    }
}
