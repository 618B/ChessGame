using ChessGame.Moves;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.MoveRules
{
    abstract class PawnStepMoveRule : StepMoveRule
    {
        protected IPromotionProvider provider;
        protected PawnStepMoveRule(IPromotionProvider provider, Point direction, ChessBoard board) : 
            base(direction, board)
        {
            this.provider = provider;
        }

        public override Move CreateMove(Point startPosition, Point endPosition)
        {
            if (board.Size - 1 == endPosition.Y || endPosition.Y == 0)
                return new Promotion(provider.GetChessPiece(board[startPosition.X, startPosition.Y].Side), startPosition, endPosition, board);
            return base.CreateMove(startPosition, endPosition);
        }
    }
}
