using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using InfoTrackSearch.Models;
using System.Diagnostics;
using System.Threading.Tasks;
using System;

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
            var searchQuery = new SearchQuery();
            return View(searchQuery);
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
                searchQuery.SearchForUrl = "www.infotrack.com.au";
            }
            var parser = new HtmlParser(searchQuery);
            var searchEngine = new SearchEngine(parser, searchQuery);
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
