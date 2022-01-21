using ChessGame.MoveManagment;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChessGame.PGN
{
    class PGNParser
    {
        ChessGame game;

        string fen = string.Empty;

        string moves;

        public PGNParser(string input)
        {
            string[] pgnParts = input.Split("\n\n");
            ParseData(pgnParts[0]);
            moves = pgnParts[1].Replace("1-0", "").Replace("0-1", "").Replace("1/2-1/2", "");
        }

        public ChessGame ChessGame => game;

        public string FEN => fen;

        private void ParseData(string data)
        {
            string[] dataTags = data.Split('\n');
            string fenTag = dataTags.FirstOrDefault(x => x.Contains("FEN"));
            if (!string.IsNullOrEmpty(fenTag))
                fen = fenTag.Substring(fenTag.IndexOf("\"") + 1, fenTag.LastIndexOf("\"") - fenTag.IndexOf("\"") - 1);

        }

        enum Mode
        {
            Comment, Default, Branch
        }

        public void ParseMoves(ChessGame game, ChessBoard board)
        {
            ParseMoves(game, board, moves);
        }

        static int MoveCounter = 0;
        static int deep = 0;

        private void ParseMoves(ChessGame game, ChessBoard board, string moves)
        {
            moves = Regex.Replace(moves, "\\$[0-9]{1,}", "");
            string startFen = new FEN.FENGenerator().AddBorderState(board).Result;
            if (startFen.Trim() == "r2q1rk1/ppp2ppp/2np1n2/4p3/2B1P1b1/2NPPN2/PPP3PP/R2Q1RK1 - - -")
            {
                int salam = 0;
            }

            List<string> mv = new List<string>();
            List<(int, string)> branches = new List<(int, string)>();
            List<(int, string)> comments = new List<(int, string)>();

            int moveCount = 0;
            string move = string.Empty;
            Mode md = Mode.Default;

            string branch = "";
            string comment = "";

            int branchDeep = 0;

            foreach (var item in moves)
            {
                if (item == '(' && md != Mode.Comment)
                {
                    if (Mode.Branch == md)
                        branch += item;
                    md = Mode.Branch;
                    branchDeep++;
                    continue;
                }
                else if (item == ')' && md != Mode.Comment)
                {
                    branchDeep--;
                    if (branchDeep == 0)
                    {
                        branches.Add((moveCount, branch));
                        md = Mode.Default;
                        branch = "";
                        continue;
                    }
                }
                else if (item == '{' && md != Mode.Branch)
                {
                    md = Mode.Comment;
                    continue;
                }
                else if (item == '}' && md != Mode.Branch)
                {
                    comments.Add((moveCount, comment));
                    comment = "";
                    md = Mode.Default;
                    continue;
                }
                else if (item == ' ' && md == Mode.Default && move.Trim().Length != 0)
                {
                    if (!move.Contains('.'))
                    {
                        mv.Add(move);
                        moveCount++;
                    }
                    move = "";

                    continue;
                }

                if (md == Mode.Branch)
                {
                    branch += item;
                }
                else if (md == Mode.Comment)
                {
                    comment += item;
                }
                else
                {
                    if (item != ' ')
                        move += item;
                }
            }

            if (move.Length != 0 && !move.Contains('\n'))
                mv.Add(move);

            int mvApplied = 0;
            for(int i = 0; i < mv.Count; i++)
            {
                var m = CreateMove(board, mv[i].Replace("+", "").Replace("#", "").Replace("!", "").Replace("?", ""), game.Turn, game);
                game.Move(m.StartPoint, m.EndPoint);
                mvApplied++;
                MoveCounter++;
            }

            string after = new FEN.FENGenerator().AddBorderState(board).Result;

            for (int i = 0; i < branches.Count; i++)
            {
                int targetMove = branches[i].Item1 - 1 < 0 ? 0 : branches[i].Item1 - 1;
                if (targetMove < mvApplied)
                {
                    while (targetMove < mvApplied)
                    {
                        game.Undo();
                        mvApplied--;
                    }
                }
                else
                {
                    while (targetMove > mvApplied)
                    {
                        game.Redo();
                        mvApplied++;
                    }
                }
                if (mvApplied == 0)
                {
                    int k = 15;
                }
                deep++;
                ParseMoves(game, board, branches[i].Item2);
            }
            while (mvApplied != 0)
            {
                game.Undo();
                mvApplied--;
            }

            string endFen = new FEN.FENGenerator().AddBorderState(board).Result;
            if (endFen != startFen)
            {
                int salam = 0;
            }
            deep--;
        }

        private Move CreateMove(ChessBoard board, string pgnMove, Side side, ChessGame game)
        {
            List<char> registry = new List<char>() { 'K', 'Q', 'B', 'N', 'R' };

            // TODO Castling
            if (pgnMove == "O-O")
            {
                if (side == Side.White)
                {
                    Point? king = board.FindPieceByName("K");
                    if (!king.HasValue)
                        throw new Exception("No white king on the board");

                    var rule = board[king.Value.X, king.Value.Y].Rules.Find((rule) => rule.CanExecute(king.Value, new Point(6, 7)));
                    if (rule == null)
                        throw new Exception("No castling rule on the king");
                    return rule.CreateMove(king.Value, new Point(6, 7));
                }
                else
                {
                    Point? king = board.FindPieceByName("k");
                    if (!king.HasValue)
                        throw new Exception("No black king on the board");

                    var rule = board[king.Value.X, king.Value.Y].Rules.Find((rule) => rule.CanExecute(king.Value, new Point(6, 0)));
                    if (rule == null)
                        throw new Exception("No castling rule on the king");
                    return rule.CreateMove(king.Value, new Point(6, 0));
                }
            }

            if (pgnMove == "O-O-O")
            {
                if (side == Side.White)
                {
                    Point? king = board.FindPieceByName("K");
                    if (!king.HasValue)
                        throw new Exception("No white king on the board");

                    var rule = board[king.Value.X, king.Value.Y].Rules.Find((rule) => rule.CanExecute(king.Value, new Point(2, 7)));
                    if (rule == null)
                        throw new Exception("No castling rule on the king");
                    return rule.CreateMove(king.Value, new Point(2, 7));
                }
                else
                {
                    Point? king = board.FindPieceByName("k");
                    if (!king.HasValue)
                        throw new Exception("No black king on the board");

                    var rule = board[king.Value.X, king.Value.Y].Rules.Find((rule) => rule.CanExecute(king.Value, new Point(2, 0)));
                    if (rule == null)
                        throw new Exception("No castling rule on the king");
                    return rule.CreateMove(king.Value, new Point(2, 0));
                }
            }

            string pieceName = side == Side.White ? "P" : "p";

            if (registry.Contains(pgnMove[0]))
            {
                pieceName = side == Side.White ? pgnMove[0].ToString() : pgnMove[0].ToString().ToLower();
                pgnMove = string.Join("", pgnMove.ToList().Skip(1));
            }

            if (pgnMove.Contains('='))
            {
                game.SetPromotionPiece(side, pgnMove.Last().ToString().ToLower());
                pgnMove = string.Join("", pgnMove.ToList().SkipLast(2));
            }

            if (pgnMove.Contains('x'))
                pgnMove = pgnMove.Replace("x", "");

            Point target;
            Point source = new Point(-1, -1);


            // DefaultMove
            if (pgnMove.Length == 2)
            {
                // Target Only
                target = GetPoint(pgnMove);
                return CreateMove(game, board, pieceName, target);
            }

            if (pgnMove.Length == 3)
            {
                string temp = pgnMove[1].ToString() + pgnMove[2];
                target = GetPoint(temp);

                if (char.IsDigit(pgnMove[0]))
                    return CreateMoveY(game, board, pieceName, target, 8 - Int32.Parse(pgnMove[0].ToString()));
                source = new Point(ConvertX(pgnMove[0]), -1);
                return CreateMove(game, board, pieceName, target, source.X);
            }

            if (pgnMove.Length == 4)
            {
                string src = pgnMove[0].ToString() + pgnMove[1];
                string trg = pgnMove[2].ToString() + pgnMove[3];
                target = GetPoint(trg);
                source = GetPoint(src);
                return CreateMove(game, board, pieceName, target, source.X, source.Y);
            }



            throw new Exception("PGN parsing move creation error");
        }

        private Point GetPoint(string mv)
        {
            Dictionary<char, int> x = new()
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

            return new Point(x[mv[0]], 8 - Int32.Parse(mv[1].ToString()));
        }

        private int ConvertX(char s)
        {
            Dictionary<char, int> x = new()
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

            return x[s];
        }

        private Move CreatePromotion(string move, Side side)
        {
            string[] parts = move.Split('=');


            return null;
        }

        private Move CreateMove(ChessGame game, ChessBoard board, string pieceName, Point target)
        {
            for (int i = 0; i < board.Size; i++)
            {
                for (int j = 0; j < board.Size; j++)
                {
                    if (board[i, j] != null && board[i, j].Name == pieceName)
                    {
                        var rule = board[i, j].Rules.Find((mv) => { return mv.CanExecute(new Point(i, j), target); });
                        if (!game.IsMoveAllowed(new Point(i, j), target))
                            continue;
                        if (rule != null)
                            return rule.CreateMove(new Point(i, j), target);
                    }
                }
            }

            throw new Exception("PGN Parsing error! Target only");
        }

        private Move CreateMove(ChessGame game, ChessBoard board, string pieceName, Point target, int x)
        {
            for (int j = 0; j < board.Size; j++)
            {
                if (board[x, j] != null && board[x, j].Name == pieceName)
                {
                    var rule = board[x, j].Rules.Find((mv) => { return mv.CanExecute(new Point(x, j), target); });
                    if (!game.IsMoveAllowed(new Point(x, j), target))
                        continue;
                    if (rule != null)
                        return rule.CreateMove(new Point(x, j), target);
                }
            }

            throw new Exception("PGN Parsing error! X axe");
        }      
        
        private Move CreateMoveY(ChessGame game, ChessBoard board, string pieceName, Point target, int y)
        {
            for (int j = 0; j < board.Size; j++)
            {
                if (board[j, y] != null && board[j, y].Name == pieceName)
                {
                    var rule = board[j, y].Rules.Find((mv) => { return mv.CanExecute(new Point(j, y), target); });
                    if (!game.IsMoveAllowed(new Point(j, y), target))
                        continue;
                    if (rule != null)
                        return rule.CreateMove(new Point(j, y), target);
                }
            }

            throw new Exception("PGN Parsing error! X axe");
        }

        private Move CreateMove(ChessGame game, ChessBoard board, string pieceName, Point target, int x, int y)
        {
            if (board[x, y] != null && board[x, y].Name == pieceName)
            {
                var rule = board[x, y].Rules.Find((mv) => { return mv.CanExecute(new Point(x, y), target); });
                if (game.IsMoveAllowed(new Point(x, y), target))
                {
                    if (rule != null)
                        return rule.CreateMove(new Point(x, y), target);
                }
            }

            throw new Exception("PGN Parsing error! Source Target");
        }
    }

}
