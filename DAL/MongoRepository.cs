using DAL.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL
{
    public class MongoRepository<TEntity, TIdentifier> : IRepository<TEntity, TIdentifier> where TEntity : class, IEntity<TIdentifier>
    {

        private IMongoDatabase _database { get; set; }

        public MongoRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public MongoRepository(string connection_string)
        {
            var con = new MongoUrlBuilder(connection_string);
            MongoClient client = new MongoClient(connection_string);
            IMongoDatabase database = client.GetDatabase(con.DatabaseName);

            _database = database;
        }

        public TEntity Get(TIdentifier id)
        {
            return _database.GetCollection<TEntity>(typeof(TEntity).Name).Find(x => x.Id.Equals(id)).FirstOrDefaultAsync().Result;
        }

        public IEnumerable<TEntity> GetAll()
        {
            string s = typeof(TEntity).Name;
            return _database.GetCollection<TEntity>(typeof(TEntity).Name).Find(new BsonDocument()).ToListAsync().Result;
        }

        public TEntity Save(TEntity entity)
        {
            var collection = _database.GetCollection<TEntity>(typeof(TEntity).Name);

            collection.ReplaceOneAsync(x => x.Id.Equals(entity.Id), entity, new UpdateOptions {
                IsUpsert = true
            });

            return entity;
        }

        public TIdentifier Add(TEntity entity)
        {
            var collection = _database.GetCollection<TEntity>(typeof(TEntity).Name);

            try { collection.InsertOne(entity);
            }
            catch(MongoDB.Driver.MongoWriteException)
                { throw; } // This operation updates the id of the entity
            return entity.Id;
        }

        public void Delete(TIdentifier id)
        {
            var collection = _database.GetCollection<TEntity>(typeof(TEntity).Name);

            collection.DeleteOneAsync(x => x.Id.Equals(id));
        }

        public void Delete(TEntity entity)
        {
            var collection = _database.GetCollection<TEntity>(typeof(TEntity).Name);

            collection.DeleteOneAsync(x => x.Id.Equals(entity.Id));
        }
    }
}
