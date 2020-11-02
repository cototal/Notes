using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Web.Models
{
    public class Reference
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public DateTime CreatedAt { get; set; }
        public IEnumerable<string> Tags { get; set; } = new List<string>();
    }
}
