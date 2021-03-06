﻿using InfoTrackSearch.Interfaces;
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
            SearchQuery.Positions = position;
        }

        public void SetMaxNumberOfResults(int maxResults)
        {
            SearchQuery.MaxNumberOfResults = maxResults;
        }

        public void SetMaxResultPage(int page)
        {
            SearchQuery.MaxResultPage = page;
        }

        public void SetSearchEngine()
        {
            SearchQuery.SearchEngine = SearchEngineEnum.Google;
        }

        public void SetSearchForUrl(string url)
        {
            SearchQuery.SearchForUrl = url;
        }

        public void SetSearchKeywords(string keywords)
        {
            SearchQuery.Keywords = keywords;
        }

        public SearchQuery GetSearchQuery()
        {
            return SearchQuery;
        }
    }
}
