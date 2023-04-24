using System;
using WebCrawler.Service.Crawler.Models;

namespace WebCrawler.Service.Crawler.Services
{
    public interface ISiteCrawler
    {
        Task Run(CrawlerJob job);

        List<LinkResult> GetResult();

        List<LinkResult> GetSkipScannedResult();
    }
}

