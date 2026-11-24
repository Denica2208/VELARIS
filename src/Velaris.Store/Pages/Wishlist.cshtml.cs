using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Velaris.Store.Models;
using Velaris.Store.Services;

namespace Velaris.Store.Pages;

public class WishlistModel : PageModel
{
    private readonly WishlistService _wishlistService;

    public WishlistModel(WishlistService wishlistService)
    {
        _wishlistService = wishlistService;
    }

    public List<WishlistItem> Items { get; set; } = new();

    [TempData]
    public string? SuccessMessage { get; set; }

    [TempData]
    public string? ErrorMessage { get; set; }

    public async Task OnGetAsync()
    {
        Items = await _wishlistService.GetWishlistItemsAsync();
    }

    public async Task<IActionResult> OnPostRemoveAsync(int productId)
    {
        await _wishlistService.RemoveFromWishlistAsync(productId);
        SuccessMessage = "Produsul a fost eliminat din favorite.";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostMoveToCartAsync(int productId, string size = "M")
    {
        var (success, message) = await _wishlistService.MoveToCartAsync(productId, size);
        if (success)
        {
            SuccessMessage = message;
        }
        else
        {
            ErrorMessage = message;
        }

        return RedirectToPage();
    }
}
