using HtmlAgilityPack;
using InfoTrackSearch.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace InfoTrackSearch.Models
{
    public class HtmlParser : IParser
    {
        public SearchQuery SearchQuery { get; set; }
        public int MaxResultPage { get; set; }
        public string SearchEngineUrl { get; set; }
        public int MaxNumberOfResults { get; set; }
        HtmlDocument Doc { get; set; } = new HtmlDocument();


        public HtmlParser(SearchQuery searchQuery, int maxResultsPage = 10, 
            string searchEngineUrl = "https://infotrack-tests.infotrack.com.au/Google/", int maxNumberOfResults = 50)
        {
            SearchQuery = searchQuery;
            MaxResultPage = maxResultsPage;
            SearchEngineUrl = searchEngineUrl;
            MaxNumberOfResults = maxNumberOfResults;

            SearchQuery.Url = SearchQuery.Url.Replace("https://", "");
        }


        public HtmlNodeCollection RemoveJSContent(string xpath)
        {
            var nodes = Doc.DocumentNode.SelectNodes(xpath);
            foreach (var node in nodes)
            {
                node.ParentNode.RemoveChild(node);
            }
            return nodes;
        }

        public List<HtmlNode> GetHrefList()
        {
            var htmlItemList = Doc.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("r")).ToList();

            var hrefList = htmlItemList[0].Descendants("a")
                .Where(node => node.GetAttributeValue("href", "")
                .Contains(SearchQuery.Url)).ToList();

            return hrefList;
        }

        public string GetMatchingPositions()
        {
            var position = 1;
            var positionString = "";
            foreach (var item in GetHrefList())
            {
                if (item.InnerHtml.Contains(SearchQuery.Url))
                {
                    if (position <= MaxNumberOfResults)
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
            return positionString;
        }

        public async Task<SearchQuery> GetSearchResults()
        {
            var currentPage = 1;
            var maxPage = MaxResultPage;
            var decimalLength = currentPage.ToString("D").Length + 1;

            var htmlString = "";
            var httpClient = new HttpClient();
            for (int i = 0; i < maxPage; i++)
            {
                var infoTrackUrl = SearchEngineUrl + "Page" + currentPage.ToString("D" + decimalLength.ToString()) + ".html";
                htmlString += await httpClient.GetStringAsync(infoTrackUrl);
                currentPage++;
            }
            Doc.LoadHtml(htmlString);

            // remove JS content
            RemoveJSContent("//script|//style");

            SearchQuery.Positions = GetMatchingPositions();

            return SearchQuery;
        }

    }
}