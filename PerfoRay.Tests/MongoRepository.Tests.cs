using BLL;
using DAL;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PerfoRay.Tests
{
    public class MongoRepositoryTests
    {
        [Fact(Skip = "Long run; already tested;")]
        public void AddingToScanned_GetAll_ContainsScanned()
        {
            // Arrange
            string uri = "https://stackoverflow.com";
            string cs = "mongodb://mitutee:Hardware2017@ds157500.mlab.com:57500/storeage";
            MongoRepository<ScanResult, ObjectId> repo
                = new MongoRepository<ScanResult, ObjectId>(cs);

            ScansManager manager = new ScansManager(repo);
            // Act
            ScanResult res = new ScanResult(new Uri(uri));
            manager.Create(res);
            // Assert
            Assert.Contains(manager.GetAll(), r => r.Id.Equals(res.Id) );
        }



    }
}
