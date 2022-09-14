using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Text.Json;
using ChessGame.DefaultPieces;
using ChessGame.GameRules;
using ChessGame.Json;
using ChessGame.MoveManagement;

namespace ChessGame
{
    public enum GameResult
    {
        White, Black, Draw, Stalemate, OnGoing
    }

    public enum CheckState
    {
        None, White, Black
    }

    public class ChessGame
    {
        private readonly Dictionary<int, Mark> _marks = new();
        private int _marksNextKey = 0;

        readonly MoveManagement.MoveManager _history = new();

        ChessBoard _board = new();

        Dictionary<Point, List<Point>> _moves = new();


        FiftyMoves _fiftyMovesRule = new();

        FEN.FENGenerator _fen = new();

        King _whiteKing;
        King _blackKing;

        PieceAttacked _whiteKingAttacked;
        PieceAttacked _blackKingAttacked;

        PromotionManager _promotionManager;

        bool _currentSideHasMoves;

        public GameResult GameResult { get; private set; } = GameResult.OnGoing;

        public CheckState CheckState { get; private set; } = CheckState.None;

        public Side Turn { get; private set; } = Side.White;

        public List<Mark> Marks => _marks.Values.ToList();

        public ChessGame()
        {
            _promotionManager = new PromotionManager(_board);


            _board[1, 0] = new Knight(Side.Black, _board);
            _board[6, 0] = new Knight(Side.Black, _board);
            _board[1, 7] = new Knight(Side.White, _board);
            _board[6, 7] = new Knight(Side.White, _board);

            _board[2, 0] = new Bishop(Side.Black, _board);
            _board[5, 0] = new Bishop(Side.Black, _board);
            _board[2, 7] = new Bishop(Side.White, _board);
            _board[5, 7] = new Bishop(Side.White, _board);


            _board[0, 0] = new Rook(Side.Black, _board);
            _board[7, 0] = new Rook(Side.Black, _board);
            _board[0, 7] = new Rook(Side.White, _board);
            _board[7, 7] = new Rook(Side.White, _board);

            _board[3, 7] = new Queen(Side.White, _board);
            _board[3, 0] = new Queen(Side.Black, _board);

            _board[0, 1] = new Pawn(Side.Black, _promotionManager, _history, _board);
            _board[1, 1] = new Pawn(Side.Black, _promotionManager, _history, _board);
            _board[2, 1] = new Pawn(Side.Black, _promotionManager, _history, _board);
            _board[3, 1] = new Pawn(Side.Black, _promotionManager, _history, _board);
            _board[4, 1] = new Pawn(Side.Black, _promotionManager, _history, _board);
            _board[5, 1] = new Pawn(Side.Black, _promotionManager, _history, _board);
            _board[6, 1] = new Pawn(Side.Black, _promotionManager, _history, _board);
            _board[7, 1] = new Pawn(Side.Black, _promotionManager, _history, _board);

            _board[0, 6] = new Pawn(Side.White, _promotionManager, _history, _board);
            _board[1, 6] = new Pawn(Side.White, _promotionManager, _history, _board);
            _board[2, 6] = new Pawn(Side.White, _promotionManager, _history, _board);
            _board[3, 6] = new Pawn(Side.White, _promotionManager, _history, _board);
            _board[4, 6] = new Pawn(Side.White, _promotionManager, _history, _board);
            _board[5, 6] = new Pawn(Side.White, _promotionManager, _history, _board);
            _board[6, 6] = new Pawn(Side.White, _promotionManager, _history, _board);
            _board[7, 6] = new Pawn(Side.White, _promotionManager, _history, _board);

            _whiteKing = new King(Side.White, new Point(0, 7), new Point(7, 7), _history, _board);
            _blackKing = new King(Side.Black, new Point(0, 0), new Point(7, 0), _history, _board);


            _board[4, 7] = _whiteKing;
            _board[4, 0] = _blackKing;

            _whiteKingAttacked = new PieceAttacked(_whiteKing, _board);
            _blackKingAttacked = new PieceAttacked(_blackKing, _board);

            CalcMoves();
        }


        public string GetChessBoard()
        {
            return new FEN.FENGenerator()
                .AddBorderState(_board)
                .AddFiftyMovesRule(_fiftyMovesRule.MovesCount)
                .AddCastlingState(_whiteKing, _board)
                .AddCastlingState(_blackKing, _board)
                .AddSide(Turn)
                .AddEnPassant(_history, _board)
                .AddMovesCounter(0)//_history.MovesCount)
                .Result;
        }
        
        public bool IsMoveAllowed(Point start, Point end)
        {
            return _moves.ContainsKey(start) && _moves[start].Contains(end);
        }

        public void Move(Point start, Point end)
        {
            ClearMarks();
          
            if (!_moves.ContainsKey(start) || !_moves[start].Contains(end)) 
                return;
            
            ChessPiece piece = _board[start.X, start.Y];

            var rule = piece.Rules.Find((mv) => mv.CanExecute(start, end));
            if (rule == null) 
                return;
            
            var mv = rule.CreateMove(start, end);
            mv.Execute();

            _fiftyMovesRule.RegisterMove(mv);
            var stateBefore = new GameState(Turn, _fiftyMovesRule.MovesCount, GameResult, CheckState, SetState);
                
            CalcState();

            Turn = Turn == Side.White ? Side.Black : Side.White;
                

            CalcMoves();

            if (!_currentSideHasMoves)
            {
                if (CheckState != CheckState.None)
                    GameResult = Turn == Side.White ? GameResult.Black : GameResult.White;
                else
                    GameResult = GameResult.Stalemate;
            }

            GameState after = new GameState(Turn, _fiftyMovesRule.MovesCount, GameResult, CheckState, SetState);
            _history.AddMove(mv, stateBefore, after);

            CalcMoves();
           
        }

