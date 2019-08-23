using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Notes.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var noteQuery = _notes.AsQueryable();
            var searchSegments = search.Split(" ");
            foreach(var seg in searchSegments)
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
                        noteQuery = noteQuery.Where(n => n.Title.Contains(segParts[1]));
                        break;
                    case "category":
                        noteQuery = noteQuery.Where(n => n.Category.Contains(segParts[1]));
                        break;
                    case "sequence":
                        noteQuery = noteQuery.Where(n => n.Sequence.Contains(segParts[1]));
                        break;
                    case "tag":
                        foreach (var val in values)
                        {
                            noteQuery = noteQuery.Where(n => n.Tags.Contains(val));
                        }
                        break;
                    case "content":
                        foreach (var val in values)
                        {
                            noteQuery = noteQuery.Where(n => n.Content.Contains(val));
                        }
                        break;
                }
            }

            return await noteQuery.ToListAsync();
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
    }
}
