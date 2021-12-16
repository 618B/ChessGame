using ChessGame.Moves;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.MoveRules
{
    class StepMoveRule : MoveRule
    {
        public StepMoveRule(Point direction, ChessBoard board) :
            base(direction, board)
        { }

        public override IEnumerable<Move> AvailableMoves(Point startPosition)
        {
            List<Move> moves = new List<Move>();

            Point endPosition = new Point(startPosition.X + direction.X, startPosition.Y + direction.Y);
            if (CanExecute(startPosition, endPosition))
                moves.Add(CreateMove(startPosition, endPosition));

            return moves;
        }

        public override Move CreateMove(Point startPosition, Point endPosition)
        {
            return new DefaultMove(startPosition, endPosition, board);
        }

        protected override bool IsMoveValid(Point startPosition, Point endPosition)
        {
            Point stepTarget = new Point(startPosition.X + direction.X, startPosition.Y + direction.Y);

            return stepTarget == endPosition;
        }
    }
}
