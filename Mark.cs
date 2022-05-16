using System;
using System.Drawing;
using System.Text.Json.Serialization;

namespace ChessGame
{
    public class Mark
    {
        public int UniqueKey { get; set; }
        
        [JsonPropertyName("markStart")]
        public Point StartPoint { get; set; }
        
        [JsonPropertyName("color")]
        public string Color { get; set; }
        
        [JsonPropertyName("markPosition")]
        public Point EndPoint { get; set; }
    }
}