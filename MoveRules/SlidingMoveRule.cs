using ChessGame.Moves;
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

        public override Move CreateMove(Point startPosition, Point endPosition)
        {
            return new DefaultMove(startPosition, endPosition, board);
        }

        protected override bool IsMoveValid(Point startPosition, Point endPosition)
        {
            Point currentPosition = new Point(startPosition.X + direction.X, startPosition.Y + direction.Y);
            while (board.IsFieldExists(currentPosition.X, currentPosition.Y) && currentPosition != endPosition)
            {
                if (board[currentPosition.X, currentPosition.Y] != null)
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
