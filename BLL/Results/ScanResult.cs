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

        public TimeSpan AvgDownloadTime { get; private set; } = new TimeSpan(0);

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
            switch (result.Type)
            {
                case DocType.HtmlPage:
                    Pages.Add(result);
                    break;
                case DocType.Asset:
                    Assets.Add(result);
                    break;


            }
            TimeSpan totalTime = AvgDownloadTime + result.DownloadTime;
            double totalMiliSec = totalTime.TotalMilliseconds;
            AvgDownloadTime = TimeSpan.FromMilliseconds( totalMiliSec / (Pages.Count + Assets.Count));

        }
    }
}