        public void ToStart()
        {
            ClearMarks();
            _history.PointerToStart();
            foreach (var historyMark in _history.Marks)
                AddMark(historyMark);
            CalcMoves();
        }

        public void ToEnd()
        {
            ClearMarks();
            _history.PointerToEnd();
            foreach (var historyMark in _history.Marks)
                AddMark(historyMark);
            CalcMoves();
        }

        public void Undo()
        {
            ClearMarks();
            _history.Undo();
            foreach (var historyMark in _history.Marks)
                AddMark(historyMark);
            CalcMoves();

        }

        public void Redo()
        {
            ClearMarks();
            _history.Redo();
            foreach (var historyMark in _history.Marks)
                AddMark(historyMark);
            CalcMoves();
        }

        public string MovePosition
        {
            get => _history.Position;
            set
            {
                ClearMarks();
                _history.Position = value;
                foreach (var historyMark in _history.Marks)
                    AddMark(historyMark);
                CalcMoves();
            }
        }

        public List<Point> PreviousMove => _history.PreviousMove;

        public Dictionary<Point, List<Point>> MoveList => new Dictionary<Point, List<Point>>(_moves);

        public void SetPromotionPiece(Side side, string pieceName)
        {
            _promotionManager.SetPromotionPiece(side, pieceName);
        }

        private void SetState(GameState state)
        {
            Turn = state.Turn;
            _fiftyMovesRule = new FiftyMoves(state.FiftyMovesCount);
            CheckState = state.CheckState;
            GameResult = state.GameResult;
        }


        private void CalcMoves()
        {
            _currentSideHasMoves = false;
            _moves = new Dictionary<Point, List<Point>>();

            var kingAttacked = Turn == Side.White ? _whiteKingAttacked : _blackKingAttacked;
            for (int i = 0; i < _board.Size; i++)
            {
                for (int j = 0; j < _board.Size; j++)
                {
                    if (_board[i, j] != null && _board[i, j].Side == Turn)
                    {
                        Point current = new Point(i, j);
                        _moves.Add(current, new List<Point>());

                        foreach (var move in _board[i, j].Rules.SelectMany(moveRule => moveRule.AvailableMoves(new Point(i, j))))
                        {
                            move.Execute();
                            if (kingAttacked == null || !kingAttacked.IsApplied)
                            {
                                _moves[current].Add(move.EndPoint);
                                _currentSideHasMoves = true;
                            }
                            move.Undo();
                        }
                    }
                }
            }
        }

        private void CalcState()
        {
            var enemyKingAttacked = Turn == Side.White ? _blackKingAttacked : _whiteKingAttacked;
            if (enemyKingAttacked.IsApplied)
                CheckState = Turn == Side.White ? CheckState.Black : CheckState.White;
            else
                CheckState = CheckState.None;

            if (!_currentSideHasMoves)
            {
                if (CheckState != CheckState.None)
                    GameResult = Turn == Side.White ? GameResult.Black : GameResult.White;
                else
                    GameResult = GameResult.Stalemate;
            }
        }

        public void AddComment(string comment)
        {
            _history.CommentCurrentMove(comment);
        }

        public void AddMark(Mark mark)
        {
            mark.UniqueKey = _marksNextKey;
            _marks.Add(_marksNextKey++, mark);
        }

        public void RemoveMark(int markKey)
        {
            if (!_marks.ContainsKey(markKey))
                return;
            _marks.Remove(markKey);
        }

        public void ClearMarks()
        {
            _marks.Clear();
            _marksNextKey = 0;
        }

        public void AddMoveMark(Mark mark)
        {
            _history.AddMark(mark);
        }

        public static ChessGame FromFEN(string fen)
        {
            ChessGame newGame = new ChessGame();

            var parser = new FEN.FENParser(fen, newGame._history, newGame.SetState);
            newGame._board = parser.ChessBoard;
            newGame._whiteKing = parser.WhiteKing;
            newGame._blackKing = parser.BlackKing;

            if (newGame._whiteKing != null)
                newGame._whiteKingAttacked = new PieceAttacked(newGame._whiteKing, newGame._board);
            if (newGame._blackKing != null)
                newGame._blackKingAttacked = new PieceAttacked(newGame._blackKing, newGame._board);

            newGame._fiftyMovesRule = new FiftyMoves(parser.FiftyMoves);

            newGame.Turn = parser.Turn == Side.White ? Side.Black : Side.White;

            newGame._promotionManager = parser.PromotionProvider;

            // TODO Кол-во ходов

            newGame.CalcMoves();

            newGame.CalcState();

            newGame.Turn = parser.Turn;

            newGame.CalcMoves();

            newGame.CalcState();

            return newGame;
        }

        
        public string CreatePGN()
        {
            PGN.PGNGenerator pGNGenerator = new PGN.PGNGenerator();

            _history.Serialize(pGNGenerator);

            return pGNGenerator.Result;
        }

        public string CreateMoveJson()
        {
            MovesJsonGenerator generator = new MovesJsonGenerator();

            _history.Serialize(generator);

            return generator.Result;
        }

        public static ChessGame FromPGN(string pgn)
        {
            var parser = new PGN.PGNParser(pgn);

            ChessGame game = new ChessGame();
            if (parser.FEN != string.Empty)
                game = FromFEN(parser.FEN);


            try
            {
                parser.ParseMoves(game, game._board);

            }
            catch (Exception ex) 
            { 
                Console.WriteLine("[ChessGame] Exception: " + ex.Message);
            }
            return game;
        }
    }
}
