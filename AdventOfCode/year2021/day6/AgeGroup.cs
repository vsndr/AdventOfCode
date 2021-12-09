namespace AdventOfCode.day6
{
    public class AgeGroup
    {
        private int age;
        private long groupSize;

        public AgeGroup(int age, long groupSize)
        {
            this.age = age;
            this.groupSize = groupSize;
        }

        public void Tick()
        {
            age--;
        }

        public void AddToGroup(long numToAdd)
        {
            this.groupSize += numToAdd;
        }

        public long GetGroupSize()
        {
            return this.groupSize;
        }

        public int GetAge()
        {
            return this.age;
        }

        public bool IsReproducing()
        {
            return this.age < 0;
        }
    }
}
