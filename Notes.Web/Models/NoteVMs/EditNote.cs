using System;
using System.Linq;

namespace Notes.Web.Models.NoteVMs
{
    public class EditNote : BaseNoteForm
    {
        public EditNote() : base()
        {}

        public EditNote(Note note) : base(note)
        {}

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
