using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Velaris.Store.Data;
using Velaris.Store.Models;

namespace Velaris.Store.Pages;

public class IndexModel : PageModel
{
    private readonly VelarisDbContext _context;

    public IndexModel(VelarisDbContext context)
    {
        _context = context;
    }

    public List<Product> Products { get; set; } = new();
    public List<Category> Categories { get; set; } = new();

    public string? CurrentCategory { get; set; }
    public string? SearchQuery { get; set; }
    
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; }
    public const int PageSize = 12; // Capped below the 24 items limit (A1)

    public async Task OnGetAsync(string? category, string? search, int p = 1)
    {
        CurrentCategory = category;
        SearchQuery = search;
        CurrentPage = Math.Max(1, p);

        Categories = await _context.Categories.AsNoTracking().ToListAsync();

        var query = _context.Products
            .Include(p => p.Category)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(p => p.Category != null && p.Category.Slug == category);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(term) || 
                                     p.Description.ToLower().Contains(term) ||
                                     p.SKU.ToLower().Contains(term));
        }

        var totalItems = await query.CountAsync();
        TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);

        Products = await query
            .OrderBy(p => p.Id)
            .Skip((CurrentPage - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();
    }
}
