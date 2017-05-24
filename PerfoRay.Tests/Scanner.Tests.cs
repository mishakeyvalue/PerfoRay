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
            Scanner sc = new Scanner();
            string invalidUri = "invalid_string";
            // Act

            // Assert
            Assert.Throws(typeof(ArgumentException),  () =>  sc.ScanWebsite(invalidUri));
        }

        [Fact]
        public void StartScanning_ValidUri_ReturnsScanResult()
        {
            // Arrange
            Scanner sc = new Scanner();
            string validUri = "https://mdbootstrap.com";
            // Act
            ScanResult res = sc.ScanWebsite(validUri);
            // Assert
            Assert.NotNull(res);
        }

    }
}
