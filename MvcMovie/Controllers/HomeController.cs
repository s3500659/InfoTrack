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

        public async Task<IActionResult> Index()
        {
            var searchResult = await GooglSearchAsync();
            return View(searchResult);
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

        private async Task<SearchResult> GooglSearchAsync(string url = "www.infotrack.com.au", string keywords = "online title search")
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

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlString);

            // remove JS content
            var nodes = htmlDocument.DocumentNode.SelectNodes("//script|//style");
            foreach (var node in nodes)
            {
                node.ParentNode.RemoveChild(node);
            }

            string htmlOutput = htmlDocument.DocumentNode.OuterHtml;
            htmlDocument.LoadHtml(htmlOutput);

            var htmlItemList = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("r")).ToList();

            var productListItems = htmlItemList[0].Descendants("a")
                .Where(node => node.GetAttributeValue("href", "")
                .Contains(url)).ToList();

            var position = 1;
            var positionString = "";
            foreach (var item in productListItems)
            {
                if (item.InnerHtml.Contains(url))
                {
                    positionString = String.Concat(positionString, position, ", ");
                    position++;
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
