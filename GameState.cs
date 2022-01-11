using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{
    class GameState
    {
        public Side Turn { get; private set; }

        public int FiftyMovesCount { get; private set; }

        public GameResult GameResult { get; private set; }

        public CheckState CheckState { get; private set; }

        Action<GameState> apply;

        public GameState(Side turn, int fiftyMovesCount, GameResult gameResult, CheckState checkState, Action<GameState> apply)
        {
            GameResult = gameResult;
            CheckState = checkState;
            Turn = turn;
            FiftyMovesCount = fiftyMovesCount;
        }

        public void Apply()
        {
            apply(this);
        }
    }
}
