using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessGame.MoveManagement
{
    internal class MoveStorageItem
    {
        public MoveStorageItem(Move move, GameState before, GameState after)
        {
            Before = before;
            After = after;
            Move = move;
        }

        public string MoveComment { get; set; }

        public List<Mark> Marks { get; } = new();
        public string UniqueId { get; } = Guid.NewGuid().ToString();
        
        public GameState Before { get; }
        
        public GameState After { get; }
        
        public Move Move { get; }
        
        public MoveStorageItem Next { get; private set; }
        
        public MoveStorageItem Prev { get; private set; }

        public List<MoveStorageItem> Children { get; } = new();

        private bool Same(MoveStorageItem item)
        {
            return item.Move != null &&
                   item.Move.GetType() == Move.GetType() && 
                   item.Move.StartPoint == Move.StartPoint && 
                   item.Move.EndPoint == Move.EndPoint;
        }
        
        public MoveStorageItem AddItem(MoveStorageItem item)
        {
            if (Next != null && Next.Same(item))
                return Next;

            var sameItem = Children.FirstOrDefault(childItem => childItem.Same(item));
            if (sameItem != null)
                return sameItem;

            item.Prev = this;

            if (Next == null)
                Next = item;
            else
                Children.Add(item);

            return item;
        }

        public void Serialize(IGameSerializer serializer, bool recursive = true)
        {
            var moveFormatter = serializer.CreateMoveFormatter();
            Move.Serialize(moveFormatter);
            
            if (After.CheckState != CheckState.None)
                moveFormatter.SetEnemyKingAttacked();

            if (After.GameResult != GameResult.OnGoing)
                moveFormatter.SetMate();
            
            if (Move.Piece.Side == Side.White)
                serializer.AddWhiteMove(moveFormatter);
            else
                serializer.AddBlackMove(moveFormatter);
            
            serializer.AddPointer(UniqueId);
            serializer.AddComment(MoveComment);

            foreach (var childItem in Children)
            {
                serializer.StartBranch();
                childItem.Serialize(serializer);
                serializer.EndBranch();
            }
            
            if (recursive)
                Next?.Serialize(serializer);
        }
    }
}