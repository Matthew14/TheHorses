using System.Web;
using TheHorses.Database;

namespace TheHorses.API
{
    public static class Globals
    {
        private static DatabaseCredentials _credentials;
        private static string _dbCred = HttpContext.Current.Server.MapPath("~/App_Data/DatabaseCredentials.xml");
        public static DatabaseCredentials DBCredentials => _credentials ?? (_credentials = DatabaseCredentials.LoadFromFile(_dbCred));
    }
}
