using HtmlAgilityPack;
using InfoTrackSearch.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InfoTrackSearch.Models
{
    public class HtmlParser :IParser
    {
        private static HtmlParser Instance { get; set; }
        public SearchQuery SearchQuery { get; set; }
        public HtmlDocument Doc { get; set; } = new HtmlDocument();

        private HtmlParser()
        {
            
        }

        public static HtmlParser GetInstance()
        {
            if (Instance == null)
            {
                Instance = new HtmlParser();
            }
            return Instance;
        }

        public void SetSearchQuery(SearchQuery searchQuery)
        {
            SearchQuery = searchQuery;
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
            var filteredList = new List<HtmlNode>(SearchQuery.MaxNumberOfResults);
            if (SearchQuery.SearchEngine == SearchEngineEnum.Google)
            {
                var htmlItemList = Doc.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("r")).ToList();

                for (int i = 0; i < SearchQuery.MaxNumberOfResults; i++)
                {
                    filteredList.Add(htmlItemList[i]);
                }
            }
            else
            {
                var htmlItemList = Doc.DocumentNode.Descendants("li")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("b_algo")).ToList();

                for (int i = 0; i < SearchQuery.MaxNumberOfResults; i++)
                {
                    filteredList.Add(htmlItemList[i]);
                }
            }
            
            return filteredList;
        }

        // find matching positions
        public string GetMatchingPositions()
        {
            var positionString = "";
            var filteredList = GetHrefList();
            for (int i = 0; i < filteredList.Count; i++)
            {
                if (filteredList[i].InnerHtml.Contains(SearchQuery.SearchForUrl))
                {
                    if (i < SearchQuery.MaxNumberOfResults)
                    {
                        positionString = String.Concat(positionString, i+1, ", ");
                    }
                }
            }

            if (positionString == "")
            {
                positionString = $"URL not found within the first {SearchQuery.MaxNumberOfResults}th results.";
            }
            else
            {
                positionString = positionString.Remove(positionString.Length - 2);
            }
            return positionString;
        }
    }
}