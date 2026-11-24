using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Velaris.Store.Models;

public class Category
{
    public int Id { get; set; }
    
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required, MaxLength(100)]
    public string Slug { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    public List<Product> Products { get; set; } = new();
}

public class Product
{
    public int Id { get; set; }
    
    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    [Required, MaxLength(32)]
    public string SKU { get; set; } = string.Empty;

    [Required, MaxLength(128)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(128)]
    public string Slug { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    public int StockLevel { get; set; }

    public int FabricGsm { get; set; }

    public string Material { get; set; } = "100% GOTS Organic Cotton";

    public string ImageUrl { get; set; } = string.Empty;

    [Timestamp]
    public byte[]? RowVersion { get; set; }
}

public class User
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string Role { get; set; } = "Customer";

    public string ShippingAddress { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class CartItem
{
    public int Id { get; set; }

    public string? UserId { get; set; }
    public User? User { get; set; }

    [MaxLength(128)]
    public string? SessionId { get; set; }

    public int ProductId { get; set; }
    public Product? Product { get; set; }

    public int Quantity { get; set; } = 1;

    [MaxLength(16)]
    public string SelectedSize { get; set; } = "M";

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public class WishlistItem
{
    public int Id { get; set; }

    public string? UserId { get; set; }
    public User? User { get; set; }

    [MaxLength(128)]
    public string? SessionId { get; set; }

    public int ProductId { get; set; }
    public Product? Product { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class Order
{
    public int Id { get; set; }

    [Required, MaxLength(32)]
    public string OrderNumber { get; set; } = string.Empty;

    public string? UserId { get; set; }
    public User? User { get; set; }

    [Required, EmailAddress, MaxLength(128)]
    public string CustomerEmail { get; set; } = string.Empty;

    [Required, MaxLength(128)]
    public string CustomerName { get; set; } = string.Empty;

    [Required, MaxLength(256)]
    public string ShippingAddress { get; set; } = string.Empty;

    [Required, MaxLength(64)]
    public string City { get; set; } = string.Empty;

    [Required, MaxLength(32)]
    public string PostalCode { get; set; } = string.Empty;

    [Required, MaxLength(64)]
    public string DeliveryMethod { get; set; } = "Fan Courier Standard";

    [Column(TypeName = "decimal(10,2)")]
    public decimal Subtotal { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal ShippingCost { get; set; } = 15.00m;

    [Column(TypeName = "decimal(10,2)")]
    public decimal DiscountAmount { get; set; } = 0.00m;

    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalAmount { get; set; }

    [Required, MaxLength(32)]
    public string Status { get; set; } = "New"; // New, Paid, Shipped, Delivered, Cancelled

    [MaxLength(128)]
    public string? StripeSessionId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<OrderItem> Items { get; set; } = new();
}

public class OrderItem
{
    public int Id { get; set; }

    public int OrderId { get; set; }
    public Order? Order { get; set; }

    public int ProductId { get; set; }
    public Product? Product { get; set; }

    [Required, MaxLength(32)]
    public string SKU { get; set; } = string.Empty;

    [Required, MaxLength(128)]
    public string ProductName { get; set; } = string.Empty;

    [MaxLength(16)]
    public string SelectedSize { get; set; } = "M";

    [Column(TypeName = "decimal(10,2)")]
    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; } = 1;
}
