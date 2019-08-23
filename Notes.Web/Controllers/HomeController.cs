using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Notes");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Exit");
        }

        [AllowAnonymous]
        public IActionResult Exit()
        {
            return View();
        }
    }
}
