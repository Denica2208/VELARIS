# Catalogue & Product API Contract — VELARIS

The VELARIS catalogue API provides endpoints for browsing, paginating, searching, and inspecting the 32 clothing SKUs across 4 main apparel categories.

---

## Base URL
`/api/catalog` (or server-rendered routes at `/catalog`)

---

## Endpoints

### 1. List Products (Paginated & Filtered)
- **Method:** `GET`
- **Route:** `/api/catalog/products`
- **Access Level:** Public
- **Query Parameters:**
  - `page`: Page index (default: `1`, maximum 24 items per page per **A1**)
  - `pageSize`: Number of items per page (default: `12`, max: `24`)
  - `category`: Category slug (`hoodies`, `t-shirts`, `denim`, `jackets`)
  - `search`: Keyword query for product name / material (Requirement **A2**)
  - `sortBy`: `price-asc`, `price-desc`, `date-newest` (Bonus **B1**)
  - `minPrice`, `maxPrice`: Numerical price range filters (Bonus **B1**)
- **Response `200 OK`:**
```json
{
  "totalItems": 32,
  "currentPage": 1,
  "totalPages": 3,
  "pageSize": 12,
  "items": [
    {
      "id": 1,
      "sku": "VEL-HOD-01",
      "name": "Nocturne Oversized Hoodie",
      "slug": "nocturne-oversized-hoodie",
      "category": "Heavyweight Hoodies",
      "categorySlug": "hoodies",
      "price": 380.00,
      "currency": "RON",
      "stock": 24,
      "inStock": true,
      "fabricGsm": 450,
      "material": "100% GOTS Organic Cotton Heavy Fleece",
      "primaryImageUrl": "/images/products/nocturne-hoodie.jpg"
    }
  ]
}
```

---

### 2. Product Detail
- **Method:** `GET`
- **Route:** `/api/catalog/products/{slug}`
- **Access Level:** Public
- **Response `200 OK`:**
```json
{
  "id": 1,
  "sku": "VEL-HOD-01",
  "name": "Nocturne Oversized Hoodie",
  "slug": "nocturne-oversized-hoodie",
  "category": "Heavyweight Hoodies",
  "price": 380.00,
  "stock": 24,
  "fabricGsm": 450,
  "description": "Double-layered hood, seamless kangaroo pocket, pre-shrunk heavyweight fleece engineered for boxy draping.",
  "images": [
    "/images/products/nocturne-hoodie-front.jpg",
    "/images/products/nocturne-hoodie-detail.jpg"
  ],
  "availableSizes": ["S", "M", "L", "XL"]
}
```

---

### 3. Categories List
- **Method:** `GET`
- **Route:** `/api/catalog/categories`
- **Access Level:** Public
- **Response `200 OK`:**
```json
[
  { "id": 1, "name": "Heavyweight Hoodies & Sweats", "slug": "hoodies", "itemCount": 8 },
  { "id": 2, "name": "Boxy & Relaxed T-Shirts", "slug": "t-shirts", "itemCount": 8 },
  { "id": 3, "name": "Structured Denim & Cargo", "slug": "denim", "itemCount": 8 },
  { "id": 4, "name": "Weatherproof Jackets & Outerwear", "slug": "jackets", "itemCount": 8 }
]
```
