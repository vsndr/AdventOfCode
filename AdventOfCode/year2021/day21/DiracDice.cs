using System;
using System.Collections.Generic;

namespace AdventOfCode.year2021.day21
{
    public class DiracDice
    {

        public (long p1Wins, long p2Wins) CountGamesWon(int p1StartPosition, int p2StartPosition)
        {
            var initialGameState = new GameState
            {
                p1Score = 0,
                p2Score = 0,
                p1Position = p1StartPosition,
                p2Position = p2StartPosition,
                isP1ActivePlayer = true
            };
            var finalScores = CountGamesWon(initialGameState, new Dictionary<GameState, PlayerScores>());
            return (finalScores.p1Score, finalScores.p2Score);
        }


        private PlayerScores CountGamesWon(GameState gameState, Dictionary<GameState, PlayerScores> gameStateToScores)
        {
            if (gameState.p1Score >= 21)
            {
                return new PlayerScores(1, 0);
            }
            if (gameState.p2Score >= 21)
            {
                return new PlayerScores(0, 1);
            }
            if (gameStateToScores.TryGetValue(gameState, out var gameStatescores))
            {
                return gameStatescores;
            }

            var scores = new PlayerScores(0, 0);

            //"Roll" the 3 dices
            var distributions = this.GetTriple3SidedDiceDistribution();
            foreach ((var diceTotal, var frequency) in distributions)
            {
                var newGameState = GetGameStateAfterRoll(gameState, diceTotal);
                scores += CountGamesWon(newGameState, gameStateToScores) * frequency;
            }

            //Update the cache
            gameStateToScores.Add(gameState, scores);

            return scores;
        }

        private GameState GetGameStateAfterRoll(GameState gameState, int diceValue)
        {
            if (gameState.isP1ActivePlayer)
            {
                var newPosition = ((gameState.p1Position + diceValue - 1) % 10) + 1;
                var newScore = gameState.p1Score + newPosition;
                return new GameState
                {
                    p1Position = newPosition,
                    p1Score = newScore,
                    p2Position = gameState.p2Position,
                    p2Score = gameState.p2Score,
                    isP1ActivePlayer = false
                };
            }
            else
            {
                var newPosition = ((gameState.p2Position + diceValue - 1) % 10) + 1;
                var newScore = gameState.p2Score + newPosition;
                return new GameState
                {
                    p1Position = gameState.p1Position,
                    p1Score = gameState.p1Score,
                    p2Position = newPosition,
                    p2Score = newScore,
                    isP1ActivePlayer = true
                };
            }
        }

        private List<(int diceTotal, int frequency)> GetTriple3SidedDiceDistribution()
        {
            //With a 3-sided die, you have 3*3*3=27 possibilities, but only 7 real outcomes:
            //3     - 1/27 chance
            //4     - 3/27 chance
            //5     - 6/27 chance
            //6     - 7/27 chance
            //7     - 6/27 chance
            //8     - 3/27 chance
            //9     - 1/27 chance
            return new List<(int total, int frequency)>
            {
                (3, 1),
                (4, 3),
                (5, 6),
                (6, 7),
                (7, 6),
                (8, 3),
                (9, 1),
            };
        }


        private class GameState : IEquatable<GameState>
        {
            public int p1Score;
            public int p2Score;
            public int p1Position;
            public int p2Position;
            public bool isP1ActivePlayer;

            public override int GetHashCode()
            {
                unchecked
                {
                    int hash = 17;

                    hash = hash * 23 + p1Score.GetHashCode();
                    hash = hash * 23 + p2Score.GetHashCode();
                    hash = hash * 23 + p1Position.GetHashCode();
                    hash = hash * 23 + p2Position.GetHashCode();
                    hash = hash * 23 + isP1ActivePlayer.GetHashCode();
                    return hash;

                    //var result = 0;
                    //result = (result * 397) ^ p1Score;
                    //result = (result * 397) ^ p2Score;
                    //result = (result * 397) ^ p1Position;
                    //result = (result * 397) ^ p2Position;
                    //result = (result * 397) ^ (isP1ActivePlayer? 1 : 0);
                    //return result;
                }
            }

            public bool Equals(GameState other)
            {
                return this.p1Score == other.p1Score &&
                    this.p2Score == other.p2Score &&
                    this.p1Position == other.p1Position &&
                    this.p2Position == other.p2Position &&
                    this.isP1ActivePlayer == other.isP1ActivePlayer;
            }
        }


        private struct PlayerScores
        {
            public readonly long p1Score;
            public readonly long p2Score;

            public PlayerScores(long p1Score, long p2Score)
            {
                this.p1Score = p1Score;
                this.p2Score = p2Score;
            }

            public static PlayerScores operator +(PlayerScores score1, PlayerScores score2)
                => new PlayerScores(score1.p1Score + score2.p1Score, score1.p2Score + score2.p2Score);

            public static PlayerScores operator *(PlayerScores score, int multiplyer)
                => new PlayerScores(score.p1Score * multiplyer, score.p2Score * multiplyer);
        }

    }
}
