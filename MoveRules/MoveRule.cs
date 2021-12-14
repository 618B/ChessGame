using System;
using System.Drawing;

namespace ChessGame.MoveRules
{
    abstract class MoveRule
    {
        protected readonly ChessBorder border;
        protected readonly Point direction;

        public MoveRule(Point direction, ChessBorder border)
        {
            if (direction.X == 0 && direction.Y == 0)
                throw new Exception("Move value cannot be zero!");
            
            this.direction = direction;
            this.border = border;
        }

        public bool CanExecute(Point startPosition, Point endPosition)
        {
            if (!border.IsFieldExists(startPosition.X, startPosition.Y) ||
                !border.IsFieldExists(endPosition.X, endPosition.Y))
                return false;

            ChessPiece startPiece = border[startPosition.X, startPosition.Y];
            ChessPiece endPiece = border[endPosition.X, endPosition.Y];
            if ((startPiece == null) ||
                ((endPiece != null) && (endPiece.Side == startPiece.Side)))
                return false;



            return IsMoveValid(startPosition, endPosition);
        }

        protected abstract bool IsMoveValid(Point startPosition, Point endPosition);
    }
}
