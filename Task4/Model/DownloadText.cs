using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task4.Model
{
    public class DownloadText
    {
        private IWebClient webClient { get; set; }
        private IHtmlParser htmlParser { get; set; }
        private readonly List<Task<string>> webClientTasks = new List<Task<string>>();
        private readonly List<Task<IHtmlDocument>> parseTasks = new List<Task<IHtmlDocument>>();
        private int indexWebClientTask = 0;
        private int indexParseTask = 0;
        private object lockWebClient = new object();
        private object lockParse = new object();
        public DownloadText(IWebClient webClient, IHtmlParser htmlParser)
        {
            this.webClient = webClient;
            this.htmlParser = htmlParser;
        }     
        public async Task<IHtmlDocument> ParseHtml(string url)
        {
            if (webClientTasks.Count == 0)
            {
                var taskDowload = Task.Factory.StartNew(() => webClient.GetStringAsync(url)).Result;
                lock (lockWebClient)
                {
                    webClientTasks.Add(taskDowload);
                    indexWebClientTask=0;
                }
                var taskParse = taskDowload.ContinueWith((Task<string> t) => htmlParser.Parse(t.Result));
                lock (lockParse)
                {
                    parseTasks.Add(taskParse);
                    indexParseTask=0;
                }                
                return await taskParse;
            }
            else
            {
                var taskDowload = webClientTasks[indexWebClientTask].ContinueWith((Task<string> t) => webClient.GetStringAsync(url).Result);
                lock (lockWebClient)
                {
                    webClientTasks.Add(taskDowload);
                    indexWebClientTask++;
                }
                var taskParse = parseTasks[indexParseTask].ContinueWith((Task<IHtmlDocument> t) => 
                {
                    var parse = taskDowload.ContinueWith((Task<string> k) => htmlParser.Parse(k.Result)).Result;
                    return parse;
                });
                lock (lockParse)
                {
                    parseTasks.Add(taskParse);
                    indexParseTask++;
                }
                return await taskParse;
            }            
        }     
    }
}
