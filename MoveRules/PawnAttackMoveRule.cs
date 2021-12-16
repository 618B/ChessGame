using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.MoveRules
{
    class PawnAttackMoveRule : PawnStepMoveRule
    {
        public PawnAttackMoveRule(IPromotionProvider provider, Point direction, ChessBoard board) : 
            base(provider, direction, board)
        {
        }

        protected override bool IsMoveValid(Point startPosition, Point endPosition)
        {
            if (board[endPosition.X, endPosition.Y] == null)
                return false;

            return base.IsMoveValid(startPosition, endPosition);
        }

    }
}
