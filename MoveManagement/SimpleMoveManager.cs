using System.Collections.Generic;
using System.Linq;

namespace ChessGame.MoveManagement
{
    internal class SimpleMoveManager : IMoveStorage
    {
        private readonly List<(GameState, Move, GameState)> _moves = new();
        private int _movesCount;

        public void AddMove(Move mv, GameState before, GameState after)
        {
            _moves.Add((before, mv, after));
            _movesCount++;
        }

        public void PopMove()
        {
            var move = _moves.Last();
            _moves.Remove(move);
            move.Item2.Undo();
            move.Item1.Apply();
            _movesCount--;
        }

        public void Serialize(IGameSerializer gameSerializer)
        {
           foreach (var (_, move, after) in _moves)
           {
               var formatter = gameSerializer.CreateMoveFormatter();
               move.Serialize(formatter);

               if (after.CheckState != CheckState.None)
                   formatter.SetEnemyKingAttacked();

               if (after.GameResult != GameResult.OnGoing)
                   formatter.SetMate();

               if (move.Piece.Side == Side.White)
                   gameSerializer.AddWhiteMove(formatter);
               else
                   gameSerializer.AddBlackMove(formatter);
           }
        }

        public ChessPiece LastMovePiece => _moves.Count == 0 ? null : _moves.Last().Item2.Piece;

        public bool LastMoveType<T>()
        {
            if (_moves.Count == 0)
                return false;

            return _moves.Last().Item2.GetType() == typeof(T);
        }

        public bool WasMoved(ChessPiece piece)
        {
            return _moves.Any(item => item.Item2.Piece == piece);
        }

        public int MovesCount => _movesCount / 2 + 1;
    }
}
