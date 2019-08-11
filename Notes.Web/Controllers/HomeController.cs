using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Notes.Web.Models;
using Notes.Web.Services;

namespace Notes.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly NoteService _noteService;

        public HomeController(NoteService noteService)
        {
            _noteService = noteService;
        }

        public async Task<IActionResult> Index()
        {
            // var note = new Note
            // {
            //     Title = "Sample",
            //     Category = "Samples",
            //     Tags = new List<string> { "one", "two", "three" },
            //     Content = "This is a **sample** note.",
            //     CreatedAt = DateTime.Now,
            //     UpdatedAt = DateTime.Now,
            //     AccessedAt = DateTime.Now,
            //     AccessCount = 1
            // };
            // await _noteService.Create(note);
            var notes = await _noteService.Get();
            return View(notes);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
