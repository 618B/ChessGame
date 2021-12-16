using ChessGame.Moves;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.MoveRules
{
    /// <summary>
    /// Взятие на проходе
    /// </summary>
    /// <typeparam name="T">Ход, против которого будет применяться правило</typeparam>
    class EnPassantRule<T> : StepMoveRule where T : Move
    {
        protected IMoveHistory moveHistory;

        public EnPassantRule(IMoveHistory moveHistory, Point direction, ChessBoard board) : base(direction, board)
        {
            this.moveHistory = moveHistory;
        }

        public override Move CreateMove(Point startPosition, Point endPosition)
        {
            return new EnPassantMove(new Point(startPosition.X + direction.X, startPosition.Y), startPosition, endPosition, board);
        }

        protected override bool IsMoveValid(Point startPosition, Point endPosition)
        {
            if (!board.IsFieldExists(startPosition.X + direction.X, endPosition.Y))
                return false;

            if(board[startPosition.X + direction.X, startPosition.Y] != moveHistory.LastMovePiece)
                return false;

            if (!moveHistory.LastMoveType<T>())
                return false;

            return base.IsMoveValid(startPosition, endPosition);
        }
    }
}
