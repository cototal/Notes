using System.ComponentModel.DataAnnotations;

namespace Notes.Web.Models.NoteVMs
{
    public class BaseNoteForm
    {
        public BaseNoteForm()
        { }

        public BaseNoteForm(Note note)
        {
            Title = note.Title;
            Content = note.Content;
            Category = note.Category;
            Sequence = note.Sequence;
            Tags = string.Join(',', note.Tags);
        }

        [Required]
        public string Title { get; set; }
        public string Content { get; set; }
        public string Category { get; set; }
        public string Sequence { get; set; }
        public string Tags { get; set; }
    }
}
