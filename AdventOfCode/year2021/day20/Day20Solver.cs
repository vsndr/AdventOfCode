using AdventOfCode.shared.dataStructures;
using System;
using System.Linq;

namespace AdventOfCode.year2021.day11
{
    public class Day20Solver
    {
        private const string inputLocation = "D:/CSharpProjects/AdventOfCode/AdventOfCode/AdventOfCode/year2021/day20/day20Input.txt";

        public int SolvePart1()
        {
            (var image, var enhancement) = this.ParseFromFile(inputLocation);
            var enhancedImage = this.EnhanceTimes(image, enhancement, 2);
            return enhancedImage.Where(x => x == 1).Count();
        }

        public int SolvePart2()
        {
            (var image, var enhancement) = this.ParseFromFile(inputLocation);
            var enhancedImage = this.EnhanceTimes(image, enhancement, 50);
            return enhancedImage.Where(x => x == 1).Count();
        }


        private Matrix2D EnhanceTimes(Matrix2D matrix, int[] enhancement, int numOfEnhancements)
        {
            for (int i = 0; i < numOfEnhancements; i++)
            {
                matrix = this.EnhanceImage(matrix, enhancement, i);
            }
            return matrix;
        }

        private Matrix2D EnhanceImage(Matrix2D matrix, int[] enhancement, int cycle)
        {
            var paddedMatrix = matrix.Pad(topSize: 2, bottomSize: 2, leftSize: 2, rightSize: 2, padNumber: (cycle % 2));
            var enhancedMatrix = new Matrix2D(paddedMatrix.rows, paddedMatrix.columns);

            for (int i=0; i< paddedMatrix.rows; i++)
            {
                for (int j=0; j< paddedMatrix.columns; j++)
                {
                    enhancedMatrix[i, j] = EnhancePixel(paddedMatrix, enhancement, i, j);
                }
            }

            return enhancedMatrix[$"{1}:{enhancedMatrix.rows-2},{1}:{enhancedMatrix.columns-2}"];   //Remove the outer pixels
        }

        private int EnhancePixel(Matrix2D matrix, int[] enhancement, int i, int j)
        {
            var windowSlice = matrix[$"{i-1}:{i+1},{j-1}:{j+1}"];
            var flattenedWindow = windowSlice.Flatten();
            var decimalNumber = BinaryNumbersToDecimal(flattenedWindow);
            return enhancement[decimalNumber];
        }


        private int BinaryNumbersToDecimal(int[] binaryNumbers)
        {
            var result = 0;
            for (int i = 0; i < binaryNumbers.Length; i++)
            {
                if (binaryNumbers[i] == 1)
                {
                    result += (int)Math.Pow(2, binaryNumbers.Length - i - 1);
                }
            }
            return result;
        }


        private (Matrix2D image, int[] enhancement) ParseFromFile(string fileLocation)
        {
            var allText = System.IO.File.ReadAllText(fileLocation);

            string[] lines = allText.Split(
                new string[] { Environment.NewLine },
                StringSplitOptions.None
            );

            var enhancement = lines[0].Select(x => x == '#' ? 1 : 0).ToArray();
            var remainingLines = lines.Skip(1).Where(x => !string.IsNullOrEmpty(x)).ToList();

            var image = new int[remainingLines[0].Count() ,remainingLines.Count];
            for (int i=0; i<remainingLines.Count; i++)
            {
                for (int j=0; j<remainingLines[0].Count(); j++)
                {
                    if (remainingLines[i][j] == '#')
                    {
                        image[i, j] = 1;
                    }
                }
            }

            return (new Matrix2D(image), enhancement);
        }

    }
}
