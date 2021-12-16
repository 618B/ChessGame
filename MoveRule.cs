using System;
using System.Drawing;

namespace ChessGame
{
    abstract class MoveRule
    {
        protected readonly ChessBoard board;
        protected readonly Point direction;

        public MoveRule(Point direction, ChessBoard board)
        {
            if (direction.X == 0 && direction.Y == 0)
                throw new Exception("Move value cannot be zero!");
            
            this.direction = direction;
            this.board = board;
        }

        public bool CanExecute(Point startPosition, Point endPosition)
        {
            if (!board.IsFieldExists(startPosition.X, startPosition.Y) ||
                !board.IsFieldExists(endPosition.X, endPosition.Y))
                return false;

            ChessPiece startPiece = board[startPosition.X, startPosition.Y];
            ChessPiece endPiece = board[endPosition.X, endPosition.Y];
            if ((startPiece == null) ||
                ((endPiece != null) && (endPiece.Side == startPiece.Side)))
                return false;



            return IsMoveValid(startPosition, endPosition);
        }

        protected abstract bool IsMoveValid(Point startPosition, Point endPosition);

        public abstract Move CreateMove(Point startPosition, Point endPosition);
    }
}
