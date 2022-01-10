using ChessGame.MoveRules;

namespace ChessGame.DefaultPieces
{
    class Queen : ChessPiece
    {
        public Queen(Side side, ChessBoard board) : 
            base(side)
        {
            Name = side == Side.White ? "Q" : "q";

            Rules.Add(new SlidingMoveRule(new System.Drawing.Point(1, 1), board));
            Rules.Add(new SlidingMoveRule(new System.Drawing.Point(1, -1), board));
            Rules.Add(new SlidingMoveRule(new System.Drawing.Point(-1, 1), board));
            Rules.Add(new SlidingMoveRule(new System.Drawing.Point(-1, -1), board));
            Rules.Add(new SlidingMoveRule(new System.Drawing.Point(1, 0), board));
            Rules.Add(new SlidingMoveRule(new System.Drawing.Point(-1, 0), board));
            Rules.Add(new SlidingMoveRule(new System.Drawing.Point(0, 1), board));
            Rules.Add(new SlidingMoveRule(new System.Drawing.Point(0, -1), board));
        }
    }
}
