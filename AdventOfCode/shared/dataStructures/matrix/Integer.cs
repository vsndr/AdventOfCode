namespace AdventOfCode.shared.dataStructures.matrix
{
    /// <summary>
    /// A glorified int to enable copying an int by reference (normally an int is copied by value in C#)
    /// </summary>
    public class Integer
    {

        public int internalInt;

        public Integer(int number)
        {
            this.internalInt = number;
        }

        override
        public string ToString()
        {
            return internalInt.ToString();
        }
    }
}
