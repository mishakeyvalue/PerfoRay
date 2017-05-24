using DAL.Interfaces;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class ScanResult :IEntity<ObjectId>
    {

        ObjectId IEntity<ObjectId>.Id { get; set; }

        public Guid Id { get; set; } = new Guid();

        public Uri BaseAddress { get; set; }

        public DateTime ScannedAt { get; set; }

        public TimeSpan AvgDownloadTime { get; private set; }

        public ICollection<DocumentResult> Pages { get; set; }
        public ICollection<DocumentResult> Assets { get; set; }

        public ScanResult(Uri baseAddress)
        {
            BaseAddress = baseAddress;
            ScannedAt = DateTime.Now;

            Pages = new HashSet<DocumentResult>();
            Assets = new HashSet<DocumentResult>();
        }

        public void AddDocumentResult(DocumentResult result)
        {
            AvgDownloadTime = TimeSpan.FromMilliseconds((AvgDownloadTime + result.DownloadTime).Milliseconds / (double) (Pages.Count + Assets.Count + 1));
            switch (result.Type)
            {
                case DocType.HtmlPage:
                    Pages.Add(result);
                    break;
                case DocType.Asset:
                    Assets.Add(result);
                    break;
            }
        }
    }
}