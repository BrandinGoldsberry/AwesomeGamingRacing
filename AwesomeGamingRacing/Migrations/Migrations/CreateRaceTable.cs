using AwesomeGamingRacing.Data;

namespace AwesomeGamingRacing.Migrations.Migrations
{
    public class CreateRaceTable : Migration
    {
        public override void Create(IDatabaseFactory databaseFactory)
        {
            Database = databaseFactory.GetRaceDatabase();
            UserVersion = 0;
        }

        public override void Down()
        {
            //No
        }

        public override void Up()
        {
            Execute("CreateRaceTable.up.sqlite");
        }
    }
}
