using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame
{
    interface IGameSerializer
    {
        public void AddWhiteMove(IMoveFormatter moveFormatter);

        public void AddBlackMove(IMoveFormatter moveFormatter);

        public void StartBranch();

        public void EndBranch();

        public void AddComment(string data);

        public void AddPointer(string pointer);
        

        public IMoveFormatter CreateMoveFormatter();
    }
}
