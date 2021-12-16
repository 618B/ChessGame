using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{
    abstract class Move
    {
        protected ChessBoard board;
        protected ChessPiece piece;

        protected Point startPoint, endPoint;

        public Move(Point startPoint, Point endPoint, ChessBoard board)
        {
            this.board = board;
            this.piece = board[startPoint.X, startPoint.Y];
            this.startPoint = startPoint;
            this.endPoint = endPoint;
        }

        public abstract void Execute();

        public abstract void Undo();

        public ChessPiece Piece => piece;
    }
}
