using System;
namespace BLL
{
    internal class ScanResult
    {
        public Guid Id { get; set; }

        public Uri Uri { get; set; }

        public DateTime ScannedAt { get; set; }

        public TimeSpan DownloadTime { get; set; }



    }
}