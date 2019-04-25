using System;
using System.Threading;
using TicTacToe.Game;

namespace GameRunner
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            int gameId = 0;
            while (true)
            {
                gameId++;
                
               // var game = gameId%2==0? new Game(new IPlayer[] { new NetworkPlayer(), new MinMaxPlayer(), }) : new Game(new IPlayer[] { new MinMaxPlayer(), new NetworkPlayer() });
              var game = new Game(new IPlayer[] {new MinMaxPlayer(), new NetworkPlayer()});
               //var game = new Game(new IPlayer[] {new NetworkPlayer(), new ConsolePlayer()});
              //var game = new Game(new IPlayer[] {new ConsolePlayer(), new NetworkPlayer()});
               var winner = game.PlayGame();
               Console.Error.WriteLine();
                Console.Error.WriteLine("GAME OVER, WINNER IS: " + winner + " - " + gameId);
                if (winner != 0)
                {
                    game._game.PrintBoard();
                    Thread.Sleep(5000);
                }
            }
        }

        public static void PrintGame(TicTacToe.Game.TicTacToe ticTacToe)
        {
            ticTacToe.PrintBoard();
        }
    }
}