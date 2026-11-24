using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Velaris.Store.Models;
using Velaris.Store.Services;

namespace Velaris.Store.Pages;

public class CartModel : PageModel
{
    private readonly CartService _cartService;

    public CartModel(CartService cartService)
    {
        _cartService = cartService;
    }

    public List<CartItem> Items { get; set; } = new();

    [TempData]
    public string? SuccessMessage { get; set; }

    [TempData]
    public string? ErrorMessage { get; set; }

    public decimal Subtotal => Items.Sum(i => (i.Product?.Price ?? 0m) * i.Quantity);
    public decimal EstimatedShipping => (Subtotal >= 300m || Items.Count == 0) ? 0.00m : 15.00m;
    public decimal Total => Subtotal + EstimatedShipping;

    public async Task OnGetAsync()
    {
        Items = await _cartService.GetCartItemsAsync();
    }

    public async Task<IActionResult> OnPostUpdateQuantityAsync(int cartItemId, int quantity)
    {
        var (success, message) = await _cartService.UpdateQuantityAsync(cartItemId, quantity);
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

    public async Task<IActionResult> OnPostRemoveAsync(int cartItemId)
    {
        await _cartService.RemoveItemAsync(cartItemId);
        SuccessMessage = "Articolul a fost eliminat din coș.";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostClearAsync()
    {
        await _cartService.ClearCartAsync();
        SuccessMessage = "Coșul tău a fost golit.";
        return RedirectToPage();
    }
}
