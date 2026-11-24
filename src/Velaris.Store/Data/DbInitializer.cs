using Microsoft.EntityFrameworkCore;
using Velaris.Store.Models;

namespace Velaris.Store.Data;

public static class DbInitializer
{
    private record CategorySeed(string Slug, string Name, string Description);
    private record ProductSeed(string CategorySlug, string SKU, string Name, string Slug, decimal Price, int StockLevel, int FabricGsm, string Material, string Description, string ImageUrl);

    public static void Initialize(VelarisDbContext context)
    {
        context.Database.EnsureCreated();

        // Ensure newly introduced P3 tables exist in SQLite database
        context.Database.ExecuteSqlRaw(@"
            CREATE TABLE IF NOT EXISTS CartItems (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                UserId TEXT NULL,
                SessionId TEXT NULL,
                ProductId INTEGER NOT NULL,
                Quantity INTEGER NOT NULL DEFAULT 1,
                SelectedSize TEXT NOT NULL DEFAULT 'M',
                UpdatedAt TEXT NOT NULL,
                FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE CASCADE,
                FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
            );

            CREATE TABLE IF NOT EXISTS WishlistItems (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                UserId TEXT NULL,
                SessionId TEXT NULL,
                ProductId INTEGER NOT NULL,
                CreatedAt TEXT NOT NULL,
                FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE CASCADE,
                FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
            );

            CREATE TABLE IF NOT EXISTS Orders (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                OrderNumber TEXT NOT NULL UNIQUE,
                UserId TEXT NULL,
                CustomerEmail TEXT NOT NULL,
                CustomerName TEXT NOT NULL,
                ShippingAddress TEXT NOT NULL,
                City TEXT NOT NULL,
                PostalCode TEXT NOT NULL,
                DeliveryMethod TEXT NOT NULL,
                Subtotal NUMERIC(10,2) NOT NULL,
                ShippingCost NUMERIC(10,2) NOT NULL,
                DiscountAmount NUMERIC(10,2) NOT NULL DEFAULT 0,
                TotalAmount NUMERIC(10,2) NOT NULL,
                Status TEXT NOT NULL DEFAULT 'New',
                StripeSessionId TEXT NULL,
                CreatedAt TEXT NOT NULL,
                FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE SET NULL
            );

            CREATE TABLE IF NOT EXISTS OrderItems (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                OrderId INTEGER NOT NULL,
                ProductId INTEGER NOT NULL,
                SKU TEXT NOT NULL,
                ProductName TEXT NOT NULL,
                SelectedSize TEXT NOT NULL,
                UnitPrice NUMERIC(10,2) NOT NULL,
                Quantity INTEGER NOT NULL,
                FOREIGN KEY (OrderId) REFERENCES Orders(Id) ON DELETE CASCADE,
                FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE RESTRICT
            );
        ");

        // Seed users if empty
        if (!context.Users.Any())
        {
            context.Users.AddRange(
                new User
                {
                    Email = "admin@velaris.com",
                    FullName = "Stefan Sapca (Admin)",
                    Role = "Admin",
                    PasswordHash = BCryptHash("AdminPass123!"),
                    ShippingAddress = "Str. Cuza Voda nr. 4, Craiova",
                    City = "Craiova",
                    PostalCode = "200100"
                },
                new User
                {
                    Email = "customer@velaris.com",
                    FullName = "Andreea Predut (Customer)",
                    Role = "Customer",
                    PasswordHash = BCryptHash("CustomerPass123!"),
                    ShippingAddress = "Str. Libertatii nr. 12, Craiova",
                    City = "Craiova",
                    PostalCode = "200120"
                }
            );
            context.SaveChanges();
        }

        var categorySeeds = new List<CategorySeed>
        {
            new("hoodies", "Knitwear & Cashmere", "Fine-gauge cashmere, Italian merino wool, and minimalist knitwear for women & men."),
            new("t-shirts", "Shirts, Blouses & Tops", "100% European linen shirts, crisp Egyptian cotton poplin, and refined daily essentials for women & men."),
            new("denim", "Tailored Trousers & Denim", "Pleated virgin wool trousers, flowing wide-leg silhouettes, and clean straight-leg denim for women & men."),
            new("jackets", "Coats, Blazers & Outerwear", "Double-faced wool coats, architectural tailored blazers, and timeless gabardine trenches for women & men.")
        };

        var productSeeds = new List<ProductSeed>
        {
            // Knitwear & Cashmere (4 Women + 4 Men)
            new("hoodies", "VEL-HOD-01", "Cashmere Crewneck Sweater — Ivory (Women)", "cashmere-crewneck-sweater-ivory",
                540.00m, 24, 320, "100% Grade-A Mongolian Cashmere",
                "Minimalist crewneck pullover knitted from whisper-soft pure cashmere in luminous ivory with finely ribbed collar and cuffs.",
                "/images/products/vel-hod-01.jpg"),
            new("hoodies", "VEL-HOD-02", "Merino Ribbed Turtleneck — Espresso (Women)", "merino-ribbed-turtleneck-espresso",
                360.00m, 18, 280, "100% Extra-Fine Italian Merino Wool",
                "Sleek architectural turtleneck in deep espresso brown with subtle vertical rib texture and clean tailored drape.",
                "/images/products/vel-hod-02.jpg"),
            new("hoodies", "VEL-HOD-03", "Cable-Knit Wool Button Cardigan — Warm Stone", "cable-knit-wool-button-cardigan-stone",
                420.00m, 15, 340, "90% Virgin Wool, 10% Cashmere",
                "Heritage cable-knit cardigan in warm stone beige accented with natural tortoiseshell buttons and soft brushed wool finish.",
                "/images/products/vel-hod-03.jpg"),
            new("hoodies", "VEL-HOD-04", "Men's Merino Crewneck Sweater — Heather Grey", "mens-merino-crewneck-sweater-grey",
                440.00m, 30, 340, "100% Extra-Fine Italian Merino Wool",
                "Refined men's crewneck pullover in heathered charcoal-grey merino wool with tailored shoulders and clean ribbed trims.",
                "/images/products/vel-hod-04.jpg"),
            new("hoodies", "VEL-HOD-05", "Men's Ribbed Knit Shirt — Cream Ecru", "mens-ribbed-knit-shirt-cream",
                390.00m, 22, 310, "85% Organic Cotton, 15% Cashmere",
                "Textured vertical-ribbed knitwear in soft cream ecru crafted for relaxed Italian resort and smart-casual tailoring.",
                "/images/products/vel-hod-05.jpg"),
            new("hoodies", "VEL-HOD-06", "Merino Knit Lounge Hoodie — Warm Taupe (Women)", "merino-knit-lounge-hoodie-warm-taupe",
                450.00m, 12, 330, "100% Extra-Fine Merino Wool",
                "Luxury knitted hoodie with clean no-drawstring hood and fluid drape in warm taupe — elevated quiet luxury layering.",
                "/images/products/vel-hod-06.jpg"),
            new("hoodies", "VEL-HOD-07", "Men's Textured Wool Sweater — Warm Stone", "mens-textured-wool-sweater-stone",
                410.00m, 20, 350, "80% Merino Wool, 20% Cashmere",
                "Mid-weight knitted wool sweater in refined stone grey featuring a relaxed silhouette and soft brushed finish.",
                "/images/products/vel-hod-07.jpg"),
            new("hoodies", "VEL-HOD-08", "Chunky Ribbed Merino Turtleneck — Noir (Women)", "chunky-ribbed-merino-turtleneck-noir",
                410.00m, 16, 360, "100% Extra-Fine Italian Merino Wool",
                "Cozy structured black turtleneck knitted in bold architectural ribbing with deep folded collar and wide cuffs.",
                "/images/products/vel-hod-08.jpg"),

            // Shirts, Blouses & Tops (4 Women + 4 Men)
            new("t-shirts", "VEL-TEE-01", "100% European Linen Shirt — Crisp White (Women)", "european-linen-shirt-crisp-white",
                260.00m, 45, 165, "100% Certified European Flax Linen",
                "Breezy tailored shirt in pure crisp white linen featuring mother-of-pearl buttons and softly curved editorial hem.",
                "/images/products/vel-tee-01.jpg"),
            new("t-shirts", "VEL-TEE-02", "Men's Tailored Linen Dress Shirt — Crisp White", "mens-tailored-linen-dress-shirt-white",
                290.00m, 50, 160, "100% Belgian Flax Linen",
                "Signature Massimo-style men's white tailored shirt with sharp collar line, concealed elegance, and breathable natural weave.",
                "/images/products/vel-tee-02.jpg"),
            new("t-shirts", "VEL-TEE-03", "Men's Egyptian Poplin Shirt — Powder Blue", "mens-egyptian-poplin-shirt-powder-blue",
                275.00m, 28, 135, "100% Long-Staple Egyptian Cotton Poplin",
                "Crisp architectural men's button-down shirt in refined powder blue tailored for both suiting and relaxed trousers.",
                "/images/products/vel-tee-03.jpg"),
            new("t-shirts", "VEL-TEE-04", "Men's Striped Oxford Shirt — Charcoal & White", "mens-striped-oxford-shirt-charcoal",
                265.00m, 32, 165, "100% Organic Cotton Oxford",
                "Relaxed European cut in classic fine vertical stripes designed to pair effortlessly with cream tailored trousers.",
                "/images/products/vel-tee-04.jpg"),
            new("t-shirts", "VEL-TEE-05", "Striped Cotton Poplin Shirt — Sky Blue (Women)", "striped-cotton-poplin-shirt-sky-blue",
                250.00m, 25, 145, "100% Organic Cotton Poplin",
                "Timeless blue-and-white striped button-down with relaxed cuffs and structured collar — a quintessential Zara Studio staple.",
                "/images/products/vel-tee-05.jpg"),
            new("t-shirts", "VEL-TEE-06", "Supima Cotton Essential Tee — Ivory (Women)", "supima-cotton-essential-tee-ivory",
                160.00m, 40, 210, "100% Mercerized Supima Cotton",
                "Silky smooth tailored crewneck t-shirt in warm ivory designed to sit cleanly beneath blazers and wool coats.",
                "/images/products/vel-tee-06.jpg"),
            new("t-shirts", "VEL-TEE-07", "Men's Relaxed Cotton Overshirt — Studio White", "mens-relaxed-cotton-overshirt-white",
                280.00m, 22, 185, "100% Combed Organic Cotton Twill",
                "Minimalist clean-front men's shirt in crisp studio white with understated tailoring and breathable matte finish.",
                "/images/products/vel-tee-07.jpg"),
            new("t-shirts", "VEL-TEE-08", "Mock-Neck Fine Knit Top — Noir (Women)", "mock-neck-fine-knit-top-noir",
                220.00m, 35, 210, "88% Fine Viscose, 12% Silk",
                "Second-skin minimalist black turtleneck top styled for sleek editorial elegance with tailored trousers and gold hardware.",
                "/images/products/vel-tee-08.jpg"),

            // Tailored Trousers & Denim (4 Women + 4 Men)
            new("denim", "VEL-DNM-01", "High-Waist Double-Pleat Trousers — Cream (Women)", "high-waist-double-pleat-trousers-cream",
                420.00m, 16, 290, "65% Virgin Wool, 35% Viscose Twill",
                "Signature Massimo-inspired tailored trousers in warm cream with crisp front pleats and elongating wide straight leg.",
                "/images/products/vel-dnm-01.jpg"),
            new("denim", "VEL-DNM-02", "Men's Tailored Virgin Wool Trousers — Noir", "mens-tailored-virgin-wool-trousers-noir",
                460.00m, 24, 310, "100% Italian Virgin Wool",
                "Impeccably pressed men's formal-minimalist black wool trousers with sharp centre crease and clean concealed closure.",
                "/images/products/vel-dnm-02.jpg"),
            new("denim", "VEL-DNM-03", "Clean Straight Split-Hem Denim — Indigo (Women)", "clean-straight-split-hem-denim-indigo",
                360.00m, 19, 390, "100% Organic Cotton Rigid Denim",
                "Minimalist high-rise straight-leg jeans in clean medium-indigo with subtle modern ankle split for sleek footwear styling.",
                "/images/products/vel-dnm-03.jpg"),
            new("denim", "VEL-DNM-04", "Flowing Linen Wide Trousers — Warm Sand (Women)", "flowing-linen-wide-trousers-warm-sand",
                340.00m, 20, 210, "100% Italian Linen",
                "Fluid high-waisted wide trousers in sunlit sand linen featuring tailored belt loops and effortless summer drape.",
                "/images/products/vel-dnm-04.jpg"),
            new("denim", "VEL-DNM-05", "Men's Pleated Dress Trousers — Charcoal", "mens-pleated-dress-trousers-charcoal",
                410.00m, 14, 280, "74% Virgin Wool, 24% Viscose, 2% Elastane",
                "Razor-sharp men's tailored trousers in dark charcoal designed to bridge smart office wear and minimalist weekend styling.",
                "/images/products/vel-dnm-05.jpg"),
            new("denim", "VEL-DNM-06", "Men's Relaxed Straight Denim — Vintage Ecru", "mens-relaxed-straight-denim-ecru",
                350.00m, 26, 370, "100% Undyed Organic Cotton Denim",
                "Clean full-length straight denim in creamy ecru — iconic Mediterranean quiet luxury paired effortlessly with leather loafers.",
                "/images/products/vel-dnm-06.jpg"),
            new("denim", "VEL-DNM-07", "Light-Wash Clean Straight Denim — Sky Blue (Women)", "light-wash-clean-straight-denim-sky-blue",
                340.00m, 15, 360, "98% Organic Cotton, 2% Elastane",
                "Clean sunlit medium-light blue denim with crisp tailoring, subtle vintage fade, and classic five-pocket styling.",
                "/images/products/vel-dnm-07.jpg"),
            new("denim", "VEL-DNM-08", "Mid-Rise Clean Straight Jeans — Classic Blue (Women)", "mid-rise-clean-straight-jeans-classic-blue",
                340.00m, 28, 380, "99% Organic Cotton, 1% Elastane",
                "Timeless vintage-inspired medium blue straight jeans with zero distressing and tailored five-pocket proportions.",
                "/images/products/vel-dnm-08.jpg"),

            // Coats, Blazers & Outerwear (4 Women + 4 Men)
            new("jackets", "VEL-OUT-01", "Double-Faced Wool Belted Coat — Camel (Women)", "double-faced-wool-belted-coat-camel",
                980.00m, 10, 620, "90% Double-Faced Virgin Wool, 10% Cashmere",
                "Iconic hand-finished wrap coat in signature camel wool featuring wide notched lapels and timeless fluid drape.",
                "/images/products/vel-out-01.jpg"),
            new("jackets", "VEL-OUT-02", "Double-Breasted Tailored Blazer — Noir (Women)", "double-breasted-tailored-blazer-noir",
                640.00m, 18, 340, "100% Fine Tropical Wool",
                "Architectural tailored black blazer with sharp notched lapels and clean editorial shoulders — pure Zara Studio minimalism.",
                "/images/products/vel-out-02.jpg"),
            new("jackets", "VEL-OUT-03", "Classic Belted Gabardine Trench — Stone (Women)", "classic-belted-gabardine-trench-stone",
                790.00m, 14, 380, "100% Water-Repellent Cotton Gabardine",
                "Timeless European double-breasted trench coat in warm stone beige with storm flap and tailored waist belt.",
                "/images/products/vel-out-03.jpg"),
            new("jackets", "VEL-OUT-04", "Men's Camel Wool Overcoat — Massimo Cut", "mens-camel-wool-overcoat-massimo",
                1050.00m, 15, 640, "85% Italian Virgin Wool, 15% Cashmere",
                "Tailored single-breasted men's overcoat in rich warm camel wool with crisp notched lapels and satin interior lining.",
                "/images/products/vel-out-04.jpg"),
            new("jackets", "VEL-OUT-05", "Structured Tailored Blazer — Ivory White (Women)", "structured-tailored-blazer-ivory-white",
                620.00m, 22, 350, "72% Virgin Wool, 28% Silk Blend",
                "Luminous crisp white tailored blazer with clean single-breasted closure and elongated contemporary European silhouette.",
                "/images/products/vel-out-05.jpg"),
            new("jackets", "VEL-OUT-06", "Men's Structured Suit Blazer — Noir", "mens-structured-suit-blazer-noir",
                740.00m, 16, 360, "100% Super 120s Italian Wool",
                "Minimalist men's tailored black suit jacket with razor-sharp lapels, welt pockets, and impeccable architectural structure.",
                "/images/products/vel-out-06.jpg"),
            new("jackets", "VEL-OUT-07", "Men's Herringbone Tailored Blazer — Charcoal", "mens-herringbone-tailored-blazer-charcoal",
                690.00m, 8, 380, "100% Portuguese Brushed Wool",
                "Classic tailored men's blazer crafted in fine wool twill with soft-constructed shoulders for effortless smart-casual elegance.",
                "/images/products/vel-out-07.jpg"),
            new("jackets", "VEL-OUT-08", "Men's Minimalist Studio Blazer — Warm Taupe", "mens-minimalist-studio-blazer-taupe",
                670.00m, 25, 340, "80% Virgin Wool, 20% Linen",
                "Relaxed contemporary blazer in understated stone-taupe engineered for seamless monochromatic capsule styling.",
                "/images/products/vel-out-08.jpg")
        };

        // Seed or in-place upgrade existing catalog (preserves FKs!)
        if (!context.Categories.Any())
        {
            foreach (var c in categorySeeds)
            {
                context.Categories.Add(new Category { Slug = c.Slug, Name = c.Name, Description = c.Description });
            }
            context.SaveChanges();
        }
        else
        {
            foreach (var c in categorySeeds)
            {
                var existing = context.Categories.FirstOrDefault(x => x.Slug == c.Slug);
                if (existing != null && (existing.Name != c.Name || existing.Description != c.Description))
                {
                    existing.Name = c.Name;
                    existing.Description = c.Description;
                }
            }
            context.SaveChanges();
        }

        var categoryMap = context.Categories.ToDictionary(c => c.Slug, c => c.Id);

        if (!context.Products.Any())
        {
            foreach (var p in productSeeds)
            {
                context.Products.Add(new Product
                {
                    CategoryId = categoryMap[p.CategorySlug],
                    SKU = p.SKU,
                    Name = p.Name,
                    Slug = p.Slug,
                    Price = p.Price,
                    StockLevel = p.StockLevel,
                    FabricGsm = p.FabricGsm,
                    Material = p.Material,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl
                });
            }
            context.SaveChanges();
        }
        else if (!context.Products.Any(p => p.Name == "Men's Camel Wool Overcoat — Massimo Cut"))
        {
            foreach (var p in productSeeds)
            {
                var existing = context.Products.FirstOrDefault(x => x.SKU == p.SKU);
                if (existing != null)
                {
                    existing.CategoryId = categoryMap[p.CategorySlug];
                    existing.Name = p.Name;
                    existing.Slug = p.Slug;
                    existing.Price = p.Price;
                    existing.FabricGsm = p.FabricGsm;
                    existing.Material = p.Material;
                    existing.Description = p.Description;
                    existing.ImageUrl = p.ImageUrl;
                }
            }
            context.SaveChanges();
        }
    }

    private static string BCryptHash(string password)
    {
        return Convert.ToBase64String(System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(password)));
    }
}
