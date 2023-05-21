using AwesomeGamingRacing.Data;
using AwesomeGamingRacing.Migrations.Migrations;

namespace AwesomeGamingRacing.Migrations
{
    public class Migrator
    {
        private readonly IDatabaseFactory databaseFactory;
        private List<Migration> migrations;

        public Migrator(IDatabaseFactory databaseFactory)
        {
            this.databaseFactory = databaseFactory;
            migrations = new List<Migration>();

            Register<CreateTrackTable>();
        }

        public void RunUp()
        {
            foreach (var item in migrations)
            {
                item.Up();
            }
        }

        private void Register<T>() where T : Migration, new()
        {
            T t = new T();
            t.Create(databaseFactory);
            migrations.Add(t);
        }
    }
}
