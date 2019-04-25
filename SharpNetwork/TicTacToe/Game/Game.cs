namespace TicTacToe.Game
{
    public class Game
    {
        private readonly IPlayer[] m_players;
        private TicTacToe _game;
        public Game(IPlayer[] players)
        {
            m_players = players;
            _game = new TicTacToe();
        }

        /// <summary>
        /// Returns ID of winner 
        /// </summary>
        /// <returns></returns>
        public int PlayGame()
        {
            var player = 1;
            m_players[0].Initialize(1);
            m_players[1].Initialize(2);
            while (!_game.IsGameOver())
            {
                var currentPlayer = m_players[player - 1];
                var move = currentPlayer.GetMove(_game.Clone() as TicTacToe);
                _game.DoMove(move, player);
                player = player == 1 ? 2 : 1;
            }

            return _game.GetWinner();
        }
    }
}