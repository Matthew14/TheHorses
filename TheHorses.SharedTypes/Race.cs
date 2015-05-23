using System;

namespace TheHorses.SharedTypes
{
    [Serializable]
    public class Race 
    {
        public Race() { }

        public Race(string venue, DateTime when)
        {
            Venue = venue;
            When = when;
        }

        public DateTime When { get; set; }
        public string Name { get; set; }
        public string Venue { get; set; }
    }
}