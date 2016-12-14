using System;
using System.Collections.Generic;
using System.Net;
using HtmlAgilityPack;
using System.Linq;
using System.Globalization;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var institutions = new List<Institution>();
            string page = null;
            var doc = new HtmlDocument();
            var table = doc.DocumentNode.SelectSingleNode("//table[@class='table table-bordered table-hover']")
                        .Descendants("tr").Skip(1);
            string name = null;

            using (WebClient webClient = new WebClient())
            {
                for (int i = 2012; i <= 2016; i++)
                {
                    page = webClient.DownloadString("http://cwur.org/" + i.ToString() + ".php");
                    doc.LoadHtml(page);

                    foreach (HtmlNode row in doc.DocumentNode.SelectSingleNode("//table[@class='table table-bordered table-hover']")
                        .Descendants("tr").Skip(1))
                    {
                        name = row.Elements("td").Select(td => td.InnerText).ToList()[1];
                        if (!institutions.Any(e => e.Name == name))
                        institutions.Add(new Institution { Name = name });
                   }
                }
            }

            Console.ReadKey();
        }
    }

    class Institution
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}