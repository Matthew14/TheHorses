using System;
using System.IO;
using System.Xml.Serialization;

namespace TheHorses.Database
{
    [Serializable]
    public class DatabaseCredentials
    {
        public string User { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string Database { get; set; }

        public static DatabaseCredentials LoadFromFile(string filePath)
        {
            var serializer = new XmlSerializer(typeof(DatabaseCredentials));
            DatabaseCredentials credentials;

            try
            {
                using (var f = File.OpenRead(filePath))
                    credentials = (DatabaseCredentials) serializer.Deserialize(f);
            }
            catch
            {
                credentials = null;
            }

            return credentials;
        }

        public void SaveToFile(string filePath)
        {
            var serializer = new XmlSerializer(typeof(DatabaseCredentials));
            using (var f = File.OpenWrite(filePath))
                serializer.Serialize(f, this);
        }
    }
}