using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Velaris.Store.Data;
using Velaris.Store.Models;

namespace Velaris.Store.Services;

public class CartService
{
    private readonly VelarisDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public const string SessionCookieName = "Velaris.CartSession";

    public CartService(VelarisDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetOrCreateSessionId()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null) return Guid.NewGuid().ToString("N");

        if (httpContext.Request.Cookies.TryGetValue(SessionCookieName, out var sessionId) && !string.IsNullOrWhiteSpace(sessionId))
        {
            return sessionId;
        }

        var newSessionId = Guid.NewGuid().ToString("N");
        httpContext.Response.Cookies.Append(SessionCookieName, newSessionId, new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddDays(30)
        });
        return newSessionId;
    }

    public string? GetCurrentUserId()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        return httpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    public async Task<List<CartItem>> GetCartItemsAsync()
    {
        var userId = GetCurrentUserId();
        var sessionId = GetOrCreateSessionId();

        var query = _context.CartItems
            .Include(c => c.Product)
                .ThenInclude(p => p!.Category)
            .AsQueryable();

        if (!string.IsNullOrEmpty(userId))
        {
            query = query.Where(c => c.UserId == userId);
        }
        else
        {
            query = query.Where(c => c.SessionId == sessionId);
        }

        return await query.OrderByDescending(c => c.UpdatedAt).ToListAsync();
    }

    public async Task<int> GetCartItemCountAsync()
    {
        var userId = GetCurrentUserId();
        var sessionId = GetOrCreateSessionId();

        var query = _context.CartItems.AsQueryable();
        if (!string.IsNullOrEmpty(userId))
        {
            query = query.Where(c => c.UserId == userId);
        }
        else
        {
            query = query.Where(c => c.SessionId == sessionId);
        }

        return await query.SumAsync(c => c.Quantity);
    }

    public async Task<(bool success, string message)> AddToCartAsync(int productId, string size, int quantity = 1)
    {
        if (quantity <= 0)
        {
            return (false, "Cantitatea trebuie să fie de cel puțin 1.");
        }

        var product = await _context.Products.FindAsync(productId);
        if (product == null)
        {
            return (false, "Produsul selectat nu a fost găsit.");
        }

        if (product.StockLevel <= 0)
        {
            return (false, $"Ne pare rău, «{product.Name}» este momentan epuizat din stoc.");
        }

        var normalizedSize = string.IsNullOrWhiteSpace(size) ? "M" : size.Trim().ToUpperInvariant();
        var userId = GetCurrentUserId();
        var sessionId = GetOrCreateSessionId();

        CartItem? existingItem;
        if (!string.IsNullOrEmpty(userId))
        {
            existingItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId && c.SelectedSize == normalizedSize);
        }
        else
        {
            existingItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.SessionId == sessionId && c.ProductId == productId && c.SelectedSize == normalizedSize);
        }

        int currentQty = existingItem?.Quantity ?? 0;
        int requestedTotal = currentQty + quantity;

        if (requestedTotal > product.StockLevel)
        {
            int canAdd = Math.Max(0, product.StockLevel - currentQty);
            return (false, $"Stoc insuficient pentru «{product.Name}» (Mărimea {normalizedSize})! Stoc total disponibil: {product.StockLevel} bucăți. (Ai deja {currentQty} în coș; mai poți adăuga cel mult {canAdd}).");
        }

        if (existingItem != null)
        {
            existingItem.Quantity = requestedTotal;
            existingItem.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            var newItem = new CartItem
            {
                UserId = userId,
                SessionId = string.IsNullOrEmpty(userId) ? sessionId : null,
                ProductId = productId,
                SelectedSize = normalizedSize,
                Quantity = quantity,
                UpdatedAt = DateTime.UtcNow
            };
            _context.CartItems.Add(newItem);
        }

        await _context.SaveChangesAsync();
        return (true, $"«{product.Name}» (Mărimea {normalizedSize}) a fost adăugat în coșul tău capsule!");
    }

    public async Task<(bool success, string message)> UpdateQuantityAsync(int cartItemId, int newQuantity)
    {
        var userId = GetCurrentUserId();
        var sessionId = GetOrCreateSessionId();

        var item = await _context.CartItems
            .Include(c => c.Product)
            .FirstOrDefaultAsync(c => c.Id == cartItemId && 
                ((!string.IsNullOrEmpty(userId) && c.UserId == userId) || 
                 (string.IsNullOrEmpty(userId) && c.SessionId == sessionId)));

        if (item == null)
        {
            return (false, "Articolul nu a fost găsit în coș.");
        }

        if (newQuantity <= 0)
        {
            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
            return (true, "Articolul a fost eliminat din coș.");
        }

        if (item.Product != null && newQuantity > item.Product.StockLevel)
        {
            return (false, $"Cantitatea solicitată ({newQuantity}) depășește stocul disponibil ({item.Product.StockLevel} buc.) pentru «{item.Product.Name}».");
        }

        item.Quantity = newQuantity;
        item.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return (true, "Cantitate actualizată cu succes.");
    }

    public async Task<bool> RemoveItemAsync(int cartItemId)
    {
        var userId = GetCurrentUserId();
        var sessionId = GetOrCreateSessionId();

        var item = await _context.CartItems
            .FirstOrDefaultAsync(c => c.Id == cartItemId && 
                ((!string.IsNullOrEmpty(userId) && c.UserId == userId) || 
                 (string.IsNullOrEmpty(userId) && c.SessionId == sessionId)));

        if (item == null) return false;

        _context.CartItems.Remove(item);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task ClearCartAsync()
    {
        var userId = GetCurrentUserId();
        var sessionId = GetOrCreateSessionId();

        var query = _context.CartItems.AsQueryable();
        if (!string.IsNullOrEmpty(userId))
        {
            query = query.Where(c => c.UserId == userId);
        }
        else
        {
            query = query.Where(c => c.SessionId == sessionId);
        }

        _context.CartItems.RemoveRange(query);
        await _context.SaveChangesAsync();
    }

    public async Task MergeGuestCartToUserAsync(string sessionId, string userId)
    {
        if (string.IsNullOrWhiteSpace(sessionId) || string.IsNullOrWhiteSpace(userId)) return;

        var guestItems = await _context.CartItems
            .Where(c => c.SessionId == sessionId)
            .Include(c => c.Product)
            .ToListAsync();

        if (!guestItems.Any()) return;

        var userItems = await _context.CartItems
            .Where(c => c.UserId == userId)
            .ToListAsync();

        foreach (var guestItem in guestItems)
        {
            var match = userItems.FirstOrDefault(u => u.ProductId == guestItem.ProductId && u.SelectedSize == guestItem.SelectedSize);
            if (match != null)
            {
                int maxStock = guestItem.Product?.StockLevel ?? 999;
                match.Quantity = Math.Min(match.Quantity + guestItem.Quantity, maxStock);
                match.UpdatedAt = DateTime.UtcNow;
                _context.CartItems.Remove(guestItem);
            }
            else
            {
                guestItem.UserId = userId;
                guestItem.SessionId = null;
                guestItem.UpdatedAt = DateTime.UtcNow;
            }
        }

        await _context.SaveChangesAsync();
    }
}
