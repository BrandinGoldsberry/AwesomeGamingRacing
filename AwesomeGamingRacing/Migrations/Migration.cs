using AwesomeGamingRacing.Data;
using Microsoft.Data.Sqlite;
using System.Reflection;
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
            string sqlContent = ReadResource(FilePath);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"Detecting Migration: {FilePath}");
            Console.ForegroundColor = ConsoleColor.White;
            if (!string.IsNullOrWhiteSpace(sqlContent))
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
                        StringBuilder builder = new StringBuilder(sqlContent);
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
        private string ReadResource(string name)
        {
            // Determine path
            var assembly = Assembly.GetExecutingAssembly();
            string resourcePath = name;
            // Format: "{Namespace}.{Folder}.{filename}.{Extension}"
            if (!name.StartsWith(nameof(AwesomeGamingRacing)))
            {
                resourcePath = assembly.GetManifestResourceNames()
                    .Single(str => str.EndsWith(name));
            }

            using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public abstract void Create(IDatabaseFactory databaseFactory);
    }
}
