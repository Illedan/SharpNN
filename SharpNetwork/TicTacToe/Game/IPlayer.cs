namespace TicTacToe.Game
{
    public interface IPlayer
    {
        void Initialize(int playerId);
        int GetMove(TicTacToe game);
    }
}