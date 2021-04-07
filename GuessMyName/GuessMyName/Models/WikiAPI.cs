using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GuessMyName.Models
{
    public class WikiAPI
    {
        WebClient apiConnection = new WebClient();
        string startAPI = "https://en.wikipedia.org/w/api.php?action=query&format=json&prop=extracts&exsentences=3&explaintext&titles=";
        string keyWord { get; set; }
        string apiEndPoint
        {
            get
            {
                return startAPI + keyWord;
            }
        }

        public WikiAPI(string search)
        {
            keyWord = search;
        }

        private class Result
        {
            public Query query { get; set; }
        }

        private class Query
        {
            public Dictionary<string, Page> pages { get; set; }
        }

        private class Page
        {
            public string extract { get; set; }
        }

        public async Task<string> GetIntro()
        {
            string apiString = await apiConnection.DownloadStringTaskAsync(apiEndPoint);
            Result data = JsonConvert.DeserializeObject<Result>(apiString);
            foreach(Page page in data.query.pages.Values)
            {
                return page.extract;
            }

            return "Infomation not found";
        }
    }
}
