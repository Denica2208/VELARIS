using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Velaris.Store.Data;
using Velaris.Store.Models;
using Velaris.Store.Services;

namespace Velaris.Store.Pages;

public class ProductModel : PageModel
{
    private readonly VelarisDbContext _context;
    private readonly CartService _cartService;
    private readonly WishlistService _wishlistService;

    public ProductModel(VelarisDbContext context, CartService cartService, WishlistService wishlistService)
    {
        _context = context;
        _cartService = cartService;
        _wishlistService = wishlistService;
    }

    public Product? Product { get; set; }
    public bool IsInWishlist { get; set; }

    [TempData]
    public string? SuccessMessage { get; set; }

    [TempData]
    public string? ErrorMessage { get; set; }

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

        IsInWishlist = await _wishlistService.IsInWishlistAsync(Product.Id);

        return Page();
    }

    public async Task<IActionResult> OnPostAddToCartAsync(int productId, string selectedSize, int quantity, string slug)
    {
        var (success, message) = await _cartService.AddToCartAsync(productId, selectedSize, Math.Max(1, quantity));
        if (success)
        {
            SuccessMessage = message;
        }
        else
        {
            ErrorMessage = message;
        }

        return RedirectToPage(new { slug });
    }

    public async Task<IActionResult> OnPostToggleWishlistAsync(int productId, string slug)
    {
        var (inWishlist, message) = await _wishlistService.ToggleWishlistAsync(productId);
        SuccessMessage = message;
        return RedirectToPage(new { slug });
    }
}
