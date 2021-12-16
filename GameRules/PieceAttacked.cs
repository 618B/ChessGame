using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.GameRules
{
    class PieceAttacked
    {
        protected ChessPiece piece;
        protected ChessBoard board;

        public PieceAttacked(ChessPiece piece, ChessBoard board)
        {
            this.piece = piece;
            this.board = board;
        }


        public bool IsApplied
        {
            get
            {
                Side attackSide = piece.Side == Side.White ? Side.Black : Side.White;
                System.Drawing.Point? piecePosition = board.FindPiece(piece);
                if (piecePosition == null)
                    throw new Exception("Piece for attack cheking not found!");
                return board.IsUnderAttack((System.Drawing.Point)piecePosition, attackSide);
            }
        }
    }
}
