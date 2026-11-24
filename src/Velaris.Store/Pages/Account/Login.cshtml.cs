using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Velaris.Store.Data;
using Velaris.Store.Services;

namespace Velaris.Store.Pages.Account;

public class LoginModel : PageModel
{
    private readonly VelarisDbContext _context;
    private readonly CartService _cartService;
    private readonly WishlistService _wishlistService;

    public LoginModel(VelarisDbContext context, CartService cartService, WishlistService wishlistService)
    {
        _context = context;
        _cartService = cartService;
        _wishlistService = wishlistService;
    }

    [BindProperty]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    public string Password { get; set; } = string.Empty;

    public string? ErrorMessage { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Please provide both email and password.";
            return Page();
        }

        var hash = Convert.ToBase64String(System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(Password)));
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == Email.Trim().ToLower() && u.PasswordHash == hash);

        if (user == null)
        {
            ErrorMessage = "Invalid credentials. Use customer@velaris.com or admin@velaris.com (password: CustomerPass123! or AdminPass123!).";
            return Page();
        }

        var guestSessionId = _cartService.GetOrCreateSessionId();

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Role, user.Role)
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

        // Merge guest cart and wishlist items into user account (A3 & B7)
        await _cartService.MergeGuestCartToUserAsync(guestSessionId, user.Id);
        await _wishlistService.MergeGuestWishlistToUserAsync(guestSessionId, user.Id);

        return RedirectToPage("/Index");
    }
}
