using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using InfoTrackSearch.Models;
using System.Diagnostics;
using System.Threading.Tasks;

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

            SearchQuery newSearchQuery;
            var maxResults = 50;
            var maxPage = 10;
            if (selectedSearchEngine == SearchEngineEnum.Bing)
            {
                var bingSearch = new BingSearchQuery();
                var queryDirector = new SearchQueryDirector(bingSearch);
                queryDirector.CreateSearchQuery(blankSearchQuery.SearchForUrl, blankSearchQuery.Keywords, maxResults, maxPage);
                newSearchQuery = queryDirector.GetSearchQuery();
            }
            else
            {
                var googleSearch = new GoogleSearchQuery();
                var queryDirector = new SearchQueryDirector(googleSearch);
                queryDirector.CreateSearchQuery(blankSearchQuery.SearchForUrl, blankSearchQuery.Keywords, maxResults, maxPage);
                newSearchQuery = queryDirector.GetSearchQuery();
            }

            var htmlParser = HtmlParser.GetInstance();
            htmlParser.SetSearchQuery(newSearchQuery);

            var searchEngine = new SearchEngine(htmlParser, newSearchQuery);
            var result = await searchEngine.GetSearchResults();

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
