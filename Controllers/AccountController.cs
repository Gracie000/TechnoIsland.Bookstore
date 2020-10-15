using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechnoIsland_Bookstore.Models;

namespace TechnoIsland_Bookstore.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AccountController(ApplicationDbContext db)
        {
            _db = db;
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();

        }


        [HttpPost]
        [AllowAnonymous]
        public  async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _db.Users.SingleOrDefaultAsync(a => a.Username == model.Username && a.Password == model.Password);
                if (model.Username == "Administrator" && model.Password == "PASSword")
                {
                     HttpContext.Session.SetString("userId", user.Username);
                     return View("Dashboard", "Books");

                    
                }
                if (model.Username == user.Username && model.Password == user.Password)
                {
                     HttpContext.Session.SetString("userId", user.Username);
                     return View("Index", "Books");
                    
                }
            }
                             
            else
            {
                return View("Index");
            }
                     
            return View();

        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Register(Register register)
        {

            if (ModelState.IsValid)
            {
                User user = new User
                {
                    MobileNumber = register.MobileNumber,
                    Username = register.Username,
                    Password = register.Password,
                    

                };
                _db.Add(user);
                await _db.SaveChangesAsync();

            }
            else
            {
                return View("Register");
            }
            return RedirectToAction("Index", "Account");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Index", "Home");
        }
    }
}
