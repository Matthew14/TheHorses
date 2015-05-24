using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using TheHorses.Database;
using TheHorses.SharedTypes;

namespace TheHorses.API.Controllers
{
    //[Authorize]
    public class ResultsController : ApiController
    {
        private readonly Dao _dao;

        public ResultsController()
        {
            IDatabase db;

            lock (Globals.DBCredentials)
                db = new SQLServerDatabase(Globals.DBCredentials);

            _dao = new Dao(db);
        }

        // GET api/values
        public string Get()
        {
            return "hello";
        }

        // GET api/results/today
        /// <exception cref="HttpException">404</exception>
        public IEnumerable<RaceResult> Get(string id)
        {
            int daysAgo;

            switch (id.ToLower())
            {
                case "today":
                    daysAgo = 0;
                    break;
                case "yesterday":
                    daysAgo = 1;
                    break;
                case "daybeforeyesterday":
                    daysAgo = 2;
                    break;
                default:
                    throw new HttpException(404, $"What the hell is {id}?");
            }
            
            return _dao.GetResultsForDay(DateTime.Now - new TimeSpan(daysAgo, 0, 0, 0));
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
