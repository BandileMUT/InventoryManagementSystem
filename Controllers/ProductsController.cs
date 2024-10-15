using Microsoft.AspNetCore.Mvc;
using InventoryManagementSystem.Models; // Adjust based on your namespace
using Microsoft.EntityFrameworkCore;
using InventoryManagementSystem.Data;

namespace InventoryManagementSystem.Controllers
{
    public class ProductsController : Controller
    {
        private readonly InventoryDbContext _context;

        public ProductsController(InventoryDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(string searchString)
        {
            var products = from p in _context.Products
                           select p;

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.ProductName.Contains(searchString));
            }

            return View(await products.ToListAsync());
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,Description,Supplier,Quantity,UnitPrice,ExpiryDate,RestockDate")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingProduct = await _context.Products.FindAsync(id);
                if (existingProduct != null)
                {
                    // Track stock change
                    int quantityChanged = product.Quantity - existingProduct.Quantity;
                    if (quantityChanged != 0)
                    {
                        // Create a new StockHistory entry
                        var stockHistory = new StockHistory
                        {
                            ProductId = product.ProductId,
                            QuantityChanged = quantityChanged,
                            ChangeDate = DateTime.Now,
                            Note = quantityChanged > 0 ? "Restocked" : "Sold"
                        };

                        _context.StockHistories.Add(stockHistory);
                    }

                    // Update product details
                    _context.Entry(existingProduct).CurrentValues.SetValues(product);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }


        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product); // Return the Details view with the product model
        }

        public IActionResult Dashboard()
        {
            // Get all products
            var products = _context.Products.ToList();

            // Calculate total inventory value
            decimal totalInventoryValue = products.Sum(p => p.Quantity * p.UnitPrice);

            // Find low-stock products (for example, if quantity is less than 5)
            var lowStockProducts = products.Where(p => p.Quantity < 5).ToList();

            // Pass data to the view
            ViewBag.TotalInventoryValue = totalInventoryValue;
            ViewBag.LowStockProducts = lowStockProducts.Count;
            ViewBag.ProductNames = products.Select(p => p.ProductName).ToArray();
            ViewBag.ProductQuantities = products.Select(p => p.Quantity).ToArray();

            return View(products);
        }

        public JsonResult GetExpiringSoonItems()
        {
            DateTime upcomingDate = DateTime.Now.AddDays(7); // Notify for products expiring in 7 days
            var expiringItems = _context.Products
                .Where(p => p.ExpiryDate.HasValue && p.ExpiryDate <= upcomingDate)
                .Select(p => new
                {
                    ProductName = p.ProductName,
                    ExpiryDate = p.ExpiryDate
                })
                .ToList();

            return Json(expiringItems);
        }



        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
