using System.Collections.Generic;

namespace ChessGame.Json
{
    public class MoveJson
    {
        public string Pointer { get; set; }
        
        public string Value { get; set; }
        
        public Side Side { get; set; }

        public List<List<MoveJson>> Branches { get; } = new();
        
        public string Comment { get; set; }
        
        // TODO add marks
    }
}