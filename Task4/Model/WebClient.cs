using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task4.Model
{
    public class WebClient : IWebClient
    {
        public Task<string> GetStringAsync(string urlText)
        {
            Task<string> task = Task.Factory.StartNew(() =>
            {              
                Thread.Sleep(5000);
                Console.WriteLine($"Скачали {urlText}");
                return urlText;                
            });
            return task;
        }
    }
}
