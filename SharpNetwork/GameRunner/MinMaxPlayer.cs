using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TicTacToe.Game;

namespace GameRunner
{
    public class MinMaxPlayer : IPlayer
    {
        private static HashSet<int> _data = new HashSet<int>();
        private int _playerId;
        private int OtherPlayer => _playerId == 1 ? 2 : 1;
        public void Initialize(int playerId)
        {
            _playerId = playerId;
        }
        private static readonly Random rnd = new Random(42);
        public int GetMove(TicTacToe.Game.TicTacToe game)
        {
            if (rnd.NextDouble() < 0.5)
            {
                var move = -1;
                while (!game.IsPossible(move))
                {
                    move = rnd.Next(0, 9);
                }

                return move;
            }
            var bestMoves = new List<int>();
            var bestScore = -10;
            var a = -100;
            var b = 100;
            for (var i = 0; i < 9; i++)
            {
                if (game.IsPossible(i))
                {
                    game.DoMove(i, _playerId);
                    var score = Search(game, a, b, false);
                    if (score > bestScore)
                    {
                        bestMoves.Clear();
                        bestMoves.Add(i);
                        bestScore = score;
                    }
                    else if (score == bestScore)
                    {
                        bestMoves.Add(i);
                    }
                    
                    game.RevertMove(i);
                }
            }

            var best = bestMoves.OrderBy(i => rnd.NextDouble()).First();
            var state = game.GetBoard();
            if(_data.Add(GetHash(state)))
                FileSaver.AddData(string.Join(",", state.Select(s => s == _playerId?1:(0)).Concat(state.Select(s => s == _playerId?0:(s==OtherPlayer?1:0)))) + "," + best+"\n");
            // Console.Error.WriteLine();
            // Console.Error.WriteLine("TIME: " + s.ElapsedMilliseconds);
            // Console.Error.WriteLine("BestScore: " + bestScore);
            return best;
        }

        private int GetHash(int[] state)
        {
            var val = 0;
            var multi = 10;
            for (var i = 0; i < state.Length; i++)
            {
                multi *= 10;
                val += multi * state[i];
            }

            return val;
        }

        private int Search(TicTacToe.Game.TicTacToe game, int alpha, int beta, bool maximizing)
        {
            if (game.IsGameOver())
            {
                var winner = game.GetWinner();
                if (winner == _playerId) return 1;
                if (winner == 0) return 0;
                return -1;
            }

            if (maximizing)
            {
                var value = -100;
                for (var i = 0; i < 9; i++)
                {
                    if (game.IsPossible(i))
                    {
                        game.DoMove(i, _playerId);
                        var score = Search(game, alpha, beta, false);
                        game.RevertMove(i);
                        value = Math.Max(score, value);
                        alpha = Math.Max(alpha, value);
                        if (alpha >= beta)
                            return value;
                    }
                }

                return value;
            }
            else
            {
                var value = 100;
                for (var i = 0; i < 9; i++)
                {
                    if (game.IsPossible(i))
                    {
                        game.DoMove(i, OtherPlayer);
                        var score = Search(game, alpha, beta, true);
                        game.RevertMove(i);
                        value = Math.Min(score, value);
                        beta = Math.Min(beta, value);
                        if (alpha >= beta)
                            return value;
                    }
                }

                return value; 
            }
        }
    }
}