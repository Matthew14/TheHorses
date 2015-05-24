using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace TheHorses.SharedTypes
{
    [DataContract]
    public class RaceResult
    {
        [DataMember]
        public List<Place> Places { get; set; }
        [DataMember]
        public Race Race { get; set; }

        public static bool operator ==(RaceResult one, RaceResult two)
        {
            if (ReferenceEquals(one, two))
                return true;

            if (((object) one == null) || ((object) two == null))
                return false;

            if (one.Places.Count != two.Places.Count)
                return false;

            for (int i = 0; i < one.Places.Count; ++i)
                if (one.Places[i] != two.Places[i])
                    return false;

            return one.Equals(two) || one.Race == two.Race;
        }

        public static bool operator !=(RaceResult one, RaceResult two)
        {
            return !(one == two);
        }
    }
    [DataContract]
    public class Place 
    {
        [DataMember]
        public Horse Horse { get; set; }
        [DataMember]
        public int Position { get; set; }


        public static bool operator ==(Place one, Place two)
        {

            if (ReferenceEquals(one, two))
                return true;

            if (((object) one == null) || ((object) two == null))
                return false;

            return one.Equals(two) || (one.Horse == two.Horse && one.Position == two.Position);
        }

        
        public static bool operator !=(Place one, Place two)
        {
            return !(one == two);
        }
    }
    
}