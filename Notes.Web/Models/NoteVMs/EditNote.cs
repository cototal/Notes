using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Web.Models.NoteVMs
{
    public class EditNote
    {
        public EditNote()
        {}

        public EditNote(Note note)
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

        public Note ToNote(ref Note oldNote)
        {
            oldNote.Title = Title;
            oldNote.Content = Content;
            oldNote.Category = Category;
            oldNote.Sequence = Sequence;
            oldNote.Tags = Tags.Split(",").Select(t => t.Trim());
            oldNote.UpdatedAt = DateTime.Now;
            return oldNote;
        }
    }
}
