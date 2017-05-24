using System;
using System.Net;

namespace BLL
{
    public class DocumentResult
    {
        public Uri Uri { get; set; }

        public DateTime ScannedAt { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public TimeSpan DownloadTime { get; set; }
        public DocType Type { get; set; }

    }
    public enum DocType { HtmlPage, Asset }
}