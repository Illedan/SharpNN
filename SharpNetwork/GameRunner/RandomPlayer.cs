using System;
using TicTacToe.Game;

namespace GameRunner
{
    public class RandomPlayer: IPlayer
    {
        private Random rnd = new Random(42);
        public void Initialize(int playerId)
        {
        }

        public int GetMove(TicTacToe.Game.TicTacToe game)
        {
            var move = -1;
            while (!game.IsPossible(move))
            {
                move = rnd.Next(0, 9);
            }

            return move;
        }
    }
}