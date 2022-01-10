using ChessGame.MoveRules;

namespace ChessGame.DefaultPieces
{
    class Rook : ChessPiece
    {
        public Rook(Side side, ChessBoard board) : 
            base(side)
        {
            Name = side == Side.White ? "R" : "r";

            Rules.Add(new SlidingMoveRule(new System.Drawing.Point(1, 0), board));
            Rules.Add(new SlidingMoveRule(new System.Drawing.Point(-1, 0), board));
            Rules.Add(new SlidingMoveRule(new System.Drawing.Point(0, 1), board));
            Rules.Add(new SlidingMoveRule(new System.Drawing.Point(0, -1), board));
        }
    }
}
