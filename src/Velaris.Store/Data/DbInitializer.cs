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

        // Auto-upgrade if legacy external URLs are present in database
        if (context.Products.Any(p => p.ImageUrl.StartsWith("http")))
        {
            context.Products.RemoveRange(context.Products);
            context.Categories.RemoveRange(context.Categories);
            context.SaveChanges();
        }

        // Seed categories & products if empty
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

            // Seed exactly 32 products (8 per category) with verified local photography
            var products = new List<Product>
            {
                // Hoodies (8 SKUs)
                new() {
                    CategoryId = catHoodies.Id, SKU = "VEL-HOD-01", Name = "Nocturne Oversized Hoodie",
                    Slug = "nocturne-oversized-hoodie", Price = 380.00m, StockLevel = 24, FabricGsm = 450,
                    Description = "Double-layered architectural hood, seamless kangaroo pocket, pre-shrunk heavyweight 450 GSM matte black fleece.",
                    ImageUrl = "/images/products/vel-hod-01.jpg"
                },
                new() {
                    CategoryId = catHoodies.Id, SKU = "VEL-HOD-02", Name = "Taupe Brushed Boxy Sweatshirt",
                    Slug = "taupe-brushed-boxy-sweatshirt", Price = 350.00m, StockLevel = 18, FabricGsm = 420,
                    Description = "Crewneck design with ribbed collar, dropped shoulders, and brushed fleece thermal lining in warm taupe.",
                    ImageUrl = "/images/products/vel-hod-02.jpg"
                },
                new() {
                    CategoryId = catHoodies.Id, SKU = "VEL-HOD-03", Name = "Raw Earth Heavy Zip-Up",
                    Slug = "raw-earth-heavy-zip-up", Price = 410.00m, StockLevel = 15, FabricGsm = 460,
                    Description = "Full two-way matte steel zipper with oversized relaxed fit and deep split kangaroo pockets in rich earth brown fleece.",
                    ImageUrl = "/images/products/vel-hod-03.jpg"
                },
                new() {
                    CategoryId = catHoodies.Id, SKU = "VEL-HOD-04", Name = "Chalk Minimalist Drop Hoodie",
                    Slug = "chalk-minimalist-drop-hoodie", Price = 370.00m, StockLevel = 30, FabricGsm = 440,
                    Description = "Raw unbleached organic cotton fleece with clean drawstring hood, kangaroo pocket, and relaxed streetwear drape.",
                    ImageUrl = "/images/products/vel-hod-04.jpg"
                },
                new() {
                    CategoryId = catHoodies.Id, SKU = "VEL-HOD-05", Name = "Vintage Wash French Terry Crew",
                    Slug = "vintage-wash-french-terry-crew", Price = 340.00m, StockLevel = 22, FabricGsm = 400,
                    Description = "Heavyweight French terry crewneck in clean off-white featuring ribbed collar, cuffs, and hem.",
                    ImageUrl = "/images/products/vel-hod-05.jpg"
                },
                new() {
                    CategoryId = catHoodies.Id, SKU = "VEL-HOD-06", Name = "Crimson Thermal Streetwear Hoodie",
                    Slug = "crimson-thermal-streetwear-hoodie", Price = 390.00m, StockLevel = 12, FabricGsm = 480,
                    Description = "Deep crimson red heavyweight fleece pullover hoodie with relaxed drop-shoulder cut and pouch pocket.",
                    ImageUrl = "/images/products/vel-hod-06.jpg"
                },
                new() {
                    CategoryId = catHoodies.Id, SKU = "VEL-HOD-07", Name = "Sunburst Graphic Fleece Hoodie",
                    Slug = "sunburst-graphic-fleece-hoodie", Price = 380.00m, StockLevel = 20, FabricGsm = 450,
                    Description = "Vibrant mustard heavyweight fleece hoodie featuring custom archival skull and botanical rose artwork.",
                    ImageUrl = "/images/products/vel-hod-07.jpg"
                },
                new() {
                    CategoryId = catHoodies.Id, SKU = "VEL-HOD-08", Name = "Canary Cropped Streetwear Hoodie",
                    Slug = "canary-cropped-streetwear-hoodie", Price = 420.00m, StockLevel = 16, FabricGsm = 500,
                    Description = "Cropped heavyweight fleece hoodie with raw edge hem, matching tonal drawstrings, and relaxed streetwear proportions.",
                    ImageUrl = "/images/products/vel-hod-08.jpg"
                },

                // T-Shirts (8 SKUs)
                new() {
                    CategoryId = catTees.Id, SKU = "VEL-TEE-01", Name = "Core Heavyweight Tee - Chalk White",
                    Slug = "core-heavyweight-tee-chalk-white", Price = 170.00m, StockLevel = 45, FabricGsm = 280,
                    Description = "The quintessential daily boxy tee. Preshrunk 280 GSM combed jersey on clean wooden hanger.",
                    ImageUrl = "/images/products/vel-tee-01.jpg"
                },
                new() {
                    CategoryId = catTees.Id, SKU = "VEL-TEE-02", Name = "Core Heavyweight Tee - Washed Black",
                    Slug = "core-heavyweight-tee-washed-black", Price = 170.00m, StockLevel = 50, FabricGsm = 280,
                    Description = "Garment washed for a vintage soft-hand texture with circular minimalist chest typography emblem.",
                    ImageUrl = "/images/products/vel-tee-02.jpg"
                },
                new() {
                    CategoryId = catTees.Id, SKU = "VEL-TEE-03", Name = "Anime Graphic Streetwear Tee - Sand",
                    Slug = "anime-graphic-streetwear-tee-sand", Price = 185.00m, StockLevel = 28, FabricGsm = 260,
                    Description = "Sand/beige organic combed cotton tee featuring Japanese block print artwork and royal blue accents.",
                    ImageUrl = "/images/products/vel-tee-03.jpg"
                },
                new() {
                    CategoryId = catTees.Id, SKU = "VEL-TEE-04", Name = "Oversized Minimalist Tee - Moss",
                    Slug = "oversized-minimalist-tee-moss", Price = 180.00m, StockLevel = 32, FabricGsm = 290,
                    Description = "Earthy sage-moss tone dyed with low environmental impact pigments, double-needle collar ribbing.",
                    ImageUrl = "/images/products/vel-tee-04.jpg"
                },
                new() {
                    CategoryId = catTees.Id, SKU = "VEL-TEE-05", Name = "Drop-Tail Organic Cotton Shirt",
                    Slug = "drop-tail-organic-cotton-shirt", Price = 190.00m, StockLevel = 25, FabricGsm = 270,
                    Description = "Discreet micro-typography on clean white cotton jersey with reinforced interior taped seams.",
                    ImageUrl = "/images/products/vel-tee-05.jpg"
                },
                new() {
                    CategoryId = catTees.Id, SKU = "VEL-TEE-06", Name = "Velaris Atelier Typo Graphic Tee",
                    Slug = "velaris-atelier-typo-graphic-tee", Price = 195.00m, StockLevel = 40, FabricGsm = 280,
                    Description = "Clean white boxy heavyweight t-shirt featuring bold red typographic block graphic on chest.",
                    ImageUrl = "/images/products/vel-tee-06.jpg"
                },
                new() {
                    CategoryId = catTees.Id, SKU = "VEL-TEE-07", Name = "Waffle Texture Long-Sleeve - Ecru",
                    Slug = "waffle-texture-long-sleeve-ecru", Price = 220.00m, StockLevel = 22, FabricGsm = 320,
                    Description = "Textured thermal waffle knit in light grey/ecru with tailored ribbed cuffs for effortless layering.",
                    ImageUrl = "/images/products/vel-tee-07.jpg"
                },
                new() {
                    CategoryId = catTees.Id, SKU = "VEL-TEE-08", Name = "Mercerized Graphic Tee - Pitch Black",
                    Slug = "mercerized-graphic-tee-pitch-black", Price = 210.00m, StockLevel = 35, FabricGsm = 240,
                    Description = "Silky luster combed cotton tee in deep pitch black with custom bone peace-sign screenprint.",
                    ImageUrl = "/images/products/vel-tee-08.jpg"
                },

                // Denim & Trousers (8 SKUs)
                new() {
                    CategoryId = catDenim.Id, SKU = "VEL-DNM-01", Name = "Raw Indigo Selvedge Wide-Leg (14oz)",
                    Slug = "raw-indigo-selvedge-wide-leg", Price = 440.00m, StockLevel = 16, FabricGsm = 480,
                    Description = "Custom Japanese shuttle-loom selvedge denim in deep indigo. Clean wide straight cut with antique copper rivets.",
                    ImageUrl = "/images/products/vel-dnm-01.jpg"
                },
                new() {
                    CategoryId = catDenim.Id, SKU = "VEL-DNM-02", Name = "Washed Black Relaxed Denim",
                    Slug = "washed-black-relaxed-denim", Price = 390.00m, StockLevel = 24, FabricGsm = 440,
                    Description = "Soft sulfur wash black denim with authentic knee distressing and relaxed straight-leg drape.",
                    ImageUrl = "/images/products/vel-dnm-02.jpg"
                },
                new() {
                    CategoryId = catDenim.Id, SKU = "VEL-DNM-03", Name = "Classic Selvedge Denim Capsule - Indigo & Black",
                    Slug = "classic-selvedge-denim-capsule", Price = 410.00m, StockLevel = 19, FabricGsm = 420,
                    Description = "Three-shade selvedge capsule featuring raw indigo, vintage wash, and midnight black five-pocket denim.",
                    ImageUrl = "/images/products/vel-dnm-03.jpg"
                },
                new() {
                    CategoryId = catDenim.Id, SKU = "VEL-DNM-04", Name = "Heavy Duck Canvas Cargo Trousers",
                    Slug = "heavy-duck-canvas-cargo-trousers", Price = 425.00m, StockLevel = 20, FabricGsm = 430,
                    Description = "Streamlined low-profile 3D bellow cargo pockets with reinforced knee seams in washed charcoal duck canvas.",
                    ImageUrl = "/images/products/vel-dnm-04.jpg"
                },
                new() {
                    CategoryId = catDenim.Id, SKU = "VEL-DNM-05", Name = "Tailored Wool Pleated Trousers",
                    Slug = "tailored-wool-pleated-trousers", Price = 460.00m, StockLevel = 14, FabricGsm = 380,
                    Description = "High-waisted single front pleat trousers in fine woven cream wool blend with relaxed wide tapered leg.",
                    ImageUrl = "/images/products/vel-dnm-05.jpg"
                },
                new() {
                    CategoryId = catDenim.Id, SKU = "VEL-DNM-06", Name = "Linen-Cotton Relaxed Drawstring Pants",
                    Slug = "linen-cotton-relaxed-drawstring-pants", Price = 340.00m, StockLevel = 26, FabricGsm = 300,
                    Description = "Breathable summer blend featuring an elastic waistband with side pockets and relaxed drape.",
                    ImageUrl = "/images/products/vel-dnm-06.jpg"
                },
                new() {
                    CategoryId = catDenim.Id, SKU = "VEL-DNM-07", Name = "Japanese Vintage Wash Straight Denim",
                    Slug = "japanese-vintage-wash-straight-denim", Price = 450.00m, StockLevel = 15, FabricGsm = 460,
                    Description = "Light sun-bleached vintage blue denim with authentic distressed abrasion and chain-stitched hems.",
                    ImageUrl = "/images/products/vel-dnm-07.jpg"
                },
                new() {
                    CategoryId = catDenim.Id, SKU = "VEL-DNM-08", Name = "Technical Water-Repellent Chinos",
                    Slug = "technical-water-repellent-chinos", Price = 380.00m, StockLevel = 28, FabricGsm = 310,
                    Description = "DWR coated four-way stretch twill in classic tan/khaki designed for all-weather urban commuting.",
                    ImageUrl = "/images/products/vel-dnm-08.jpg"
                },

                // Outerwear (8 SKUs)
                new() {
                    CategoryId = catJackets.Id, SKU = "VEL-OUT-01", Name = "Minimalist Wool Overcoat - Camel",
                    Slug = "minimalist-wool-overcoat-camel", Price = 720.00m, StockLevel = 10, FabricGsm = 650,
                    Description = "Structured tailored double-face camel wool blend with notched lapels and horn button closure.",
                    ImageUrl = "/images/products/vel-out-01.jpg"
                },
                new() {
                    CategoryId = catJackets.Id, SKU = "VEL-OUT-02", Name = "Storm Insulated Winter Parka Jacket",
                    Slug = "storm-insulated-winter-parka-jacket", Price = 490.00m, StockLevel = 18, FabricGsm = 350,
                    Description = "Waterproof technical nylon exterior with high funnel collar, hood, and thermal recycled insulation.",
                    ImageUrl = "/images/products/vel-out-02.jpg"
                },
                new() {
                    CategoryId = catJackets.Id, SKU = "VEL-OUT-03", Name = "Weatherproof Shell Parka - Forest Olive",
                    Slug = "weatherproof-shell-parka-forest-olive", Price = 640.00m, StockLevel = 14, FabricGsm = 420,
                    Description = "Three-layer taped waterproof rain shell with high-neck storm hood and water-shedding DWR finish.",
                    ImageUrl = "/images/products/vel-out-03.jpg"
                },
                new() {
                    CategoryId = catJackets.Id, SKU = "VEL-OUT-04", Name = "Heavyweight Denim Trucker Jacket",
                    Slug = "heavyweight-denim-trucker-jacket", Price = 520.00m, StockLevel = 15, FabricGsm = 500,
                    Description = "Boxy cropped trucker cut in 14.5oz rigid light vintage wash selvedge with side welt pockets.",
                    ImageUrl = "/images/products/vel-out-04.jpg"
                },
                new() {
                    CategoryId = catJackets.Id, SKU = "VEL-OUT-05", Name = "Technical Field Jacket - Military Olive",
                    Slug = "technical-field-jacket-military-olive", Price = 480.00m, StockLevel = 22, FabricGsm = 400,
                    Description = "Heavy cotton gabardine field jacket featuring four exterior bellow pockets and button storm placket.",
                    ImageUrl = "/images/products/vel-out-05.jpg"
                },
                new() {
                    CategoryId = catJackets.Id, SKU = "VEL-OUT-06", Name = "Cropped Harrington Jacket - Espresso",
                    Slug = "cropped-harrington-jacket-espresso", Price = 540.00m, StockLevel = 16, FabricGsm = 380,
                    Description = "Satin-finish MA-1 Harrington bomber jacket in espresso copper with utility sleeve zip pocket and ribbed trims.",
                    ImageUrl = "/images/products/vel-out-06.jpg"
                },
                new() {
                    CategoryId = catJackets.Id, SKU = "VEL-OUT-07", Name = "Double-Breasted Trench - Camel Gabardine",
                    Slug = "double-breasted-trench-camel-gabardine", Price = 760.00m, StockLevel = 8, FabricGsm = 550,
                    Description = "Weather-resistant cotton gabardine trench coat featuring wide lapels, storm flap, and belted cuffs.",
                    ImageUrl = "/images/products/vel-out-07.jpg"
                },
                new() {
                    CategoryId = catJackets.Id, SKU = "VEL-OUT-08", Name = "Ripstop Windbreaker Coach Jacket - Black",
                    Slug = "ripstop-windbreaker-coach-jacket-black", Price = 420.00m, StockLevel = 25, FabricGsm = 280,
                    Description = "Matte black nylon coach jacket with spread collar, zip front, and relaxed streetwear fit for urban layering.",
                    ImageUrl = "/images/products/vel-out-08.jpg"
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
