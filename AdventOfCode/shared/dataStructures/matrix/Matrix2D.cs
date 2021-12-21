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

        /// <summary>
        /// Returns the total number of elements in this matrix, i.e. rows * columns
        /// </summary>
        /// <returns></returns>
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
        /// Returns a new matrix where the left and right sides are flipped
        /// </summary>
        /// <returns></returns>
        public Matrix2D FlipVertically()
        {
            var result = new int[rows, columns];
            for (int i=0; i<rows; i++)
            {
                for (int j=0; j<columns; j++)
                {
                    var column = Math.Abs(j - columns) - 1;
                    result[i, column] = this[i, j];
                }
            }
            return new Matrix2D(result);
        }

        /// <summary>
        /// Returns a new matrix where the top and bottom sides are flipped
        /// </summary>
        /// <returns></returns>
        public Matrix2D FlipHorizontally()
        {
            var result = new int[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    var row = Math.Abs(i - rows) - 1;
                    result[row, j] = this[i, j];
                }
            }
            return new Matrix2D(result);
        }


        public Matrix2D Pad(int topSize = 0, int bottomSize = 0, int leftSize = 0, int rightSize = 0, int padNumber = 0)
        {
            var totalRows = rows + topSize + bottomSize;
            var totalColumns = columns + leftSize + rightSize;
            var result = new int[totalRows, totalColumns];
            //Set all numbers to the padding number
            for (int i=0; i < totalRows; i++)
            {
                for (int j = 0; j < totalColumns; j++)
                {
                    result[i, j] = padNumber;
                }
            }
            //Overwrite any number that's not part of the padding
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    result[i+topSize, j+leftSize] = this[i, j];
                }
            }

            return new Matrix2D(result);
        }

        /// <summary>
        /// Returns a prettified version of the matrix as a string
        /// </summary>
        /// <returns></returns>
        public string PrettyPrint(int? paddingWidth = null)
        {
            var result = new StringBuilder();
            var largestNumber = this.GetMax();
            var mostNumberOfDigits = 1;
            if (largestNumber >= 10)
            {
                mostNumberOfDigits = (int)Math.Log10(largestNumber);
            }

            var padding = paddingWidth.HasValue ? paddingWidth.Value : 2;

            for (int i = 0; i < this.rows; i++)
            {
                for (int j = 0; j < this.columns; j++)
                {
                    result.Append(this.internalMatrix[i, j].ToString().PadLeft(mostNumberOfDigits + padding));
                }
                result.AppendLine();
            }
            return result.ToString();
        }

        public Matrix2D Clone()
        {
            var array = this.As2DArray();
            return new Matrix2D(array);
        }

        public int[,] As2DArray()
        {
            var result = new int[this.rows, this.columns];
            for (int i = 0; i < this.rows; i++)
            {
                for (int j = 0; j < this.columns; j++)
                {
                    result[i, j] = this[i, j];
                }
            }
            return result;
        }

        public int[] Flatten()
        {
            var result = new int[this.rows * this.columns];
            for (int i=0; i< this.rows; i++)
            {
                for (int j=0; j<this.columns; j++)
                {
                    result[i * this.rows + j] = this[i, j];
                }
            }
            return result;
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

        /// <summary>
        /// Produces a new matrix by adding the two matrixes together.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Matrix2D operator +(Matrix2D a, Matrix2D b)
        {
            if (a.rows != b.rows || a.columns != b.columns)
            {
                throw new Exception("Failed to add two matrices. The matrix dimensions are not equal.");
            }

            var result = new int[a.rows, a.columns];
            for (int i = 0; i < a.rows; i++)
            {
                for (int j = 0; j < a.columns; j++)
                {
                    result[i, j] = a[i, j] + b[i, j];
                }
            }
            return new Matrix2D(result);
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
