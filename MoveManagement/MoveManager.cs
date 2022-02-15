using System;
using System.Collections.Generic;
using System.Drawing;

namespace ChessGame.MoveManagement
{
    internal class MoveManager : IMoveStorage
    {
        private readonly MoveStorageItem _head;

        private MoveStorageItem _current;

        private readonly Dictionary<string, MoveStorageItem> _dataBase = new();

        private readonly HashSet<ChessPiece> _movedPieces = new();

        public MoveManager()
        {
            _head = new MoveStorageItem(null, null, null); // Initial
            _current = _head;
        }

        public void CommentCurrentMove(string comment)
        {
            _current.MoveComment = comment;
        }
        
        public string Position
        {
            get =>  _current == _head ? null : _current.UniqueId;
            set => SetPosition(value);
        }

        public List<Point> PreviousMove => _current != _head ? 
            new List<Point>() {_current.Move.StartPoint, _current.Move.EndPoint} : 
            new List<Point>();

        public void Serialize(IGameSerializer serializer)
        {
            serializer.AddStartMessage(_head.MoveComment);
            
            _head.Next?.Serialize(serializer, false);

            foreach (var childItem in _head.Children)
            {
                serializer.StartBranch();
                childItem.Serialize(serializer);
                serializer.EndBranch();
            }
            
            _head.Next?.Next?.Serialize(serializer);
        }

        public void AddMove(Move move, GameState before, GameState after)
        {
            _current = _current.AddItem(new MoveStorageItem(move, before, after));
            
            if (!_dataBase.ContainsKey(_current.UniqueId))
                _dataBase.Add(_current.UniqueId, _current);

            if (_current.Move != null && !_movedPieces.Contains(_current.Move.Piece))
                _movedPieces.Add(_current.Move.Piece);
        }

        public void Undo()
        {
            if (_current?.Move == null)
                return;

            _movedPieces.Remove(_current.Move.Piece);
            _current.Move.Undo();
            _current.Before.Apply();
            _current = _current.Prev;
        }

        public void Redo()
        {
            if (_current.Next == null) 
                return;
            
            _current = _current.Next;
            _current.After.Apply();
            _current.Move.Execute();
            _movedPieces.Add(_current.Move.Piece);
        }

        public void PointerToStart()
        {
            while (_current != _head)
            {
                _current.Move.Undo();
                _movedPieces.Remove(_current.Move.Piece);
                _current = _current.Prev;
            }
            _current.Next?.Before.Apply();
        }

        public void PointerToEnd()
        {
            while (_current.Next != null)
            {
                _current = _current.Next;
                _current.Move.Execute();
                _movedPieces.Add(_current.Move.Piece);
            }
            _current.After.Apply();
        }

        private void SetPosition(string position)
        {
            _dataBase.TryGetValue(position, out var target);
            if (target == null)
                return;

            Stack<MoveStorageItem> path = new Stack<MoveStorageItem>();
            while (target != _head)
            {
                path.Push(target);
                target = target.Prev;
            }
            
            PointerToStart();
            while (path.Count != 1)
                path.Pop().Move.Execute();

            _current = path.Pop();
            _current.Move.Execute();
            
            _current.After.Apply();
        }
        
        
        public bool LastMoveType<T>()
        {
            return _current?.Move?.GetType() == typeof(T);
        }

        public ChessPiece LastMovePiece => _current?.Move?.Piece;

        public bool WasMoved(ChessPiece piece)
        {
            return _movedPieces.Contains(piece);
        }
    }
}