using ChessGame.MoveRules;
using System;
using System.Drawing;

namespace ChessGame.DefaultPieces
{
    class King : ChessPiece
    {
        bool longCastlingAdded = false;
        bool shortCastlingAdded = false;

        public King(Side side, IMoveHistory moveHistory, ChessBoard board) : 
            base(side)
        {
            Name = side == Side.White ? "K" : "k";

            Rules.Add(new StepMoveRule(new Point(1, 1), board));
            Rules.Add(new StepMoveRule(new Point(1, -1), board));
            Rules.Add(new StepMoveRule(new Point(-1, 1), board));
            Rules.Add(new StepMoveRule(new Point(-1, -1), board));
            Rules.Add(new StepMoveRule(new Point(1, 0), board));
            Rules.Add(new StepMoveRule(new Point(-1, 0), board));
            Rules.Add(new StepMoveRule(new Point(0, 1), board));
            Rules.Add(new StepMoveRule(new Point(0, -1), board));
        }

        public King(Side side, Point leftRookPosition, Point rightRookPosition, IMoveHistory moveHistory, ChessBoard board) :
            this(side, moveHistory, board)
        {
            AddLongCastling(leftRookPosition, moveHistory, board);
            AddShortCastling(rightRookPosition, moveHistory, board);
        }

        public void AddLongCastling(Point leftRookPosition, IMoveHistory moveHistory, ChessBoard board)
        {
            if (longCastlingAdded)
                throw new Exception("Cant add castling long rule to King. Already exists");

            int yPosition = Side == Side.White ? 7 : 0;
            Rules.Add(new CastlingRule(CastlingType.Long, leftRookPosition, new Point(3, yPosition), new Point(2, yPosition), moveHistory, new Point(-1, 0), board));
        }

        public void AddShortCastling(Point rightRookPosition, IMoveHistory moveHistory, ChessBoard board)
        {
            if (shortCastlingAdded)
                throw new Exception("Cant add castling short rule to King. Already exists");

            int yPosition = Side == Side.White ? 7 : 0;
            Rules.Add(new CastlingRule(CastlingType.Short, rightRookPosition, new Point(5, yPosition), new Point(6, yPosition), moveHistory, new Point(1, 0), board));
        }
    }
}
