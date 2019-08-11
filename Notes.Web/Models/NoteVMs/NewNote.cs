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
            return new Note
            {
                Title = Title,
                Content = Content,
                Category = string.IsNullOrWhiteSpace(Category) ? "Uncategorized" : Category,
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
