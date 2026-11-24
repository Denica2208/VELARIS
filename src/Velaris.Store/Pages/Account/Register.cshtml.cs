using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Velaris.Store.Data;
using Velaris.Store.Models;
using Velaris.Store.Services;

namespace Velaris.Store.Pages.Account;

public class RegisterModel : PageModel
{
    private readonly VelarisDbContext _context;
    private readonly CartService _cartService;
    private readonly WishlistService _wishlistService;

    public RegisterModel(VelarisDbContext context, CartService cartService, WishlistService wishlistService)
    {
        _context = context;
        _cartService = cartService;
        _wishlistService = wishlistService;
    }

    [BindProperty]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    public string FullName { get; set; } = string.Empty;

    [BindProperty]
    public string Password { get; set; } = string.Empty;

    [BindProperty]
    public string ShippingAddress { get; set; } = string.Empty;

    public string? ErrorMessage { get; set; }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Email and password are required.";
            return Page();
        }

        var exists = await _context.Users.AnyAsync(u => u.Email.ToLower() == Email.Trim().ToLower());
        if (exists)
        {
            ErrorMessage = "An account with this email address already exists.";
            return Page();
        }

        var guestSessionId = _cartService.GetOrCreateSessionId();
        var hash = Convert.ToBase64String(System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(Password)));

        var user = new User
        {
            Email = Email.Trim().ToLower(),
            FullName = FullName.Trim(),
            PasswordHash = hash,
            Role = "Customer",
            ShippingAddress = ShippingAddress.Trim()
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Role, user.Role)
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

        // Merge guest cart and wishlist items into new user account (A3 & B7)
        await _cartService.MergeGuestCartToUserAsync(guestSessionId, user.Id);
        await _wishlistService.MergeGuestWishlistToUserAsync(guestSessionId, user.Id);

        return RedirectToPage("/Index");
    }
}
