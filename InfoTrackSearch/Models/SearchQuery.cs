using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InfoTrackSearch.Models
{
    public enum SearchEngine
    {
        Google,
        Bing
    }
    public class SearchQuery
    {
        [Display(Name = "Search for URL")]
        public string SearchForUrl { get; set; }
        public string Keywords { get; set; }
        public string Positions { get; set; }
        public SearchEngine SearchEngine { get; set; }

    }
}
