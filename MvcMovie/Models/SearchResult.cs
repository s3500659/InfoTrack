using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcMovie.Models
{
    public class SearchResult
    {
        public string Url { get; set; }
        public string Keywords { get; set; }
        public string Positions { get; set; }
        public DateTime Date { get; } = DateTime.Now;
    }
}
