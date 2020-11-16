using InfoTrackSearch.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;

namespace InfoTrackSearch.Models
{
    public enum SearchEngineEnum
    {
        Google,
        Bing
    }

    public class SearchEngine : ISearchEngine
    {
        HtmlParser HtmlParser { get; set; }
        SearchQuery SearchQuery { get; set; }
        public string Url { get; set; } 

        public SearchEngine(HtmlParser htmlParser, SearchQuery searchQuery, string url = "https://infotrack-tests.infotrack.com.au/Google/")
        {
            HtmlParser = htmlParser;
            SearchQuery = searchQuery;
            Url = url;

            if (SearchQuery.SearchEngine == SearchEngineEnum.Bing)
            {
                Url = "https://infotrack-tests.infotrack.com.au/Bing/";
            }
        }

        public async Task<SearchQuery> GetSearchResults()
        {
            var currentPage = 1;
            var maxPage = SearchQuery.MaxResultPage;
            var decimalLength = currentPage.ToString("D").Length + 1;

            var htmlString = "";
            var httpClient = new HttpClient();
            for (int i = 0; i < maxPage; i++)
            {
                var infoTrackUrl = Url + "Page" + currentPage.ToString("D" + decimalLength.ToString()) + ".html";
                htmlString += await httpClient.GetStringAsync(infoTrackUrl);
                currentPage++;
            }
            HtmlParser.Doc.LoadHtml(htmlString);

            // remove JS content
            HtmlParser.RemoveJSContent("//script|//style");

            SearchQuery.Positions = HtmlParser.GetMatchingPositions();

            return SearchQuery;
        }



    }
}
