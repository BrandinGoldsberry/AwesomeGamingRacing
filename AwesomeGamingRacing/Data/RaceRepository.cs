using AwesomeGamingRacing.Models;
using AwesomeGamingRacing.Models.Enums;
using Microsoft.Data.Sqlite;

namespace AwesomeGamingRacing.Data
{
    public class RaceRepository : IRaceRepository
    {
        private readonly Database raceDatabase;
        private readonly IImageManager imageManager;
        public RaceRepository(IDatabaseFactory databaseFactory, IImageManager imageManager)
        {
            raceDatabase = databaseFactory.GetRaceDatabase();
            this.imageManager = imageManager;
        }

        public void AddTrack(Track Track)
        {
            SqliteConnection conn = raceDatabase.GetConnection<SqliteConnection>();
            SqliteCommand cmd = conn.CreateCommand();
            cmd.CommandText =
            @"
                INSERT INTO tracks (Name, Length, GameName, Image) VALUES (@Name, @Length, @GameName, @Image);
            ";
            cmd.Parameters.AddWithValue("@Name", Track.Name);
            cmd.Parameters.AddWithValue("@Length", Track.Length);
            cmd.Parameters.AddWithValue("@GameName", Track.GameName);
            cmd.Parameters.AddWithValue("@Image", Track.Image.PathAndQuery);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public List<Track> GetAllTracks()
        {
            List<Track> results = new List<Track>();
            string sql =
            @"
                SELECT 
                    Name,
                    Image,
                    Length,
                    GameName
                FROM tracks;
            ";
            
            QueryReader reader = new QueryReader(raceDatabase, sql, imageManager);
            results = reader.GetQueryables<Track>();

            return results;
        }
    }
}
