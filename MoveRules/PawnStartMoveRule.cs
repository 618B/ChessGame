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

        public PawnStartMoveRule(IMoveHistory history, Point direction, ChessBoard board) : 
            base(direction, board)
        {
            this.history = history;
        }

        public override Move CreateMove(Point startPosition, Point endPosition)
        {
            return new PawnStartMove(startPosition, endPosition, board);
        }


        protected override bool IsMoveValid(Point startPosition, Point endPosition)
        {
            if (history.WasMoved(board[startPosition.X, startPosition.Y]))
                return false;

            if (board[endPosition.X, endPosition.Y] != null)
                return false;

            return base.IsMoveValid(startPosition, endPosition);
        }
    }
}
