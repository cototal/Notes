using Microsoft.AspNetCore.Hosting;
using MongoDB.Bson;
using MongoDB.Driver;
using Notes.Web.Data;
using Notes.Web.Models;
using Notes.Web.Models.ReferenceVMs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Web.Services
{
    public class ReferenceService
    {
        private readonly IMongoCollection<Reference> _refs;
        private readonly MongoContext _context;
        private readonly IWebHostEnvironment _env;

        public ReferenceService(MongoContext context, IWebHostEnvironment env)
        {
            _context = context;
            _refs = context.References;
            _env = env;
        }

        public async Task<List<Reference>> Find(string search = null)
        {
            return await _refs.Find(n => true, _context.FindOpts).Sort(new BsonDocument()).ToListAsync();
        }

        public async Task<Reference> FindOne(string id)
        {
            return await _refs.Find(r => r.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Reference> Create(NewReference newRef)
        {
            var tags = newRef.Tags == null ? new List<string>() : newRef.Tags.Split(",").Select(t => t.Trim());
            var id = ObjectId.GenerateNewId().ToString();
            var newPath = string.Join('-', Path.GetFileNameWithoutExtension(newRef.File.FileName), id) + Path.GetExtension(newRef.File.FileName);
            var dest = Path.Combine(_env.WebRootPath, "uploads", newPath);
            using var stream = File.Create(dest);
            await newRef.File.CopyToAsync(stream);

            var reference = new Reference
            {
                Id = id,
                Name = newRef.Name,
                Description = newRef.Description,
                Tags = tags,
                CreatedAt = DateTime.Now,
                Path = newPath
            };
            await _refs.InsertOneAsync(reference);
            return reference;
        }
    }
}
