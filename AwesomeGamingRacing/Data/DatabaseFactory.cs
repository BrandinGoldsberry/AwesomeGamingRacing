namespace AwesomeGamingRacing.Data
{
    public class DatabaseFactory : IDatabaseFactory
    {
        private IConfiguration _configuration;

        public DatabaseFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Database GetRaceDatabase()
        {
            return new Database(_configuration["connectionstrings:race"]);
        }

        public Database GetUserDatabase()
        {
            return new Database(_configuration.GetConnectionString("user"));
        }
    }
}
