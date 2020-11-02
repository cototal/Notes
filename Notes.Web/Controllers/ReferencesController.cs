using Microsoft.AspNetCore.Mvc;
using Notes.Web.Models.ReferenceVMs;
using Notes.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Web.Controllers
{
    public class ReferencesController : BaseController
    {
        private readonly ReferenceService _refService;

        public ReferencesController(ReferenceService refService)
        {
            _refService = refService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var refs = await _refService.Find();
            return View(refs);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }
            var reference = await _refService.FindOne(id);
            if (reference == null)
            {
                return NotFound();
            }
            return View(reference);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new NewReference());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewReference newReference)
        {
            if (ModelState.IsValid)
            {
                var reference = await _refService.Create(newReference);
                return RedirectToAction("Details", new { reference.Id });
            }
            return View(newReference);
        }
    }
}
