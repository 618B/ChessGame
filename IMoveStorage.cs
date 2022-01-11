using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{
    interface IMoveStorage : IMoveHistory
    {

        public void PushMove(Move mv, GameState turn);
    }
}
