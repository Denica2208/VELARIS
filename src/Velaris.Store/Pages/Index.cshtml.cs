using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Velaris.Store.Data;
using Velaris.Store.Models;
using Velaris.Store.Services;

namespace Velaris.Store.Pages;

public class IndexModel : PageModel
{
    private readonly VelarisDbContext _context;
    private readonly WishlistService _wishlistService;
    private readonly CartService _cartService;

    public IndexModel(VelarisDbContext context, WishlistService wishlistService, CartService cartService)
    {
        _context = context;
        _wishlistService = wishlistService;
        _cartService = cartService;
    }

    public List<Product> Products { get; set; } = new();
    public List<Category> Categories { get; set; } = new();
    public HashSet<int> WishlistProductIds { get; set; } = new();

    public string? CurrentCategory { get; set; }
    public string? SearchQuery { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public bool? InStock { get; set; }
    public string? Sort { get; set; }

    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; }
    public int TotalItems { get; set; }
    public const int PageSize = 12;

    [TempData]
    public string? SuccessMessage { get; set; }

    [TempData]
    public string? ErrorMessage { get; set; }

    public async Task OnGetAsync(
        string? category, 
        string? search, 
        decimal? minPrice, 
        decimal? maxPrice, 
        bool? inStock, 
        string? sort, 
        int? p = null, 
        int? page = null)
    {
        CurrentCategory = category;
        SearchQuery = search;
        MinPrice = minPrice;
        MaxPrice = maxPrice;
        InStock = inStock;
        Sort = sort;

        int requestedPage = p ?? page ?? 1;
        CurrentPage = Math.Max(1, requestedPage);

        Categories = await _context.Categories.AsNoTracking().ToListAsync();

        var query = _context.Products
            .Include(p => p.Category)
            .AsNoTracking()
            .AsQueryable();

        // 1. Category Filter
        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(p => p.Category != null && p.Category.Slug == category);
        }

        // 2. Keyword Search
        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(term) || 
                                     p.Description.ToLower().Contains(term) ||
                                     p.SKU.ToLower().Contains(term));
        }

        // 3. Price Range Filters (B1)
        if (minPrice.HasValue && minPrice.Value > 0)
        {
            query = query.Where(p => p.Price >= minPrice.Value);
        }
        if (maxPrice.HasValue && maxPrice.Value > 0)
        {
            query = query.Where(p => p.Price <= maxPrice.Value);
        }

        // 4. In Stock Filter (B1)
        if (inStock == true)
        {
            query = query.Where(p => p.StockLevel > 0);
        }

        // 5. Faceted Sorting (B1)
        query = sort switch
        {
            "price-asc" => query.OrderBy(p => p.Price),
            "price-desc" => query.OrderByDescending(p => p.Price),
            "newest" => query.OrderByDescending(p => p.Id),
            _ => query.OrderBy(p => p.Id)
        };

        TotalItems = await query.CountAsync();
        TotalPages = (int)Math.Ceiling(TotalItems / (double)PageSize);

        if (TotalPages > 0 && CurrentPage > TotalPages)
        {
            CurrentPage = TotalPages;
        }

        Products = await query
            .Skip((CurrentPage - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();

        // Load Wishlist IDs for quick favorite indicator
        var wishlistItems = await _wishlistService.GetWishlistItemsAsync();
        WishlistProductIds = wishlistItems.Select(w => w.ProductId).ToHashSet();
    }

    public async Task<IActionResult> OnPostToggleWishlistAsync(int productId, string returnUrl)
    {
        var (inWishlist, message) = await _wishlistService.ToggleWishlistAsync(productId);
        SuccessMessage = message;

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return LocalRedirect(returnUrl);
        }
        return RedirectToPage();
    }

    public string GetPageUrl(int pageNumber)
    {
        var queryParams = new List<string> { $"p={pageNumber}" };
        if (!string.IsNullOrWhiteSpace(CurrentCategory)) queryParams.Add($"category={Uri.EscapeDataString(CurrentCategory)}");
        if (!string.IsNullOrWhiteSpace(SearchQuery)) queryParams.Add($"search={Uri.EscapeDataString(SearchQuery)}");
        if (MinPrice.HasValue) queryParams.Add($"minPrice={MinPrice.Value}");
        if (MaxPrice.HasValue) queryParams.Add($"maxPrice={MaxPrice.Value}");
        if (InStock == true) queryParams.Add("inStock=true");
        if (!string.IsNullOrWhiteSpace(Sort)) queryParams.Add($"sort={Uri.EscapeDataString(Sort)}");
        return "/?" + string.Join("&", queryParams);
    }
}
