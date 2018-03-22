using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using DuckDuckGo.Net;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SearchMachineR.Models;

namespace SearchMachineR.Utils
{
    public class RequestSearch
    {
       
        public static SearchEngineValueReturned DataFromGoogle(string textParam)
        {
           
            var query = HttpUtility.UrlEncode(textParam);
            var valueReturned = new SearchEngineValueReturned()
            {
                SearchEngineOriginType = SearchEngineOrigin.Google,
                SearchEngineName = SearchEngineOrigin.Google.ToString(),
                TextQuery = query
                
            };
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

                    var counter = long.Parse(new String(whatUrLookingFor.Where(Char.IsDigit).ToArray()));
                    valueReturned.Counter = counter;
                    return valueReturned;
                }
            }
            valueReturned.Counter = 0;
            return valueReturned;
        }
        public static SearchEngineValueReturned DataFromBing(string textParam)
        {
            string accessKey = "62d6fbbe656b4afba1ced6ff8eee8d7a";
            string query = textParam;
            var valueReturned = new SearchEngineValueReturned()
            {
                SearchEngineOriginType = SearchEngineOrigin.Bing,
                SearchEngineName = SearchEngineOrigin.Bing.ToString()
            };
            var uriQuery = $"https://api.cognitive.microsoft.com/bing/v7.0/search?q={Uri.EscapeDataString(query)}";
            // Perform the Web request and get the response
            var request = WebRequest.Create(uriQuery);
            request.Headers["Ocp-Apim-Subscription-Key"] = accessKey;
            var response = (HttpWebResponse)request.GetResponseAsync().Result;
            string json = new StreamReader(response.GetResponseStream()).ReadToEnd();
            // Create result object for return
            var searchResult = new SearchResultBing()
            {
                JsonResult = json,
                RelevantHeaders = new Dictionary<String, String>()
            };
            var jsonResult = searchResult.JsonResult;
            dynamic dinamycdata = JsonConvert.DeserializeObject<JObject>(jsonResult);
            var matches = dinamycdata.webPages.totalEstimatedMatches.Value;
            valueReturned.Counter = Convert.ToInt64(matches);
            valueReturned.TextQuery = query;
            return valueReturned;
           
        }
        public static SearchEngineValueReturned DataFromEcosia(string textParam)
        {
            var query = HttpUtility.UrlEncode(textParam);
            var valueReturned = new SearchEngineValueReturned()
            {
                SearchEngineOriginType = SearchEngineOrigin.Ecosia,
                SearchEngineName = SearchEngineOrigin.Ecosia.ToString(),
                TextQuery = query
            };
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
                    string counterstring = "";
                    foreach (var node in bodyNodes)
                    {
                        counterstring = new String(node.InnerText.Where(Char.IsDigit).ToArray());
                    }

                    var counter = int.Parse(counterstring);
                    valueReturned.Counter = counter;
                    valueReturned.TextQuery = query;
                    return valueReturned;
                }
            }
            valueReturned.Counter = 0;
            return valueReturned;
        }
        public static SearchEngineValueReturned DataFromGogoDuck(string textParam)
        {
            var query = HttpUtility.UrlEncode(textParam);
            var valueReturned = new SearchEngineValueReturned()
            {
                SearchEngineOriginType = SearchEngineOrigin.GoGoDuck,
                SearchEngineName = SearchEngineOrigin.GoGoDuck.ToString(),
                TextQuery = query
            };
            var search = new Search
            {
                NoHtml = true,
                NoRedirects = true,
                IsSecure = true,
                SkipDisambiguation = true,
                ApiClient = new HttpWebApi()
            };
            var searchResult = search.Query(query, "RediveTest");
            valueReturned.Counter = searchResult.Results.Count;
            valueReturned.TextQuery = query;
            return valueReturned;
        }
    }
}
