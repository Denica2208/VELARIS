# VELARIS
### Contemporary Streetwear & Sustainable Capsule Living

**VELARIS** is a premium e-commerce project developed for the **E-Commerce Semester Project, Academic Year 2026–2027**.  
The platform represents a curated streetwear and capsule wardrobe store inspired by minimalist architectural aesthetics and sustainable tailoring. The catalogue contains **32 products (SKUs)** across heavyweight hoodies, boxy organic tees, structured denim and weatherproof technical outerwear.

The platform supports the complete online shopping flow: product browsing, search, persistent shopping cart, user accounts, checkout, sandbox payments, stock management, order history, and a custom administration panel.

---

## 👥 Team & Responsibilities

### **Predut Andreea-Denisa** — *Frontend Architecture & Full-Stack Developer*
**Primary responsibilities:**
- ASP.NET Core Razor Pages + Tailwind CSS storefront
- UI/UX implementation and mobile responsiveness down to 360px (`A11`)
- Product catalogue browsing, category pages, and product detail views (`A1`)
- Client-side keyword search with empty state feedback (`A2`)
- Persistent shopping cart across sessions and auth reload (`A3`)
- Multi-step checkout flow with carrier selection (`A5`)
- Stripe checkout integration and customer order history (`A8`)
- Filtered faceted search and URL state management (`B1`)
- Customer wishlist and saved items (`B7`)

### **Sapca Roman-Stefan** — *Backend Architecture & Database Developer*
**Primary responsibilities:**
- ASP.NET Core custom API layer and middleware pipeline
- SQLite relational database & Entity Framework Core (EF Core) modeling
- User registration, authentication, and role authorization (Argon2 / PBKDF2 hashing) (`A4`)
- Server-side security, anti-CSRF token enforcement, and injection guards (`A10`)
- Atomic stock management and optimistic concurrency tokens (`RowVersion`) (`A7`)
- Stripe webhook signature verification and async order payment processing (`A6`)
- Custom administration dashboard endpoints and order lifecycle transitions (`A9`)
- Automated unit and integration testing suite (`A12`)
- Server-validated discount coupon code engine (`B5`)

---

## 🛠️ Technology Stack

| Layer | Technology |
| :--- | :--- |
| **Frontend Framework** | ASP.NET Core Razor Pages & ViewComponents |
| **Styling & Design System** | Tailwind CSS (Minimalist Dark / Luxury Gold theme) |
| **Backend Core** | C# / ASP.NET Core 8.0 & 10.0 |
| **Database** | SQLite (Relational, Zero-Configuration local portability) |
| **ORM / Data Access** | Entity Framework Core (EF Core) |
| **Authentication** | ASP.NET Core Identity (Argon2 / PBKDF2, secure cookie session) |
| **Payment Gateway** | Stripe (Sandbox / Test Mode) with CLI Webhook Tunneling |
| **Email Testing** | Mailtrap SMTP Service (Transactional receipts) |
| **Automated Testing** | xUnit & Moq (In-memory EF Core integration tests) |
| **Local Environment** | .NET CLI (`dotnet run` / Docker Compose compatible) |
| **Version Control** | Git & GitHub (`main` branch, milestone tags: `P1`, `P2`, `P3`) |

> The administration panel is custom-built within the application and secured against unauthorized role access at the server middleware level (Requirement `A10`).

---

## 📦 Project Scope & Catalogue

The store features **32 SKUs** organized into 4 core categories and curated signature drops:

### Signature Capsule Drops:
1. **Atelier Heavyweight** — Ultra-dense 450 GSM organic fleece staples.
2. **Monochrome Basics** — Everyday boxy 280 GSM combed cotton essentials.
3. **Raw Indigo Series** — Japanese selvedge 14oz structured denim and utility twill.
4. **Nocturne Technical** — Weatherproof ripstop jackets and weatherproof outerwear.

### Product Categories:
- **Heavyweight Hoodies & Sweats** (8 SKUs)
- **Boxy & Relaxed T-Shirts** (8 SKUs)
- **Structured Denim & Cargo Trousers** (8 SKUs)
- **Weatherproof Jackets & Outerwear** (8 SKUs)

---

## 🎨 Brand Identity

