using System.Collections.Generic;
using System.Threading.Tasks;
using SearchMachineR.Models;
using SearchMachineR.Utils;

namespace SearchMachineR.Services
{
  
    public interface ISearchService
    {
        Task<SearchEngineResult> SearchOnGoogle(string textQueryString);
        Task<SearchEngineResult> SearchOnBing(string textQueryString);
        Task<SearchEngineResult> SearchOnEcosia(string textQueryString);
        Task<SearchEngineResult> SearchOnGoGoDuck(string textQueryString);

    }

    public class SearchService : ISearchService
    {
        

        public async Task<SearchEngineResult> SearchOnGoogle(string textQuery)
        {
       
            var result = new SearchEngineResult {SearchEngineValues = new List<SearchEngineValueReturned>()};
            var textQueryParams = textQuery.Split(" ");

            foreach (var textParam in textQueryParams)
            {
                if (string.IsNullOrWhiteSpace(textParam))
                    continue;
                var valueReturned = RequestSearch.DataFromGoogle(textParam);
                result.SearchEngineValues.Add(valueReturned);
            }
            return await Task.FromResult(result);
        }

        public async  Task<SearchEngineResult> SearchOnBing(string textQuery)
        {
            var result = new SearchEngineResult { SearchEngineValues = new List<SearchEngineValueReturned>() };
            var textQueryParams = textQuery.Split(" ");

            foreach (var textParam in textQueryParams)
            {
                if (string.IsNullOrWhiteSpace(textParam))
                    continue;
                var valueReturned = RequestSearch.DataFromBing(textParam);
                result.SearchEngineValues.Add(valueReturned);

            }
            return await Task.FromResult(result);
        }

        public  async  Task<SearchEngineResult> SearchOnEcosia(string textQuery)
        {
            var result = new SearchEngineResult { SearchEngineValues = new List<SearchEngineValueReturned>() };
            var textQueryParams = textQuery.Split(" ");

            foreach (var textParam in textQueryParams)
            {
                if (string.IsNullOrWhiteSpace(textParam))
                    continue;
                var valueReturned = RequestSearch.DataFromEcosia(textParam);
                result.SearchEngineValues.Add(valueReturned);
            }
            return await Task.FromResult(result);
        }

        public async Task<SearchEngineResult> SearchOnGoGoDuck(string textQuery)
        {
            var result = new SearchEngineResult { SearchEngineValues = new List<SearchEngineValueReturned>() };
            var textQueryParams = textQuery.Split(" ");

            foreach (var textParam in textQueryParams)
            {
                if (string.IsNullOrWhiteSpace(textParam))
                    continue;
                var valueReturned = RequestSearch.DataFromGogoDuck(textParam);
                result.SearchEngineValues.Add(valueReturned);
            }
            return await Task.FromResult(result);
        }

    }

}
