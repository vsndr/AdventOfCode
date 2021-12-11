using AdventOfCode.utils;

namespace AdventOfCode.shared.dataStructures.matrix
{
    public static class SliceParser
    {

        /// <summary>
        /// Parses a string like "3:, 1:3" to (3, matrix2D.rows, 1, 3) 
        /// </summary>
        /// <param name="slicingOperator"></param>
        /// <param name="matrix2D"></param>
        /// <returns></returns>
        public static (int fromRow, int toRow, int fromCol, int toCol) ParseSlicingOperator(string slicingOperator, Matrix2D matrix2D)
        {
            //TODO: Some exception handling would be nice
            var indexers = slicingOperator.Split(',');
            var rowRange = ParseRowIndexer(indexers[0], matrix2D);
            var colRange = ParseColumnIndexer(indexers[1], matrix2D);
            return (rowRange.from, rowRange.to, colRange.from, colRange.to);
        }

        private static (int from, int to) ParseRowIndexer(string indexer, Matrix2D matrix2D)
        {
            indexer = indexer.RemoveWhitespace();
            if (indexer == ":")
            {
                return (0, matrix2D.rows);
            }
            else
            {
                return ParseRangeRow(indexer, matrix2D);
            }
        }

        private static (int from, int to) ParseColumnIndexer(string indexer, Matrix2D matrix2D)
        {
            indexer = indexer.RemoveWhitespace();
            if (indexer == ":")
            {
                return (0, matrix2D.columns);
            }
            else
            {
                return ParseRangeColumn(indexer, matrix2D);
            }
        }

        /// <summary>
        /// Parses a range such as:
        /// 1:3 -> (1, 3)
        /// :3 -> (0, 3)
        /// 3: -> (3, matrix2D.rows)
        /// </summary>
        /// <param name="rangeIndexer"></param>
        /// <param name="matrix2D"></param>
        /// <returns></returns>
        private static (int from, int to) ParseRangeRow(string rangeIndexer, Matrix2D matrix2D)
        {
            var fromTo = rangeIndexer.Split(':');
            var fromString = fromTo[0];
            var toString = fromTo[1];
            var from = 0;
            var to = 0;
            if (!string.IsNullOrEmpty(fromString))
            {
                from = int.Parse(fromString);
            }
            if (string.IsNullOrEmpty(toString))
            {
                to = matrix2D.rows;
            }
            else
            {
                to = int.Parse(toString) + 1;   //+1 because it's inclusive. for example: 1:3 means rows 1, 2 and 3
            }
            return (from, to);
        }

        /// <summary>
        /// Parses a range such as:
        /// 1:3 -> (1, 3)
        /// :3 -> (0, 3)
        /// 3: -> (3, matrix2D.columns)
        /// </summary>
        /// <param name="rangeIndexer"></param>
        /// <param name="matrix2D"></param>
        /// <returns></returns>
        private static (int from, int to) ParseRangeColumn(string rangeIndexer, Matrix2D matrix2D)
        {
            var fromTo = rangeIndexer.Split(':');
            var fromString = fromTo[0];
            var toString = fromTo[1];
            var from = 0;
            var to = 0;
            if (!string.IsNullOrEmpty(fromString))
            {
                from = int.Parse(fromString);
            }
            if (string.IsNullOrEmpty(toString))
            {
                to = matrix2D.columns;
            }
            else
            {
                to = int.Parse(toString) + 1;   //+1 because it's inclusive. for example: 1:3 means columns 1, 2 and 3
            }
            return (from, to);
        }

    }
}
