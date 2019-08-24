using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Notes.Web.Models
{
    public class Note
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Category { get; set; }
        public string Sequence { get; set; }
        public IEnumerable<string> Tags { get; set; } = new List<string>();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime AccessedAt { get; set; }
        public int AccessCount { get; set; }

        public static Dictionary<string, string> PropertyMap {
            get {
                return new Dictionary<string, string>
                {
                    { "title", "Title" },
                    { "content", "Content" },
                    { "category", "Category" },
                    { "sequence", "Sequence" },
                    { "tag", "Tags" },
                    { "tags", "Tags" },
                    { "accessedAt", "AccessedAt" },
                    { "createdAt", "CreatedAt" },
                    { "accessCount", "AccessCount" },
                    { "count", "AccessCount" }
                };
            }
        }
    }
}
