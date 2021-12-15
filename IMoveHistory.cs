using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{
    interface IMoveHistory
    {

        bool LastMoveType<T>();

        ChessPiece LastMovePiece { get; }

        bool WasMoved(ChessPiece piece);
    }
}
