using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{
    interface IMoveStorage : IMoveHistory
    {

        public void AddMove(Move mv, GameState before, GameState after);
    }
}
