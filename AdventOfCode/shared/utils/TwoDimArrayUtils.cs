using System.Collections.Generic;

namespace AdventOfCode
{
    public static class TwoDimArrayUtils
    {


        public static T[] GetColumn<T>(this T[,] matrix, int columnIdx)
        {
            var numRows = matrix.GetLength(0);
            var result = new T[numRows];
            for (int i = 0; i < numRows; i++)
            {
                result[i] = matrix[i, columnIdx];
            }
            return result;
        }


        public static T[] GetRow<T>(this T[,] matrix, int rowIdx)
        {
            var numColumns = matrix.GetLength(1);
            var result = new T[numColumns];
            for (int i = 0; i < numColumns; i++)
            {
                result[i] = matrix[rowIdx, i];
            }
            return result;
        }

        public static T[,] ConvertToMatrix<T>(this List<T[]> arrayList)
        {
            var rowLength = arrayList.Count;
            var colLength = arrayList[0].Length;
            var result = new T[rowLength, colLength];
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
