using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Notes.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Notes.Web.Services
{
    public class NoteService
    {
        private readonly IMongoCollection<Note> _notes;

        public NoteService(string connectionString)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("notedb");
            _notes = database.GetCollection<Note>("notes");

            _notes.Indexes.CreateOne(new CreateIndexModel<Note>(
                Builders<Note>.IndexKeys.Text(n => n.Content)));
        }

        public async Task<List<Note>> Find()
        {
            return await _notes.Find(n => true).ToListAsync();
        }

        public async Task<List<Note>> Find(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return await Find();
            }
            var searchSegments = search.Split(" ");
            var doc = new BsonDocument();
            foreach (var seg in searchSegments)
            {
                var segParts = seg.Split(":");
                if (segParts.Length < 2)
                {
                    continue;
                }
                var field = segParts[0];
                var values = segParts[1].Split("-");
                switch (field)
                {
                    case "title":
                        doc.Add("Title", new BsonDocument { { "$regex", new BsonRegularExpression(Regex.Escape(segParts[1]), "i") } });
                        break;
                    case "category":
                        doc.Add("Category", new BsonDocument { { "$regex", new BsonRegularExpression(Regex.Escape(segParts[1]), "i") } });
                        break;
                    case "sequence":
                        doc.Add("Sequence", new BsonDocument { { "$regex", new BsonRegularExpression(Regex.Escape(segParts[1]), "i") } });
                        break;
                    case "tag":
                        doc.Add("Tags", ListOfRegexes(values));
                        break;
                    case "content":
                        doc.Add("Content", ListOfRegexes(values));
                        break;
                }
            }

            return await _notes.Find(doc).ToListAsync();
        }

        public async Task<Note> FindOne(string id, bool skipAccess = false)
        {
            var note = await _notes.Find(n => n.Id == id).FirstOrDefaultAsync();
            if (!skipAccess)
            {
                ++note.AccessCount;
                note.AccessedAt = DateTime.Now;
                await _notes.ReplaceOneAsync(n => n.Id == note.Id, note);
            }
            return note;
        }

        public async Task<Note> Create(Note note)
        {
            await _notes.InsertOneAsync(note);
            return note;
        }

        public async Task<Note> Update(Note updatedNote)
        {
            await _notes.ReplaceOneAsync(n => n.Id == updatedNote.Id, updatedNote);
            return updatedNote;
        }

        public async Task Remove(string id)
        {
            await _notes.DeleteOneAsync(n => n.Id == id);
        }

        private BsonDocument ListOfRegexes(IEnumerable<string> values)
        {
            var bsonElements = new List<BsonElement>();
            foreach (var val in values)
            {
                bsonElements.Add(new BsonElement("$regex", new BsonRegularExpression(Regex.Escape(val), "i")));
            }
            var regexDoc = new BsonDocument(true);
            regexDoc.AddRange(bsonElements);
            return regexDoc;
        }
    }
}
