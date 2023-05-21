using AwesomeGamingRacing.Data;
using AwesomeGamingRacing.Models;
using AwesomeGamingRacing.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AwesomeGamingRacing.Controllers
{
    public class RaceController : Controller
    {
        private IConfiguration _configuration;
        private readonly IRaceRepository _raceRepository;

        public RaceController(IConfiguration config, IRaceRepository raceRepository)
        {
            _configuration = config;
            _raceRepository = raceRepository;
        }

        public ActionResult List()
        {
            ViewBag.Message = _configuration["ConnectionStrings:Race"];
            return View();
        }

        [HttpGet]
        public ActionResult NewTrack()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewTrack(Track track)
        {
            _raceRepository.AddTrack(track);
            return RedirectToPage("/Index");
        }

        //public ActionResult Track(string Name)
        //{

        //}

        public ActionResult ListTracks()
        {
            List<Track> tracks = _raceRepository.GetAllTracks();
            return View(tracks);
        }
    }
}
