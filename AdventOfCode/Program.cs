namespace AdventOfCode
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var solver = new Day22Solver();
            var solution = solver.SolvePart2();

            System.Diagnostics.Debug.WriteLine(solution);
        }
    }
}
