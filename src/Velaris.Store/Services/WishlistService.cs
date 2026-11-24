using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Velaris.Store.Data;
using Velaris.Store.Models;

namespace Velaris.Store.Services;

public class WishlistService
{
    private readonly VelarisDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly CartService _cartService;

    public WishlistService(VelarisDbContext context, IHttpContextAccessor httpContextAccessor, CartService cartService)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _cartService = cartService;
    }

    private string GetOrCreateSessionId() => _cartService.GetOrCreateSessionId();

    private string? GetCurrentUserId()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        return httpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    public async Task<List<WishlistItem>> GetWishlistItemsAsync()
    {
        var userId = GetCurrentUserId();
        var sessionId = GetOrCreateSessionId();

        var query = _context.WishlistItems
            .Include(w => w.Product)
                .ThenInclude(p => p!.Category)
            .AsQueryable();

        if (!string.IsNullOrEmpty(userId))
        {
            query = query.Where(w => w.UserId == userId);
        }
        else
        {
            query = query.Where(w => w.SessionId == sessionId);
        }

        return await query.OrderByDescending(w => w.CreatedAt).ToListAsync();
    }

    public async Task<int> GetWishlistCountAsync()
    {
        var userId = GetCurrentUserId();
        var sessionId = GetOrCreateSessionId();

        var query = _context.WishlistItems.AsQueryable();
        if (!string.IsNullOrEmpty(userId))
        {
            query = query.Where(w => w.UserId == userId);
        }
        else
        {
            query = query.Where(w => w.SessionId == sessionId);
        }

        return await query.CountAsync();
    }

    public async Task<bool> IsInWishlistAsync(int productId)
    {
        var userId = GetCurrentUserId();
        var sessionId = GetOrCreateSessionId();

        if (!string.IsNullOrEmpty(userId))
        {
            return await _context.WishlistItems.AnyAsync(w => w.UserId == userId && w.ProductId == productId);
        }

        return await _context.WishlistItems.AnyAsync(w => w.SessionId == sessionId && w.ProductId == productId);
    }

    public async Task<(bool isInWishlist, string message)> ToggleWishlistAsync(int productId)
    {
        var product = await _context.Products.FindAsync(productId);
        if (product == null)
        {
            return (false, "Produsul nu a fost găsit.");
        }

        var userId = GetCurrentUserId();
        var sessionId = GetOrCreateSessionId();

        WishlistItem? existing;
        if (!string.IsNullOrEmpty(userId))
        {
            existing = await _context.WishlistItems.FirstOrDefaultAsync(w => w.UserId == userId && w.ProductId == productId);
        }
        else
        {
            existing = await _context.WishlistItems.FirstOrDefaultAsync(w => w.SessionId == sessionId && w.ProductId == productId);
        }

        if (existing != null)
        {
            _context.WishlistItems.Remove(existing);
            await _context.SaveChangesAsync();
            return (false, $"«{product.Name}» a fost eliminat din favorite.");
        }
        else
        {
            var item = new WishlistItem
            {
                ProductId = productId,
                UserId = userId,
                SessionId = string.IsNullOrEmpty(userId) ? sessionId : null,
                CreatedAt = DateTime.UtcNow
            };
            _context.WishlistItems.Add(item);
            await _context.SaveChangesAsync();
            return (true, $"«{product.Name}» a fost adăugat la favorite!");
        }
    }

    public async Task<bool> RemoveFromWishlistAsync(int productId)
    {
        var userId = GetCurrentUserId();
        var sessionId = GetOrCreateSessionId();

        var item = await _context.WishlistItems
            .FirstOrDefaultAsync(w => w.ProductId == productId &&
                ((!string.IsNullOrEmpty(userId) && w.UserId == userId) ||
                 (string.IsNullOrEmpty(userId) && w.SessionId == sessionId)));

        if (item == null) return false;

        _context.WishlistItems.Remove(item);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<(bool success, string message)> MoveToCartAsync(int productId, string size = "M")
    {
        var (added, cartMsg) = await _cartService.AddToCartAsync(productId, size, 1);
        if (added)
        {
            await RemoveFromWishlistAsync(productId);
            return (true, cartMsg);
        }
        return (false, cartMsg);
    }

    public async Task MergeGuestWishlistToUserAsync(string sessionId, string userId)
    {
        if (string.IsNullOrWhiteSpace(sessionId) || string.IsNullOrWhiteSpace(userId)) return;

        var guestItems = await _context.WishlistItems
            .Where(w => w.SessionId == sessionId)
            .ToListAsync();

        if (!guestItems.Any()) return;

        var userProductIds = await _context.WishlistItems
            .Where(w => w.UserId == userId)
            .Select(w => w.ProductId)
            .ToListAsync();

        foreach (var guestItem in guestItems)
        {
            if (userProductIds.Contains(guestItem.ProductId))
            {
                _context.WishlistItems.Remove(guestItem);
            }
            else
            {
                guestItem.UserId = userId;
                guestItem.SessionId = null;
            }
        }

        await _context.SaveChangesAsync();
    }
}
