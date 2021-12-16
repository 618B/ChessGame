using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChessGame.Moves;

namespace ChessGame.MoveRules
{
    class PawnMoveRule : PawnStepMoveRule
    {

        public PawnMoveRule(IPromotionProvider provider, Point direction, ChessBoard board):
            base(provider, direction, board)
        { }

        public override bool Attacking => false;

        protected override bool IsMoveValid(Point startPosition, Point endPosition)
        {
            if (board[endPosition.X, endPosition.Y] != null)
                return false;

            return base.IsMoveValid(startPosition, endPosition);
        }
    }
}
