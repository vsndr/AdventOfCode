using AdventOfCode.year2021.day21;
using System;
using System.Collections.Generic;
using System.Linq;

public class Day21Solver
{

    private const int P1_START = 7;
    private const int P2_START = 4;

    public int SolvePart1()
    {
        var p1Position = P1_START;
        var p2Position = P2_START;

        var p1Score = 0;
        var p2Score = 0;

        var turn = 0;

        while (p1Score < 1000 && p2Score < 1000)
        {
            var diceRollsTotal = this.Deterministic100SidedDie().Skip(3 * turn).Take(3).Sum();
            if (turn % 2 == 0)
            {
                p1Position = ((p1Position + diceRollsTotal - 1) % 10) + 1;
                p1Score += p1Position;
            }
            else
            {
                p2Position = ((p2Position + diceRollsTotal - 1) % 10) + 1;
                p2Score += p2Position;
            }
            turn++;
        }

        var totalDiceRolls = turn * 3;
        var losingPlayerScore = Math.Min(p1Score, p2Score);
        return totalDiceRolls * losingPlayerScore;
    }


    public long SolvePart2()
    {
        var diracDice = new DiracDice();
        (var p1Wins, var p2Wins) = diracDice.CountGamesWon(P1_START, P2_START);
        return Math.Max(p1Wins, p2Wins);
    }


    private IEnumerable<int> Deterministic100SidedDie()
    {
        var roll = 1;
        while(true)
        {
            yield return roll++;

            if (roll > 100)
            {
                roll = 1;
            }
        }
    }

}
