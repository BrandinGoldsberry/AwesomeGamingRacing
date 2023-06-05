using AwesomeGamingRacing.Data;
using AwesomeGamingRacing.Migrations.Migrations;

namespace AwesomeGamingRacing.Migrations
{
    public class Migrator
    {
        private readonly IDatabaseFactory databaseFactory;
        private List<Migration> raceMigrations;
        private List<Migration> usersMigrations;

        public Migrator(IDatabaseFactory databaseFactory)
        {
            this.databaseFactory = databaseFactory;
            raceMigrations = new List<Migration>();
            usersMigrations = new List<Migration>();

            RegisterRace<CreateTrackTable>();
            RegisterRace<CreateRaceTable>();
            RegisterUser<CreateUsersTable>();
        }

        public void RunRaceMigrations()
        {
            foreach (var item in raceMigrations)
            {
                item.Up();
            }
        }

        public void RunUsersMigrations()
        {
            foreach (var item in usersMigrations)
            {
                item.Up();
            }
        }

        private void RegisterRace<T>() where T : Migration, new()
        {
            T t = new T();
            t.Create(databaseFactory);
            raceMigrations.Add(t);
        }
        private void RegisterUser<T>() where T : Migration, new()
        {
            T t = new T();
            t.Create(databaseFactory);
            usersMigrations.Add(t);
        }
    }
}
