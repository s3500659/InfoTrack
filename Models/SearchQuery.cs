using InfoTrackSearch.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace InfoTrackSearch.Models
{

    public class SearchQuery
    {
        [Display(Name = "Search for URL")]
        public int MaxResultPage { get; set; }
        public int MaxNumberOfResults { get; set; }
        public string SearchForUrl { get; set; } 
        public string Keywords { get; set; } 
        public string Positions { get; set; }
        public SearchEngineEnum SearchEngine { get; set; }


    }
}
