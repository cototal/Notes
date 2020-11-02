using MongoDB.Driver;
using Notes.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Web.Data
{
    public class MongoContext
    {
        public MongoContext(string connectionString)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("notedb");
            Notes = database.GetCollection<Note>("notes");
            References = database.GetCollection<Reference>("references");

            Notes.Indexes.CreateOne(new CreateIndexModel<Note>(
                Builders<Note>.IndexKeys.Text(n => n.Content)));
        }

        public IMongoCollection<Note> Notes { get; set; }
        public IMongoCollection<Reference> References { get; set; }

        public FindOptions FindOpts {
            get {
                return new FindOptions
                {
                    Collation = new Collation("en")
                };
            }
        }
    }
}
