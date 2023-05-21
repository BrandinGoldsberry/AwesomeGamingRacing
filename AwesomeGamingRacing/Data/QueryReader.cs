using Microsoft.Data.Sqlite;
using System.Reflection;

namespace AwesomeGamingRacing.Data
{
    public class QueryReader
    {
        private readonly Database _database;
        private readonly string _query;
        private readonly IImageManager _imageManager;

        public QueryReader(Database db, string query, IImageManager imageManager)
        {
            _database = db;
            _query = query;
            _imageManager = imageManager;
        }

        public List<T> GetQueryables<T>() where T : new()
        {
            List<T> Ret = new List<T>();
            SqliteConnection _connection = _database.GetConnection<SqliteConnection>();
            SqliteCommand cmd = _connection.CreateCommand();
            cmd.CommandText = _query;
            SqliteDataReader r = cmd.ExecuteReader();

            while (r.Read())
            {
                T newT = GetFromQuery<T>(r);
                
                Ret.Add(newT);
            }
            _connection.Close();
            return Ret;
        }

        public T GetFromQuery<T>(SqliteDataReader dataReader) where T : new()
        {
            T newT = new T();
            Type tType = typeof(T);

            int count = dataReader.FieldCount;
            for (int i = 0; i < count; i++)
            {
                string name = dataReader.GetName(i);
                PropertyInfo prop = tType.GetProperty(name);
                if (prop != null)
                {
                    Type type = dataReader.GetFieldType(i);
                    string value = null;
                    if (!dataReader.IsDBNull(i))
                        value = dataReader.GetString(i);
                    object trueVal = null;
                    if (prop.PropertyType.IsValueType)
                        trueVal = 0;
                    if (prop.PropertyType.IsEnum)
                    {
                        if(!Enum.TryParse(prop.PropertyType, value, out trueVal))
                        {
                            trueVal = 0;
                        }
                    }
                    else if(prop.PropertyType == typeof(Uri))
                    {
                        UriBuilder uriBuilder = new UriBuilder(_imageManager.ImageProtocol, _imageManager.BaseImagePath);
                        uriBuilder.Path = value ?? _imageManager.DefaultImage;
                        trueVal = uriBuilder.Uri;
                    }
                    else if (value != null)
                    {
                        trueVal = Convert.ChangeType(value, type);
                    }
                    prop.SetValue(newT, trueVal);
                }
            }
            return newT;
        }
    }
}
