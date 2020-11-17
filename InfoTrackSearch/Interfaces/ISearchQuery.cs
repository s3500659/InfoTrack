using InfoTrackSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoTrackSearch.Interfaces
{
    public interface ISearchQuery
    {
        public void SetMaxResultPage(int page);
        public void SetMaxNumberOfResults(int maxResults);
        public void SetSearchForUrl(string url);
        public void SetSearchKeywords(string keywords);
        public void SetFoundPosition(string position);
        public void SetSearchEngine(SearchEngineEnum searchEngine);
    }
}
