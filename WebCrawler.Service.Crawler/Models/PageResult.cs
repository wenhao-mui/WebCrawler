using System;
using System.Net;

namespace WebCrawler.Service.Crawler.Models
{
    public class PageResult
    {
        public PageResult(string htmlContent, string contentType, string redirectLocation, HttpStatusCode statusCode )
        {
            this.HtmlContent = htmlContent;
            this.ContentType = contentType;
            this.StatusCode = statusCode;
            this.RedirectLocation = redirectLocation;
        }

        public string HtmlContent { internal set; get; }
        public string ContentType { internal set; get; }
        public string RedirectLocation { internal set; get; }
        public HttpStatusCode StatusCode { internal set; get; }
        public bool IsSuccessCode
        {
            get { return ((int)this.StatusCode) >= 200 && ((int)this.StatusCode) < 300; }
        }
    }
}

