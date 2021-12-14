using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{
    enum Side
    {
        White, Black
    }

    class ChessPiece
    {
        readonly Side _side;

        public ChessPiece(Side side)
        {
            this._side = side;
        }
    }
}
