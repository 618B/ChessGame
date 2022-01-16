using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.PGN
{
    class PGNMoveFormatter : IMoveFormatter
    {
        bool kingAttacked;
        bool mate;
        string startPosition, endPosition;
        string movablePiece;
        bool attacking;
        string castling;
        string promotion;

        public IMoveFormatter SetAttackedPiece(ChessPiece piece)
        {
            if (piece != null)
                attacking = true;
            return this;
        }

        public IMoveFormatter SetEnemyKingAttacked()
        {
            kingAttacked = true;
            return this;
        }

        public IMoveFormatter SetLongCastling()
        {
            castling = "O-O-O";
            return this;
        }

        public IMoveFormatter SetMovablePiece(ChessPiece piece)
        {
            if (piece.Name != "p" && piece.Name != "P")
                movablePiece += piece.Name.ToUpper();
            return this;
        }

        public IMoveFormatter SetPoistions(Point startPosition, Point endPosition)
        {
            Dictionary<int, string> converter = new()
            {
                { 0, "a" },
                { 1, "b" },
                { 2, "c" },
                { 3, "d" },
                { 4, "e" },
                { 5, "f" },
                { 6, "g" },
                { 7, "h" }
            };
            this.startPosition = converter[startPosition.X] + (8 - startPosition.Y) .ToString();
            this.endPosition = converter[endPosition.X] + (8 - endPosition.Y).ToString();
            return this;
        }

        public IMoveFormatter SetPromotion(ChessPiece piece)
        {
            promotion += "=" + piece.Name.ToUpper();
            return this;
        }

        public IMoveFormatter SetShortCastling()
        {
            castling = "O-O";
            return this;
        }

        public IMoveFormatter SetMate()
        {
            mate = true;
            return this;
        }

        public string Result
        {
            get
            {
                if (!string.IsNullOrEmpty(castling))
                    return castling;

                string result = movablePiece;
                result += startPosition;
                result += attacking ? "x" : "";
                result += endPosition;
                result += promotion;
                if (mate)
                    result += "#";
                else
                    result += kingAttacked ? "+" : "";
                return result;
            }
        }

    }
}
