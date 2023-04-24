using System;
namespace WebCrawler.Service.Crawler.Models
{
    public class CrawlerJob
    {
        public CrawlerJob(string url, string scope, int maxSize)
        {
            this.Scope = scope;
            this.InitialJob = this;
            this.ParentJob = this;

            var formattedUrl = this.FormatUrl(url);
            this.URL = formattedUrl;
            this.MaxCrawlSize = maxSize;

            var uri = new Uri(formattedUrl);
            this.DomainName = uri.Host;
            this.Scheme = uri.Scheme;
            this.CanBeRun = true;
        }

        public CrawlerJob(CrawlerJob initialJob, CrawlerJob parentJob, string url, string scope, int maxSize)
        {
            this.Scope = scope;
            this.InitialJob = initialJob;
            this.ParentJob = parentJob;

            var formattedUrl = this.FormatUrl(url);
            Console.WriteLine($"Construct crawler job: {url} > {formattedUrl}");
            this.URL = formattedUrl;
            this.MaxCrawlSize = maxSize;

            var uri = new Uri(formattedUrl);
            this.DomainName = uri.Host;
            this.Scheme = uri.Scheme;
            this.CanBeRun = this.IsQualifiedForScanning(parentJob, uri);
        }

        public string Scope
        {
            internal set;
            get;
        }

        public string URL
        {
            internal set;
            get;
        }

        public int MaxCrawlSize
        {
            internal set;
            get;
        }

        public string DomainName
        {
            internal set;
            get;
        }

        public string Scheme
        {
            internal set;
            get;
        }

        public bool CanBeRun
        {
            internal set;
            get;
        }

        public CrawlerJob InitialJob
        {
            internal set;
            get;
        }

        public CrawlerJob ParentJob
        {
            internal set;
            get;
        }

        private bool IsQualifiedForScanning(CrawlerJob parentJob, Uri uri)
        {
            return parentJob.DomainName == uri.Host;
        }

        private string FormatUrl(string url)
        {
            if (url.StartsWith($"/entertainment"))
            {

            }
            if (url.StartsWith($"//"))
            {
                return $"{ParentJob.Scheme}:{url}";
            }
            else if (url.StartsWith($"/"))
            {
                //return $"{InitialJob.Scope}{url.Substring(1)}";
                return $"{InitialJob.Scheme}://{InitialJob.DomainName}{url}";
            }
            else if (!url.Contains($"/"))
            {
                return $"{InitialJob.Scope}/{url}";
            }
            else if (url.StartsWith($".."))
            {
                return $"{InitialJob.Scope}{url}";
            }
            else if (url.StartsWith($"."))
            {
                // remove ./
                return $"{InitialJob.Scope}{url.Substring(2)}";

            }
            else if (!url.StartsWith($"http") && !url.StartsWith($"https"))
            {
                return $"{InitialJob.Scope}{url}";
            }

            return url;
        }
    }
}

