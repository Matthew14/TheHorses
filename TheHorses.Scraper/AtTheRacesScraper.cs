﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using TheHorses.SharedTypes;

namespace TheHorses.Scraper
{
    /// <summary>
    ///     Scrapes the At the Races results page
    /// </summary>
    public class AtTheRacesScraper : IResultsScraper
    {
        private readonly string _baseUrl = "http://www.attheraces.com/results";
        
        #region IResultsScraper Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns>A List of race results scraped from the web page</returns>
        public async Task<IEnumerable<RaceResult>> ScrapeResults(DateTime when)
        {
            //Sorry, future me, in advance...
            var results = new List<RaceResult>();
            string url = DetermineUrlForDate(when);
            string thePage = await HTTPStuff.DownloadPageAsync(url);
            var pageAsStream = new MemoryStream(Encoding.UTF8.GetBytes(thePage));

            var doc = new HtmlDocument();
            doc.Load(pageAsStream);

            var wrapper = doc.GetElementbyId("wrapper");
            var centerCol = wrapper.ChildNodes.FirstOrDefault(n => n.Attributes["class"]?.Value == "center_col");
            var accordion = centerCol?.ChildNodes[3];

            if (accordion == null) return null;

            //accordion is split by venue
            foreach (var venue in accordion.ChildNodes.Where(node => node.Name == "div"))
            {
                string track = venue.ChildNodes[1].FirstChild.InnerHtml;

                if(track.Contains("USA") || track.Contains("RSA") || track.ToLower().Contains("france")) continue;

                track = track.Replace("&nbsp;", " ");
                
                var liChildren = venue.ChildNodes[3].ChildNodes[1].ChildNodes;

                Race race = new Race(); 
                
                foreach (var c in liChildren.Where(l=>l.Name!="#text"))
                {

                    //it alternates h5 -> div, h5 -> div. info in h5, positions in div
                    if (c.Name == "h5")
                    {
                        // lots of data in the text of this link (time, race name, class, distance)
                        var linkText = c.FirstChild.InnerHtml.RemoveMany(new[] {"\r", "\n"});
                        linkText = linkText.Replace("&nbsp;", "|");
                        var fields = linkText.Split('|').Select(f => f.Trim()).ToArray();

                        //time is in the link as hh:mm
                        var time = fields[0].Split(':');
                        int hour, minute;
                        if (!int.TryParse(time[0], out hour)) hour = 0;
                        if (!int.TryParse(time[1], out minute)) minute = 0;

                        race = new Race
                        {
                            Venue = track,
                            Name = fields[1],
                            When = new DateTime(when.Year, when.Month, when.Day, hour, minute, 0)
                        };

                    }
                    else if (c.Name == "div")
                    {
                        HtmlNode left = c.ChildNodes.FirstOrDefault(cn => cn.Attributes["class"]?.Value == "split_left");
                        List<HtmlNode> listChildren = left?.ChildNodes.FirstOrDefault(n => n.Name == "ul")?.ChildNodes?.Where(n => n.Name == "li").ToList();

                        var places = new List<Place>();
                        for (var i = 0; i < listChildren?.Count; ++i)
                        {
                            var horse = listChildren[i].ChildNodes?.FirstOrDefault(n => n.Name == "a")?.FirstChild.InnerHtml;
                            places.Add(new Place {Position = i+1, Horse = new Horse { Name = horse } });
                        }

                        results.Add(new RaceResult { Race = race, Places = places });
                    }
                }
            }

            return results;
        }
        
        #endregion

        #region Private Helpers

        private string DetermineUrlForDate(DateTime when)
        {
            Func<int, string> zeroMaybe = i => i < 10 ? $"0{i}" : i.ToString();
            Func<DateTime, string> getUrl = dt => $"{_baseUrl}/?date={dt.Year}-{zeroMaybe(dt.Month)}-{zeroMaybe(dt.Day)}";

            return getUrl(when);
        }

        #endregion
    }
}