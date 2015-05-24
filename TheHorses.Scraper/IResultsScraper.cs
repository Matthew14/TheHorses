using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheHorses.SharedTypes;

namespace TheHorses.Scraper
{
    public interface IResultsScraper
    {
        Task<IEnumerable<RaceResult>> ScrapeResults(DateTime dateTime);
    }
}