using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TechnoIsland_Bookstore.Models;

namespace TechnoIsland_Bookstore.Controllers
{
    public class BorrowHistoriesController : Controller
    {
        private readonly ApplicationDbContext _db;
        public BorrowHistoriesController(ApplicationDbContext db)
        {
            _db = db;
        }
       

        public Book Book { get; set; }
        public BorrowHistory BorrowHistory { get; set; }
        public new User User { get; set; }
        
        public IActionResult Create(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var book = _db.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }

            var borrowHistory = new BorrowHistory { Id = book.Id, BorrowDate = DateTime.Now, Book = book };
            ViewBag.UserId = new SelectList(_db.Users, "UserId", "Username");
            return View(borrowHistory);
        }
                     
         [HttpPost]
         [ValidateAntiForgeryToken]
         public IActionResult Create([Bind("BorrowHistoryId", "Id", "UserId", "BorrowDate", "ReturnDate")] BorrowHistory borrowHistory)
         {
                if (ModelState.IsValid)
                {
                    _db.BorrowHistories.Add(borrowHistory);
                    _db.SaveChanges();
                    return RedirectToAction("Index", "Books");
                }

                ViewBag.UserId = new SelectList(_db.Users, "UserId", "Username", borrowHistory.UserId);
                borrowHistory.Book = _db.Books.Find(borrowHistory.Id);
                return View(borrowHistory);
         }

         
         public IActionResult Edit(int? id)
         {
                if (id == null)
                {
                    return new StatusCodeResult(400);
                }
                BorrowHistory borrowHistory = _db.BorrowHistories
                    .Include(b => b.Book)
                    .Include(c => c.User)
                    .Where(b => b.Id == id && b.ReturnDate == null)
                    .FirstOrDefault();
                if (borrowHistory == null)
                {
                    return StatusCode(404);
                }
                return View(borrowHistory);
         }

         [HttpPost]
         [ValidateAntiForgeryToken]
         public IActionResult Edit([Bind("BorrowHistoryId", "Id", "UserId", "BorrowDate", "ReturnDate")] BorrowHistory borrowHistory)
         {
            if (ModelState.IsValid)
            {
               var borrowHistoryItem = _db.BorrowHistories.Include(i => i.Book)
                   .FirstOrDefault(i => i.BorrowHistoryId == borrowHistory.BorrowHistoryId);
                if (borrowHistoryItem == null)
                {
                   return BadRequest();
                }

                 borrowHistoryItem.ReturnDate = DateTime.Now;
                 _db.SaveChanges();
                 return RedirectToAction("Index", "Books");
            }
                return View(borrowHistory);
         }

         protected override void Dispose(bool disposing)
         {
                if (disposing)
                {
                    _db.Dispose();
                }
                base.Dispose(disposing);
         }


    }
}
