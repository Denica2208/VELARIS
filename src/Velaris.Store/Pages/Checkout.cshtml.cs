using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Velaris.Store.Data;
using Velaris.Store.Models;
using Velaris.Store.Services;

namespace Velaris.Store.Pages;

public class CheckoutModel : PageModel
{
    private readonly VelarisDbContext _context;
    private readonly CartService _cartService;

    public CheckoutModel(VelarisDbContext context, CartService cartService)
    {
        _context = context;
        _cartService = cartService;
    }

    [BindProperty]
    [Required(ErrorMessage = "Numele complet este obligatoriu.")]
    [Display(Name = "Nume și Prenume")]
    public string CustomerName { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Adresa de email este obligatorie.")]
    [EmailAddress(ErrorMessage = "Adresă de email invalidă.")]
    [Display(Name = "Adresă Email")]
    public string CustomerEmail { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Adresa de livrare este obligatorie.")]
    [Display(Name = "Adresă de Livrare")]
    public string ShippingAddress { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Orașul este obligatoriu.")]
    [Display(Name = "Oraș")]
    public string City { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Codul poștal este obligatoriu.")]
    [Display(Name = "Cod Poștal")]
    public string PostalCode { get; set; } = string.Empty;

    [BindProperty]
    [Required]
    [Display(Name = "Metodă de Livrare")]
    public string DeliveryMethod { get; set; } = "Fan Courier Standard";

    public List<CartItem> CartItems { get; set; } = new();

    public decimal Subtotal => CartItems.Sum(i => (i.Product?.Price ?? 0m) * i.Quantity);
    public decimal ShippingCost => DeliveryMethod == "DPD Express Priority" ? 30.00m : (Subtotal >= 300m ? 0.00m : 15.00m);
    public decimal TotalAmount => Subtotal + ShippingCost;

    [TempData]
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        CartItems = await _cartService.GetCartItemsAsync();
        if (!CartItems.Any())
        {
            return RedirectToPage("/Cart");
        }

        // Pre-fill user data if authenticated
        var userId = _cartService.GetCurrentUserId();
        if (!string.IsNullOrEmpty(userId))
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                CustomerName = user.FullName;
                CustomerEmail = user.Email;
                ShippingAddress = user.ShippingAddress;
                City = "București";
                PostalCode = "010101";
            }
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        CartItems = await _cartService.GetCartItemsAsync();
        if (!CartItems.Any())
        {
            return RedirectToPage("/Cart");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Check stock availability for all cart items
        foreach (var item in CartItems)
        {
            if (item.Product == null || item.Quantity > item.Product.StockLevel)
            {
                ErrorMessage = $"Stoc insuficient pentru «{item.Product?.Name ?? "Produs"}». Te rugăm să ajustezi coșul.";
                return Page();
            }
        }

        // Calculate delivery fee
        decimal shippingFee = DeliveryMethod == "DPD Express Priority" 
            ? 30.00m 
            : (Subtotal >= 300m ? 0.00m : 15.00m);

        decimal finalTotal = Subtotal + shippingFee;

        // Generate unique order number (VEL-2026-XXXX)
        string orderNumber;
        do
        {
            orderNumber = $"VEL-2026-{Random.Shared.Next(1000, 9999)}";
        } while (await _context.Orders.AnyAsync(o => o.OrderNumber == orderNumber));

        var userId = _cartService.GetCurrentUserId();

        var order = new Order
        {
            OrderNumber = orderNumber,
            UserId = userId,
            CustomerEmail = CustomerEmail.Trim(),
            CustomerName = CustomerName.Trim(),
            ShippingAddress = ShippingAddress.Trim(),
            City = City.Trim(),
            PostalCode = PostalCode.Trim(),
            DeliveryMethod = DeliveryMethod,
            Subtotal = Subtotal,
            ShippingCost = shippingFee,
            DiscountAmount = 0.00m,
            TotalAmount = finalTotal,
            Status = "New", // Initial state ready for payment/shipping
            CreatedAt = DateTime.UtcNow
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        // Create OrderItems
        foreach (var item in CartItems)
        {
            var orderItem = new OrderItem
            {
                OrderId = order.Id,
                ProductId = item.ProductId,
                ProductName = item.Product?.Name ?? "Garment",
                SKU = item.Product?.SKU ?? "SKU",
                SelectedSize = item.SelectedSize,
                UnitPrice = item.Product?.Price ?? 0m,
                Quantity = item.Quantity
            };
            _context.OrderItems.Add(orderItem);
        }

        await _context.SaveChangesAsync();

        // Clear cart items
        await _cartService.ClearCartAsync();

        return RedirectToPage("/Confirmation", new { orderNumber });
    }
}
