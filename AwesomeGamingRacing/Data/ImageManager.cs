namespace AwesomeGamingRacing.Data
{
    public class ImageManager : IImageManager
    {
        private readonly IConfiguration _configuration;
        public string BaseImagePath { get; set; }
        public string DefaultImage { get; set; }
        public string ImageProtocol { get; set; }

        public ImageManager(IConfiguration configuration)
        {
            _configuration = configuration;
            BaseImagePath = configuration["ImageSettings:BaseImagePath"];
            DefaultImage = configuration["ImageSettings:DefaultImage"];
            ImageProtocol = configuration["ImageSettings:ImageProtocol"];
        }
    }
}
