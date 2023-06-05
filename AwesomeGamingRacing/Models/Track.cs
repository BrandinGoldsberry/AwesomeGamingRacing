using AwesomeGamingRacing.Data;
using AwesomeGamingRacing.Models.Enums;
using Microsoft.Data.Sqlite;

namespace AwesomeGamingRacing.Models
{
    public class Track
    {
        private Uri uri;
        public int RowId { get; set; }
        public string Name { get; set; }
        public Uri Image 
        {
            get
            {
                if(uri == null)
                {
                    UriBuilder uriBuilder = new UriBuilder();
                    uriBuilder.Scheme = "file";
                    uriBuilder.Path = "/Default.jpg";
                    uri = uriBuilder.Uri;
                }
                return uri;
            }
            set
            {
                uri = value;
            } 
        }
        public double Length { get; set; }
        public GameName GameName { get; set; }
    }
}
