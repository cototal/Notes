using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Web.Models.NoteVMs
{
    public class NewNote
    {
        public NewNote()
        {}

        public NewNote(Note note)
        {
            Title = note.Title;
            Content = note.Content;
            Category = note.Category;
            Sequence = note.Sequence;
            Tags = string.Join(',', note.Tags);
        }

        public string Title { get; set; }
        public string Content { get; set; }
        public string Category { get; set; }
        public string Sequence { get; set; }
        public string Tags { get; set; }

        public Note ToNote()
        {
            return new Note
            {
                Title = Title,
                Content = Content,
                Category = Category,
                Sequence = Sequence,
                Tags = Tags.Split(",").Select(t => t.Trim()),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                AccessedAt = DateTime.Now,
                AccessCount = 0
            };
        }
    }
}
