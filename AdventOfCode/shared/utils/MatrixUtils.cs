using System.Collections.Generic;

namespace AdventOfCode
{
    public static class MatrixUtils
    {

        public static int[] GetColumn(this int[,] matrix, int columnIdx)
        {
            var numRows = matrix.GetLength(0);
            var result = new int[numRows];
            for (int i = 0; i < numRows; i++)
            {
                result[i] = matrix[i, columnIdx];
            }
            return result;
        }


        public static int[] GetRow(this int[,] matrix, int rowIdx)
        {
            var numColumns = matrix.GetLength(1);
            var result = new int[numColumns];
            for (int i = 0; i < numColumns; i++)
            {
                result[i] = matrix[rowIdx, i];
            }
            return result;
        }

        public static int[,] ConvertToMatrix(this List<int[]> arrayList)
        {
            var rowLength = arrayList.Count;
            var colLength = arrayList[0].Length;
            var result = new int[rowLength, colLength];
            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < colLength; j++)
                {
                    result[i, j] = arrayList[i][j];
                }
            }
            return result;
        }

    }
}
