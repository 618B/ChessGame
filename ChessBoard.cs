using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ChessGame
{
    class ChessBoard
    {
        ChessPiece[,] _figures;
        readonly int _size;

        public ChessBoard(int size = 8)
        {
            _figures = new ChessPiece[size, size];
            _size = size;
        }

        public bool IsFieldExists(int x, int y)
        {
            return (x >= 0 && x < _size) && 
                    (y >= 0 && y < _size);
        }

        public ChessPiece this[int x, int y]
        {
            get
            {
                if (!IsFieldExists(x, y))
                    throw new Exception("Out of border!");

                return _figures[x, y];
            }
            set
            {
                if (!IsFieldExists(x, y))
                    throw new Exception("Out of border!");

                _figures[x, y] = value;
            }
        }

        public bool IsUnderAttack(Point attackField, Side attackSide)
        {
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    if (_figures[i, j] != null && _figures[i, j].Side == attackSide)
                    {
                        if (_figures[i, j].Rules.Any(moveRule => moveRule.Attacking && moveRule.CanExecute(new Point(i, j), attackField)))
                            return true;
                    }
                }
            }

            return false;
        }

        public Point? FindPiece(ChessPiece piece)
        {
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    if (_figures[i, j] == piece)
                        return new Point(i, j);
                }
            }

            return null;
        }

        public int Size => _size;
        
    }
}
