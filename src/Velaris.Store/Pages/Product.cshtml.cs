using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Velaris.Store.Data;
using Velaris.Store.Models;

namespace Velaris.Store.Pages;

public class ProductModel : PageModel
{
    private readonly VelarisDbContext _context;

    public ProductModel(VelarisDbContext context)
    {
        _context = context;
    }

    public Product? Product { get; set; }

    public async Task<IActionResult> OnGetAsync(string slug)
    {
        if (string.IsNullOrEmpty(slug))
        {
            return RedirectToPage("/Index");
        }

        Product = await _context.Products
            .Include(p => p.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Slug == slug);

        if (Product == null)
        {
            return NotFound();
        }

        return Page();
    }
}
