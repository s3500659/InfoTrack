using InfoTrackSearch.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace InfoTrackSearch.Models
{

    public class SearchQuery : ISearchQuery
    {
        [Display(Name = "Search for URL")]
        public int MaxResultPage { get; set; }
        public int MaxNumberOfResults { get; set; }
        public string SearchForUrl { get; set; } 
        public string Keywords { get; set; } 
        public string Positions { get; set; }
        public SearchEngineEnum SearchEngine { get; set; }

        public void SetFoundPosition(string position)
        {
            Positions = position;
        }

        public void SetMaxNumberOfResults(int maxResults)
        {
            MaxNumberOfResults = maxResults;
        }

        public void SetMaxResultPage(int page)
        {
            MaxResultPage = page;
        }

        public void SetSearchEngine(SearchEngineEnum searchEngine)
        {
            SearchEngine = searchEngine;
        }

        public void SetSearchForUrl(string url)
        {
            SearchForUrl = url;
        }

        public void SetSearchKeywords(string keywords)
        {
            Keywords = keywords;
        }
    }
}
