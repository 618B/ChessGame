using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Moves
{
    class PawnStartMove : DefaultMove
    {
        public PawnStartMove(Point startPoint, Point endPoint, ChessBoard board) : base(startPoint, endPoint, board)
        {
        }
    }
}
