using BLL;
using System;
using Xunit;

namespace PerfoRay.Tests
{
    public class ScannerTests
    {
        [Fact]
        public void StartScanning_InvalidUir_TrowsEx()
        {
            // Arrange
            string invalidUri = "invalid_string";
            // Act

            // Assert
            Assert.Throws(typeof(ArgumentException),  () => new Scanner(invalidUri));
        }

        [Fact(Skip = "Long run; already tested;")]
        public void StartScanning_ValidUri_ReturnsScanResult()
        {
            // Arrange
            string validUri = "https://mdbootstrap.com";
            Scanner sc = new Scanner(validUri);
            // Act
            ScanResult res = sc.Scan();
            // Assert
            Assert.NotNull(res);
        }

        [Fact]
        public void AddingAvrgTimeIsCorrect()
        {
            // Arrange
            ScanResult res = new ScanResult(new Uri("http://mock.com"));

            TimeSpan ts1 = new TimeSpan(0, 1, 0);
            TimeSpan ts2 = new TimeSpan(0, 3, 0);
            TimeSpan avg = new TimeSpan(0, 2, 0); //TimeSpan.FromMilliseconds((ts1 + ts2).TotalMilliseconds  / 2);

            DocumentResult d_res1 = new DocumentResult() { DownloadTime = ts1 };
            DocumentResult d_res2 = new DocumentResult() { DownloadTime = ts2 };

            // Act
            res.AddDocumentResult(d_res1);
            res.AddDocumentResult(d_res2);

            // Assert
            Assert.Equal(avg, res.AvgDownloadTime);


        }

    }
}
