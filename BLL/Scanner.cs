using AngleSharp;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BLL
{
    /// <summary>
    /// Main class for scanning web sites
    /// </summary>
    public class Scanner : IDisposable
    {
        #region Fields and properties
        private HttpClient _client { get; set; } = new HttpClient();

        /// <summary>
        /// Limit of the pages to scan
        /// </summary>
        private const int PAGES_LIMIT = 20;

        private Stopwatch sw = new Stopwatch();

        /// <summary>
        /// Base URI where we start
        /// </summary>
        private Uri _baseUri;

        private Url _baseUrl;

        /// <summary>
        /// Queue of URIs to scan
        /// </summary>
        private Queue<Uri> _uriQueue { get; } = new Queue<Uri>();

        /// <summary>
        /// Collection to store already processed URIs 
        /// </summary>
        private ICollection<Uri> _scanned = new HashSet<Uri>();
        #endregion

        public Scanner(string URI)
        {
            if (!Uri.TryCreate(URI, UriKind.Absolute, out _baseUri))
            {
                throw new ArgumentException("Invalid URI.");
            }

            _baseUrl = new Url(URI);
        }

        /// <summary>
        /// Entry point for scanning process
        /// </summary>
        /// <param name="website"> URI of website to scan </param>
        /// <returns> Summary of scanning process </returns>
        public ScanResult Scan()
        {

            ScanResult result = new ScanResult(_baseUri);
            _uriQueue.Enqueue(_baseUri);
            _scanned.Add(_baseUri);
            int counter = 0;
            while(_uriQueue.Count > 0 && counter++ < PAGES_LIMIT)
            {
                DocumentResult docResult = processDocument(_uriQueue.Dequeue());
                _scanned.Add(docResult.Uri);
                result.AddDocumentResult(docResult);
            }

            return result;

        }

        private DocumentResult processDocument(Uri uri)
        {
            OnMeasuringStarted(uri);
            sw.Start();

            HttpResponseMessage response;
            try
            {
                response = _client.GetAsync(uri).Result;
            }
            catch (Exception e)
            {

                return new DocumentResult() { Exception = e };
            }
            sw.Stop();
            
            DocumentResult result = new DocumentResult()
            {
                Uri = uri,
                ScannedAt = DateTime.Now,
                StatusCode = response.StatusCode,
                Size = response.Content.ReadAsByteArrayAsync().Result.Length,
                DownloadTime = sw.Elapsed
            };

            string contentType = response.Content?.Headers.ContentType?.MediaType; // Detect type of response
            if (Utilities.IsHtmlMimeType(contentType))
            {
                result.Type = DocType.HtmlPage;
                parseResponse(response);
            }
            else result.Type = DocType.Asset;

            OnMeasuringEnded(result);

            sw.Reset();
            return result;
        }

        private static string anchorSelector = "a";

        private void parseResponse(HttpResponseMessage response)
        {
            HtmlParser parser = new HtmlParser();
            string html = response.Content.ReadAsStringAsync().Result;
            var anchors = parser.Parse(html).QuerySelectorAll(anchorSelector).OfType<IHtmlAnchorElement>();
            foreach(var anchor in anchors)
            {

                Uri link = null;
                List<string> hrefs = new List<string>();
                hrefs.Add(anchor.Href);

                if (anchor.Href.Length > 8 && anchor.Href.Contains("about://")) hrefs.Add(anchor.Href.Substring(8));
                
                foreach (string h in hrefs)
                {
                    if (Uri.TryCreate(getAbsoluteUrl(_baseUrl, h), UriKind.RelativeOrAbsolute, out link))
                    {
                        if (isSameHost(link) && !_uriQueue.Contains(link) && !_scanned.Contains(link))
                        {
                            _uriQueue.Enqueue(link);
                        }
                    }
                }
            }
        }
        private string getAbsoluteUrl(Url baseUrl, string url)
        {
            var u = new Url(url);
            if (u.IsAbsolute)
                return u.Href;

            var fullUrl = new Url(baseUrl, url);
            return fullUrl.Href;
        }

        private bool isSameHost(Uri uri)
        {
            return _baseUrl.Host == new Url(uri.AbsoluteUri).Host;

        }

        #region Events
        public EventHandler<Uri> MeasuringStarted;
        public EventHandler<DocumentResult> MeasuringEnded;

        private void OnMeasuringEnded(DocumentResult result)
        {
            MeasuringEnded?.Invoke(this, result);
        }

        private void OnMeasuringStarted(Uri uri)
        {
            MeasuringStarted?.Invoke(this, uri);
        }

        #endregion

        public void Dispose()
        {
            _client?.Dispose();
            _client = null;
        }
    }
}
