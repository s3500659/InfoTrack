using InfoTrackSearch.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoTrackSearch.Models
{
    public class SearchQueryDirector
    {
        public ISearchQueryBuilder SearchQueryBuilder { get; set; }

        public SearchQueryDirector(ISearchQueryBuilder searchQueryBuilder)
        {
            SearchQueryBuilder = searchQueryBuilder;
        }

        public SearchQuery GetSearchQuery()
        {
            return SearchQueryBuilder.GetSearchQuery();
        }

        public void CreateSearchQuery(string searchForUrl, string keywords)
        {
            SearchQueryBuilder.SetMaxNumberOfResults(50);
            SearchQueryBuilder.SetMaxResultPage(10);
            SearchQueryBuilder.SetSearchEngine();
            SearchQueryBuilder.SetSearchForUrl(searchForUrl);
            SearchQueryBuilder.SetSearchKeywords(keywords);
        }


    }
}
