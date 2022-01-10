using ChessGame.MoveRules;

namespace ChessGame.DefaultPieces
{
    class Bishop : ChessPiece
    {
        public Bishop(Side side, ChessBoard board) : base(side)
        {
            Name = side == Side.White ? "B" : "b";

            Rules.Add(new SlidingMoveRule(new System.Drawing.Point(1, 1), board));
            Rules.Add(new SlidingMoveRule(new System.Drawing.Point(1, -1), board));
            Rules.Add(new SlidingMoveRule(new System.Drawing.Point(-1, 1), board));
            Rules.Add(new SlidingMoveRule(new System.Drawing.Point(-1, -1), board));
        }
    }
}
