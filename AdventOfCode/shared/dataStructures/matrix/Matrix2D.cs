using AdventOfCode.shared.dataStructures.matrix;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.shared.dataStructures
{
    public class Matrix2D
    {

        public readonly int rows;
        public readonly int columns;
        private Integer[,] internalMatrix;

        public Matrix2D(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
            this.internalMatrix = ToReferenceableMatrix(new int[rows, columns]);
        }

        public Matrix2D(int[,] matrix)
        {
            this.rows = matrix.GetLength(0);
            this.columns = matrix.GetLength(1);
            this.internalMatrix = ToReferenceableMatrix(matrix);
        }

        public Matrix2D(Integer[,] matrix)
        {
            this.rows = matrix.GetLength(0);
            this.columns = matrix.GetLength(1);
            this.internalMatrix = matrix;
        }

        public int this[int i, int j]
        {
            get {
                var element = this.internalMatrix[i, j];
                if (element != null)
                {
                    return element.internalInt;
                }
                return 0;
            }
            set {
                var element = this.internalMatrix[i, j];
                if (element != null)
                {
                    element.internalInt = value;
                }
            }
        }

        public Matrix2D this[string slicingOperator]
        {
            get {
                var slice = SliceParser.ParseSlicingOperator(slicingOperator, this);
                var result = new Integer[slice.toRow - slice.fromRow, slice.toCol - slice.fromCol];

                for (int i=slice.fromRow; i<slice.toRow; i++)
                {
                    for (int j=slice.fromCol; j<slice.toCol; j++)
                    {
                        if (i >= 0 && i<this.rows && j>=0 && j<this.columns)
                        {
                            result[i - slice.fromRow, j - slice.fromCol] = this.internalMatrix[i, j];
                        }
                    }
                }

                return new Matrix2D(result);
            }
        }

        /// <summary>
        /// Returns the biggest number in the matrix
        /// </summary>
        /// <returns></returns>
        public int GetMax()
        {
            var result = 0;
            for (int i=0; i<this.rows; i++)
            {
                for (int j=0; j<this.columns; j++)
                {
                    result = Math.Max(result, this[i, j]);
                }
            }
            return result;
        }

        public int GetSize()
        {
            return this.rows * this.columns;
        }

        /// <summary>
        /// Returns the indices in the matrix for which the given condition is true
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<Point> Where(Func<int, bool> predicate)
        {
            for (int i=0; i<rows;i++)
            {
                for (int j=0; j<columns; j++)
                {
                    if (predicate(this[i,j]))
                    {
                        yield return new Point(i, j);
                    }
                }
            }
        }

        /// <summary>
        /// Returns a prettified version of the matrix as a string
        /// </summary>
        /// <returns></returns>
        public string PrettyPrint()
        {
            var result = new StringBuilder();
            var largestNumber = this.GetMax();
            var mostNumberOfDigits = (int) Math.Log10(largestNumber);


            for (int i = 0; i < this.rows; i++)
            {
                for (int j = 0; j < this.columns; j++)
                {
                    result.Append(this.internalMatrix[i, j].ToString().PadLeft(mostNumberOfDigits + 2));
                }
                result.AppendLine();
            }
            return result.ToString();
        }


        public static Matrix2D operator +(Matrix2D mat, int num)
        {
            for (int i=0; i<mat.rows; i++)
            {
                for (int j=0; j<mat.columns; j++)
                {
                    mat[i, j] = mat[i, j] + num;
                }
            }
            return mat;
        }

        public static Matrix2D operator ++(Matrix2D mat)
        {
            for (int i = 0; i < mat.rows; i++)
            {
                for (int j = 0; j < mat.columns; j++)
                {
                    mat[i, j] = ++mat[i, j];
                }
            }
            return mat;
        }

        public static Matrix2D operator -(Matrix2D mat, int num)
        {
            for (int i = 0; i < mat.rows; i++)
            {
                for (int j = 0; j < mat.columns; j++)
                {
                    mat[i, j] = mat[i, j] - num;
                }
            }
            return mat;
        }

        public static Matrix2D operator --(Matrix2D mat)
        {
            for (int i = 0; i < mat.rows; i++)
            {
                for (int j = 0; j < mat.columns; j++)
                {
                    mat[i, j] = --mat[i, j];
                }
            }
            return mat;
        }

        public static Matrix2D operator *(Matrix2D mat, int num)
        {
            for (int i = 0; i < mat.rows; i++)
            {
                for (int j = 0; j < mat.columns; j++)
                {
                    mat[i, j] = mat[i, j] * num;
                }
            }
            return mat;
        }

        public static Matrix2D operator /(Matrix2D mat, int num)
        {
            for (int i = 0; i < mat.rows; i++)
            {
                for (int j = 0; j < mat.columns; j++)
                {
                    mat[i, j] = mat[i, j] / num;
                }
            }
            return mat;
        }


        private Integer[,] ToReferenceableMatrix(int[,] matrix)
        {
            var rows = matrix.GetLength(0);
            var columns = matrix.GetLength(1);
            var result = new Integer[rows, columns];
            for (int i=0; i< rows; i++)
            {
                for (int j=0; j< columns; j++)
                {
                    result[i, j] = new Integer(matrix[i,j]);
                }
            }
            return result;
        }
        

    }
}
