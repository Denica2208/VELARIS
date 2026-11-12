using Velaris.Store.Models;

namespace Velaris.Store.Data;

public static class DbInitializer
{
    public static void Initialize(VelarisDbContext context)
    {
        context.Database.EnsureCreated();

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

        // Seed categories if empty
        if (!context.Categories.Any())
        {
            var catHoodies = new Category
            {
                Name = "Heavyweight Hoodies & Sweats",
                Slug = "hoodies",
                Description = "Ultra-dense 450 GSM organic fleece staples engineered for enduring comfort and boxy drape."
            };
            var catTees = new Category
            {
                Name = "Boxy & Relaxed T-Shirts",
                Slug = "t-shirts",
                Description = "280 GSM combed organic cotton tees with double-needle collar reinforcement and dropped shoulders."
            };
            var catDenim = new Category
            {
                Name = "Structured Denim & Cargo",
                Slug = "denim",
                Description = "14oz Japanese raw selvedge denim and heavyweight utility double-knee cargo trousers."
            };
            var catJackets = new Category
            {
                Name = "Weatherproof Jackets & Outerwear",
                Slug = "jackets",
                Description = "Technical shell parkas, insulated overshirts, and structured wool overcoats built for all climates."
            };

            context.Categories.AddRange(catHoodies, catTees, catDenim, catJackets);
            context.SaveChanges();

            // Seed exactly 32 products (8 per category)
            var products = new List<Product>
            {
                // Hoodies (8 SKUs)
                new() {
                    CategoryId = catHoodies.Id, SKU = "VEL-HOD-01", Name = "Nocturne Oversized Hoodie",
                    Slug = "nocturne-oversized-hoodie", Price = 380.00m, StockLevel = 24, FabricGsm = 450,
                    Description = "Double-layered architectural hood, seamless kangaroo pocket, pre-shrunk heavyweight fleece.",
                    ImageUrl = "https://images.unsplash.com/photo-1556905055-8f358a7a47b2?auto=format&fit=crop&w=800&q=80"
                },
                new() {
                    CategoryId = catHoodies.Id, SKU = "VEL-HOD-02", Name = "Taupe Brushed Boxy Sweatshirt",
                    Slug = "taupe-brushed-boxy-sweatshirt", Price = 350.00m, StockLevel = 18, FabricGsm = 420,
                    Description = "Crewneck design with ribbed collar, dropped shoulders, and brushed fleece thermal lining.",
                    ImageUrl = "https://images.unsplash.com/photo-1578768079052-aa76e520028b?auto=format&fit=crop&w=800&q=80"
                },
                new() {
                    CategoryId = catHoodies.Id, SKU = "VEL-HOD-03", Name = "Raw Earth Heavy Zip-Up",
                    Slug = "raw-earth-heavy-zip-up", Price = 410.00m, StockLevel = 15, FabricGsm = 460,
                    Description = "Full two-way matte steel zipper with oversized relaxed fit and deep inset pockets.",
                    ImageUrl = "https://images.unsplash.com/photo-1509967419530-da38b4704bc6?auto=format&fit=crop&w=800&q=80"
                },
                new() {
                    CategoryId = catHoodies.Id, SKU = "VEL-HOD-04", Name = "Chalk Minimalist Drop Hoodie",
                    Slug = "chalk-minimalist-drop-hoodie", Price = 370.00m, StockLevel = 30, FabricGsm = 440,
                    Description = "Raw unbleached organic cotton fleece with tonal embroidery at left cuff.",
                    ImageUrl = "https://images.unsplash.com/photo-1512436991641-6745cdb1723f?auto=format&fit=crop&w=800&q=80"
                },
                new() {
                    CategoryId = catHoodies.Id, SKU = "VEL-HOD-05", Name = "Vintage Wash French Terry Crew",
                    Slug = "vintage-wash-french-terry-crew", Price = 340.00m, StockLevel = 22, FabricGsm = 400,
                    Description = "Pigment-dyed stone-washed crewneck featuring distressed ribbing accents.",
                    ImageUrl = "https://images.unsplash.com/photo-1543163521-1bf539c55dd2?auto=format&fit=crop&w=800&q=80"
                },
                new() {
                    CategoryId = catHoodies.Id, SKU = "VEL-HOD-06", Name = "Obsidian Thermal Pullover",
                    Slug = "obsidian-thermal-pullover", Price = 390.00m, StockLevel = 12, FabricGsm = 480,
                    Description = "High-neck storm pullover engineered for wind deflection and urban warmth.",
                    ImageUrl = "https://images.unsplash.com/photo-1578587018452-892bacefd3f2?auto=format&fit=crop&w=800&q=80"
                },
                new() {
                    CategoryId = catHoodies.Id, SKU = "VEL-HOD-07", Name = "Sage Brushed Fleece Hoodie",
                    Slug = "sage-brushed-fleece-hoodie", Price = 380.00m, StockLevel = 20, FabricGsm = 450,
                    Description = "Subtle botanical dye with relaxed waist rib and wide sleeves.",
                    ImageUrl = "https://images.unsplash.com/photo-1620799140408-edc6dcb6d633?auto=format&fit=crop&w=800&q=80"
                },
                new() {
                    CategoryId = catHoodies.Id, SKU = "VEL-HOD-08", Name = "Espresso Heavy Knit Hoodie",
                    Slug = "espresso-heavy-knit-hoodie", Price = 420.00m, StockLevel = 16, FabricGsm = 500,
                    Description = "Dense waffle-knit organic blend with clean interior taped seams.",
                    ImageUrl = "https://images.unsplash.com/photo-1509631179647-0177331693ae?auto=format&fit=crop&w=800&q=80"
                },

                // T-Shirts (8 SKUs)
                new() {
                    CategoryId = catTees.Id, SKU = "VEL-TEE-01", Name = "Core Heavyweight Tee - Chalk White",
                    Slug = "core-heavyweight-tee-chalk-white", Price = 170.00m, StockLevel = 45, FabricGsm = 280,
                    Description = "The quintessential daily boxy tee. Preshrunk 280 GSM combed jersey.",
                    ImageUrl = "https://images.unsplash.com/photo-1521572267360-ee0c2909d518?auto=format&fit=crop&w=800&q=80"
                },
                new() {
                    CategoryId = catTees.Id, SKU = "VEL-TEE-02", Name = "Core Heavyweight Tee - Washed Black",
                    Slug = "core-heavyweight-tee-washed-black", Price = 170.00m, StockLevel = 50, FabricGsm = 280,
                    Description = "Garment washed for a vintage soft-hand texture with thick collar ribbing.",
                    ImageUrl = "https://images.unsplash.com/photo-1503342217505-b0a15ec3261c?auto=format&fit=crop&w=800&q=80"
                },
                new() {
                    CategoryId = catTees.Id, SKU = "VEL-TEE-03", Name = "Relaxed Boxy Pocket Tee - Sand",
                    Slug = "relaxed-boxy-pocket-tee-sand", Price = 185.00m, StockLevel = 28, FabricGsm = 260,
                    Description = "Minimalist patch chest pocket with reinforced stitching and clean lines.",
                    ImageUrl = "https://images.unsplash.com/photo-1583743814966-8936f5b7be1a?auto=format&fit=crop&w=800&q=80"
                },
                new() {
                    CategoryId = catTees.Id, SKU = "VEL-TEE-04", Name = "Oversized Minimalist Tee - Moss",
                    Slug = "oversized-minimalist-tee-moss", Price = 180.00m, StockLevel = 32, FabricGsm = 290,
                    Description = "Earthy tone dyed with low environmental impact pigments.",
                    ImageUrl = "https://images.unsplash.com/photo-1618354691373-d851c5c3a990?auto=format&fit=crop&w=800&q=80"
                },
                new() {
                    CategoryId = catTees.Id, SKU = "VEL-TEE-05", Name = "Drop-Tail Organic Cotton Shirt",
                    Slug = "drop-tail-organic-cotton-shirt", Price = 190.00m, StockLevel = 25, FabricGsm = 270,
                    Description = "Elongated rear hem and split side vents for effortless layered looks.",
                    ImageUrl = "https://images.unsplash.com/photo-1581655353564-df123a1eb820?auto=format&fit=crop&w=800&q=80"
                },
                new() {
                    CategoryId = catTees.Id, SKU = "VEL-TEE-06", Name = "Velaris Atelier Typo Graphic Tee",
                    Slug = "velaris-atelier-typo-graphic-tee", Price = 195.00m, StockLevel = 40, FabricGsm = 280,
                    Description = "Discreet micro typography across rear shoulder; archival studio cut.",
                    ImageUrl = "https://images.unsplash.com/photo-1527719327859-c6ce80353573?auto=format&fit=crop&w=800&q=80"
                },
                new() {
                    CategoryId = catTees.Id, SKU = "VEL-TEE-07", Name = "Waffle Texture Long-Sleeve - Ecru",
                    Slug = "waffle-texture-long-sleeve-ecru", Price = 220.00m, StockLevel = 22, FabricGsm = 320,
                    Description = "Textured thermal waffle weave with ribbed cuffs and tailored sleeve length.",
                    ImageUrl = "https://images.unsplash.com/photo-1562157873-818bc0726f68?auto=format&fit=crop&w=800&q=80"
                },
                new() {
                    CategoryId = catTees.Id, SKU = "VEL-TEE-08", Name = "Mercerized Pima Cotton Tee - Pitch",
                    Slug = "mercerized-pima-cotton-tee-pitch", Price = 210.00m, StockLevel = 35, FabricGsm = 240,
                    Description = "Silky luster and superior tensile strength from extra-long staple Pima cotton.",
                    ImageUrl = "https://images.unsplash.com/photo-1618354691438-25bc04584c03?auto=format&fit=crop&w=800&q=80"
                },

                // Denim & Trousers (8 SKUs)
                new() {
                    CategoryId = catDenim.Id, SKU = "VEL-DNM-01", Name = "Raw Indigo Selvedge Wide-Leg (14oz)",
                    Slug = "raw-indigo-selvedge-wide-leg", Price = 440.00m, StockLevel = 16, FabricGsm = 480,
                    Description = "Custom Japanese shuttle-loom selvedge denim. Clean wide straight cut.",
                    ImageUrl = "https://images.unsplash.com/photo-1542272604-780c96856592?auto=format&fit=crop&w=800&q=80"
                },
                new() {
                    CategoryId = catDenim.Id, SKU = "VEL-DNM-02", Name = "Washed Black Relaxed Denim",
                    Slug = "washed-black-relaxed-denim", Price = 390.00m, StockLevel = 24, FabricGsm = 440,
                    Description = "Soft sulfur wash with vintage whiskering and custom antique silver hardware.",
                    ImageUrl = "https://images.unsplash.com/photo-1541099649105-f69ad21f3246?auto=format&fit=crop&w=800&q=80"
                },
                new() {
                    CategoryId = catDenim.Id, SKU = "VEL-DNM-03", Name = "Minimal Utility Double-Knee Pants",
                    Slug = "minimal-utility-double-knee-pants", Price = 410.00m, StockLevel = 19, FabricGsm = 420,
                    Description = "Reinforced knee paneling, hammer loop, and deep utility pocket in 12oz duck canvas.",
                    ImageUrl = "https://images.unsplash.com/photo-1584370848010-d7fe6bc767ec?auto=format&fit=crop&w=800&q=80"
                },
                new() {
                    CategoryId = catDenim.Id, SKU = "VEL-DNM-04", Name = "Heavy Duck Canvas Cargo Trousers",
                    Slug = "heavy-duck-canvas-cargo-trousers", Price = 425.00m, StockLevel = 20, FabricGsm = 430,
                    Description = "Streamlined low-profile 3D bellow cargo pockets with drawcord ankle cuffs.",
                    ImageUrl = "https://images.unsplash.com/photo-1624378439575-d8705ad7ae80?auto=format&fit=crop&w=800&q=80"
                },
                new() {
                    CategoryId = catDenim.Id, SKU = "VEL-DNM-05", Name = "Tailored Wool Pleated Trousers",
                    Slug = "tailored-wool-pleated-trousers", Price = 460.00m, StockLevel = 14, FabricGsm = 380,
                    Description = "Single front pleat, hook-and-bar closure, and relaxed tapered leg.",
                    ImageUrl = "https://images.unsplash.com/photo-1506629082955-511b1aa562c8?auto=format&fit=crop&w=800&q=80"
                },
                new() {
                    CategoryId = catDenim.Id, SKU = "VEL-DNM-06", Name = "Linen-Cotton Relaxed Drawstring Pants",
                    Slug = "linen-cotton-relaxed-drawstring-pants", Price = 340.00m, StockLevel = 26, FabricGsm = 300,
                    Description = "Breathable summer blend featuring an elastic waistband with waxed cotton cord.",
                    ImageUrl = "https://images.unsplash.com/photo-1517445312882-bc9910d016b7?auto=format&fit=crop&w=800&q=80"
                },
                new() {
                    CategoryId = catDenim.Id, SKU = "VEL-DNM-07", Name = "Japanese Vintage Wash Straight Denim",
                    Slug = "japanese-vintage-wash-straight-denim", Price = 450.00m, StockLevel = 15, FabricGsm = 460,
                    Description = "Light sun-bleached vintage tone with authentic chain-stitched hems.",
                    ImageUrl = "https://images.unsplash.com/photo-1475178626620-a4d074967452?auto=format&fit=crop&w=800&q=80"
                },
                new() {
                    CategoryId = catDenim.Id, SKU = "VEL-DNM-08", Name = "Technical Water-Repellent Chinos",
                    Slug = "technical-water-repellent-chinos", Price = 380.00m, StockLevel = 28, FabricGsm = 310,
                    Description = "DWR coated four-way stretch twill designed for all-weather commuting.",
                    ImageUrl = "https://images.unsplash.com/photo-1594633312681-425c7b97ccd1?auto=format&fit=crop&w=800&q=80"
                },

                // Outerwear (8 SKUs)
                new() {
                    CategoryId = catJackets.Id, SKU = "VEL-OUT-01", Name = "Minimalist Wool Overcoat - Midnight",
                    Slug = "minimalist-wool-overcoat-midnight", Price = 720.00m, StockLevel = 10, FabricGsm = 650,
                    Description = "Structured double-face recycled wool blend with concealed horn buttons.",
                    ImageUrl = "https://images.unsplash.com/photo-1548883354-7622d03aca27?auto=format&fit=crop&w=800&q=80"
                },
                new() {
                    CategoryId = catJackets.Id, SKU = "VEL-OUT-02", Name = "Quilted Insulated Liner Jacket",
                    Slug = "quilted-insulated-liner-jacket", Price = 490.00m, StockLevel = 18, FabricGsm = 350,
                    Description = "Onion-quilted ripstop nylon filled with lightweight recycled thermal down.",
                    ImageUrl = "https://images.unsplash.com/photo-1544022613-e87ca75a784a?auto=format&fit=crop&w=800&q=80"
                },
                new() {
                    CategoryId = catJackets.Id, SKU = "VEL-OUT-03", Name = "Weatherproof Shell Parka - Desert",
                    Slug = "weatherproof-shell-parka-desert", Price = 640.00m, StockLevel = 14, FabricGsm = 420,
                    Description = "Three-layer taped waterproof shell with YKK Aquaguard taped zippers.",
                    ImageUrl = "https://images.unsplash.com/photo-1551028719-00167b16eac5?auto=format&fit=crop&w=800&q=80"
                },
                new() {
                    CategoryId = catJackets.Id, SKU = "VEL-OUT-04", Name = "Heavyweight Denim Trucker Jacket",
                    Slug = "heavyweight-denim-trucker-jacket", Price = 520.00m, StockLevel = 15, FabricGsm = 500,
                    Description = "Boxy cropped trucker cut in 14.5oz rigid selvedge with side welt pockets.",
                    ImageUrl = "https://images.unsplash.com/photo-1591047139829-d91aecb6caea?auto=format&fit=crop&w=800&q=80"
                },
                new() {
                    CategoryId = catJackets.Id, SKU = "VEL-OUT-05", Name = "Technical Fleece Field Jacket",
                    Slug = "technical-fleece-field-jacket", Price = 480.00m, StockLevel = 22, FabricGsm = 400,
                    Description = "Heavy sherpa fleece body with high-density nylon contrast overlays.",
                    ImageUrl = "https://images.unsplash.com/photo-1520975916090-3105956dac38?auto=format&fit=crop&w=800&q=80"
                },
                new() {
                    CategoryId = catJackets.Id, SKU = "VEL-OUT-06", Name = "Cropped Harrington Jacket - Espresso",
                    Slug = "cropped-harrington-jacket-espresso", Price = 540.00m, StockLevel = 16, FabricGsm = 380,
                    Description = "Classic stand collar with two-button closure and custom tartan satin lining.",
                    ImageUrl = "https://images.unsplash.com/photo-1516257984-b1b4d707412e?auto=format&fit=crop&w=800&q=80"
                },
                new() {
                    CategoryId = catJackets.Id, SKU = "VEL-OUT-07", Name = "Double-Breasted Trench - Camel",
                    Slug = "double-breasted-trench-camel", Price = 760.00m, StockLevel = 8, FabricGsm = 550,
                    Description = "Weather-resistant cotton gabardine featuring storm flap and belted waist.",
                    ImageUrl = "https://images.unsplash.com/photo-1539571696357-5a69c17a67c6?auto=format&fit=crop&w=800&q=80"
                },
                new() {
                    CategoryId = catJackets.Id, SKU = "VEL-OUT-08", Name = "Ripstop Windbreaker Coach Jacket",
                    Slug = "ripstop-windbreaker-coach-jacket", Price = 420.00m, StockLevel = 25, FabricGsm = 280,
                    Description = "Matte snap closures, elastic cuffs, and drawstring waist for quick layering.",
                    ImageUrl = "https://images.unsplash.com/photo-1515886657613-9f3515b0c78f?auto=format&fit=crop&w=800&q=80"
                }
            };

            context.Products.AddRange(products);
            context.SaveChanges();
        }
    }

    private static string BCryptHash(string password)
    {
        // Simple deterministic salt/hash for seed accounts
        return Convert.ToBase64String(System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(password)));
    }
}
