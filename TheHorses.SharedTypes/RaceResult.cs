using System.Collections.Generic;

namespace TheHorses.SharedTypes
{
    public class RaceResult
    {
        public Dictionary<int, Horse> Places { get; set; } 
        public Race Race { get; set; }

    }
}