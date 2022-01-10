using ChessGame.DefaultPieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.GameRules
{
    class FiftyMoves
    {
        int movesCount = 0;

        public int MovesCount => movesCount;

        public FiftyMoves(int movesCount = 0)
        {
            this.movesCount = movesCount;
        }

        public void RegisterMove(Move mv)
        {
            if (mv.Attacked || mv.Piece is Pawn)
                movesCount = 0;
            else
                movesCount++;
        }
    }
}
