using AwesomeGamingRacing.Data;

namespace AwesomeGamingRacing.Migrations.Migrations
{
    public class CreateTrackTable : Migration
    {
        public override void Create(IDatabaseFactory databaseFactory)
        {
            Database = databaseFactory.GetRaceDatabase();
            UserVersion = 1;
        }

        public override void Down()
        {
            //No
        }

        public override void Up()
        {
            Execute("CreateTrackTable.up.sqlite");
        }
    }
}
