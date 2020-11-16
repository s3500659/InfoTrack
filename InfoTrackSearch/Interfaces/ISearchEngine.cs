using InfoTrackSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoTrackSearch.Interfaces
{
    interface ISearchEngine
    {
        public Task<SearchQuery> GetSearchResults();
    }
}
