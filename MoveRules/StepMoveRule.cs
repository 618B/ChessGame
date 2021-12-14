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
        public StepMoveRule(Point direction, ChessBorder border) :
            base(direction, border)
        { }

        protected override bool IsMoveValid(Point startPosition, Point endPosition)
        {
            Point stepTarget = new Point(startPosition.X + direction.X, startPosition.Y + direction.Y);

            return stepTarget == endPosition;
        }
    }
}
