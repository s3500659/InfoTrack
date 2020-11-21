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

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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
        public async Task<IActionResult> Search(SearchQuery blankSearchQuery)
        {
            if (blankSearchQuery.Keywords == null)
            {
                blankSearchQuery.Keywords = "online title search";
            }

            if (blankSearchQuery.SearchForUrl == null)
            {
                blankSearchQuery.SearchForUrl = "https://www.infotrack.com.au";
            }

            var selectedSearchEngine = blankSearchQuery.SearchEngine;
            var maxresults = 50;
            var maxpage = 10;

            var searchStrategy = new SearchStrategy();
            var director = new SearchQueryDirector();
            var htmlParser = HtmlParser.GetInstance();
            SearchQuery result;
            if (selectedSearchEngine == SearchEngineEnum.Bing)
            {
                searchStrategy.SetStrategy(new BingSearchStrategy());
                result = await searchStrategy.DoSearchAsync(blankSearchQuery, director, htmlParser, maxresults, maxpage);
            } 
            else
            {
                searchStrategy.SetStrategy(new GoogleSearchStrategy());
                result = await searchStrategy.DoSearchAsync(blankSearchQuery, director, htmlParser, maxresults, maxpage);
            }

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
