﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using TheHorses.SharedTypes;

namespace TheHorses.Database
{
    public class Dao
    {
        private readonly IDatabase _database;
        public Dao(IDatabase database)
        {
            _database = database;
        }

        public void AddResults(IEnumerable<RaceResult> results)
        {
            _database.Open();

            foreach (var result in results)
            {
                Race race = result.Race;
                int raceId = GetRaceId(race);
                if (raceId == -1)
                {
                    InsertRace(race);
                    raceId = GetRaceId(race);
                }

                foreach (var place in result.Places)
                {
                    Horse horse = place.Horse;
                    int position = place.Position;
                    
                    int horseId = GetHorseId(horse);
                    if (horseId == -1)
                    {
                        InsertHorse(horse);
                        horseId = GetHorseId(horse);
                    }

                    if(!RaceResultExists(raceId, horseId))
                        InsertRaceResult(raceId, horseId, position);
                }
            }

            _database.Close();
        }

        public IEnumerable<RaceResult> GetResultsForDay(DateTime day)
        {
            bool needsClosing = false;
            var results = new List<RaceResult>();
            var sql = "SELECT r.name as race_name, r.venue, r.race_time, rr.position, h.name as horse_name " +
                      "FROM race r " +
                      "join race_result rr on r.id = rr.race_id " +
                      "join horse h on rr.horse_id = h.id " +
                      "WHERE CONVERT(date, r.race_time) = @date";

            if (!_database.IsOpen)
            {
                _database.Open();
                needsClosing = true;
            }

            var command = _database.GetCommand(sql);
            command.Parameters.Add(_database.GetParam("@date", $"{day.Year}-{day.Month}-{day.Day}"));

            var reader = _database.Query(command);

            while (reader.Read())
            {
                Race race = new Race
                {
                    Name = reader["race_name"] as string,
                    Venue = reader["venue"] as string,
                    When = (DateTime) reader["race_time"]
                };
                RaceResult result = results.SingleOrDefault(res => res.Race == race);

                if (result == null)
                {
                    result = new RaceResult {Places = new List<Place>(), Race = race};
                    results.Add(result);
                }

                result.Places.Add(
                    new Place
                    {
                        Horse = new Horse {Name = reader["horse_name"] as string},
                        Position = (int) reader["position"]
                    });
            }

            reader.Close();

            if (_database.IsOpen && needsClosing)
                _database.Close();

            return results;
        }

        #region Privates

        private bool RaceExists(Race race) => GetRaceId(race) != -1;

        private bool HorseExists(Horse horse) => GetHorseId(horse) != -1;

        private bool RaceResultExists(int raceId, int horseId)
        {
            bool needsClosing = false;
            
            if (!_database.IsOpen)
            {
                _database.Open();
                needsClosing = true;
            }

            string sql = "SELECT * FROM race_result WHERE race_id = @rID AND horse_id = @hID";

            var command = _database.GetCommand(sql);
            command.Parameters.AddRange(new[]
            {
                _database.GetParam("@rID", raceId),
                _database.GetParam("@hID", horseId)
            });

            var reader = _database.Query(command);

            bool exists = reader.HasRows;

            reader.Close();

            if (_database.IsOpen && needsClosing)
                _database.Close();

            return exists;
        }

        private void InsertRaceResult(int raceId, int horseId, int position)
        {
            bool needsClosing = false;
            if (!_database.IsOpen)
            {
                _database.Open();
                needsClosing = true;
            }

            string sql = "INSERT INTO race_result (race_id, horse_id, position) VALUES (@rID, @hID, @pos);";

            var command = _database.GetCommand(sql);
            command.Parameters.AddRange(new[]
            {
                _database.GetParam("@rID", raceId),
                _database.GetParam("@hID", horseId),
                _database.GetParam("@pos", position)
            });

            _database.NonQuery(command);

            if (needsClosing && _database.IsOpen)
                _database.Close();
        }

        private void InsertRace(Race race)
        {
            bool needsClosing = false;
            if (!_database.IsOpen)
            {
                _database.Open();
                needsClosing = true;
            }

            string sql = "INSERT INTO race (name, venue, race_time) VALUES (@name, @venue, @race_time)";
            var command = _database.GetCommand(sql);
            command.Parameters.AddRange(new[]
            {
                _database.GetParam("@name", race.Name),
                _database.GetParam("@venue", race.Venue),
                _database.GetParam("@race_time", race.When)
            });

            _database.NonQuery(command);

            if (needsClosing && _database.IsOpen)
                _database.Close();
        }

        private void InsertHorse(Horse horse)
        {
            bool needsClosing = false;
            if (!_database.IsOpen)
            {
                _database.Open();
                needsClosing = true;
            }

            string sql = "INSERT INTO horse (name) VALUES (@name)";
            var command = _database.GetCommand(sql);
            command.Parameters.AddRange(new[]
            {
                _database.GetParam("@name", horse.Name)
            });

            _database.NonQuery(command);

            if (needsClosing && _database.IsOpen)
                _database.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="race"></param>
        /// <returns>-1 if not found</returns>
        private int GetRaceId(Race race)
        {
            bool needsClosing = false;
            int id;

            if (!_database.IsOpen)
            {
                _database.Open();
                needsClosing = true;
            }

            string checkRaceExistsSQL = "SELECT * FROM race WHERE LOWER(name) LIKE @name AND LOWER(venue) LIKE @venue";

            var command = _database.GetCommand(checkRaceExistsSQL);
            command.Parameters.AddRange(new[]
            {
                _database.GetParam("@name", race.Name.ToLower()),
                _database.GetParam("@venue", race.Venue.ToLower())
            });
            var reader = _database.Query(command);

            if (!reader.HasRows)
                id = -1;

            else
            {
                reader.Read();
                id = (int)reader["id"];
            }

            reader.Close();

            if (_database.IsOpen && needsClosing)
                _database.Close();

            return id;
        }

        private int GetHorseId(Horse horse)
        {
            bool needsClosing = false;
            int id;

            if (!_database.IsOpen)
            {
                _database.Open();
                needsClosing = true;
            }

            var checkHorseExistsSQL = "SELECT * FROM horse where lower(name) LIKE @name";
            var command = _database.GetCommand(checkHorseExistsSQL);
            var nameParam = _database.GetParam("@name", horse.Name?.ToLower());
            command.Parameters.Add(nameParam);

            var reader = _database.Query(command);

            if (!reader.HasRows)
                id = -1;
            else
            {
                reader.Read();
                id = (int)reader["id"];
            }
            reader.Close();

            if (needsClosing && _database.IsOpen)
                _database.Close();

            return id;
        }
        #endregion
    }
}
