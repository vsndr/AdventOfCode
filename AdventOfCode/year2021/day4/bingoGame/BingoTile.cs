namespace AdventOfCode.day4.bingoGame
{
    public class BingoTile
    {
        private readonly int number;
        private bool isMarked;

        public BingoTile(int number)
        {
            this.number = number;
        }

        public void MarkTile()
        {
            this.isMarked = true;
        }

        public bool IsMarked()
        {
            return this.isMarked;
        }

        public int GetNumber()
        {
            return this.number;
        }
    }
}
