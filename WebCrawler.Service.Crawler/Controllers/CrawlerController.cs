using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebCrawler.Service.Crawler.Models;
using WebCrawler.Service.Crawler.Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebCrawler.Service.Crawler.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CrawlerController : ControllerBase
    {
        private readonly ILogger<CrawlerController> _logger;
        private ISiteCrawler _siteCrawler;

        public CrawlerController(ILogger<CrawlerController> logger, ISiteCrawler siteCrawler)
        {
            _logger = logger;
            _siteCrawler = siteCrawler;
        }

        [HttpGet(Name = "start")]
        public async Task<IEnumerable<LinkResult>> StartCrawl(string url, string scope, int maxSize = 50)
        {
            Console.WriteLine($"Crawling url: {url} max size: {maxSize}");
            if(!url.EndsWith($"/"))
            {
                url = $"{url}/";
            }

            if (!scope.EndsWith($"/"))
            {
                scope = $"{scope}/";
            }

            CrawlerJob job = new CrawlerJob(url, scope, maxSize);
            await _siteCrawler.Run(job);

            var scannedResult = _siteCrawler.GetResult();
            var skippedScannedResult = _siteCrawler.GetSkipScannedResult();

            var combinedResult = new List<LinkResult>();
            combinedResult.AddRange(scannedResult);
            combinedResult.AddRange(skippedScannedResult);

            return combinedResult;

        }
    }
}

