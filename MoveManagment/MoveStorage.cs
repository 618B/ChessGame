using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.MoveManagment
{
    class MoveStorage : IMoveStorage
    {
        Branch _mainBranch = new Branch();

        Branch _currentBranch;

        List<int> _pointer = new List<int>();

        public MoveStorage()
        {
            _currentBranch = _mainBranch;
        }

        public int MovesCount { get; private set; } = 0;

        // Метод всех ходов
        public void Serialize(IGameSerializer gameSerializer)
        {
            _mainBranch.Serialize(gameSerializer, "");
        }

        public IList<(IList<int>, Move)> Data => _mainBranch.Moves;

        public IEnumerable<int> Position
        {
            get
            {
                var result = new List<int>(_pointer);
                result.Add(_currentBranch.CurrentIndex);

                return result;
            }
        }

        // Метод перехода к какому-либо ходу
        public void SetPositiion(IList<int> pointer)
        {
            SetBranch(pointer.SkipLast(1));

            _currentBranch.ToMove(pointer.Last());
            _pointer = pointer.SkipLast(1).ToList();
        }

        public void AddMove(Move move, GameState before, GameState after)
        {
            int newIndex;
            _currentBranch = _currentBranch.AddMove(new BranchItem(move, before, after), out newIndex);

            if (newIndex != -1)
                _pointer.Add(newIndex);
        }

        public void Undo()
        {
            if (_currentBranch == _mainBranch && _currentBranch.CurrentIndex == 0)
                return;

            if (_currentBranch.CurrentIndex == 0)
            {
                //_pointer = _pointer.SkipLast(1).ToList();
                int branchPos = GetBranchById(_pointer.SkipLast(1).ToList()).CurrentIndex;
                SetBranch(_pointer.SkipLast(1));
                _currentBranch.ToMove(branchPos);
            }

            _currentBranch.Undo();
        }

        public void Redo()
        {
            _currentBranch.Redo();
        }

        public void PointerToStart()
        {
            Branch temp = _currentBranch;

            for (int i = 0; i < _pointer.Count; i++)
            {
                BranchToStart(temp);
                temp = GetBranchById(_pointer.SkipLast(i + 1).ToList());
            }

            BranchToStart(_mainBranch);

            _pointer = new List<int>();
            _currentBranch = _mainBranch;

        }

        public void PointerToEnd()
        {
            while (_currentBranch.CanRedo)
                _currentBranch.Redo();
        }

        public ChessPiece LastMovePiece
        {
            get
            {
                if (_currentBranch.CurrentIndex == 0)
                    return null;

                return _currentBranch.CurrentItem.Move.Piece;
            }
        }


        public bool LastMoveType<T>()
        {
            if (_currentBranch.CurrentIndex == 0)
                return false;

            return _currentBranch.CurrentItem.GetType() == typeof(T);
        }

        public bool WasMoved(ChessPiece piece)
        {
            Branch temp = _mainBranch;
            for (int i = 0; i < _pointer.Count; i++)
            {
                foreach (var item in temp.Items)
                {
                    if (item.Move.Piece == piece)
                        return true;
                }
                temp = temp.GetBranchById(_pointer[i]);
            }

            foreach (var item in temp.Items)
            {
                if (item.Move.Piece == piece)
                    return true;
            }

            return false;
        }

        private Branch GetBranchById(IList<int> pointer)
        {
            Branch temp = _mainBranch;
            for (int i = 0; i < pointer.Count; i++)
            {
                temp = temp.GetBranchById(pointer[i]);
            }

            return temp;
        }

        private void BranchToStart(Branch branch)
        {
            while (branch.CanUndo)
                branch.Undo();
        }

        private void SetBranch(IEnumerable<int> pointer)
        {
            PointerToStart();

            
            foreach (var item in pointer)
                _currentBranch = _currentBranch.SelectBranch(item);

            _pointer = pointer.ToList();
        }
    }
}
