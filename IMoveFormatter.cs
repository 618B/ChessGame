using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ChessGame
{
    interface IMoveFormatter
    {

        IMoveFormatter SetPoistions(Point startPosition, Point endPosition);

        IMoveFormatter SetMovablePiece(ChessPiece piece);

        IMoveFormatter SetAttackedPiece(ChessPiece piece);

        IMoveFormatter SetPromotion(ChessPiece piece);

        IMoveFormatter SetEnemyKingAttacked(bool attacked);

        IMoveFormatter SetCastling(bool longestCastling);

        string Result { get; }
    }
}
