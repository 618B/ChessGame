using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.MoveManagment
{
    class Branch
    {
        List<BranchItem> moves = new List<BranchItem>();
        int currentIndex = 0;

        List<(int, Branch)> branches = new List<(int, Branch)>();


        public BranchItem CurrentItem
        {
            get
            {
                if (currentIndex == 0)
                    throw new Exception("No items in the branch");

                return moves[currentIndex - 1];
            }
        }

        public void Serialize(IGameSerializer gameSerializer, string branchPointer)
        {
            gameSerializer.StartBranch();
            for (int i = 0; i < moves.Count; i++)
            {
                BranchItem item = moves[i];
                IMoveFormatter formatter = gameSerializer.CreateMoveFormatter();
                item.Move.Serialize(formatter);

                if (item.After.CheckState != CheckState.None)
                    formatter.SetEnemyKingAttacked();

                if (item.After.GameResult != GameResult.OnGoing)
                    formatter.SetMate();

                if (item.Move.Piece.Side == Side.White)
                    gameSerializer.AddWhiteMove(formatter);
                else
                    gameSerializer.AddBlackMove(formatter);

                gameSerializer.AddAdditional(branchPointer + " " + (i + 1).ToString());

                //foreach (var branch in branches.FindAll((index) => { return index.Item1 == i; }))
                //    branch.Item2.Serialize(gameSerializer);
                for (int j = 0; j < branches.Count; j++)
                {
                    if (branches[j].Item1 == i)
                    {
                        branches[j].Item2.Serialize(gameSerializer, branchPointer + " " + j.ToString());
                    }
                }
            }
            gameSerializer.EndBranch();
        }

        public IList<(IList<int>, Move)> Moves
        {
            get
            {
                IList<(IList<int>, Move)> moves = new List<(IList<int>, Move)>();
                for (int i = 0; i < this.moves.Count; i++)
                    moves.Add((new List<int>() { i }, this.moves[i].Move));

                for (int i = 0; i < branches.Count; i++)
                {
                    foreach (var mvs in branches[i].Item2.Moves)
                    {
                        mvs.Item1.Insert(0, i);
                        moves.Add(mvs);
                    }
                }

                return moves;
            }
        }

        public int CurrentIndex
        {
            get => currentIndex;
            set
            {
                if (value > moves.Count - 1)
                    throw new Exception(string.Format("Cannot set index {0}. Not in range.", value));

                currentIndex = value;
            }
        }

        public bool CanRedo => currentIndex != moves.Count;

        public bool CanUndo => currentIndex != 0;

        public IEnumerable<(int, Branch)> Branches => branches;

        public IEnumerable<BranchItem> Items => moves.Take(currentIndex);

        public Branch SelectBranch(int branchId)
        {
            for (int i = 0; i < branches[branchId].Item1; i++)
                Redo();

            return branches[branchId].Item2;
        }

        public void ToMove(int moveId)
        {
            if (currentIndex < moveId)
            {
                for (int i = currentIndex; i != moveId; i++)
                    Redo();
            }
            else
            {
                for (int i = currentIndex; i != moveId; i--)
                    Undo();
            }
        }

        public Branch GetBranchById(int branchID)
        {
            return branches[branchID].Item2;
        }

        public Branch AddMove(BranchItem item, out int newBranchIndex)
        {
            newBranchIndex = -1;
            if (currentIndex == moves.Count)
            {
                moves.Add(item);
                currentIndex++;
                return this;
            }

            if (moves[currentIndex].Same(item))
            {
                currentIndex++;
                return this;
            }

            // Проверить ветки на существования такого же хода
            for (int i = 0; i < branches.Count; i++)
            {
                if (branches[i].Item2.First.Same(item))
                {
                    newBranchIndex = i;
                    return branches[i].Item2.AddMove(item, out _);
                }
            }

            newBranchIndex = branches.Count;
            var newBranch = new Branch();
            newBranch.AddMove(item, out _);
            branches.Add((currentIndex, newBranch));
            return newBranch;
        }

        public void Redo()
        {
            if (currentIndex == moves.Count)
                return;

            moves[currentIndex].Move.Execute();
            moves[currentIndex].After.Apply();
            currentIndex++;
        }

        public void Undo()
        {
            if (currentIndex == 0)
                return;

            currentIndex--;
            moves[currentIndex].Move.Undo();
            moves[currentIndex].Before.Apply();
        }


        private BranchItem First => moves[0];
    }
}
