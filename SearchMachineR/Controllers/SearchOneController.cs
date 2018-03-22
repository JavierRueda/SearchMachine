using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SearchMachineR.Models;
using SearchMachineR.Services;

namespace SearchMachineR.Controllers
{

    [Route("api/[controller]")]
    public class SearchOneController: Controller
    {

        private readonly SearchService searchService;

        public SearchOneController(SearchService searchService)
        {
            this.searchService = searchService;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchQueryText"></param>
        /// <returns></returns>
        //[HttpPost("[action]")]
        //public async Task<ResultSearch> ResultSearch([FromBody]string searchQueryText)
        //{
        //    var resultSearch = new ResultSearch {SearchEngineResults = new List<SearchEngineResult>()};

        //    var googleresult =  await searchService.SearchOnGoogle(searchQueryText);
        //    resultSearch.SearchEngineResults.Add(googleresult);

        //    var bingresult = await searchService.SearchOnBing(searchQueryText);
        //    resultSearch.SearchEngineResults.Add(bingresult);

        //    var ecosiaresult = await searchService.SearchOnEcosia(searchQueryText);
        //    resultSearch.SearchEngineResults.Add(ecosiaresult);

        //    var gogoDuckresult = await searchService.SearchOnGoGoDuck(searchQueryText);
        //    resultSearch.SearchEngineResults.Add(gogoDuckresult);
            
        //    return resultSearch; 
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchQueryText"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public  async Task<IActionResult> ResultSearch([FromBody]string searchQueryText)
        {            var resultSearch = new ResultSearch { SearchEngineResults = new List<SearchEngineResult>() };

            var googleresult = await searchService.SearchOnGoogle(searchQueryText);
            resultSearch.SearchEngineResults.Add(googleresult);

            var bingresult = await searchService.SearchOnBing(searchQueryText);
            resultSearch.SearchEngineResults.Add(bingresult);

            var ecosiaresult = await searchService.SearchOnEcosia(searchQueryText);
            resultSearch.SearchEngineResults.Add(ecosiaresult);

            var gogoDuckresult = await searchService.SearchOnGoGoDuck(searchQueryText);
            resultSearch.SearchEngineResults.Add(gogoDuckresult);

            return Ok(resultSearch);
        }

    }
}
