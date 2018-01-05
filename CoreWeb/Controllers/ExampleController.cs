using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreWeb.Data.Interfaces;
using CoreWeb.Data.ViewModels;
using CoreWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreWeb.Controllers
{
    public class ExampleController : Controller
    {
        private IExampleRepo _repo;
        private IEmailService _emailService;

        public ExampleController(IExampleRepo repo, IEmailService emailService)
        {
            this._repo = repo;
            this._emailService = emailService;
        }

        // GET: Example
        public async Task<ActionResult> Index()
        {
            return View(await _repo.spGetManyExamplesAsync());
        }

        // GET: Example/Details/5
        public async Task<ActionResult> Details(int id)
        {
            return View(await _repo.spGetOneExampleAsync(id));
        }

        // POST: Example/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ExampleViewModel vm)
        {
            try
            {
                // TODO: Add insert logic here
                await _repo.InsertExampleAsync(vm, User.Identity.Name);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: Example/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, ExampleViewModel vm)
        {
            try
            {
                // TODO: Add update logic here
                await _repo.UpdateExampleAsync(id, vm, User.Identity.Name);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: Example/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                // TODO: Add delete logic here
                await _repo.DeleteExampleAsync(id, User.Identity.Name);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult SendEmail(IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                _emailService.Send(collection["subject"], collection["body"]);
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return RedirectToAction("Contact", "Home");
            }
        }
    }
}