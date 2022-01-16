using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.MoveManagment
{
    class BranchItem
    {
        GameState _before;
        GameState _after;
        Move _mv;

        public BranchItem(Move mv, GameState before, GameState after)
        {
            this._mv = mv;
            this._before = before;
            this._after = after;
        }

        public GameState Before => _before;

        public GameState After => _after;

        public Move Move => _mv;


        public bool Same(BranchItem item)
        {
            return item.Move.GetType() == _mv.GetType() && 
                item.Move.StartPoint == _mv.StartPoint && 
                item.Move.EndPoint == _mv.EndPoint;
        }
    }
}
