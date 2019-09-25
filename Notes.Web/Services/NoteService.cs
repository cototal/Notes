using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Notes.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        private BsonDocument SortDocument(string sortBy = "accessedAt", int sortDir = -1)
        {
            if (Note.PropertyMap.ContainsKey(sortBy) && (sortDir == -1 || sortDir == 1))
            {
                return new BsonDocument { { Note.PropertyMap[sortBy], sortDir } };
            }
            return new BsonDocument { { "accessedAt", -1 } };
        }

        private FindOptions FindOpts()
        {
            return new FindOptions
            {
                Collation = new Collation("en")
            };
        }
        
        public async Task<List<Note>> Find(string search = null, string sortBy = "accessedAt", int sortDir = -1)
        {
            var sortDoc = new BsonDocument { { Note.PropertyMap[sortBy], sortDir } };
            if (string.IsNullOrWhiteSpace(search))
            {
                return await _notes.Find(n => true, FindOpts()).Sort(SortDocument(sortBy, sortDir)).Limit(50).ToListAsync();
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
                var name = string.Join(" ", values);
                switch (field)
                {
                    case "title":
                    case "category":
                    case "sequence":
                        doc.Add(Note.PropertyMap[field], new BsonDocument { { "$regex", new BsonRegularExpression(Regex.Escape(name), "i") } });
                        break;
                    case "tag":
                    case "tags":
                    case "content":
                        doc.Add(Note.PropertyMap[field], ListOfRegexes(values));
                        break;
                }
            }
            // TODO: Check if document is empty (indicating invalid search) and return no results if so
            return await _notes.Find(doc, FindOpts()).Sort(SortDocument(sortBy, sortDir)).ToListAsync();
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
