using System;
using Task4.Model;

namespace Task4
{
    class Program
    {
        static void Main(string[] args)
        {
            HtmlParser parser = new HtmlParser();
            WebClient webclient = new WebClient();
            DownloadText test = new DownloadText(webclient, parser);
            var site1 = "Сайт 1";
            var site2 = "Сайт 2";
            var site3 = "Сайт 3";
            var task1 = test.ParseHtml(site1);
            Console.WriteLine($"Задача по {site1} запущена");
            var task2 = test.ParseHtml(site2);
            Console.WriteLine($"Задача по {site2} запущена");
            var task3 = test.ParseHtml(site3);
            Console.WriteLine($"Задача по {site3} запущена");
            Console.WriteLine("---Ждем результатов---");
          /*  var result1 = (HtmlDocuments)task1.Result;
            Console.WriteLine($"Результат задачи # 1: {result1.HmtlText}");
            var result2 = (HtmlDocuments)task2.Result; 
            Console.WriteLine($"Результат задачи # 2: {result2.HmtlText}"); */
            var result3 = (HtmlDocuments)task3.Result;
            Console.WriteLine($"Результат задача # 3: {result3.HmtlText}");
            Console.WriteLine("Готово");
            Console.ReadKey();
        }           
    }
}
