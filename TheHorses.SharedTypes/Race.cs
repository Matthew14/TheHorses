using System;
using System.Runtime.Serialization;

namespace TheHorses.SharedTypes
{
    [DataContract]
    public class Race 
    {
        [DataMember]
        public DateTime When { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Venue { get; set; }

        public static bool operator ==(Race one, Race two)
        {
            if (ReferenceEquals(one, two))
                return true;

            if (((object)one == null) || ((object)two == null))
                return false;


            return one.Equals(two) ||
                   (one.Name == two.Name && one.Venue == two.Venue &&
                    Math.Abs((one.When - two.When).TotalSeconds) < 60);
        }

        public static bool operator !=(Race one, Race two)
        {
            return !(one == two);
        }
    }
}