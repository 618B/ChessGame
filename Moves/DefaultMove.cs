using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Moves
{
    class DefaultMove : Move
    {
        protected ChessPiece targetPiece;

        public DefaultMove(Point startPoint, Point endPoint, ChessBoard board) : 
            base(startPoint, endPoint, board)
        {
            targetPiece = board[endPoint.X, endPoint.Y];
        }

        public override void Execute()
        {
            board[endPoint.X, endPoint.Y] = piece;
            board[startPoint.X, startPoint.Y] = null;
        }

        public override void Serialize(IMoveFormatter formatter)
        {
            formatter.SetPoistions(startPoint, endPoint)
                .SetAttackedPiece(targetPiece)
                .SetMovablePiece(piece);
        }

        public override void Undo()
        {
            board[endPoint.X, endPoint.Y] = targetPiece;
            board[startPoint.X, startPoint.Y] = piece;
        }
    }
}
