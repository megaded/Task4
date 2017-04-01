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
        private readonly List<Task<string>> webClients = new List<Task<string>>();
        private readonly List<Task<string>> webClientsTest = new List<Task<string>>();
        private readonly List<Task<IHtmlDocument>> queueTasks = new List<Task<IHtmlDocument>>();
        private readonly List<Task<IHtmlDocument>> queueParseTask = new List<Task<IHtmlDocument>>();
        private int indexTask = 0;
        private int indexWebClientTask = 0;
        private int indexParseTask = 0;
        private object lockObject = new object();
        private object lockWebClient = new object();
        private object lockParse = new object();
        public DownloadText(IWebClient webClient, IHtmlParser htmlParser)
        {
            this.webClient = webClient;
            this.htmlParser = htmlParser;
        }

        public async Task<string> Download(string url)
        {
            if (webClients.Count == 0)
            {
                var task = Task.Factory.StartNew(() => webClient.GetStringAsync(url)).Result;
                lock (lockObject)
                {
                    webClients.Add(task);
                    Console.WriteLine($"Задача {url} добавлена");
                    indexTask = 0;
                }
                return await task;
            }
            else
            {
                Console.WriteLine("Вошли в элс");
                var queueTask = webClients[indexTask].ContinueWith((Task<string> t) =>
                {
                    Console.WriteLine("Задача 2");
                    return webClient.GetStringAsync(url).Result;
                });
                lock (lockObject)
                {
                    webClients.Add(queueTask);
                    Console.WriteLine($"Задача {url} добавлена");
                    indexTask++;
                }
                return await queueTask;
            }
        }

        public async Task<IHtmlDocument> DownloadAndParse(string url)
        {
            if (webClientsTest.Count == 0)
            {
                var taskDowload = Task.Factory.StartNew(() => webClient.GetStringAsync(url)).Result;
                lock (lockWebClient)
                {
                    webClientsTest.Add(taskDowload);
                    Console.WriteLine($"Скачивание по {url} добавлено");
                    indexWebClientTask=0;
                }
                var taskParse = taskDowload.ContinueWith((Task<string> t) => htmlParser.Parse(t.Result));
                lock (lockParse)
                {
                    queueParseTask.Add(taskParse);
                    Console.WriteLine($"Парсинг по {url} добавлено");
                    indexParseTask=0;
                }                
                return await taskParse;
            }
            else
            {
                var taskDowload = webClientsTest[indexWebClientTask].ContinueWith((Task<string> t) => webClient.GetStringAsync(url).Result);
                lock (lockWebClient)
                {
                    webClientsTest.Add(taskDowload);
                    Console.WriteLine($"Скачивание по {url} добавлено");
                    indexWebClientTask++;
                }
                var taskParse = queueParseTask[indexParseTask].ContinueWith((Task<IHtmlDocument> t) => 
                {
                    Console.WriteLine("Парсим!");
                    var parse = taskDowload.ContinueWith((Task<string> k) => htmlParser.Parse(k.Result)).Result;
                    return parse;
                });
                lock (lockParse)
                {
                    queueParseTask.Add(taskParse);
                    Console.WriteLine($"Парсинг по {url} добавлено");
                    indexParseTask++;
                }
                return await taskParse;
            }
            
        }

        public async Task<IHtmlDocument> Test(string url)
        {
            if (queueTasks.Count == 0)
            {
                var task = Task.Factory.StartNew(() =>
               {
                   var taskWebclient = Task.Factory.StartNew(() => webClient.GetStringAsync(url).Result);
                   lock (lockObject)
                   {
                       webClientsTest.Add(taskWebclient);
                       indexWebClientTask = 0;
                   }
                   var taskParser = taskWebclient.ContinueWith((Task<string> t) => htmlParser.Parse(t.Result));
                   Console.WriteLine($"Парсинг {url}");
                   return taskParser;
               }).Result;
                AddTask(task);
                Console.WriteLine($"Задача {url} добавлена");
                return await task;
            }
            else
            {
                Console.WriteLine("Вошли в элс");
                var taskWebclient = webClientsTest[indexWebClientTask].ContinueWith((Task<string> t) => webClient.GetStringAsync(url).Result);
                var queuetask = queueTasks[indexTask].ContinueWith((Task<IHtmlDocument> j) =>
           {                              
               var taskParser = taskWebclient.ContinueWith((Task<string> t) => htmlParser.Parse(t.Result));
               Console.WriteLine("Скачивание 2 Парсер запущен");
               return taskParser.Result;
           });
                AddTask(queuetask);
                Console.WriteLine($"Задача {url} добавлена");
                return await queuetask;
            }

        }
        private void AddTask(Task<IHtmlDocument> task)
        {
            lock (lockObject)
            {
                queueTasks.Add(task);
                indexTask = 0;
            }
        }


    }
}
