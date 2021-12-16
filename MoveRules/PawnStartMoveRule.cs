using ChessGame.Moves;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.MoveRules
{
    class PawnStartMoveRule : SlidingMoveRule
    {
        protected IMoveHistory history;
        protected Point movement;

        public PawnStartMoveRule(IMoveHistory history, Point movement, Point direction, ChessBoard board) : 
            base(direction, board)
        {
            this.history = history;
            this.movement = movement;
        }

        public override Move CreateMove(Point startPosition, Point endPosition)
        {
            return new PawnStartMove(startPosition, endPosition, board);
        }

        public override bool Attacking => false;

        protected override bool IsMoveValid(Point startPosition, Point endPosition)
        {
            if (new Point(startPosition.X + movement.X, startPosition.Y + movement.Y) != endPosition)
                return false;

            if (history.WasMoved(board[startPosition.X, startPosition.Y]))
                return false;

            if (board[endPosition.X, endPosition.Y] != null)
                return false;

            return base.IsMoveValid(startPosition, endPosition);
        }
    }
}
