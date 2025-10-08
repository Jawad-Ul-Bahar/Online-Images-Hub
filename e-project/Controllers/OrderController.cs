using e_project.Models;
using e_project.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Microsoft.AspNetCore.DataProtection;

namespace e_project.Controllers
{
    public class OrdersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IDataProtector _creditCardProtector;

        public OrdersController(AppDbContext context, IWebHostEnvironment env, IDataProtectionProvider provider)
        {
            _context = context;
            _env = env;
            _creditCardProtector = provider.CreateProtector("CreditCardProtector");
        }

        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new OrderViewModel
            {
                Photos = new List<PhotoPrintSelection>
        {
            new PhotoPrintSelection() // One default photo entry to show in form
        }
            };

            ViewBag.Prices = _context.PrintSizePrice.ToList(); // or however you're passing sizes
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Prices = _context.PrintSizePrice.ToList();
                return View(model);
            }

            var userEmail = HttpContext.Session.GetString("UserEmail");
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (user == null) return RedirectToAction("Login", "Users");

            // Validate credit card if selected
            if (model.PaymentMethod == "CreditCard")
            {
                if (string.IsNullOrWhiteSpace(model.CardholderName) ||
                    string.IsNullOrWhiteSpace(model.CreditCardNumber) ||
                    !model.ExpiryMonth.HasValue ||
                    !model.ExpiryYear.HasValue)
                {
                    ModelState.AddModelError("", "Please provide all credit card details.");
                    ViewBag.Prices = _context.PrintSizePrice.ToList();
                    return View(model);
                }
            }

            var order = new Order
            {
                UserId = user.Id,
                OrderDate = DateTime.Now,
                Status = "Pending",
                ShippingAddress = model.ShippingAddress,
                PaymentMethod = model.PaymentMethod,
                CardholderName = model.CardholderName,
                CreditCardNumber = model.CreditCardNumber != null ? _creditCardProtector.Protect(model.CreditCardNumber) : null,
                ExpiryMonth = model.ExpiryMonth,
                ExpiryYear = model.ExpiryYear,
                PhotoOrderItems = new List<PhotoOrderItem>()
            };

            decimal totalAmount = 0;

            // Save order first to generate Order.Id
            _context.Order.Add(order);
            await _context.SaveChangesAsync();

            // Create folder: wwwroot/uploads/orders/Order_X/
            var orderFolderName = "Order_" + order.Id;
            var orderFolderPath = Path.Combine(_env.WebRootPath, "uploads", "orders", orderFolderName);
            Directory.CreateDirectory(orderFolderPath);

            foreach (var item in model.Photos)
            {
                if (item.PhotoFile != null && item.PhotoFile.ContentType == "image/jpeg")
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(item.PhotoFile.FileName);
                    var filePath = Path.Combine(orderFolderPath, fileName);
                    using var stream = new FileStream(filePath, FileMode.Create);
                    await item.PhotoFile.CopyToAsync(stream);

                    var price = await _context.PrintSizePrice.FirstOrDefaultAsync(p => p.Size == item.PrintSize);
                    if (price == null)
                    {
                        ModelState.AddModelError("", $"No price found for size: {item.PrintSize}");
                        ViewData["Prices"] = _context.PrintSizePrice.ToList();
                        return View(model);
                    }

                    decimal itemTotal = price.Price * item.Quantity;
                    totalAmount += itemTotal;

                    order.PhotoOrderItems.Add(new PhotoOrderItem
                    {
                        PhotoFilePath = $"/uploads/orders/{orderFolderName}/{fileName}",
                        PrintSize = item.PrintSize,
                        Quantity = item.Quantity,
                        TotalPrice = itemTotal
                    });
                }
            }

            order.TotalAmount = totalAmount;
            _context.Update(order); // Because we added PhotoOrderItems after saving
            await _context.SaveChangesAsync();

            TempData["Success"] = "Order placed successfully!";
            return RedirectToAction("MyOrders");
        }



        public async Task<IActionResult> MyOrders()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email)) return RedirectToAction("Login", "Users");

            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return NotFound();

            var orders = await _context.Order
                .Include(o => o.PhotoOrderItems)
                .Where(o => o.UserId == user.Id)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }

    }
}
