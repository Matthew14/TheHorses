namespace TheHorses.SharedTypes
{
    public class Horse
    {
        public Horse()
        {
            Name = "unknown";
        }
        public string Name { get; set; }

        public static bool operator ==(Horse one, Horse two)
        {
            if (ReferenceEquals(one, two))
                return true;

            if (((object)one == null) || ((object)two == null))
                return false;


            return one.Equals(two) || (one.Name == two.Name);
        }

        public static bool operator !=(Horse one, Horse two)
        {
            return !(one == two);
        }
    }
}
