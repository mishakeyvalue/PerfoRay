using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace BLL
{
    public class Scanner : IDisposable
    {
        private HttpClient _client { get; set; }
        private const int PAGES_LIMIT = 100;
        private Stopwatch sw = new Stopwatch();
        private Uri _baseUri;

        private Queue<Uri> _uriQueue { get; } = new Queue<Uri>();


        public void ScanWebsite(string website)
        {
            if(!Uri.TryCreate(website, UriKind.Absolute, out  _baseUri))
            {
                throw new ArgumentException("Invalid URI.");
            }
            _uriQueue.Enqueue(_baseUri);
            int counter = 0;
            while(_uriQueue.Count > 0 && counter++ < PAGES_LIMIT)
            {
                var response = processDocument(_uriQueue.Dequeue());

            }

        }

        private ScanResult processDocument(Uri uri)
        {
            OnMeasuringStarted(uri);
            sw.Start();

            HttpResponseMessage response = _client.GetAsync(uri).Result;

            sw.Stop();
            OnMeasuringEnded(uri, sw.Elapsed);

            ScanResult result = new ScanResult();

            return result;
        }

        private void parseResponse(HttpResponseMessage response)
        {
            throw new NotImplementedException();
        }

        private void OnMeasuringEnded(Uri uri, TimeSpan elapsed)
        {
            throw new NotImplementedException();
        }

        private void OnMeasuringStarted(Uri uri)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _client?.Dispose();
            _client = null;
        }
    }
}
