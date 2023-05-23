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
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"Detecting Migration: {FilePath}");
            Console.ForegroundColor = ConsoleColor.White;
            if (File.Exists(FilePath))
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"Running Migration: {FilePath}");
                Console.ForegroundColor = ConsoleColor.White;

                SqliteConnection connection = Database.GetConnection<SqliteConnection>();
                SqliteTransaction transaction = connection.BeginTransaction();
                SqliteCommand cmd = connection.CreateCommand();
                cmd.CommandText = "PRAGMA user_version;";
                long? version = (long?)cmd.ExecuteScalar();
                if (UserVersion > version)
                {
                    try
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Version found");
                        Console.ForegroundColor = ConsoleColor.White;

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
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Migration FAIL");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.StackTrace);
                    }
                }
                else
                {
                    Console.WriteLine($"Migration N/A");
                }
            }
        }

        public abstract void Create(IDatabaseFactory databaseFactory);
    }
}
