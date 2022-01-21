using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using ChessGame.DefaultPieces;
using ChessGame.GameRules;
using ChessGame.MoveManagment;

namespace ChessGame
{
    public enum GameResult
    {
        White, Black, Draw, Stalemate, OnGoing
    }

    public enum CheckState
    {
        White, Black, None
    }

    public class ChessGame
    {


        MoveStorage history = new();

        ChessBoard board = new();

        Dictionary<Point, List<Point>> moves = new();


        FiftyMoves fiftyMovesRule = new();

        FEN.FENGenerator fen = new();

        King whiteKing;
        King blackKing;

        PieceAttacked whiteKingAttacked;
        PieceAttacked blackKingAttacked;

        PromotionManager promotionManager;

        bool currentSideHasMoves;

        public GameResult GameResult { get; private set; } = GameResult.OnGoing;

        public CheckState CheckState { get; private set; } = CheckState.None;

        public Side Turn { get; private set; } = Side.White;

        public string FUCK { get; private set; }

        public ChessGame()
        {
            promotionManager = new PromotionManager(board);


            board[1, 0] = new Knight(Side.Black, board);
            board[6, 0] = new Knight(Side.Black, board);
            board[1, 7] = new Knight(Side.White, board);
            board[6, 7] = new Knight(Side.White, board);

            board[2, 0] = new Bishop(Side.Black, board);
            board[5, 0] = new Bishop(Side.Black, board);
            board[2, 7] = new Bishop(Side.White, board);
            board[5, 7] = new Bishop(Side.White, board);


            board[0, 0] = new Rook(Side.Black, board);
            board[7, 0] = new Rook(Side.Black, board);
            board[0, 7] = new Rook(Side.White, board);
            board[7, 7] = new Rook(Side.White, board);

            board[3, 7] = new Queen(Side.White, board);
            board[3, 0] = new Queen(Side.Black, board);

            board[0, 1] = new Pawn(Side.Black, promotionManager, history, board);
            board[1, 1] = new Pawn(Side.Black, promotionManager, history, board);
            board[2, 1] = new Pawn(Side.Black, promotionManager, history, board);
            board[3, 1] = new Pawn(Side.Black, promotionManager, history, board);
            board[4, 1] = new Pawn(Side.Black, promotionManager, history, board);
            board[5, 1] = new Pawn(Side.Black, promotionManager, history, board);
            board[6, 1] = new Pawn(Side.Black, promotionManager, history, board);
            board[7, 1] = new Pawn(Side.Black, promotionManager, history, board);

            board[0, 6] = new Pawn(Side.White, promotionManager, history, board);
            board[1, 6] = new Pawn(Side.White, promotionManager, history, board);
            board[2, 6] = new Pawn(Side.White, promotionManager, history, board);
            board[3, 6] = new Pawn(Side.White, promotionManager, history, board);
            board[4, 6] = new Pawn(Side.White, promotionManager, history, board);
            board[5, 6] = new Pawn(Side.White, promotionManager, history, board);
            board[6, 6] = new Pawn(Side.White, promotionManager, history, board);
            board[7, 6] = new Pawn(Side.White, promotionManager, history, board);

            whiteKing = new King(Side.White, new Point(0, 7), new Point(7, 7), history, board);
            blackKing = new King(Side.Black, new Point(0, 0), new Point(7, 0), history, board);


            board[4, 7] = whiteKing;
            board[4, 0] = blackKing;

            whiteKingAttacked = new PieceAttacked(whiteKing, board);
            blackKingAttacked = new PieceAttacked(blackKing, board);

            CalcMoves();
        }


        public string GetChessBoard()
        {
            return new FEN.FENGenerator()
                .AddBorderState(board)
                .AddFiftyMovesRule(fiftyMovesRule.MovesCount)
                .AddCastlingState(whiteKing, board)
                .AddCastlingState(blackKing, board)
                .AddSide(Turn)
                .AddEnPassant(history, board)
                .AddMovesCounter(history.MovesCount)
                .Result;
        }

        int counter = 0;
        int ctr = 0;

        public bool IsMoveAllowed(Point start, Point end)
        {
            return moves.ContainsKey(start) && moves[start].Contains(end);
        }

