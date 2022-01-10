using ChessGame.MoveRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.DefaultPieces
{
    class Knight : ChessPiece
    {
        public Knight(Side side, ChessBoard board) : 
            base(side)
        {
            Name = side == Side.White ? "N" : "n";

            Rules.Add(new StepMoveRule(new System.Drawing.Point(2, 1), board));
            Rules.Add(new StepMoveRule(new System.Drawing.Point(2, -1), board));
            Rules.Add(new StepMoveRule(new System.Drawing.Point(-2, 1), board));
            Rules.Add(new StepMoveRule(new System.Drawing.Point(-2, -1), board));
            Rules.Add(new StepMoveRule(new System.Drawing.Point(1, 2), board));
            Rules.Add(new StepMoveRule(new System.Drawing.Point(-1, 2), board));
            Rules.Add(new StepMoveRule(new System.Drawing.Point(1, -2), board));
            Rules.Add(new StepMoveRule(new System.Drawing.Point(-1, -2), board));
        }
    }
}
