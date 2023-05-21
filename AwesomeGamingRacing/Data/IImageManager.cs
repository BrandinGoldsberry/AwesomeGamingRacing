namespace AwesomeGamingRacing.Data
{
    public interface IImageManager
    {
        public string BaseImagePath { get; set; }
        public string DefaultImage { get; set; }
        public string ImageProtocol { get; set; }
    }
}
