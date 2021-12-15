using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.MoveRules
{
    class PawnMoveRule : StepMoveRule
    {

        public PawnMoveRule(Point direction, ChessBoard board):
            base(direction, board)
        { }

        protected override bool IsMoveValid(Point startPosition, Point endPosition)
        {
            if (board[endPosition.X, endPosition.Y] != null)
                return false;

            return base.IsMoveValid(startPosition, endPosition);
        }
    }
}
