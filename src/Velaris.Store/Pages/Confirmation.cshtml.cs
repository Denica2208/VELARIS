using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Velaris.Store.Data;
using Velaris.Store.Models;

namespace Velaris.Store.Pages;

public class ConfirmationModel : PageModel
{
    private readonly VelarisDbContext _context;

    public ConfirmationModel(VelarisDbContext context)
    {
        _context = context;
    }

    public Order? Order { get; set; }

    public async Task<IActionResult> OnGetAsync(string orderNumber)
    {
        if (string.IsNullOrWhiteSpace(orderNumber))
        {
            return RedirectToPage("/Index");
        }

        Order = await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);

        if (Order == null)
        {
            return RedirectToPage("/Index");
        }

        return Page();
    }
}
