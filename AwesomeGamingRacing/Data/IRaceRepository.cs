using AwesomeGamingRacing.Models;

namespace AwesomeGamingRacing.Data
{
    public interface IRaceRepository
    {
        void AddTrack(Track Track);
        List<Track> GetAllTracks();
    }
}
