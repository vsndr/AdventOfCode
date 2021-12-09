using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.day3
{
    public class Day3Solver
    {
        private const string inputLocation = "D:/CSharpProjects/AdventOfCode/AdventOfCode/AdventOfCode/year2021/day3/day3Input.txt";

        public int SolvePart1()
        {
            var matrix = ParseFromFile(inputLocation);
            var numColumns = matrix.GetLength(1);
            var gammaRay = new int[numColumns];
            
            for (int i=0; i < numColumns; i++)
            {
                gammaRay[i] = GetMostCommonBit(matrix.GetColumn(i));
            }

            //EpsilonRate is the inverse of gammaRay
            var epsilonRate = gammaRay.Select(x => Math.Abs(x - 1)).ToArray();

            return BinaryNumbersToDecimal(gammaRay) * BinaryNumbersToDecimal(epsilonRate);
        }

        public int SolvePart2()
        {
            var matrix = ParseFromFile(inputLocation);
            var oxygenRating = FilterRowsByMostCommonFirstDigit(matrix, 0);
            var scubbingRating = FilterRowsByLeastCommonFirstDigit(matrix, 0);

            return BinaryNumbersToDecimal(oxygenRating) * BinaryNumbersToDecimal(scubbingRating);
        }


        private int[] FilterRowsByLeastCommonFirstDigit(int[,] matrix, int currentColumn)
        {
            //Check if the number of rows of the matrix is reduced to 1
            if (matrix.GetLength(0) == 1)
            {
                return MatrixUtils.GetRow(matrix, 0);
            }
            if (currentColumn < 0)
            {
                throw new Exception("Boo!");
            }

            var leastCommonNumber = GetLeastCommonBit(matrix.GetColumn(currentColumn));
            //Reduce the rows of the matrix to the rows whose number at the currentColumn equals the least common number at currentColumn
            var numRows = matrix.GetLength(0);
            var result = new List<int[]>();

            for (int i = 0; i < numRows; i++)
            {
                if (matrix[i, currentColumn] == leastCommonNumber)
                {
                    result.Add(matrix.GetRow(i));
                }
            }

            return FilterRowsByLeastCommonFirstDigit(result.ConvertToMatrix(), ++currentColumn);
        }


        private int[] FilterRowsByMostCommonFirstDigit(int[,] matrix, int currentColumn)
        {
            //Check if the number of rows of the matrix is reduced to 1
            if (matrix.GetLength(0) == 1)
            {
                return matrix.GetRow(0);
            }

            if (currentColumn < 0)
            {
                throw new Exception("Boo!");
            }

            var mostCommonNumber = GetMostCommonBit(matrix.GetColumn(currentColumn));
            //Reduce the rows of the matrix to the rows whose number at the currentColumn equals the most common number at currentColumn
            var numRows = matrix.GetLength(0);
            var result = new List<int[]>();
            
            for (int i=0; i<numRows; i++)
            {
                if (matrix[i, currentColumn] == mostCommonNumber)
                {
                    result.Add(matrix.GetRow(i));
                }
            }

            return FilterRowsByMostCommonFirstDigit(result.ConvertToMatrix(), ++currentColumn);
        }


        private int GetLeastCommonBit(int[] array)
        {
            var ones = array.Sum();
            var zeroes = array.Length - ones;
            return zeroes <= ones ? 0 : 1;
        }

        private int GetMostCommonBit(int[] array)
        {
            var ones = array.Sum();
            var zeroes = array.Length - ones;
            return ones >= zeroes ? 1 : 0;
        }

        private int BinaryNumbersToDecimal(int[] binaryNumbers)
        {
            var result = 0;
            for (int i=0; i< binaryNumbers.Length; i++)
            {
                if (binaryNumbers[i] == 1)
                {
                    result += (int)Math.Pow(2, binaryNumbers.Length - i - 1);
                }
            }
            return result;
        }


        /// <summary>
        /// Parses the input file to a matrix
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <returns></returns>
        private int[,] ParseFromFile(string fileLocation)
        {
            var text = System.IO.File.ReadAllText(fileLocation);
            string[] lines = text.Split(
                new string[] { Environment.NewLine },
                StringSplitOptions.None
            );

            var result = new int[lines.Count(), lines[0].Length];

            for (int i=0; i<lines.Count(); i++)
            {
                for (int j=0; j<lines[0].Length; j++)
                {
                    result[i, j] = (int) char.GetNumericValue(lines[i][j]);
                }
            }

            return result;
        }
    }
}
