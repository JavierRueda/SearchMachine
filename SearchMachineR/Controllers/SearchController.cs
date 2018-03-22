using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using DuckDuckGo.Net;
using Google.Apis.Services;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SearchMachineR.Models;

namespace SearchMachineR.Controllers
{

    [Route("api/[controller]")]
    public class SearchController: Controller
    {
        
        [HttpPost("[action]")]
        public GoogleResult GoogleSearch([FromBody]string searchText)
        {
            var apiKey = "AIzaSyAYEw4E8k2V4n2xhEAtl3gPJatZfQ0mbhM";
            var cx = "004052071860325123994:6giwpag8kue";
            var query = searchText;
            var svc = new Google.Apis.Customsearch.v1.CustomsearchService(new BaseClientService.Initializer { ApiKey = apiKey });
            var listRequest = svc.Cse.List(query);

            listRequest.Cx = cx;
            var search = listRequest.Execute();
          
            var googleResult = new GoogleResult();
            googleResult.Counter = search.Items.Count;
            googleResult.Query = query; 
            return googleResult;
        }

        [HttpPost("[action]")]
        public GoogleResult GoogleSearchWild([FromBody]string searchText)
        {
            var query = HttpUtility.UrlEncode(searchText);
            GoogleResult googleResult = new GoogleResult();
            var uri = $"https://www.google.de/search?q={query}";
            if (WebRequest.Create(uri) is HttpWebRequest request)
            {
                var response = request.GetResponse();
                using (var responseStream = response.GetResponseStream())
                using (var streamReader = new StreamReader(responseStream ?? throw new InvalidOperationException(), Encoding.UTF8))
                {
                    var responseText = streamReader.ReadToEnd();
                    var doc = new HtmlDocument();
                    doc.LoadHtml(responseText);
                    var whatUrLookingFor = doc.GetElementbyId("resultStats").InnerHtml;
                   
                    var counter = int.Parse(new String(whatUrLookingFor.Where(Char.IsDigit).ToArray()));
                    googleResult.Counter = counter;
                    googleResult.Query = query;
                    return googleResult;
                }
                
            }
            googleResult.Counter = 0;
            return googleResult;
        }
        [HttpPost("[action]")]
        public  BingResult BingSearch([FromBody]string searchText)
        {

            var query = HttpUtility.UrlEncode(searchText);
            BingResult bingResult = new BingResult();
            var uri = $"https://www.bing.com/search?q={query}";
            if (WebRequest.Create(uri) is HttpWebRequest request)
            {
                var response = request.GetResponse();
                using (var responseStream = response.GetResponseStream())
                using (var streamReader = new StreamReader(responseStream ?? throw new InvalidOperationException(), Encoding.UTF8))
                {
                    var responseText = streamReader.ReadToEnd();
                    string stringThatKeepsYourHtml = responseText;
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(stringThatKeepsYourHtml);
                    
                    var whatUrLookingFor = doc.GetElementbyId("b_tween");
                    var whatUrLookingForValue = whatUrLookingFor.ChildNodes.FirstOrDefault(x => x.Attributes["class"].Value == "sb_count");
                    var value = new String(whatUrLookingForValue.InnerHtml.Where(Char.IsDigit).ToArray());
                    var counter = int.Parse(value);
                    bingResult.Counter = counter;
                    return bingResult;
                }

            }
            bingResult.Counter = 0;
            return bingResult;

        }
       

        struct SearchResult
        {
            public String JsonResult;
            public Dictionary<String, String> RelevantHeaders;
        }

