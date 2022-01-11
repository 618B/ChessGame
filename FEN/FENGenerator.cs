using ChessGame.MoveRules;
using ChessGame.Moves;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ChessGame.FEN
{
    class FENGenerator
    {
        string borderState;
        string whiteKingCastling;
        string blackKingCastling;
        string enPassantPoint;
        string side;
        string movesCount;
        string fiftyMoves;


        public FENGenerator AddBorderState(ChessBoard board)
        {
            borderState = string.Empty;
            for (int i = 0; i < board.Size; i++)
            {
                int emptyFields = 0;
                for (int j = 0; j < board.Size; j++)
                {
                    if (board[j, i] != null)
                    {
                        if (emptyFields != 0)
                        {
                            borderState += emptyFields.ToString();
                            emptyFields = 0;
                        }
                        borderState += board[j, i].Name;
                    }
                    else
                    {
                        emptyFields++;
                    }
                }
                if (emptyFields != 0)
                {
                    borderState += emptyFields.ToString();
                }
                if (i + 1 != board.Size)
                    borderState += "/";
            }

            return this;
        }

        public FENGenerator AddCastlingState(ChessPiece king, ChessBoard board)
        {

            Point? position = board.FindPiece(king);
            if (position == null)
                throw new Exception("Cant find king on the board");

            bool shortCastling = false;
            bool longCastling = false;
            foreach (MoveRule mr in king.Rules)
            {
                if (mr is CastlingRule castling)
                {
                    if (castling.MinimalRequirements(position.Value))
                    {
                        if (castling.CastlingType == MoveRules.CastlingType.Short)
                            shortCastling = true;
                        if (castling.CastlingType == MoveRules.CastlingType.Long)
                            longCastling = true;
                    }
                }
            }

            string result = string.Empty;
            if (shortCastling)
                result += "k";
            if (longCastling)
                result += "q";

            if (king.Side == Side.White)
            {
                result = result.ToUpper();
                whiteKingCastling = result;
            }
            else
            {
                blackKingCastling = result;
            }

            return this;
        }

        public FENGenerator AddEnPassant(IMoveHistory moveHistory, ChessBoard board)
        {
            Dictionary<int, string> x = new()
            {
                { 0, "a"},
                { 1, "b"},
                { 2, "c"},
                { 3, "d"},
                { 4, "e"},
                { 5, "f"},
                { 6, "g"},
                { 7, "h"}
            };
            if (moveHistory.LastMoveType<PawnStartMove>())
            {
                int sideOffset = 1;
                if (moveHistory.LastMovePiece.Side == Side.Black)
                    sideOffset = -1;

                Point? posiiton = board.FindPiece(moveHistory.LastMovePiece);
                if (posiiton == null)
                    throw new Exception("Cant find pawn on the board");

                Point pos = posiiton.Value;
                enPassantPoint += x[pos.X] + (8  - (pos.Y + sideOffset)).ToString(); // invert
            }

            return this;
        }

        public FENGenerator AddSide(Side side)
        {
            this.side = side == Side.White ? "w" : "b";

            return this;
        }

        public FENGenerator AddMovesCounter(int movesCount)
        {
            this.movesCount = movesCount.ToString();

            return this;
        }

        public FENGenerator AddFiftyMovesRule(int movesCount)
        {
            fiftyMoves = movesCount.ToString();

            return this;
        }

        public string Result
        {
            get
            {
                string result = string.Empty;
                result += string.IsNullOrEmpty(borderState) ? "-" : borderState;
                result += " ";

                result += string.IsNullOrEmpty(side) ? "-" : side;
                result += " ";

                if (string.IsNullOrEmpty(whiteKingCastling) && string.IsNullOrEmpty(blackKingCastling))
                    result += "-";
                if (!string.IsNullOrEmpty(whiteKingCastling))
                    result += whiteKingCastling; 
                if (!string.IsNullOrEmpty(blackKingCastling))
                    result += blackKingCastling;
                result += " ";

                result += string.IsNullOrEmpty(enPassantPoint) ? "-" : enPassantPoint;
                result += " ";

                result += fiftyMoves;
                result += " ";

                result += movesCount;

                return result;
            }
        }
    }
}
