using System.Collections.Generic;
using System.Linq;

namespace TheHorses.Scraper
{
    public static class Utils
    {
        public static string RemoveMany(this string theString, IEnumerable<string> removeThese)
        {
            return removeThese.Aggregate(theString, (current, s) => current.Replace(s, string.Empty));
        }
    }
}