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

        public SearchQueryDirector()
        {
            
        }

        public SearchQueryDirector(ISearchQueryBuilder searchQueryBuilder)
        {
            SearchQueryBuilder = searchQueryBuilder;
        }

        public void SetSearchQueryBuilder(ISearchQueryBuilder searchQueryBuilder)
        {
            SearchQueryBuilder = searchQueryBuilder;
        }

        public SearchQuery GetSearchQuery()
        {
            return SearchQueryBuilder.GetSearchQuery();
        }

        public void CreateSearchQuery(string searchForUrl, string keywords, int maxNumberOfResults, int maxResultPage)
        {
            SearchQueryBuilder.SetMaxNumberOfResults(maxNumberOfResults);
            SearchQueryBuilder.SetMaxResultPage(maxResultPage);
            SearchQueryBuilder.SetSearchEngine();
            SearchQueryBuilder.SetSearchForUrl(searchForUrl);
            SearchQueryBuilder.SetSearchKeywords(keywords);
        }


    }
}
