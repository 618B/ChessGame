using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.PGN
{
    class PGNGenerator : IGameSerializer
    {
        StringBuilder result = new StringBuilder();

        int moveCounter = 1;

        Stack<int> counters = new Stack<int>();

        bool branchStarted = false;
        bool branchEnded = false; // Цифра перед черным ходов после встраивания ветки

        public void AddWhiteMove(IMoveFormatter moveData)
        {
            result.Append(moveCounter.ToString());
            result.Append(". ");
            result.Append(moveData.Result);
            result.Append(" ");

            branchStarted = false;
            branchEnded = false;
        }

        public void AddBlackMove(IMoveFormatter moveData)
        {
            if (branchStarted)
            {
                moveCounter--;
                AddEmptyWhite();
            }
            if (branchEnded)
            {
                AddEmptyWhite();
            }

            result.Append(moveData.Result);
            result.Append(" ");
            moveCounter++;

            branchStarted = false;
            branchEnded = false;
        }

        public void AddAdditional(string data)
        {
            // TODO Board signs
            result.Append("{");
            result.Append("PTR: ");
            result.Append(data);
            result.Append("} ");
        }

        public void StartBranch()
        {
            branchStarted = true;

            counters.Push(moveCounter);
            result.Append("( ");
        }

        public void EndBranch()
        {
            branchEnded = true;

            moveCounter = counters.Pop();
            result.Append(") ");
        }

        public IMoveFormatter CreateMoveFormatter()
        {
            return new PGNMoveFormatter();
        }

        public string Result => result.ToString();

        private void AddEmptyWhite()
        {
            result.Append(moveCounter.ToString());
            result.Append("... ");
        }
    }
}
