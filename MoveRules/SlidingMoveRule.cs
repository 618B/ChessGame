using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.MoveRules
{
    class SlidingMoveRule : MoveRule
    {
        public SlidingMoveRule(Point direction, ChessBoard board) :
            base(direction, board)
        { }

        protected override bool IsMoveValid(Point startPosition, Point endPosition)
        {
            Size diffrence = (Size)endPosition - (Size)startPosition;

            Point currentPosition = new Point(startPosition.X + direction.X, startPosition.Y + direction.Y);
            while (currentPosition.X != endPosition.X && currentPosition.Y != endPosition.Y)
            {
                if (!board.IsFieldExists(currentPosition.X, currentPosition.Y) ||
                    board[currentPosition.X, currentPosition.Y] != null)
                    return false;
                currentPosition.X += direction.X;
                currentPosition.Y += direction.Y;
            }
            if (currentPosition != endPosition)
                return false;

            return true;
        }

    }
}
