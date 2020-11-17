using InfoTrackSearch.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoTrackSearch.Models
{
    public class GoogleSearchQuery : ISearchQueryBuilder
    {
        
        public SearchQuery SearchQuery { get; set; }

        public GoogleSearchQuery()
        {
            SearchQuery = new SearchQuery();
        }


        public void SetFoundPosition(string position)
        {
            SearchQuery.SetFoundPosition(position);
        }

        public void SetMaxNumberOfResults(int maxResults)
        {
            SearchQuery.SetMaxNumberOfResults(maxResults);
        }

        public void SetMaxResultPage(int page)
        {
            SearchQuery.SetMaxResultPage(page);
        }

        public void SetSearchEngine()
        {
            SearchQuery.SetSearchEngine(SearchEngineEnum.Google);
        }

        public void SetSearchForUrl(string url)
        {
            SearchQuery.SetSearchForUrl(url);
        }

        public void SetSearchKeywords(string keywords)
        {
            SearchQuery.SetSearchKeywords(keywords);
        }

        public SearchQuery GetSearchQuery()
        {
            return SearchQuery;
        }
    }
}
