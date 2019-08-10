using MongoDB.Driver;
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
            var database = client.GetDatabase("NoteDB");
            _notes = database.GetCollection<Note>("notes");
        }

        public async Task<List<Note>> Get()
        {
            return await _notes.Find(n => true).ToListAsync();
        }

        public async Task<Note> Create(Note note)
        {
            await _notes.InsertOneAsync(note);
            return note;
        }
    }
}
