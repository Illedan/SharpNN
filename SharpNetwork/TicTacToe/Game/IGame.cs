namespace TicTacToe.Game
{
    public interface IGame
    {
        bool IsPossible(int move);
        void DoMove(int move, int player);
        void RevertMove(int move);
        bool IsGameOver();
        void Reset();
        int GetWinner();
        void PrintBoard();
        IGame Clone();
        int[] GetBoard();
    }
}