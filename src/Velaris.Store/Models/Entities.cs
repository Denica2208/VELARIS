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
