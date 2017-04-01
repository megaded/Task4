using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task4.Model
{
    public class HtmlDocuments : IHtmlDocument
    {
        public string HmtlText { get; set; }
        public HtmlDocuments(string htmltext)
        {
            HmtlText = htmltext;
        }
    }
}
