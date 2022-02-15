using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using ChessGame.PGN;

namespace ChessGame.Json
{
    class MovesJsonGenerator : IGameSerializer
    {
        private readonly Stack<MoveJson> _movesDeep = new();
        private readonly Stack<List<MoveJson>> _branchDeep = new();
        
        private readonly List<MoveJson> _mainBranch = new();

        private List<MoveJson> _currentBranch;
        
        private MoveJson _currentMove;

        private bool _inserted;

        public MovesJsonGenerator()
        {
            _currentBranch = _mainBranch;
        }


        public void AddWhiteMove(IMoveFormatter moveFormatter)
        {
            if (_currentMove != null && !_inserted)
                _currentBranch.Add(_currentMove);

            _inserted = false;
            _currentMove = new MoveJson()
            {
                Value = moveFormatter.Result,
                Side = Side.White
            };
        }

        public void AddBlackMove(IMoveFormatter moveFormatter)
        {
            if (_currentMove != null && !_inserted)
                _currentBranch.Add(_currentMove);

            _inserted = false;
            _currentMove = new MoveJson()
            {
                Value = moveFormatter.Result,
                Side = Side.Black
            };
        }

        public void StartBranch()
        {
            if (_currentMove != null && !_inserted)
            {
                _currentBranch.Add(_currentMove);
                _inserted = true;
            }
            if (_mainBranch.Count == 0)
                return;

            _branchDeep.Push(_currentBranch);
            _currentBranch = new List<MoveJson>();
            _currentMove.Branches.Add(_currentBranch);
            
            _movesDeep.Push(_currentMove);
            _currentMove = null;
        }

        public void EndBranch()
        {
            if (_currentMove != null && !_inserted)
            {
                _currentBranch.Add(_currentMove);
                _inserted = true;
            }

            if (_movesDeep.Count != 0)
            {
                _inserted = true;
                _currentMove = _movesDeep.Pop();
                _currentBranch = _branchDeep.Pop();
            }
        }

        public void AddComment(string data)
        {
            _currentMove.Comment = data;
        }

        public void AddPointer(string pointer)
        {
            _currentMove.Pointer = pointer;
        }

        public IMoveFormatter CreateMoveFormatter()
        {
            return new PGNMoveFormatter();
        }

        public string Result
        {
            get
            {
                if (_currentMove != null && !_inserted)
                {
                    _currentBranch.Add(_currentMove);
                    _inserted = true;
                }
                
                return JsonSerializer.Serialize(_mainBranch, new JsonSerializerOptions(JsonSerializerDefaults.Web));
            }
        }
    }
}