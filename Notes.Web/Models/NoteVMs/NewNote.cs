using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Web.Models.NoteVMs
{
    public class NewNote : BaseNoteForm
    {
        public NewNote() : base()
        { }

        public NewNote(Note note) : base(note)
        { }

        public Note ToNote()
        {
            var tags = Tags == null ? new List<string>() : Tags.Split(",").Select(t => t.Trim());
            return new Note
            {
                Title = Title,
                Content = Content,
                Category = string.IsNullOrWhiteSpace(Category) ? "Uncategorized" : Category,
                Sequence = Sequence,
                Tags = tags,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                AccessedAt = DateTime.Now,
                AccessCount = 0
            };
        }
    }
}