- **Design Philosophy:** Luxury Contemporary Minimalist / Architectural Streetwear
- **Typography:**
  - *Outfit* — Headlines, brand typography and numbers
  - *Plus Jakarta Sans* — Body text, garment specifications and UI elements

### Color Palette:

| Color Token | HEX | Role |
| :--- | :--- | :--- |
| **Obsidian Onyx** | `#0D1017` | Primary background canvas |
| **Dark Slate Card** | `#141A26` | Card and modal surface background |
| **Champagne Gold** | `#E5B869` | Primary luxury accent and brand identity |
| **Honey Glow** | `#F3D39B` | Hover states and highlighted text |
| **Warm Terracotta** | `#E07A5F` | Alerts, critical flows, and badge tags |
| **Sage Green** | `#4EBA87` | Stock availability, success states, and mitigation indicators |
| **Cream Ivory** | `#F8FAFC` | Primary readable high-contrast typography |

---

## 📂 Repository Structure

```text
ProjectEC/
├── docs/
│   ├── api/                     # REST API contracts and payload specifications
│   │   ├── auth.md              # Identity, registration, login and session contract
│   │   └── catalog.md           # Catalogue listing, pagination and product detail specs
│   ├── diagrams/                # Architectural diagrams
│   │   ├── erd.md               # Database Entity-Relationship Diagram
│   │   ├── architecture.md      # Component architecture diagram
│   │   └── payment_sequence.md  # Stripe checkout & webhook sequence diagram
│   └── p2/                      # Presentation 2 Deliverables
│       ├── VELARIS_P2_Specification.pdf    # Official 8-page specification document
│       └── VELARIS_Presentation_P2.pdf     # Official 15-slide presentation deck
├── src/
│   └── Velaris.Store/           # ASP.NET Core Web Application
│       ├── Controllers/         # API and MVC Controllers
│       ├── Data/                # DbContext, Migrations, and 32-item Seed Data script
│       ├── Models/              # Domain entities (Product, Category, Order, User)
│       ├── Pages/ / Views/      # Razor Pages (Catalog, Product, Cart, Checkout, Admin)
│       └── wwwroot/             # Static assets (Tailwind styles, product imagery)
├── .editorconfig                # Unified code style configuration
├── .env.example                 # Environment variables template
├── .gitignore                   # Ignore rules for build output, databases, and OS files
├── README.md                    # Project documentation (this file)
└── VELARIS_Presentation_P1.pdf  # Stage P1 approved presentation slides
```

---

## ⚡ Local Setup

### Prerequisites
- **Git**
- **.NET SDK 8.0 or 10.0+** (verify with `dotnet --version`)

### 1. Clone the repository
```bash
git clone https://github.com/Denica2208/VELARIS.git
cd VELARIS
```

### 2. Configure Environment Secrets
Copy the template configuration file:
```bash
# Windows (PowerShell)
Copy-Item .env.example .env
```
*(The `.env` file is ignored by Git and stores local database strings and Stripe sandbox keys).*

### 3. Run the Application (Single-Command Setup)
From the project root:
```bash
dotnet run --project src/Velaris.Store
```
> **What happens automatically on first launch:**  
> 1. SQLite database (`velaris.db`) is automatically initialized and migrated.  
> 2. The **seed script executes**, loading all **32 products across 4 categories** with images, prices, stock levels, and fabric GSM specifications.  
> 3. Standard test accounts (`customer@velaris.com` and `admin@velaris.com`) are provisioned.

### 4. Open in Browser
- **Storefront:** `http://localhost:5000` or `https://localhost:7001`
- **Admin Dashboard:** `http://localhost:5000/Admin` (Log in with `admin@velaris.com` / `AdminPass123!`)

---

## 📋 Task Plan and Status

**Status values:**
- `Done`: Implemented, tested, and merged into `main`
- `To do`: Scheduled for completion before Presentation 2
- `Planned`: Scheduled for execution after Presentation 2 (leading to P3)

### Presentation 2: Prototype Milestone

