using System;
using System.Xml;
using HtmlAgilityPack;

namespace WebCrawler.Service.Crawler.Services
{
    public class HtmlAgilityParser : IHtmlParser
    {
        public List<string> GetLinks(string htmlContent)
        {
            if (string.IsNullOrWhiteSpace(htmlContent))
            {
                return new List<string>();
            }
            var document = new HtmlDocument();
            document.LoadHtml(htmlContent);
            var linkNodes = document.DocumentNode.SelectNodes("//a[@href]");
            if (linkNodes == null)
            {
                return new List<string>();
            }
            var links = linkNodes.Where(n => n.Attributes.Contains("href")).Select(n => n.Attributes["href"]).ToList();

            //Filter out invalid url
            links = links.Where(
                link =>
                !string.IsNullOrEmpty(link.Value) &&
                !link.Value.StartsWith("#") &&
                !link.Value.StartsWith("javascript:")
            ).ToList();
            return links.Select(l => l.Value.Trim().Trim('\n')).ToList();
        }
    }
}