        [HttpPost("[action]")]
        public BingResult BingSearchWithKey([FromBody]string searchText)
        {
            BingResult bingResult = new BingResult();
            string accessKey = "62d6fbbe656b4afba1ced6ff8eee8d7a";
            string query = searchText;
            var uriBase = "https://api.cognitive.microsoft.com/bing/v7.0/search";

            var uriQuery = uriBase + "?q=" + Uri.EscapeDataString(query);

            // Perform the Web request and get the response
            var request = WebRequest.Create(uriQuery);
            request.Headers["Ocp-Apim-Subscription-Key"] = accessKey;
            var response = (HttpWebResponse)request.GetResponseAsync().Result;
            string json = new StreamReader(response.GetResponseStream()).ReadToEnd();

            // Create result object for return
            var searchResult = new SearchResult()
            {
                JsonResult = json,
                RelevantHeaders = new Dictionary<String, String>()
            };

            // Extract Bing HTTP headers
            //foreach (string header in response.Headers)
            //{
            //    if (header.StartsWith("BingAPIs-") || header.StartsWith("X-MSEdge-"))
            //        searchResult.RelevantHeaders[header] = response.Headers[header];
            //}
            var jsonResult = searchResult.JsonResult;
            dynamic dinamycdata = JsonConvert.DeserializeObject<JObject>(jsonResult);
            var matches =dinamycdata.webPages.totalEstimatedMatches.Value;
            bingResult.Counter = Convert.ToInt32(matches);
            bingResult.Query = query;
                
            return bingResult;
        }
        [HttpPost("[action]")]
        public YahooResult YahooSearch([FromBody]string searchText)
        {

            var query = HttpUtility.UrlEncode(searchText);
            var yahooResult = new YahooResult();
            var theWebAddress = new StringBuilder();
            theWebAddress.Append("https://query.yahooapis.com/v1/public/yql?");
            theWebAddress.Append("q=" + HttpUtility.UrlEncode($"select * from google where text={query}'"));
            theWebAddress.Append("&format=json");
            theWebAddress.Append("&diagnostics=false");
            string results = "";
            using (WebClient wc = new WebClient())
            {
                results = wc.DownloadString(theWebAddress.ToString());
            }
            JObject dataObject = JObject.Parse(results);
            JArray jsonArray = (JArray)dataObject["query"]["results"]["Result"];


            
            ////http://query.yahooapis.com/v1/public/yql?q=select * from geo.places wheretext=’sunnyvale’&format=json
            //StringBuilder sbURL = new StringBuilder();
            //sbURL.Append(@"https://query.yahooapis.com/v1/public/yql?q=");
            //// YQL is select * from geo.places where text='sfo'
            //sbURL.Append(AntiXssEncoder.HtmlFormUrlEncode
            //    (@"select * from geo.places where text='sfo'")); // Anti XSS encoder - 
            //// Prevent cross site scripting
            //sbURL.Append(@"&diagnostics=true");
            //var baseYahooUri = $"http://.search.yahohaooo.com/search?q={query}";
            //var YahooQueryL= $"select * from  "
            //if (WebRequest.Create(uri) is HttpWebRequest request)
            //{
            //    var response = request.GetResponse();
            //    using (var responseStream = response.GetResponseStream())
            //    using (var streamReader = new StreamReader(responseStream ?? throw new InvalidOperationException(), Encoding.UTF8))
            //    {
            //        var responseText = streamReader.ReadToEnd();
            //        string stringThatKeepsYourHtml = responseText;
            //        HtmlDocument doc = new HtmlDocument();
            //        doc.LoadHtml(stringThatKeepsYourHtml);
            //        var whatUrLookingFor = doc.GetElementbyId("resultStats").InnerHtml;

            //        var counter = int.Parse(new String(whatUrLookingFor.Where(Char.IsDigit).ToArray()));
            //        yahooResult.Counter = counter;
            //        return yahooResult;
            //    }

            //}
            yahooResult.Counter = int.Parse(jsonArray["results"].ToString());
            return yahooResult;
        }

        [HttpPost("[action]")]
        public GoGoDuckResult GoGoDuckSearch([FromBody]string searchText)
        {
            
            var query = HttpUtility.UrlEncode(searchText);
            var goGoDuckResult = new GoGoDuckResult();
            
            var search = new Search
            {
                NoHtml = true,
                NoRedirects = true,
                IsSecure = true,
                SkipDisambiguation = true,
                ApiClient = new HttpWebApi()
            };

            if (string.IsNullOrWhiteSpace(searchText))
                return goGoDuckResult;
            var searchResult = search.Query(query, "RediveTest");
            goGoDuckResult.Counter = searchResult.Results.Count;
            goGoDuckResult.Query = query;
            return goGoDuckResult;
        }

        [HttpPost("[action]")]
        public EcosiaResult EcosiaSearch([FromBody]string searchText)
        {
            var query = HttpUtility.UrlEncode(searchText);
            var ecosiaResult = new EcosiaResult();
            var uri = $"https://www.ecosia.org/search?q={query}";
            if (WebRequest.Create(uri) is HttpWebRequest request)
            {
                var response = request.GetResponse();
                using (var responseStream = response.GetResponseStream())
                using (var streamReader = new StreamReader(responseStream ?? throw new InvalidOperationException(), Encoding.UTF8))
                {
                    var responseText = streamReader.ReadToEnd();
                    string stringThatKeepsYourHtml = responseText;
                    var doc = new HtmlDocument();
                    doc.LoadHtml(stringThatKeepsYourHtml);
                    var bodyNodes = doc.DocumentNode
                        .SelectNodes("//div[@class='card-title card-title-result-count']");
                    string counterstring="";
                    foreach (var node in bodyNodes)
                    {
                        counterstring = new String(node.InnerText.Where(Char.IsDigit).ToArray());
                    }

                    var counter = int.Parse(counterstring);
                    ecosiaResult.Counter = counter;
                    ecosiaResult.Query = query;
                    return ecosiaResult;
                }

            }
            ecosiaResult.Counter = 0;
            return ecosiaResult;
        }
    }
}
