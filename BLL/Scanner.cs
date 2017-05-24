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
    public class Scanner : IDisposable
    {
        private HttpClient _client { get; set; } = new HttpClient();
        private const int PAGES_LIMIT = 100;
        private Stopwatch sw = new Stopwatch();
        private Uri _baseUri;

        private Queue<Uri> _uriQueue { get; } = new Queue<Uri>();
        private HashSet<Uri> _scanned = new HashSet<Uri>();


        public ScanResult ScanWebsite(string website)
        {
            if(!Uri.TryCreate(website, UriKind.Absolute, out  _baseUri))
            {
                throw new ArgumentException("Invalid URI.");
            }
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

            HttpResponseMessage response = _client.GetAsync(uri).Result;

            sw.Stop();
            OnMeasuringEnded(uri, sw.Elapsed);

            DocumentResult result = new DocumentResult()
            {
                Uri = uri,
                ScannedAt = DateTime.Now,
                StatusCode = response.StatusCode,
                DownloadTime = sw.Elapsed
            };

            string contentType = response.Content?.Headers.ContentType?.MediaType; // Detect type of response
            if (Utilities.IsHtmlMimeType(contentType))
            {
                result.Type = DocType.HtmlPage;
                parseResponse(response);
            }
            else result.Type = DocType.Asset;


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
                if (Uri.TryCreate(anchor.Href,UriKind.Absolute, out link))
                {
                    if (isSameHost(link) && !_uriQueue.Contains(link) && !_scanned.Contains(link))
                    {
                        _uriQueue.Enqueue(link);
                    }
                }

            }
        }

        private bool isSameHost(Uri uri)
        {
            return uri.Host == _baseUri.Host;
        }

        private void OnMeasuringEnded(Uri uri, TimeSpan elapsed)
        {
        }

        private void OnMeasuringStarted(Uri uri)
        {
        }

        public void Dispose()
        {
            _client?.Dispose();
            _client = null;
        }
    }
}
