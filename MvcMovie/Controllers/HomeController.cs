using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcMovie.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MvcMovie.Controllers
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
            return View();
        }

        [HttpGet]
        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search([Bind("Keywords")] SearchResult searchResult)
        {
            var result = await GooglSearchAsync(searchResult.Keywords);
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

        private async Task<SearchResult> GooglSearchAsync(string keywords = "online title search", string url = "www.infotrack.com.au")
        {

            var currentPage = 1;
            var maxPage = 10;
            var decimalLength = currentPage.ToString("D").Length + 1;

            var htmlString = "";
            for (int i = 0; i < maxPage; i++)
            {
                var infoTrackUrl = "https://infotrack-tests.infotrack.com.au/Google/Page" + currentPage.ToString("D" + decimalLength.ToString()) + ".html";
                var httpClient = new HttpClient();
                htmlString += await httpClient.GetStringAsync(infoTrackUrl);
                currentPage++;

            }

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlString);

            // remove JS content
            var nodes = doc.DocumentNode.SelectNodes("//script|//style");
            foreach (var node in nodes)
            {
                node.ParentNode.RemoveChild(node);
            }

            string htmlOutput = doc.DocumentNode.OuterHtml;
            doc.LoadHtml(htmlOutput);

            var htmlItemList = doc.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("r")).ToList();

            var hrefList = htmlItemList[0].Descendants("a")
                .Where(node => node.GetAttributeValue("href", "")
                .Contains(url)).ToList();

            var position = 1;
            var positionString = "";
            var maxNumberOfResults = 50;
            foreach (var item in hrefList)
            {
                if (item.InnerHtml.Contains(url))
                {
                    // only keep first 50 results
                    if (position <= maxNumberOfResults)
                    {
                        positionString = String.Concat(positionString, position, ", ");
                        position++;
                    }
                }
            }

            if (positionString == "")
            {
                positionString = "0";
            } else
            {
                positionString = positionString.Remove(positionString.Length - 2);
            }

            var model = new SearchResult
            {
                Url = url,
                Keywords = keywords,
                Positions = positionString
            };

            return model;

        }


    }
}
