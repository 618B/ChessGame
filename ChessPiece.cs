﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessGame.MoveRules;

namespace ChessGame
{
    public enum Side
    {
        White, Black
    }

    class ChessPiece
    {
        readonly Side _side;

        public ChessPiece(Side side)
        {
            this._side = side;
        }

        public Side Side => _side;

        public List<MoveRule> Rules { get; set; } = new List<MoveRule>();

        public string Name { get; set; }
    }
}
