using AwesomeGamingRacing.Models;
using Microsoft.Data.Sqlite;
using System.Text;

namespace AwesomeGamingRacing.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly Database userDatabase;
        private readonly IImageManager imageManager;
        public UserRepository(IDatabaseFactory databaseFactory, IImageManager imageManager)
        {
            userDatabase = databaseFactory.GetUserDatabase();
            this.imageManager = imageManager;
        }
        public async Task<bool> AddUser(User user)
        {
            int nextUserId = await GetNextUserId();
            user.RowId = nextUserId;

            SqliteConnection conn = userDatabase.GetConnection<SqliteConnection>();
            conn.BeginTransaction();
            SqliteCommand cmd = conn.CreateCommand();
            cmd.CommandText =
            @"
                INSERT INTO users (Name, Email, Image, Bio, Password, Role) VALUES (@Name, @Email, @Image, @Bio, @Password, @Role);
            ";
            cmd.Parameters.AddWithValue("@Name", user.Name);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@Image", user.Image.PathAndQuery);
            cmd.Parameters.AddWithValue("@Bio", user.Bio);
            cmd.Parameters.AddWithValue("@Password", user.NewPassword);
            cmd.Parameters.AddWithValue("@Salt", user.Salt);
            cmd.Parameters.AddWithValue("@Role", user.Role);
            int result = await cmd.ExecuteNonQueryAsync();
            if(result > 0)
            {
                bool saltRes = await AddUserSalt(user, conn);
                if(!saltRes)
                {
                    cmd.Transaction.Rollback();
                }
                else
                {
                    cmd.Transaction.Commit();
                }
                return saltRes;
            }
            else
            {
                return false;
            }
        }

        private async Task<bool> AddUserSalt(User user, SqliteConnection conn)
        {
            SqliteCommand cmd = conn.CreateCommand();
            cmd.CommandText =
            @"
                INSERT INTO usersalts (UserId, Salt) VALUES (@UserId, @Salt);
            ";
            cmd.Parameters.AddWithValue("@UserId", user.RowId);
            cmd.Parameters.AddWithValue("@Salt", user.Salt);
            int result = await cmd.ExecuteNonQueryAsync();
            return result > 0;
        }

        private async Task<int> GetNextUserId()
        {
            SqliteConnection conn = userDatabase.GetConnection<SqliteConnection>();
            SqliteCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"
            SELECT MAX(RowId) FROM users
            ";
            object result = await cmd.ExecuteScalarAsync();
            conn.Close();
            if(result.GetType() == typeof(DBNull))
            {
                return 1;
            }
            else
            {
                return (int)result;
            }
        }

        public User GetUser(string Name)
        {
            string sql =
            @"
                SELECT * FROM users
                WHERE Name = @Name
                LIMIT 1;
            ";
            QueryReader reader = new QueryReader(userDatabase, sql, imageManager);
            reader.AddParameter("@Name", Name);
            User result = reader.GetQueryables<User>().FirstOrDefault();

            return result;
        }
    }
}
