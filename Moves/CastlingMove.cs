using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Moves
{
    enum CastlingType
    {
        Long, Short
    }

    class CastlingMove : Move
    {
        protected CastlingType moveType;
        protected Point startTargetPoint, endTargetPoint;

        public CastlingMove(CastlingType type, Point startTargetPoint, Point endTargetPoint, Point startPoint, Point endPoint, ChessBoard board) : 
            base(startPoint, endPoint, board)
        {
            this.moveType = type;
            this.startTargetPoint = startTargetPoint;
            this.endTargetPoint = endTargetPoint;
        }

        public override void Execute()
        {
            board[endTargetPoint.X, endTargetPoint.Y] = board[startTargetPoint.X, startTargetPoint.Y];
            board[startTargetPoint.X, startTargetPoint.Y] = null;
            board[endPoint.X, endPoint.Y] = piece;
            board[startPoint.X, startPoint.Y] = null;
        }

        public override void Serialize(IMoveFormatter formatter)
        {
            throw new NotImplementedException("Cant serialize castling move");
        }

        public override void Undo()
        {
            board[endTargetPoint.X, endTargetPoint.Y] = null;
            board[startTargetPoint.X, startTargetPoint.Y] = board[endTargetPoint.X, endTargetPoint.Y];
            board[endPoint.X, endPoint.Y] = null;
            board[startPoint.X, startPoint.Y] = piece;
        }

        public CastlingType CastlingType => moveType;
    }
}
