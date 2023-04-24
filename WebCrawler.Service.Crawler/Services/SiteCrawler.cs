using System;
using System.Net.Http.Headers;
using WebCrawler.Service.Crawler.Models;

namespace WebCrawler.Service.Crawler.Services
{
    public class SiteCrawler : ISiteCrawler
    {
        private List<string> _scannedLinks = new List<string>();
        private List<LinkResult> _scannedResult = new List<LinkResult>();
        private List<LinkResult> _skipScanResult = new List<LinkResult>();

        private readonly HttpClient _httpClient;
        private readonly IHtmlParser _htmlParser;

        public SiteCrawler(HttpClient httpClient, IHtmlParser htmlParser)
        {
            this._httpClient = httpClient;
            this._htmlParser = htmlParser;
        }

        public async Task Run(CrawlerJob job)
        {
            if (this._scannedLinks.Count >= job.MaxCrawlSize && job.MaxCrawlSize != -1)
            {
                return;
            }

            if (!job.CanBeRun)
            {
                var skipJobRun = new LinkResult(job, job.URL, "", -1);
                _skipScanResult.Add(skipJobRun);
                return;
            }

            var pageResult = await this.ScanUrl(job.URL, job.Scope);
            if (pageResult != null)
            {
                var linkResult = new LinkResult(job, job.URL, pageResult.RedirectLocation, ((int)pageResult.StatusCode));
                this._scannedResult.Add(linkResult);

                var links = this.FindLinksInContent(pageResult.HtmlContent);

                foreach (var link in links)
                {
                    await this.Run(new CrawlerJob(job.InitialJob, job, link, job.Scope, job.MaxCrawlSize));
                }
            }
        }

        public List<LinkResult> GetResult() { return this._scannedResult; }

        private async Task<PageResult> ScanUrl(string url, string scope)
        {
            try
            {
                if (!this._scannedLinks.Contains(url.ToLower()) && url.Contains(scope))
                {
                    this._scannedLinks.Add(url.ToLower());
                    var pageResult = await this.GetContentByUrl(url);
                    return pageResult;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error when scanning for url {url}. Msg {ex.Message}");
            }
            
            return null;
        }

        private async Task<PageResult> GetContentByUrl(string url)
        {
            Console.WriteLine($"GetContentByUrl {url}");
            var htmlResponse = await _httpClient.GetAsync(url);
            var contentType = htmlResponse.Content.Headers.ContentType;
            var statusCode = htmlResponse.StatusCode;
            var htmlContent = "";
            var redirectLocation = "";

            if(htmlResponse.IsSuccessStatusCode && contentType.MediaType.Contains("text/html"))
            {
                htmlContent = await htmlResponse.Content.ReadAsStringAsync();
            }
            else
            {
                HttpResponseHeaders headers = htmlResponse.Headers;
                if (headers != null && headers.Location != null)
                {
                    redirectLocation = headers.Location.AbsoluteUri;
                }
            }

            return new PageResult(htmlContent, contentType.MediaType, redirectLocation, statusCode);
        }

        private List<string> FindLinksInContent(string htmlContent)
        {
            return _htmlParser.GetLinks(htmlContent);
        }

        public List<LinkResult> GetSkipScannedResult()
        {
            return this._skipScanResult;
        }
    }
}

