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
        }

        public override void Execute()
        {
            targetPiece = board[endPoint.X, endPoint.Y];
            board[endPoint.X, endPoint.Y] = piece;
            board[startPoint.X, endPoint.Y] = null;
        }

        public override void Undo()
        {
            board[endPoint.X, endPoint.Y] = targetPiece;
            board[startPoint.X, endPoint.Y] = piece;
        }
    }
}
