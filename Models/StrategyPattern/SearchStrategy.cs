using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoTrackSearch.Models.Strategy
{
    public interface ISearchStrategy
    {
        Task<SearchQuery> DoSearchAsync(SearchQuery searchQuery, SearchQueryDirector director, HtmlParser htmlParser, int maxNumberResults, int maxResultPages);
    }

    public class SearchStrategy : ISearchStrategy
    {
        private ISearchStrategy _strategy;


        public SearchStrategy()
        {

        }

        public void SetStrategy(ISearchStrategy strategy)
        {
            _strategy = strategy;
        }

        public Task<SearchQuery> DoSearchAsync(SearchQuery searchQuery, SearchQueryDirector director, HtmlParser htmlParser, int maxNumberResults, int maxResultPages)
        {

            var result = _strategy.DoSearchAsync(searchQuery, director, htmlParser, maxNumberResults, maxResultPages);
            return result;
        }
    }

    class GoogleSearchStrategy : ISearchStrategy
    {
        public async Task<SearchQuery> DoSearchAsync(SearchQuery searchQuery, SearchQueryDirector director, HtmlParser htmlParser, int maxNumberResults, int maxResultPages)
        {
            var googleSearch = new GoogleSearchQuery();
            director.SetSearchQueryBuilder(googleSearch);
            director.CreateSearchQuery(searchQuery.SearchForUrl, searchQuery.Keywords, maxNumberResults, maxResultPages);
            var newSearchQuery = director.GetSearchQuery();
            htmlParser.SetSearchQuery(newSearchQuery);

            var searchEngine = new SearchEngine(htmlParser, newSearchQuery);
            var result = await searchEngine.GetSearchResults();

            return result;
        }
    }

    class BingSearchStrategy : ISearchStrategy
    {
        public async Task<SearchQuery> DoSearchAsync(SearchQuery searchQuery, SearchQueryDirector director, HtmlParser htmlParser, int maxNumberResults, int maxResultPages)
        {
            var bingSearch = new BingSearchQuery();
            director.SetSearchQueryBuilder(bingSearch);
            director.CreateSearchQuery(searchQuery.SearchForUrl, searchQuery.Keywords, maxNumberResults, maxResultPages);
            var newSearchQuery = director.GetSearchQuery();
            htmlParser.SetSearchQuery(newSearchQuery);

            var searchEngine = new SearchEngine(htmlParser, newSearchQuery);
            var result = await searchEngine.GetSearchResults();

            return result;
        }
    }

}
