using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Moves
{
    class EnPassantMove : Move
    {
        protected Point targetPiecePoint;
        protected ChessPiece targetPiece;

        public EnPassantMove(Point targetPiecePoint, Point startPoint, Point endPoint, ChessBoard board) : 
            base(startPoint, endPoint, board)
        {
            this.targetPiecePoint = targetPiecePoint;
        }

        public override void Execute()
        {
            targetPiece = board[targetPiecePoint.X, targetPiecePoint.Y];
            board[endPoint.X, endPoint.Y] = piece;
            board[startPoint.X, startPoint.X] = null;
            board[targetPiecePoint.X, targetPiecePoint.Y] = null;
        }

        public override void Undo()
        {
            board[endPoint.X, endPoint.Y] = null;
            board[startPoint.X, startPoint.X] = piece;
            board[targetPiecePoint.X, targetPiecePoint.Y] = targetPiece;
        }
    }
}
