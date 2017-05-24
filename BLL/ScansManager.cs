using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using DAL.Interfaces;
using MongoDB.Bson;
using System.Linq;

namespace BLL
{
    public class ScansManager
    {
        IRepository<ScanResult, ObjectId> _repo;
        public ScansManager(IRepository<ScanResult, ObjectId> repo)
        {
            _repo = repo;
        }

        public void Create(ScanResult result)
        {
            _repo.Add(result);
        }

        public IEnumerable<ScanResult> GetAll()
        {
            return _repo.GetAll().OrderByDescending( el => el.ScannedAt);
        }
    }
}
