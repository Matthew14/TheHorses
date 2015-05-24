using System;

namespace TheHorses.SharedTypes
{
    [Serializable]
    public class Race 
    {
        public DateTime When { get; set; }
        public string Name { get; set; }
        public string Venue { get; set; }
    }
}