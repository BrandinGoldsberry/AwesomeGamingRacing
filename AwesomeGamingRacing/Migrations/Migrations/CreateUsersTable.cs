using AwesomeGamingRacing.Data;

namespace AwesomeGamingRacing.Migrations.Migrations
{
    public class CreateUsersTable : Migration
    {
        public override void Create(IDatabaseFactory databaseFactory)
        {
            Database = databaseFactory.GetUserDatabase();
            UserVersion = 1;
        }

        public override void Down()
        {
            //No
        }

        public override void Up()
        {
            Execute("CreateUsersTable.up.sqlite");
        }
    }
}
