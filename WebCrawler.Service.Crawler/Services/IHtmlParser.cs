using System;
namespace WebCrawler.Service.Crawler.Services
{
    public interface IHtmlParser
    {
        List<string> GetLinks(string htmlContent);
    }
}

