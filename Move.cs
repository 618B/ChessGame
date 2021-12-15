using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{
    abstract class Move
    {
        protected ChessBoard board;
        protected ChessPiece piece;

        public Move(ChessPiece piece,ChessBoard board)
        {
            this.board = board;
            this.piece = piece;
        }

        public abstract void Execute();

        public abstract void Undo();

        public ChessPiece Piece => piece;
    }
}
