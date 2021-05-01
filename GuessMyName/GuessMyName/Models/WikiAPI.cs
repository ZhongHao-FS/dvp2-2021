/* Name: Hao Zhong
 * Course: DVP2
 * Term: April 2021
 * Assignment: 1.5 API */

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
        string imageAPI
        {
            get
            {
                return "https://en.wikipedia.org/w/api.php?action=query&titles=" + keyWord + "&prop=pageimages&format=json&pithumbsize=200";
            }
        }
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
            public Thumbnail thumbnail { get; set; }
        }

        private class Thumbnail
        {
            public string source { get; set; }
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

        public async Task<Uri> GetImage()
        {
            string apiString = await apiConnection.DownloadStringTaskAsync(imageAPI);
            Result data = JsonConvert.DeserializeObject<Result>(apiString);
            foreach (Page page in data.query.pages.Values)
            {
                return new Uri(page.thumbnail.source);
            }

            Uri noob = new Uri("ms-appx:///drawable/noob.png");
            return noob;
        }
    }
}
