using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task4.Model
{
    public class HtmlParser : IHtmlParser
    {
        public IHtmlDocument Parse(string htmlText)
        {
            Thread.Sleep(2000);
            Console.WriteLine($"Распарсено {htmlText}");
            htmlText = $"{htmlText} Распарсено!";
            return  new HtmlDocuments(htmlText);
        }
    }
}
