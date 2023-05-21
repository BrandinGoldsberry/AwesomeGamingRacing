using AwesomeGamingRacing.Data;
using Microsoft.Data.Sqlite;
using System.Text;

namespace AwesomeGamingRacing.Migrations
{
    public abstract class Migration
    {
        public int UserVersion { get; set; }
        public abstract void Up();
        public abstract void Down();

        protected Database Database { get; set; }

        public Migration() 
        { 
        
        }

        protected void Execute(string FilePath)
        {
            FilePath = Environment.CurrentDirectory + "\\Migrations\\SqlFiles\\" + FilePath;
            if (File.Exists(FilePath))
            {
                SqliteConnection connection = Database.GetConnection<SqliteConnection>();
                SqliteTransaction transaction = connection.BeginTransaction();
                SqliteCommand cmd = connection.CreateCommand();
                cmd.CommandText = "PRAGMA user_version;";
                long? version = (long?)cmd.ExecuteScalar();
                if (UserVersion > version)
                {
                    try
                    {
                        transaction.Save("Start");
                        string SQL = File.ReadAllText(FilePath);
                        StringBuilder builder = new StringBuilder(SQL);
                        builder.Append("PRAGMA user_version = {0}");
                        string SQLToRun = String.Format(builder.ToString(), UserVersion);

                        SqliteCommand alterCmd = connection.CreateCommand();
                        alterCmd.CommandText = SQLToRun;
                        alterCmd.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback("Start");
                    }
                }
            }
        }

        public abstract void Create(IDatabaseFactory databaseFactory);
    }
}
