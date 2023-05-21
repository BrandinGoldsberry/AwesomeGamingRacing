using System.Data.Common;

namespace AwesomeGamingRacing.Data
{
    public class Database
    {
        private DbConnection connection;
        private string ConnectionString { get; set; }

        public Database(string ConnString)
        {
            this.ConnectionString = ConnString;
        }

        ~Database()
        {
            if(connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }

        public T GetConnection<T>() where T : DbConnection, new()
        {
            T t = new T();
            t.ConnectionString = ConnectionString;
            connection = t;
            t.Open();
            return t;
        }
    }
}
