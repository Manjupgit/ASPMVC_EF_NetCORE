using ContactManagerMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ContactManagerMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext db;
        public HomeController(ILogger<HomeController> logger, AppDbContext db)
        {
            _logger = logger;
            this.db = db;
        }

        public IActionResult Index()
        {
           // ViewBag.Title = AppConfig.Title;
            var query = db.Contacts.ToList();
                //var query = from c in db.Contacts
                //            orderby c.Id ascending
                //            select c;
                List<Contact> model = query.ToList();
                return View(model);
        }
        public IActionResult AddContact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddContact(Contact obj)
        {
            if (ModelState.IsValid)
            {
                db.Contacts.Add(obj);
                db.SaveChanges();
                ViewBag.Message = "Contact added successfully!";
            }
            return View(obj);
        }

        public IActionResult DeleteContact(int id)
        {
                var contact = (from c in db.Contacts
                               where c.Id == id
                               select c).SingleOrDefault();
                db.Contacts.Remove(contact);
                db.SaveChanges();
                return RedirectToAction("Index");
            
        }

        public IActionResult Details(int id)
        {
            var contact = (from c in db.Contacts
                           where c.Id == id
                           select c).SingleOrDefault();
            return View(contact);
        }

        public IActionResult Edit(int id)
        {
            var contactToEdit = db.Set<Contact>().FirstOrDefault(p => p.Id == id);

            if (contactToEdit == null)
            {
                return NotFound();
            }
            return View(contactToEdit);
        }

        public IActionResult EditContact(Contact editedContact)
        {
            if (ModelState.IsValid)
            {
                //var contactToEdit = db.Set<Contact>().FirstOrDefault(p => p.Id == editedContact.Id);

                //if (contactToEdit == null)
                //{
                //    return NotFound();
                //}
                //contactToEdit.Id = editedContact.Id;
                //db.Set<Contact>().Update(editedContact);
                db.Contacts.Update(editedContact);
                db.SaveChanges();
                ViewBag.Message = "Contact Modified successfully!";
            }
            return View("Edit", editedContact);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
