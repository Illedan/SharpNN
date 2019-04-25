using System;
using System.Linq;

namespace TicTacToe.Game
{
    public class TicTacToe : IGame
    {
        private static readonly int[,] possibleWins = 
        {
            {0, 1, 2},
            {3, 4, 5},
            {6, 7, 8},
            {0, 3, 6},
            {1, 4, 7},
            {2, 5, 8},
            {0, 4, 8},
            {6, 4, 2}
        };
        
        private int[] board = new int[9];
        public bool IsPossible(int move) => move >= 0 && move < board.Length && board[move] == 0;

        public void DoMove(int move, int player)
        {
            if(!IsPossible(move)) throw new Exception("Invalid input");
            board[move] = player;
        }

        public void RevertMove(int move)
        {
            board[move] = 0;
        }

        public bool IsGameOver() => GetWinner() != 0 || board.All(b => b > 0);
        public void Reset()
        {
            board = new int[9];
        }

        public int GetWinner()
        {
            for (var i = 0; i < possibleWins.GetLength(0); i++)
            {
                var state = GetWinner(possibleWins[i, 0], possibleWins[i, 1], possibleWins[i, 2]);
                if (state != 0) return state;
            }

            return 0;
        }

        private int GetWinner(int p1, int p2, int p3)
        {
            if (board[p1] == 0) return 0;
            if (board[p1] == board[p2] && board[p2] == board[p3]) return board[p1];
            return 0;
        }

        public void PrintBoard()
        {
            for (var i = 0; i < 9; i++)
            {
                if (i % 3 == 0)
                {
                    Console.WriteLine();
                    Console.WriteLine();
                }
                if(board[i] == 0) Console.Write(" _");
                if(board[i] == 1) Console.Write(" x");
                if(board[i] == 2) Console.Write(" o");
            }
        }

        public IGame Clone()
        {
            var game = new TicTacToe();
            game.board = (int[])board.Clone();
            return game;
        }

        public int[] GetBoard() => board;
    }
}