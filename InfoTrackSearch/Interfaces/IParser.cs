using HtmlAgilityPack;
using InfoTrackSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoTrackSearch.Interfaces
{
    public interface IParser
    {

        public HtmlNodeCollection RemoveJSContent(string xpath);

        public List<HtmlNode> GetHrefList();

        public string GetMatchingPositions();

        public Task<SearchQuery> GetSearchResults();
    }
}
