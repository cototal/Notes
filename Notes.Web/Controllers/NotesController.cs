using Microsoft.AspNetCore.Mvc;
using Notes.Web.Models.NoteVMs;
using Notes.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Web.Controllers
{
    public class NotesController : BaseController
    {
        private readonly NoteService _noteService;
        private readonly MarkdownConverter _markdown;

        public NotesController(NoteService noteService, MarkdownConverter markdown)
        {
            _noteService = noteService;
            _markdown = markdown;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var notes = await _noteService.Get();
            return View(notes);
        }


        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }
            var note = await _noteService.Get(id);
            if (note == null)
            {
                return NotFound();
            }

            note.Content = _markdown.ToHtml(note.Content);
            return View(note);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View(new NewNote());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewNote newNote)
        {
            if (ModelState.IsValid)
            {
                var note = newNote.ToNote();
                await _noteService.Create(note);
                return RedirectToAction("Details", new { note.Id });
            }
            return View(newNote);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }
            var note = await _noteService.Get(id);
            if (note == null)
            {
                return NotFound();
            }
            return View(new EditNote(note));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, EditNote editNote)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }
            var oldNote = await _noteService.Get(id);
            if (oldNote == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var note = editNote.ToNote(ref oldNote);
                await _noteService.Update(note);
                return RedirectToAction("Details", new { id });
            }
            return View(editNote);
        }

        [HttpPost]
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            await _noteService.Remove(id);
            return RedirectToAction("Index");
        }
    }
}
