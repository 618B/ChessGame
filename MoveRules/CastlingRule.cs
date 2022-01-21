using System.Collections.Generic;
using System.Drawing;
using ChessGame.Moves;

namespace ChessGame.MoveRules
{ 
    enum CastlingType
    {
        Long, Short
    }

    class CastlingRule : MoveRule
    {
        protected ChessPiece changePiece;
        protected Point target, changePiecePosition, targetChangePiecePosition;
        protected IMoveHistory moveHistory;
        protected CastlingType type;

        public CastlingRule(CastlingType type, Point changePiecePosition, Point targetChangePiecePosition, Point target, IMoveHistory moveHistory, Point direction, ChessBoard board) : 
            base(direction, board)
        {
            changePiece = board[changePiecePosition.X, changePiecePosition.Y];
            this.type = type;
            this.target = target;
            this.moveHistory = moveHistory;
            this.changePiecePosition = changePiecePosition;
            this.targetChangePiecePosition = targetChangePiecePosition;
        }

        public override Move CreateMove(Point startPosition, Point endPosition)
        {

            return new CastlingMove((Moves.CastlingType)type, changePiecePosition, targetChangePiecePosition, startPosition, endPosition, board);
        }

        public override bool Attacking => false;

        protected override bool IsMoveValid(Point startPosition, Point endPosition)
        {
            if (changePiece == null ||
                moveHistory.WasMoved(changePiece) ||
                moveHistory.WasMoved(board[startPosition.X, startPosition.Y]) ||
                board[changePiecePosition.X, changePiecePosition.Y] == null)
                return false;

            if (endPosition != target)
                return false;

            Point currentPosition = new Point(startPosition.X + direction.X, startPosition.Y + direction.Y);
            while (board.IsFieldExists(currentPosition.X, currentPosition.Y) && currentPosition != changePiecePosition)
            {
                if (board[currentPosition.X, currentPosition.Y] != null)
                    return false;
                currentPosition.X += direction.X;
                currentPosition.Y += direction.Y;
            }
            if (currentPosition != changePiecePosition)
                return false;

            // Проверка короля на шах
            // Проверка пути на шах
            Side attackSide = board[currentPosition.X, currentPosition.Y].Side == Side.White ? Side.Black : Side.White;
            if (board.IsUnderAttack(startPosition, attackSide))
                return false;

            currentPosition = new Point(startPosition.X + direction.X, startPosition.Y + direction.Y);
            while (board.IsFieldExists(currentPosition.X, currentPosition.Y) && currentPosition != target)
            {
                if (board[currentPosition.X, currentPosition.Y] != null || board.IsUnderAttack(currentPosition, attackSide))
                    return false;
                currentPosition.X += direction.X;
                currentPosition.Y += direction.Y;
            }
            if (currentPosition != target)
                return false;

            if (board.IsUnderAttack(currentPosition, attackSide))
                return false;

            return true;
        }

        public override IEnumerable<Move> AvailableMoves(Point startPosition)
        {
            List<Move> moves = new List<Move>();

            if (CanExecute(startPosition, target))
                moves.Add(CreateMove(startPosition, target));

            return moves;
        }

        public bool MinimalRequirements(Point startPosition)
        {
            if (changePiece == null ||
                moveHistory.WasMoved(changePiece) ||
                moveHistory.WasMoved(board[startPosition.X, startPosition.Y]))
                return false;

            return true;
        }

        public CastlingType CastlingType => type;
    }
}
