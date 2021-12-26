using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Moves
{
    class Promotion : Move 
    {
        protected ChessPiece startPiece, targetPiece, promotionPiece;

        public Promotion(ChessPiece promotionPiece, Point startPoint, Point endPoint, ChessBoard board) : 
            base(startPoint, endPoint, board)
        {
            this.startPiece = board[startPoint.X, startPoint.Y];
            this.targetPiece = board[endPoint.X, endPoint.Y];
            this.promotionPiece = promotionPiece;
        }

        public override void Execute()
        {
            board[startPoint.X, startPoint.Y] = null;
            board[endPoint.X, endPoint.Y] = promotionPiece;
        }

        public override void Serialize(IMoveFormatter formatter)
        {
            formatter.SetPoistions(startPoint, endPoint)
                .SetAttackedPiece(targetPiece)
                .SetPromotion(promotionPiece)
                .SetMovablePiece(piece);
        }

        public override void Undo()
        {
            board[startPoint.X, startPoint.Y] = startPiece;
            board[endPoint.X, endPoint.Y] = targetPiece;
        }
    }
}
