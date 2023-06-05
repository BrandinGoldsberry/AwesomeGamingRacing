namespace AwesomeGamingRacing.Data
{
    public interface IDatabaseFactory
    {
        Database GetRaceDatabase();
        Database GetUserDatabase();
    }
}
