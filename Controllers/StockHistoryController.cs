using Microsoft.AspNetCore.Mvc;
using InventoryManagementSystem.Models; // Adjust based on your namespace
using Microsoft.EntityFrameworkCore;
using InventoryManagementSystem.Data;

public class StockHistoryController : Controller
{
    private readonly InventoryDbContext _context;

    public StockHistoryController(InventoryDbContext context)
    {
        _context = context;
    }

    // GET: StockHistory
    public async Task<IActionResult> Index()
    {
        var stockHistories = await _context.StockHistories.Include(sh => sh.Product).ToListAsync();
        return View(stockHistories);
    }
}
