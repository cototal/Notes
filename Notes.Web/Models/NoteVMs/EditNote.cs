using System;
using System.Collections.Generic;
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
            var tags = Tags == null ? new List<string>() : Tags.Split(",").Select(t => t.Trim());
            oldNote.Title = Title;
            oldNote.Content = Content;
            oldNote.Category = Category;
            oldNote.Sequence = Sequence;
            oldNote.Tags = tags;
            oldNote.UpdatedAt = DateTime.Now;
            return oldNote;
        }
    }
}
