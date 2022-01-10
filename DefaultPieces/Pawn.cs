using ChessGame.MoveRules;
using ChessGame.Moves;
using System.Drawing;

namespace ChessGame.DefaultPieces
{
    class Pawn : ChessPiece
    {
        public Pawn(Side side, IPromotionProvider promotionProvider, IMoveHistory moveHistory, ChessBoard board, bool startMove = true) : 
            base(side)
        {
            Name = side == Side.White ? "P" : "p";

            int moveDirectionMultiple = side != Side.White ? 1 : -1;

            Rules.Add(new PawnMoveRule(promotionProvider, new Point(0, 1 * moveDirectionMultiple), board));
            if (startMove)
                Rules.Add(new PawnStartMoveRule(moveHistory, new Point(0, 2 * moveDirectionMultiple), new Point(0, 1 * moveDirectionMultiple), board));
            Rules.Add(new PawnAttackMoveRule(promotionProvider, new Point(1, 1 * moveDirectionMultiple), board));
            Rules.Add(new PawnAttackMoveRule(promotionProvider, new Point(-1, 1 * moveDirectionMultiple), board));
            Rules.Add(new EnPassantRule<PawnStartMove>(moveHistory, new Point(1, 1 * moveDirectionMultiple), board));
            Rules.Add(new EnPassantRule<PawnStartMove>(moveHistory, new Point(-1, 1 * moveDirectionMultiple), board));
        }
    }
}
