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


        public HtmlParser(SearchQuery searchQuery, int maxResultsPage = 10, int maxNumberOfResults = 50)
        {
            SearchQuery = searchQuery;
            MaxResultPage = maxResultsPage;
            MaxNumberOfResults = maxNumberOfResults;

            if (SearchQuery.SearchEngine == SearchEngine.Bing)
            {
                SearchEngineUrl = "https://infotrack-tests.infotrack.com.au/Bing/";
            } 
            else
            {
                SearchEngineUrl = "https://infotrack-tests.infotrack.com.au/Google/";
            }  
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

        // return a list of html node filtered to max number of desired results (50)
        public List<HtmlNode> GetHrefList()
        {
            var filteredList = new List<HtmlNode>(MaxNumberOfResults);
            if (SearchQuery.SearchEngine == SearchEngine.Google)
            {
                var htmlItemList = Doc.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("r")).ToList();

                for (int i = 0; i < MaxNumberOfResults; i++)
                {
                    filteredList.Add(htmlItemList[i]);
                }
            }
            else
            {
                var htmlItemList = Doc.DocumentNode.Descendants("li")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("b_algo")).ToList();

                for (int i = 0; i < MaxNumberOfResults; i++)
                {
                    filteredList.Add(htmlItemList[i]);
                }
            }
            
            return filteredList;
        }

        public string GetMatchingPositions()
        {
            var positionString = "";
            var filteredList = GetHrefList();
            for (int i = 0; i < filteredList.Count; i++)
            {
                if (filteredList[i].InnerHtml.Contains(SearchQuery.SearchForUrl))
                {
                    if (i < MaxNumberOfResults)
                    {
                        positionString = String.Concat(positionString, i+1, ", ");
                    }
                }
            }

            if (positionString == "")
            {
                positionString = $"URL not found within the first {MaxNumberOfResults}th position.";
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
            //RemoveJSContent("//script|//style");

            SearchQuery.Positions = GetMatchingPositions();

            return SearchQuery;
        }

    }
}