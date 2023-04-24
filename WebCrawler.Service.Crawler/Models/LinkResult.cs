using System;
namespace WebCrawler.Service.Crawler.Models
{
    public class LinkResult
    {
        private string _url;

        public LinkResult(CrawlerJob job, string url, string redirectUrl, int httpStatusCode)
        {
            string formattedUrl = FormatUrl(url);
            this._url = formattedUrl;
            this.HttpStatusCode = httpStatusCode;
            this.RedirectUrl = redirectUrl;
            SetLinkMetadata(job, formattedUrl);
        }

        private string FormatUrl(string url)
        {
            if (url.StartsWith($"//"))
            {
                return $"https:{url}";
            }

            return url;
        }

        private void SetLinkMetadata(CrawlerJob job, string url)
        {
            if (url.ToLower().StartsWith("http:") || url.ToLower().StartsWith("https:"))
            {
                this.IsLinkFullyQualified = true;
                var uri = new Uri(url);
                if (uri.Host != job.DomainName)
                {
                    this.IsLinkExternalDomain = true;
                }
                else
                {
                    this.IsLinkExternalDomain = false;
                }
            }
        }

        public string AbsoluteUrl
        {
            get
            {
                return _url;
            }
        }

        public string RedirectUrl
        {
            get; internal set;
        }

        public bool IsLinkFullyQualified
        {
            get; internal set;
        }

        public bool IsLinkExternalDomain
        {
            get; internal set;
        }

        public int HttpStatusCode
        {
            get; internal set;
        }
    }
}

