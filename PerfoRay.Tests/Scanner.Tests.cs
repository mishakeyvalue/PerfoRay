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
            Scanner sc = new Scanner(invalidUri);
            // Act

            // Assert
            Assert.Throws(typeof(ArgumentException),  () =>  sc.Scan());
        }

        [Fact]
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

    }
}
