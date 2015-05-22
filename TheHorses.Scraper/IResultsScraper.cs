using System.Collections.Generic;

namespace TheHorses.Scraper
{
    public interface IResultsScraper
    {
        IEnumerable<RaceResult> ScrapeResults();
    }
}