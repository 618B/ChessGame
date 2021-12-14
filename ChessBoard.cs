using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        
    }
}
