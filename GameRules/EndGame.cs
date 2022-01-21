using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.GameRules
{
    class EndGame
    {
        protected ChessBoard board;
        protected PieceAttacked whiteKingAttacked;
        protected PieceAttacked blackKingAttacked;

        public EndGame(ChessPiece whiteKing, ChessPiece blackKing, ChessBoard board)
        {
            this.board = board;
            whiteKingAttacked = new PieceAttacked(whiteKing, board);
            blackKingAttacked = new PieceAttacked(blackKing, board);
        }

        public bool SideHasMoves(Side side)
        {
            var kingAttacked = side == Side.White ? whiteKingAttacked : blackKingAttacked;
            for (int i = 0; i < board.Size; i++)
            {
                for(int j = 0; j < board.Size; j++)
                {
                    if (board[i, j] != null && board[i, j].Side == side)
                    {
                        foreach(var moveRule in board[i, j].Rules)
                        {
                            foreach(var move in moveRule.AvailableMoves(new System.Drawing.Point(i, j)))
                            {
                                move.Execute();
                                if (!kingAttacked.IsApplied)
                                {
                                    move.Undo();
                                    return true;
                                }
                                move.Undo();
                            }
                        }
                    }
                }
            }

            return false;
        }
    }
}
