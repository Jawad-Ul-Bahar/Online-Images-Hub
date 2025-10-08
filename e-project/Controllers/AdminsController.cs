using e_project.Models;
using e_project.Services;
using e_project.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace e_project.Controllers
{
    public class AdminsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<Admin> _passwordHasher;
        private readonly CreditCardProtector _creditCardProtector;
        private readonly IWebHostEnvironment _env;


        public AdminsController(AppDbContext context, IPasswordHasher<Admin> passwordHasher, CreditCardProtector creditCardProtector, IWebHostEnvironment env)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _creditCardProtector = creditCardProtector;
            _env = env;
        }

        // GET: Admins
        public async Task<IActionResult> Index()
        {
            var totalUsers = await _context.User.CountAsync();
            var totalOrders = await _context.Order.CountAsync();
            var totalPhotos = await _context.PhotoOrderItem.CountAsync();
            var totalRevenue = await _context.Order.SumAsync(o => (decimal?)o.TotalAmount) ?? 0;

            var recentUsers = await _context.User
                .OrderByDescending(u => u.Id)
                .Take(5)
                .ToListAsync();

            var recentOrders = await _context.Order
                .OrderByDescending(o => o.OrderDate)
                .Take(5)
                .Include(o => o.User)
                .ToListAsync();

            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalOrders = totalOrders;
            ViewBag.TotalPhotos = totalPhotos;
            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.RecentUsers = recentUsers;
            ViewBag.RecentOrders = recentOrders;

            return View();
        }


        // GET: Admins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var admin = await _context.Admins.FirstOrDefaultAsync(m => m.Id == id);
            if (admin == null) return NotFound();

            return View(admin);
        }

        // GET: Admins/Create
        public IActionResult Create() => View();

        // POST: Admins/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,Email,Password,Role")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                // Hash password before saving
                admin.Password = _passwordHasher.HashPassword(admin, admin.Password);

                _context.Add(admin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(admin);
        }

        // GET: Admins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var admin = await _context.Admins.FindAsync(id);
            if (admin == null) return NotFound();

            return View(admin);
        }

        // POST: Admins/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Email,Role")] Admin updatedAdmin, string newPassword)
        {
            if (id != updatedAdmin.Id) return NotFound();

            var existingAdmin = await _context.Admins.FindAsync(id);
            if (existingAdmin == null) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    existingAdmin.FullName = updatedAdmin.FullName;
                    existingAdmin.Email = updatedAdmin.Email;
                    existingAdmin.Role = updatedAdmin.Role;

                    if (!string.IsNullOrWhiteSpace(newPassword))
                    {
                        existingAdmin.Password = _passwordHasher.HashPassword(existingAdmin, newPassword);
                    }

                    _context.Update(existingAdmin);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminExists(updatedAdmin.Id)) return NotFound();
                    else throw;
                }
            }
            return View(updatedAdmin);
        }

        // GET: Admins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var admin = await _context.Admins.FirstOrDefaultAsync(m => m.Id == id);
            if (admin == null) return NotFound();

            return View(admin);
        }

        // POST: Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var admin = await _context.Admins.FindAsync(id);
            if (admin != null)
            {
                _context.Admins.Remove(admin);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool AdminExists(int id) => _context.Admins.Any(e => e.Id == id);

        // GET: Register
        public IActionResult Register() => View();

        // POST: Register
        [HttpPost]
        public async Task<IActionResult> Register(Admin admin)
        {
            if (ModelState.IsValid)
            {
                var existingAdmin = await _context.Admins.FirstOrDefaultAsync(a => a.Email == admin.Email);
                if (existingAdmin != null)
                {
                    ModelState.AddModelError("Email", "Email already exists.");
                    return View(admin);
                }

                // Hash the password
                admin.Password = _passwordHasher.HashPassword(admin, admin.Password);

                _context.Admins.Add(admin);
                await _context.SaveChangesAsync();

                HttpContext.Session.SetString("AdminEmail", admin.Email);
                HttpContext.Session.SetString("AdminRole", admin.Role);

                return RedirectToAction("Login", "Admins");
            }

            return View(admin);
        }

        // GET: Login
        public IActionResult Login() => View();

        // POST: Login
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Email == email);
            if (admin != null)
            {
                var result = _passwordHasher.VerifyHashedPassword(admin, admin.Password, password);
                if (result == PasswordVerificationResult.Success)
                {
                    HttpContext.Session.SetString("AdminEmail", admin.Email);
                    HttpContext.Session.SetString("AdminRole", admin.Role);
                    return RedirectToAction("Index", "Admins");
                }
            }

            ViewBag.Error = "Invalid credentials";
            return View();
        }

        // POST: Logout
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Admins");
        }

        // GET: Admins/AllOrders
        public async Task<IActionResult> AllOrders()
        {
            var orders = await _context.Order
                .Include(o => o.PhotoOrderItems)
                .Include(o => o.User)
                .ToListAsync();

            var viewModels = orders.Select(o => new AdminOrderViewModel
            {
                OrderId = o.Id,
                UserEmail = o.User.Email,
                PaymentMethod = o.PaymentMethod,
                ShippingAddress = o.ShippingAddress,
                Status = o.Status,
                OrderDate = o.OrderDate,
                PhotoOrderItems = o.PhotoOrderItems
            }).ToList();

            return View(viewModels);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, string newStatus)
        {
            var order = await _context.Order
                .Include(o => o.PhotoOrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null) return NotFound();

            order.Status = newStatus;
            await _context.SaveChangesAsync();

            // ✅ Delete photo files when order is shipped
            if (newStatus == "Delivered")
            {
                var items = await _context.PhotoOrderItem
                    .Where(i => i.OrderId == orderId)
                    .ToListAsync();

                foreach (var item in items)
                {
                    var fullPath = Path.Combine(
                        _env.WebRootPath,
                        item.PhotoFilePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString())
                    );

                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
            }

            return RedirectToAction("AllOrders");
        }

        // GET: Admins/ManagePrintSizes
        public async Task<IActionResult> ManagePrintSizes()
        {
            var sizes = await _context.PrintSizePrice.ToListAsync();
            return View(sizes);
        }

        // GET: Admins/CreatePrintSize
        public IActionResult CreatePrintSize() => View();

        // POST: Admins/CreatePrintSize
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePrintSize(PrintSizePrice printSize)
        {
            if (ModelState.IsValid)
            {
                _context.PrintSizePrice.Add(printSize);
                await _context.SaveChangesAsync();
                return RedirectToAction("ManagePrintSizes");
            }
            return View(printSize);
        }

        // GET: Admins/EditPrintSize/5
        public async Task<IActionResult> EditPrintSize(int? id)
        {
            if (id == null) return NotFound();

            var size = await _context.PrintSizePrice.FindAsync(id);
            if (size == null) return NotFound();

            return View(size);
        }

        // POST: Admins/EditPrintSize/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPrintSize(int id, PrintSizePrice updatedSize)
        {
            if (id != updatedSize.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(updatedSize);
                await _context.SaveChangesAsync();
                return RedirectToAction("ManagePrintSizes");
            }
            return View(updatedSize);
        }

        // GET: Admins/DeletePrintSize/5
        public async Task<IActionResult> DeletePrintSize(int? id)
        {
            if (id == null) return NotFound();

            var size = await _context.PrintSizePrice.FindAsync(id);
            if (size == null) return NotFound();

            return View(size);
        }

        // POST: Admins/DeletePrintSize/5
        [HttpPost, ActionName("DeletePrintSize")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePrintSizeConfirmed(int id)
        {
            var size = await _context.PrintSizePrice.FindAsync(id);
            if (size != null)
            {
                _context.PrintSizePrice.Remove(size);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("ManagePrintSizes");
        }

        public async Task<IActionResult> VerifyPayment(int orderId)
        {
            var order = await _context.Order
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == orderId); // Include User for Email in view

            if (order == null)
                return NotFound();

            if (string.IsNullOrEmpty(order.CreditCardNumber))
            {
                ViewBag.DecryptedCard = "⚠️ No credit card info available for this order.";
            }
            else
            {
                try
                {
                    ViewBag.DecryptedCard = _creditCardProtector.Decrypt(order.CreditCardNumber);
                }
                catch (Exception ex)
                {
                    ViewBag.DecryptedCard = $"❌ Failed to decrypt: {ex.Message}";
                }
            }

            return View(order); // ✅ send model
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmPayment(Order inputOrder)
        {
            var order = await _context.Order
                .Include(o => o.User) // ✅ Ensure User is included
                .FirstOrDefaultAsync(o => o.Id == inputOrder.Id);

            if (order == null)
                return NotFound();

            order.Status = "Paid";
            await _context.SaveChangesAsync();

            if (order.User == null)
                return BadRequest("❌ User not found for this order.");

            string subject = $"✅ Order #{order.Id} Payment Confirmed";
            string body = $@"
        <h3>Thank you for your payment!</h3>
        <p>Your order <strong>#{order.Id}</strong> has been marked as <strong>Paid</strong>.</p>
        <p>Total Amount: <strong>{order.TotalAmount:C}</strong></p>
        <p>We will now process and deliver your photos.</p>
        <br />
        <p>Best regards,<br />MyImage Team</p>";

            SendEmail(order.User.Email, subject, body);

            return RedirectToAction("AllOrders");
        }


        public void SendEmail(string toEmail, string subject, string body)
        {
            var fromEmail = "mustafatasir0@gmail.com";
            var password = "dfhi hdnw baqm mmpr"; 

            var mail = new MailMessage();
            mail.From = new MailAddress(fromEmail, "MyImage Admin");
            mail.To.Add(toEmail);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            using (var smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.Credentials = new NetworkCredential(fromEmail, password);
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
        }
        // GET: Admins/Users
        public async Task<IActionResult> Users()
        {
            var users = await _context.User.ToListAsync();
            return View(users);
        }
        // GET: Users/Edit/5
        public async Task<IActionResult> EditUser(int? id)
        {
            if (id == null) return NotFound();

            var user = await _context.User.FindAsync(id);
            if (user == null) return NotFound();

            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(int id, User user)
        {
            if (id != user.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Users", "Admins");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.User.Any(e => e.Id == user.Id)) return NotFound();
                    else throw;
                }
            }

            return View(user);
        }
        // GET: Users/Delete/5
        public async Task<IActionResult> DeleteUser(int? id)
        {
            if (id == null) return NotFound();

            var user = await _context.User.FindAsync(id);
            if (user == null) return NotFound();

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Users", "Admins");
        }

    }
}
