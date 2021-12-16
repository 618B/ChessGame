﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Moves
{
    class CastlingMove : Move
    {
        protected Point startTargetPoint, endTargetPoint;

        public CastlingMove(Point startTargetPoint, Point endTargetPoint, Point startPoint, Point endPoint, ChessPiece piece, ChessBoard board) : 
            base(startPoint, endPoint, piece, board)
        {
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

        public override void Undo()
        {
            board[endTargetPoint.X, endTargetPoint.Y] = null;
            board[startTargetPoint.X, startTargetPoint.Y] = board[endTargetPoint.X, endTargetPoint.Y];
            board[endPoint.X, endPoint.Y] = null;
            board[startPoint.X, startPoint.Y] = piece;
        }
    }
}