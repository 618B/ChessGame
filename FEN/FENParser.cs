using ChessGame.DefaultPieces;
using ChessGame.MoveRules;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.FEN
{
    class FENParser
    {
        IMoveStorage moveHistory;
        IPromotionProvider promotionProvider;

        public King WhiteKing { get; private set; }

        public King BlackKing { get; private set; }
        
        public ChessBoard ChessBoard { get; private set; }

        public Side Turn { get; private set; } 

        public int FiftyMoves { get; private set; }
        
        public int MovesCount { get; private set; }

        Action<GameState> setGameState;
        


        public FENParser(string fen, IMoveStorage moveHistory, IPromotionProvider promotionProvider, Action<GameState> setGameState)
        {
            this.moveHistory = moveHistory;
            this.promotionProvider = promotionProvider;
            this.setGameState = setGameState;

            string[] fenFragments = fen.Split(' ');
            ChessBoard = ParseBoard(fenFragments[0]);

            if (fenFragments[1] != "w" && fenFragments[1] != "b")
                throw new Exception("Cant parse FEN turn");

            Turn = fenFragments[1] == "w" ? Side.White : Side.Black;

            ParseCastling(fenFragments[2]);
            ParseEnPassant(fenFragments[3]);
            ParseFiftyMoves(fenFragments[4]);
            ParseMovesCount(fenFragments[5]);
        }

        private ChessBoard ParseBoard(string fenBoard)
        {
            string[] boardData = fenBoard.Split('/');
            if (boardData.Length != 8)
                throw new Exception("Invalid board FEN");

            ChessBoard chessBoard = new();

            string digits = "012345678";
            for (int i = 0; i < boardData.Length; i++)
            {
                int insertPosition = 0;
                for (int j = 0; j < boardData[i].Length; j++)
                {
                    if (digits.Contains(boardData[i][j]))
                        insertPosition += Int32.Parse(boardData[i][j].ToString());
                    else
                    {
                        chessBoard[insertPosition, i] = CreatePiece(i, boardData[i][j], chessBoard);
                        insertPosition++;
                    }
                }
            }

            return chessBoard;
        }


        private ChessPiece CreatePiece(int yPosition, char pieceName, ChessBoard board)
        {
            Side pieceSide = pieceName.ToString().ToLower() == pieceName.ToString() ? Side.Black : Side.White;

            int yPawnPosition = pieceSide == Side.White ? 6 : 1;
            pieceName = pieceName.ToString().ToLower()[0];

            switch (pieceName)
            {
                case 'p':
                    if (yPosition == 0 || yPosition == 7)
                        throw new Exception($"Cannot set pawn to {yPosition}");
                    return new Pawn(pieceSide, promotionProvider, moveHistory, board, yPawnPosition == yPosition);
                case 'n':
                    return new Knight(pieceSide, board);
                case 'b':
                    return new Bishop(pieceSide, board);
                case 'r':
                    return new Rook(pieceSide, board);
                case 'q':
                    return new Queen(pieceSide, board);
                case 'k':
                    King king = new King(pieceSide, moveHistory, board);
                    if (pieceSide == Side.White)
                        WhiteKing = king;
                    else
                        BlackKing = king;
                    return king;
                default:
                    throw new Exception($"Unknown piece name {pieceName}");
            }
        }

        private void ParseCastling(string fenCastling)
        {
            if (fenCastling == "-")
                return;

            foreach (var item in fenCastling)
            {
                string castlingCheckForm = item.ToString().ToLower();
                if (castlingCheckForm != "k" && castlingCheckForm != "q")
                    throw new Exception($"Cannot parse castling. Unknown symbol {item}");

                King king = castlingCheckForm == item.ToString() ? BlackKing : WhiteKing;
                int yRookPosition = castlingCheckForm == item.ToString() ? 0 : 7;

                Point? kingPosition = ChessBoard.FindPiece(king);
                if (kingPosition == null)
                    throw new Exception("Cannot parse castling. Cant find king on the board");

                CastlingType ct = castlingCheckForm == "k" ? CastlingType.Short : CastlingType.Long;

                if (ct == CastlingType.Long)
                    king.AddLongCastling(new Point(0, yRookPosition), moveHistory, ChessBoard);
                else
                    king.AddShortCastling(new Point(7, yRookPosition), moveHistory, ChessBoard);

            }
        }

        private void ParseEnPassant(string fenEnPassant)
        {
            if (fenEnPassant == "-")
                return;

            Dictionary<char, int> convert = new()
            {
                { 'a', 0 },
                { 'b', 1 },
                { 'c', 2 },
                { 'd', 3 },
                { 'e', 4 },
                { 'f', 5 },
                { 'g', 6 },
                { 'h', 7 }
            };

            int xPos = convert[fenEnPassant[0]];
            int yPos = 8 - Int32.Parse(fenEnPassant[1].ToString()); // Invert 
           // yPos = 8 - yPos;

            // Поиск пешки, которая сделала ход на 2 клетки вперед
            Point startPoint = new Point(xPos, yPos - 1);
            Point endPoint = new Point(xPos, yPos + 1);

            ChessPiece pawn = ChessBoard[startPoint.X, startPoint.Y];
            if (pawn == null)
            {
                pawn = ChessBoard[endPoint.X, endPoint.Y];
                Point point = startPoint;
                startPoint = endPoint;
                endPoint = point;
            }

            GameState gameState = new GameState(pawn.Side, 0, GameResult.OnGoing, CheckState.None, setGameState);
            moveHistory.PushMove(new Moves.PawnStartMove(startPoint, endPoint, ChessBoard), gameState);
        }

        private void ParseFiftyMoves(string fenFiftyMoves)
        {
            if (fenFiftyMoves == "-")
                return;

            FiftyMoves = Int32.Parse(fenFiftyMoves);
        }

        private void ParseMovesCount(string fenMovesCount)
        {
            if (fenMovesCount == "-")
                return;

            MovesCount = Int32.Parse(fenMovesCount);
        }
    }
}
