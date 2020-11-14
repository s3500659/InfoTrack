using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MvcMovie.Models
{
    public class HtmlParser
    {
        public string SearchForUrl = "www.infotrack.com.au";
        public string SearchForKeywords = "online title search";


        public HtmlParser(string searchForKeywords)
        {
            SearchForKeywords = searchForKeywords;
        }

        public async Task<SearchResult> GetSearchResults()
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
                .Contains(SearchForUrl)).ToList();

            var position = 1;
            var positionString = "";
            var maxNumberOfResults = 50;
            foreach (var item in hrefList)
            {
                if (item.InnerHtml.Contains(SearchForUrl))
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
            }
            else
            {
                positionString = positionString.Remove(positionString.Length - 2);
            }

            var model = new SearchResult
            {
                Url = SearchForUrl,
                Keywords = SearchForKeywords,
                Positions = positionString
            };

            return model;
        }

    }
}