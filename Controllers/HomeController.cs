using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using InfoTrackSearch.Models;
using System.Diagnostics;
using System.Threading.Tasks;
using InfoTrackSearch.Models.Strategy;

namespace InfoTrackSearch.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SearchStrategyFactory _searchStrategyFactory;
        private readonly SearchQueryDirector _searchDirector;

        public HomeController(ILogger<HomeController> logger, SearchStrategyFactory searchStrategyFactory, SearchQueryDirector director)
        {
            _logger = logger;
            _searchStrategyFactory = searchStrategyFactory;
            _searchDirector = director;
        }

        public IActionResult Index()
        {
            var blankSearchQuery = new SearchQuery();
            return View(blankSearchQuery);
        }

        [HttpGet]
        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search(SearchQuery searchQuery)
        {
            if (searchQuery.Keywords == null)
            {
                searchQuery.Keywords = "online title search";
            }

            if (searchQuery.SearchForUrl == null)
            {
                searchQuery.SearchForUrl = "https://www.infotrack.com.au";
            }

            var maxresults = 50;
            var maxpage = 10;
            var htmlParser = HtmlParser.GetInstance();

            var result = await _searchStrategyFactory.DoSearchAsync(searchQuery, _searchDirector, htmlParser, maxresults, maxpage);

            return View(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