        public void Move(Point start, Point end)
        {
            counter++;
          //  if (moves)
          //      return;

            if (moves.ContainsKey(start) && moves[start].Contains(end))
            {
                ctr++;
                ChessPiece piece = board[start.X, start.Y];

                var rule = piece.Rules.Find((mv) => { return mv.CanExecute(start, end); });
                if (rule != null)
                {
                    var mv = rule.CreateMove(start, end);
                    mv.Execute();

                    fiftyMovesRule.RegisterMove(mv);
                    GameState before = new GameState(Turn, fiftyMovesRule.MovesCount, GameResult, CheckState, SetState);
                    //history.PushMove(mv, before);

                    CalcState();

                    Turn = Turn == Side.White ? Side.Black : Side.White;

                    //CalcState();


                    CalcMoves();

                    if (!currentSideHasMoves)
                    {
                        if (CheckState != CheckState.None)
                            GameResult = Turn == Side.White ? GameResult.Black : GameResult.White;
                        else
                            GameResult = GameResult.Stalemate;
                    }

                    GameState after = new GameState(Turn, fiftyMovesRule.MovesCount, GameResult, CheckState, SetState);
                    history.AddMove(mv, before, after);

                    CalcMoves();
                }
            }

            FUCK = counter.ToString() + "   " + ctr.ToString();
        }

        public string CurrentMovePointer
        {
            get
            {
                string result = string.Empty;
                foreach (var item in history.Position)
                    result += item.ToString() + " ";
                return result;
            }
        }

        public void ToStart()
        {
            history.PointerToStart();
            CalcMoves();
        }

        public void ToEnd()
        {
            history.PointerToEnd();
            CalcMoves();
        }

        public void Undo()
        {
            history.Undo();

            CalcMoves();

        }

        public void Redo()
        {
            history.Redo();

            CalcMoves();
        }

        public void ToMove(IList<int> pointer)
        {
            history.SetPositiion(pointer);

            CalcMoves();
        }

        public Dictionary<Point, List<Point>> MoveList => new Dictionary<Point, List<Point>>(moves);

        public void SetPromotionPiece(Side side, string pieceName)
        {
            promotionManager.SetPromotionPiece(side, pieceName);
        }

        private void SetState(GameState state)
        {
            Turn = state.Turn;
            fiftyMovesRule = new FiftyMoves(state.FiftyMovesCount);
            CheckState = state.CheckState;
            GameResult = state.GameResult;
        }


        private void CalcMoves()
        {
            currentSideHasMoves = false;
            moves = new Dictionary<Point, List<Point>>();

            var kingAttacked = Turn == Side.White ? whiteKingAttacked : blackKingAttacked;
            for (int i = 0; i < board.Size; i++)
            {
                for (int j = 0; j < board.Size; j++)
                {
                    if (board[i, j] != null && board[i, j].Side == Turn)
                    {
                        Point current = new Point(i, j);
                        moves.Add(current, new List<Point>());

                        foreach (var moveRule in board[i, j].Rules)
                        {
                            foreach (var move in moveRule.AvailableMoves(new Point(i, j)))
                            {
                                move.Execute();
                                if (kingAttacked == null || !kingAttacked.IsApplied)
                                {
                                    moves[current].Add(move.EndPoint);
                                    currentSideHasMoves = true;
                                }
                                move.Undo();
                            }
                        }
                    }
                }
            }
        }

        private void CalcState()
        {
            var enemyKingAttacked = Turn == Side.White ? blackKingAttacked : whiteKingAttacked;
            if (enemyKingAttacked.IsApplied)
                CheckState = Turn == Side.White ? CheckState.Black : CheckState.White;
            else
                CheckState = CheckState.None;

            if (!currentSideHasMoves)
            {
                if (CheckState != CheckState.None)
                    GameResult = Turn == Side.White ? GameResult.Black : GameResult.White;
                else
                    GameResult = GameResult.Stalemate;
            }
        }

        


        public static ChessGame FromFEN(string fen)
        {
            ChessGame newGame = new ChessGame();

            var parser = new FEN.FENParser(fen, newGame.history, newGame.SetState);
            newGame.board = parser.ChessBoard;
            newGame.whiteKing = parser.WhiteKing;
            newGame.blackKing = parser.BlackKing;

            if (newGame.whiteKing != null)
                newGame.whiteKingAttacked = new PieceAttacked(newGame.whiteKing, newGame.board);
            if (newGame.blackKing != null)
                newGame.blackKingAttacked = new PieceAttacked(newGame.blackKing, newGame.board);

            newGame.fiftyMovesRule = new FiftyMoves(parser.FiftyMoves);

            newGame.Turn = parser.Turn == Side.White ? Side.Black : Side.White;

            newGame.promotionManager = parser.PromotionProvider;

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

            history.Serialize(pGNGenerator);

            return pGNGenerator.Result;
        }


        public static ChessGame FromPGN(string pgn)
        {
            var parser = new PGN.PGNParser(pgn);

            ChessGame game = new ChessGame();
            if (parser.FEN != string.Empty)
                game = FromFEN(parser.FEN);


            try
            {
                parser.ParseMoves(game, game.board);

            }
            catch (Exception ex) 
            { 
              // game.FUCK = ex.ToString(); 
            }
            return game;
        }
    }
}