| No. | Task Description | Assigned To | Status |
| :---: | :--- | :---: | :---: |
| **0.1** | Agree on catalogue scope, 32 clothing SKUs, categories, and initial ERD | Andreea + Stefan | **Done** |
| **0.2** | Configure GitHub repository, branch protection rules, and issue board | Andreea | **Done** |
| **0.3** | Establish repository architecture, `.gitignore`, `.env.example`, and README | Stefan | **Done** |
| **0.4** | Initialize ASP.NET Core project, EF Core SQLite connection, and base layout | Stefan | **Done** |
| **1.1** | Create VELARIS design tokens, Tailwind palette (Gold/Onyx), Header & Footer | Andreea | **Done** |
| **1.2** | Build responsive ProductCard, catalogue grid, and category filter views (`A1`) | Andreea | **Done** |
| **1.3** | Implement product detail page with GSM badge, fit specs, and gallery | Andreea | **Done** |
| **2.1** | Define domain models (`Product`, `Category`, `User`, `CartItem`, `Order`) | Stefan | **Done** |
| **2.2** | Implement automatic SQLite database migrations and entity constraints | Stefan | **Done** |
| **2.3** | Write repeatable database seed script loading all 32 items with images | Stefan | **Done** |
| **2.4** | Implement catalogue REST API endpoints (`/api/catalog/products`) | Stefan | **Done** |
| **2.5** | Connect Razor catalog views to live SQLite database via EF Core | Andreea | **Done** |
| **3.1** | Implement ASP.NET Core Identity authentication (Argon2 / PBKDF2 hashing) (`A4`) | Stefan | **Done** |
| **3.2** | Build Register & Login views with input validation and CSRF protection (`A10`) | Andreea | **Done** |
| **3.3** | Implement role-based authorization policies (blocking customers from admin routes) | Stefan | **Done** |
| **3.4** | Connect auth state to navigation header, user profile, and session logout | Andreea | **Done** |
| **4.1** | Verify clean setup workflow on fresh clone (`dotnet run` database seed test) | Andreea + Stefan | **Done** |
| **4.2** | Test responsive mobile viewport (360px) and zero-results search state (`A11`, `A2`) | Andreea | **Done** |
| **4.3** | Compile 8-page P2 Specification PDF and 15-slide Presentation deck | Andreea + Stefan | **Done** |
| **4.4** | Merge approved milestone pull request and tag commit `P2` | Andreea + Stefan | **Done** |

---

### After Presentation 2 (Roadmap to Presentation 3)

| No. | Task Description | Requirement | Assigned To | Status |
| :---: | :--- | :---: | :---: | :---: |
| **5.1** | Filtered search by price range, size, availability with URL persistence | `A2`, `B1` | Stefan (API) + Andreea (UI) | **Planned** |
| **5.2** | Persistent shopping cart with server-side recalculation and stock validation | `A3` | Andreea | **Planned** |
| **5.3** | Customer wishlist and saved favorites list | `B7` | Andreea + Stefan | **Planned** |
| **5.4** | Multi-step checkout flow (shipping address, 2 carrier options, order summary) | `A5` | Andreea | **Planned** |
| **5.5** | Server-side validated discount promo code engine | `B5` | Stefan | **Planned** |
| **5.6** | Stripe Checkout session and asynchronous webhook order payment confirmation | `A6`, `A7` | Stefan | **Planned** |
| **5.7** | Stock management with optimistic concurrency lock (`RowVersion`) on purchase | `A7` | Stefan | **Planned** |
| **5.8** | Customer order history portal with live status transitions | `A8` | Andreea | **Planned** |
| **5.9** | Custom admin back-office for product/category CRUD and order fulfillment | `A9` | Stefan | **Planned** |
| **5.10** | Automated test suite (&ge; 10 tests covering cart, totals, and webhooks) | `A12` | Stefan | **Planned** |
| **5.11** | Final mobile hardening, P3 live demo rehearsing, and final packaging | `A10`, `A11` | Andreea + Stefan | **Planned** |

---

## 🤖 AI Tools Used

In accordance with the project guidelines, the team declares the AI tools utilized during development:
- **Claude / Gemini (Google DeepMind):** Used for architectural planning assistance, reviewing entity schema consistency, generating initial documentation drafts, and refining slide presentations.
- **Unsplash (Free License):** Sourcing high-resolution, freely licensed garment imagery for the 32 catalogue products.

---

## 👥 Authors
- **Predut Andreea-Denisa** — *Frontend Architecture & Full-Stack Developer*
- **Sapca Roman-Stefan** — *Backend Architecture & Database Developer*
