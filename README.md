# VELARIS — Contemporary Streetwear & Sustainable Capsule Essentials

> **E-Commerce Semester Project | Academic Year 2026–2027**  
> **Course:** E-Commerce  
> **Stage:** Presentation 1 (P1) — Team Setup and Planning  
> **Commit Tag:** `P1`

---

## 👥 Team Members & Roles

| Member | Roles & Main Responsibilities | Assigned Requirements |
| :--- | :--- | :--- |
| **Predut Andreea-Denisa** | Frontend Architecture, UI/UX Design, Catalog, Cart & Checkout Flows | `A1`, `A2`, `A3`, `A5`, `A8`, `A11`, `B1`, `B7` |
| **Sapca Roman-Stefan** | Backend Architecture, Authentication, Security, Payments & Admin Panel | `A4`, `A6`, `A7`, `A9`, `A10`, `A12`, `B5` |

---

## 🛍️ Project Overview

**VELARIS** is a curated contemporary streetwear and sustainable capsule wardrobe online store. Designed for Gen-Z and young professionals (18–35), the platform focuses on versatile staples (heavyweight hoodies, organic cotton tees, structured denim, and weatherproof outerwear), eliminating the clutter and decision fatigue common in fast-fashion marketplaces.

### Key Value Propositions:
- **Curated Capsule Collections:** Strictly seeded 30+ items across 4 structured apparel categories.
- **Garment & Sizing Transparency:** Detailed fabric GSM weights and accurate fit guides.
- **Atomic Stock Certainty:** Concurrency-protected checkout preventing overselling on limited garment sizes.
- **Frictionless Payments:** Fully integrated Stripe test-mode sandbox with asynchronous webhook verification.

---

## 🛠️ Technology Stack

| Layer | Technology | Justification (1 sentence) |
| :--- | :--- | :--- |
| **Presentation Layer** | ASP.NET Core Razor Pages & Tailwind CSS | We chose Razor Views paired with Tailwind CSS to build responsive UI layouts down to 360px without requiring a detached frontend build pipeline. |
| **Application Core** | ASP.NET Core (C#) | We selected ASP.NET Core because it provides high-performance custom API handling and built-in CSRF/security middleware. |
| **Persistence Layer** | SQLite & Entity Framework Core (EF Core) | SQLite managed via EF Core ensures zero-configuration local portability and reliable ORM data integrity for products and orders. |
| **Transaction Engine** | Stripe (Test Mode) | We selected Stripe's official .NET SDK to handle test card checkouts and webhook-based payment confirmation seamlessly. |

---

## 📋 Requirements Mapping & Ownership

### Mandatory Requirements (70 Points)

| Code | Requirement | Owner | Target Phase | Status |
| :---: | :--- | :--- | :---: | :---: |
| **A1** | **Product catalog** (paginated &le; 24, category page, product detail page) | Predut Andreea-Denisa | Sprint 2 (W4–5) | Planned |
| **A2** | **Basic search** (text search on names with empty state) | Predut Andreea-Denisa | Sprint 2 (W4–5) | Planned |
| **A3** | **Shopping cart** (server total recalculation, stock rejection, auth persistence) | Predut Andreea-Denisa | Sprint 4 (W8–9) | Planned |
| **A4** | **User accounts** (bcrypt/Argon2 hashing, saved delivery address) | Sapca Roman-Stefan | Sprint 2 (W4–5) | Planned |
| **A5** | **Checkout flow** (address, &ge; 2 delivery options, summary, order number confirmation) | Predut Andreea-Denisa | Sprint 5 (W10–11) | Planned |
| **A6** | **Sandbox payment** (Stripe webhook confirmation, failure leaves Unpaid) | Sapca Roman-Stefan | Sprint 5 (W10–11) | Planned |
| **A7** | **Stock management** (stock decrease on paid, restored on cancel, race condition prevention) | Sapca Roman-Stefan | Sprint 5 (W10–11) | Planned |
| **A8** | **Order history** (customer order list with date, items, total, status) | Predut Andreea-Denisa | Sprint 4 (W8–9) | Planned |
| **A9** | **Administration panel** (custom role, product/category CRUD + images, order status workflow) | Sapca Roman-Stefan | Sprint 4 (W8–9) | Planned |
| **A10** | **Basic security** (server-side validation, CSRF, ORM anti-injection, server admin guards) | Sapca Roman-Stefan | Sprint 2 (W4–5) | Planned |
| **A11** | **Responsive design** (usable down to 360px viewport width) | Predut Andreea-Denisa | Sprint 2 (W4–5) | Planned |
| **A12** | **Automated tests** (&ge; 10 tests in 1 command covering cart, total, payment confirmation) | Sapca Roman-Stefan | Sprint 5 (W10–11) | Planned |

### Selected Bonus Features (Max 20 Points)

| Code | Bonus Feature | Points | Owner | Target Phase | Status |
| :---: | :--- | :---: | :--- | :---: | :---: |
| **B1** | **Filtered search** (category, price range, availability, sort price/date in URL) | 5 | Predut Andreea-Denisa | Sprint 6 (W12) | Planned |
| **B5** | **Discount codes** (percentage/fixed, expiry, max uses, server-validated) | 3 | Sapca Roman-Stefan | Sprint 6 (W12) | Planned |
| **B7** | **Wishlist** (favourites list for authenticated users, move to cart) | 2 | Predut Andreea-Denisa | Sprint 6 (W12) | Planned |

---

## 🗓️ Execution Timeline

- **Sprint 1 (Weeks 1–3) — Inception & Architecture:** Repository initialized, architecture defined, P1 delivered.
- **Sprint 2 (Weeks 4–5) — Data Seeding & User Auth:** 30+ seed garments across 4 categories, auth system, responsive wireframes.
- **Sprint 3 (Weeks 6–7) — Specification & Prototype (P2):** 8-page specification PDF, live DB catalog demo.
- **Sprint 4 (Weeks 8–9) — Commerce Core:** Shopping cart state, order history, and custom back-office administration.
- **Sprint 5 (Weeks 10–11) — Payments & Concurrency:** Stripe webhook integration, stock concurrency locks, 10+ automated tests.
- **Sprint 6 (Week 12) — Hardening & Bonuses (P3):** Selected bonuses (B1, B5, B7), live scenario testing, final demo.

---

## ⚠️ Top 3 Risks & Mitigations

1. **Stripe Webhook Delivery Failures & Desync (A6):**
   - *Risk:* Orders remain marked as *Unpaid* if asynchronous webhooks fail to reach the server.
   - *Mitigation:* Extensive local testing via Stripe CLI (`stripe listen --forward-to`), cryptographic signature validation, and idempotent database handlers.
2. **Multi-Size Stock Overselling (A7):**
   - *Risk:* Multiple customers attempting to purchase the final unit of a specific garment size simultaneously.
   - *Mitigation:* Atomic transactions in EF Core combined with optimistic concurrency tokens (`RowVersion`) at the payment confirmation step.
3. **Scope Creep & Apparel Complexity:**
   - *Risk:* Over-engineering complex multi-variant matrix logic before securing baseline points.
   - *Mitigation:* Strict prioritization of requirements A1–A12 first with single-SKU/variant schemas; deferring bonus features to Week 12.

---

## 🤖 AI Tools Usage Declaration

In compliance with the project guidelines:
- **Tools Used:** Antigravity / Gemini for assistance in planning documentation, structuring the presentation slides, and refining project milestones.
- **Verification:** Both team members fully understand, maintain, and can explain every architectural decision, data model, and code component upon request.
