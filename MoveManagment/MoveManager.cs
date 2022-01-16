using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.MoveManagment
{
    class MoveManager : IMoveStorage
    {
        List<(GameState, Move, GameState)> moves = new();
        int movesCount = 0;

        public void AddMove(Move mv, GameState before, GameState after)
        {
            moves.Add((before, mv, after));
            movesCount++;
        }

        public void PopMove()
        {
            var move = moves.Last();
            moves.Remove(move);
            move.Item2.Undo();
            move.Item1.Apply();
            movesCount--;
        }

        public void Serialize(IGameSerializer gameSerializer)
        {
           foreach (var item in moves)
            {
                IMoveFormatter formatter = gameSerializer.CreateMoveFormatter();
                item.Item2.Serialize(formatter);

                if (item.Item3.CheckState != CheckState.None)
                    formatter.SetEnemyKingAttacked();

                if (item.Item3.GameResult != GameResult.OnGoing)
                    formatter.SetMate();

                if (item.Item2.Piece.Side == Side.White)
                    gameSerializer.AddWhiteMove(formatter);
                else
                    gameSerializer.AddBlackMove(formatter);
            }
        }

        public ChessPiece LastMovePiece => moves.Count == 0 ? null : moves.Last().Item2.Piece;

        public bool LastMoveType<T>()
        {
            if (moves.Count == 0)
                return false;

            return moves.Last().Item2.GetType() == typeof(T);
        }

        public bool WasMoved(ChessPiece piece)
        {
            foreach (var item in moves)
            {
                if (item.Item2.Piece == piece)
                    return true;
            }
            
            return false;
        }

        public int MovesCount => movesCount / 2 + 1;
    }
}
