using System;
using System.Net;

namespace BLL
{
    public class DocumentResult
    {
        private float _size;

        public Uri Uri { get; set; }

        /// <summary>
        /// Setted in bytes, gotten in megaBytes
        /// </summary>
        public float Size { get { return _size; }
            internal set {
                _size = value / (float)Math.Pow(10, 6);
            }
        }
        public DateTime ScannedAt { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public TimeSpan DownloadTime { get; set; }
        public Exception Exception { get; set; }
        public DocType Type { get; set; }

    }
    public enum DocType { HtmlPage, Asset }
}