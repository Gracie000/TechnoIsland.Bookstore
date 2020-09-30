using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechnoIsland_Bookstore.Models;

namespace TechnoIsland_Bookstore.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment webHostEnvironment;
        public BooksController(ApplicationDbContext db, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            webHostEnvironment = hostEnvironment;
        }
        [BindProperty]
        public Book Book { get; set; }
        public BookView BookView { get; set; }

        public IActionResult Index()
        {
            var books = _db.Books.Include(h => h.BorrowHistories)
                .Select(b => new BookView
                {
                    Id = b.Id,
                    Author = b.Author,
                    Publisher = b.Publisher,
                    Title = b.Title,
                    ImageName = b.ImageName,
                    IsAvailable = !b.BorrowHistories.Any(h => h.ReturnDate == null)
                }).ToList();
            return View(Book);
        }
        public IActionResult Dashboard()
        {

            var books = _db.Books.Include(h => h.BorrowHistories)
                .Select(b => new BookView
                {
                    Id = b.Id,
                    Author = b.Author,
                    Publisher = b.Publisher,
                    Title = b.Title,
                    ImageName = b.ImageName,
                    IsAvailable = !b.BorrowHistories.Any(h => h.ReturnDate == null)
                }).ToList();
            return View(Book);

        }

        public IActionResult Upsert(int? id)
        {
            Book = new Book();
            if (id == null)
            {

                //create
                return View(Book);
            }
            //update
            Book = _db.Books.FirstOrDefault(u => u.Id == id);
            if (Book == null)
            {
                return NotFound();
            }
            return View(Book);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Book model)
        {
            if (ModelState.IsValid)
            {
                if (Book.Id == 0)
                {
                    string uniqueFileName = null;
                    if (model.ImageFile != null)
                    {
                        string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        model.ImageFile.CopyTo(new FileStream(filePath, FileMode.Create));
                    }
                    Book = new Book
                    {
                        Title = model.Title,
                        Author = model.Author,
                        Publisher = model.Publisher,
                        ImageName = uniqueFileName
                    };

                    //create
                    _db.Books.Add(Book);
                }
                else
                {
                    _db.Books.Update(Book);
                }
                _db.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            return View(Book);
        }

        #region API Calls
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var books = _db.Books.Include(h => h.BorrowHistories)
                .Select(b => new BookView
                {
                    Id = b.Id,
                    Author = b.Author,
                    Publisher = b.Publisher,
                    Title = b.Title,
                    ImageName = b.ImageName,
                    IsAvailable = !b.BorrowHistories.Any(h => h.ReturnDate == null)
                }).ToList();
            return Json(new { data = await _db.Books.ToListAsync() });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var bookFromDb = await _db.Books.FirstOrDefaultAsync(u => u.Id == id);
            if (bookFromDb == null)
            {
                return Json(new { success = false, message = "Error while Deleting" });
            }
            _db.Books.Remove(bookFromDb);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Delete successful" });
        }
        #endregion


    }
}
