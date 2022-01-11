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

        Action<Side, int> apply;

        public GameState(Side turn, int fiftyMovesCount, Action<Side, int> apply)
        {
            Turn = turn;
            FiftyMovesCount = fiftyMovesCount;
        }

        public void Apply()
        {
            apply(Turn, FiftyMovesCount);
        }
    }
}
