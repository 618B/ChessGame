using ChessGame.DefaultPieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{
    class PromotionManager : IPromotionProvider
    {
        string whitePromotionPiece;
        string blackPromotionPiece;
        ChessBoard board;

        public PromotionManager(ChessBoard board)
        {
            this.board = board;
        }

        public void SetPromotionPiece(Side side, string pieceName)
        {
            if (side == Side.White)
                whitePromotionPiece = pieceName;
            else
                blackPromotionPiece = pieceName;
        }

        public ChessPiece GetChessPiece(Side pieceSide)
        {
            var promotionPiece = pieceSide == Side.White ? whitePromotionPiece : blackPromotionPiece;
            switch (promotionPiece)
            {
                case "b":
                    return new Bishop(pieceSide, board);
                case "n":
                    return new Knight(pieceSide, board);
                case "r":
                    return new Rook(pieceSide, board);

                default:
                    return new Queen(pieceSide, board);
            }
        }
    }
}
