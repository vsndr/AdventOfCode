using AdventOfCode.shared.dataStructures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.year2021.day9
{
    public class Day9Solver
    {

        private const string inputLocation = "D:/CSharpProjects/AdventOfCode/AdventOfCode/AdventOfCode/year2021/day9/day9Input.txt";

        public int SolvePart1()
        {
            var matrix = this.ParseFromFile(inputLocation);
            var lowPoints = this.GetLowPoints(matrix);
            return lowPoints.Sum(p => matrix[p.a, p.b] + 1);
        }

        public int SolvePart2()
        {
            var matrix = this.ParseFromFile(inputLocation);
            var lowPoints = this.GetLowPoints(matrix);
            var basinSizes = lowPoints.Select(x => GetBasin(matrix, x).Count);
            return basinSizes.OrderByDescending(x => x).Take(3).Aggregate((total, next) => total * next);
        }


        private HashSet<Point> GetBasin(int[,] matrix, Point currentPoint)
        {
            //Add nothing to the basin if the current point is a 9
            if (matrix[currentPoint.a, currentPoint.b] == 9)
            {
                return new HashSet<Point>();
            }

            //Checks each of the 4 neighbors whether they belong to the basin
            //If a neighbor belongs to the basin, add it to the basin via a recurring call
            var basin = new HashSet<Point>() { currentPoint };
            var row = currentPoint.a;
            var col = currentPoint.b;

            //Add top to basin
            if (row - 1 >= 0 && matrix[row, col] < matrix[row - 1, col])
            {
                basin.UnionWith(GetBasin(matrix, new Point(row - 1, col)));
            }

            //Add bottom to basin
            if (row + 1 < matrix.GetLength(0) && matrix[row, col] < matrix[row + 1, col])
            {
                basin.UnionWith(GetBasin(matrix, new Point(row + 1, col)));
            }

            //Add left to basin
            if (col - 1 >= 0 && matrix[row, col] < matrix[row, col - 1])
            {
                basin.UnionWith(GetBasin(matrix, new Point(row, col - 1)));
            }

            //Add right to basin
            if (col + 1 < matrix.GetLength(1) && matrix[row, col] < matrix[row, col + 1])
            {
                basin.UnionWith(GetBasin(matrix, new Point(row, col + 1)));
            }

            return basin;
        }


        private List<Point> GetLowPoints(int[,] matrix)
        {
            var result = new List<Point>();
            var rows = matrix.GetLength(0);
            var columns = matrix.GetLength(1);

            for (int i=0; i<rows; i++)
            {
                for (int j=0; j<columns; j++)
                {
                    if ((j-1 < 0 || matrix[i,j] < matrix[i, j-1]) &&        //left
                        (j+1 >= columns || matrix[i,j] < matrix[i, j+1]) && //right
                        (i-1 < 0 || matrix[i, j] < matrix[i-1, j]) &&       //top
                        (i+1 >= rows || matrix[i, j] < matrix[i+1, j])      //bottom
                    )
                    {
                        result.Add(new Point(i, j));
                    }
                }
            }

            return result;
        }

        private int[,] ParseFromFile(string fileLocation)
        {
            var allText = System.IO.File.ReadAllText(fileLocation);

            string[] lines = allText.Split(
                new string[] { Environment.NewLine },
                StringSplitOptions.None
            );

            var rows = lines.Length;
            var columns = lines[0].Length;
            var result = new int[rows, columns];

            for (int i=0; i<rows; i++)
            {
                for (int j=0; j<columns; j++)
                {
                    result[i, j] = (int) char.GetNumericValue(lines[i][j]);
                }
            }

            return result;
        }

    }
}
