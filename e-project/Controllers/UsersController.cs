using e_project.Models;
using e_project.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_project.Controllers
{
    public class UsersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;

        public UsersController(AppDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>(); 
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _context.User.FirstOrDefault(u => u.Email == user.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email already registered.");
                    return View(user);
                }

                user.Password = _passwordHasher.HashPassword(user, user.Password);

                // Optionally clear ConfirmPassword (if it exists in the model)
                user.ConfirmPassword = null;

                _context.User.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login"); // ✅ make sure Login action exists
            }

            return View(user);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.User.FirstOrDefault(u => u.Email == model.Email);
                if (user != null &&
                    _passwordHasher.VerifyHashedPassword(user, user.Password, model.Password) == PasswordVerificationResult.Success)
                {
                    HttpContext.Session.SetString("UserEmail", user.Email);
                    HttpContext.Session.SetString("UserRole", user.Role);
                    HttpContext.Session.SetString("FullName", user.FullName);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Invalid email or password.");
            }

            return View(model);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var user = await _context.User.FindAsync(id);
            if (user == null) return NotFound();

            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Email,Password,Role")] User user)
        {
            if (id != user.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Optionally rehash password here if you are editing it, otherwise avoid changing password
                    //_context.Entry(user).Property(x => x.Password).IsModified = false;

                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var user = await _context.User.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null) return NotFound();

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Profile()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (email == null) return RedirectToAction("Login");

            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return NotFound();

            var model = new ProfileViewModel
            {
                Email = user.Email,
                PasswordModel = new ChangePasswordViewModel()
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> ChangePassword(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Profile", model);
            }

            var email = HttpContext.Session.GetString("UserEmail");
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return NotFound();

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, model.PasswordModel.CurrentPassword);
            if (result != PasswordVerificationResult.Success)
            {
                ModelState.AddModelError("PasswordModel.CurrentPassword", "Current password is incorrect.");
                return View("Profile", model);
            }

            user.Password = _passwordHasher.HashPassword(user, model.PasswordModel.NewPassword);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Password changed successfully!";
            return RedirectToAction("Profile");
        }


        [HttpPost]
        public async Task<IActionResult> DeleteAccount()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == email);
            if (user != null)
            {
                _context.User.Remove(user);
                await _context.SaveChangesAsync();
                HttpContext.Session.Clear();
            }

            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> UpdateEmail(string newEmail)
        {
            var email = HttpContext.Session.GetString("UserEmail");
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return NotFound();

            user.Email = newEmail;
            await _context.SaveChangesAsync();
            HttpContext.Session.SetString("UserEmail", newEmail);

            TempData["Message"] = "Email updated successfully!";
            return RedirectToAction("Profile");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); 
            return RedirectToAction("Login", "Users");
        }

    }
}
