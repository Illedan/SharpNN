using System;
using TicTacToe.Game;

namespace GameRunner
{
    public class ConsolePlayer : IPlayer
    {
        public void Initialize(int playerId)
        {
        }

        public int GetMove(TicTacToe.Game.TicTacToe game)
        {
            game.PrintBoard();
            var move = -1;
            while (!game.IsPossible(move))
            {
                try
                {
                    Console.WriteLine();
                    Console.WriteLine("Write a valid move: (0-9)");
                    move = int.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    // yolo
                }
            }

            return move;
        }
    }
}