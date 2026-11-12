# Entity-Relationship Diagram (ERD) — VELARIS

The following diagram defines the complete relational data model powering the VELARIS online store, implemented using SQLite and Entity Framework Core.

```mermaid
erDiagram
    CATEGORY ||--o{ PRODUCT : contains
    PRODUCT ||--o{ PRODUCT_IMAGE : has
    PRODUCT ||--o{ CART_ITEM : included_in
    PRODUCT ||--o{ ORDER_ITEM : ordered_in
    USER ||--o{ CART_ITEM : owns
    USER ||--o{ ORDER : places
    USER ||--o{ WISHLIST_ITEM : saves
    PRODUCT ||--o{ WISHLIST_ITEM : favorited
    ORDER ||--o{ ORDER_ITEM : contains
    ORDER ||--o| PAYMENT_TRANSACTION : verified_by
    DISCOUNT_COUPON ||--o{ ORDER : applies_to

    CATEGORY {
        int Id PK
        string Name
        string Slug
        string Description
    }

    PRODUCT {
        int Id PK
        int CategoryId FK
        string SKU
        string Name
        string Slug
        string Description
        decimal Price
        int StockLevel
        int FabricGsm
        string MaterialComposition
        byte[] RowVersion
        datetime CreatedAt
    }

    PRODUCT_IMAGE {
        int Id PK
        int ProductId FK
        string ImageUrl
        bool IsPrimary
        int DisplayOrder
    }

    USER {
        string Id PK
        string Email
        string FullName
        string PasswordHash
        string Role
        string ShippingAddress
        string City
        string PostalCode
        datetime CreatedAt
    }

    CART_ITEM {
        int Id PK
        string UserId FK
        int ProductId FK
        int Quantity
        string SelectedSize
        datetime UpdatedAt
    }

    ORDER {
        int Id PK
        string OrderNumber
        string UserId FK
        decimal Subtotal
        decimal ShippingCost
        decimal DiscountAmount
        decimal TotalAmount
        string Status
        string ShippingAddress
        string DeliveryMethod
        string StripeSessionId
        datetime CreatedAt
    }

    ORDER_ITEM {
        int Id PK
        int OrderId FK
        int ProductId FK
        string ProductName
        string SKU
        string SelectedSize
        decimal UnitPrice
        int Quantity
    }

    PAYMENT_TRANSACTION {
        int Id PK
        int OrderId FK
        string StripePaymentIntentId
        string PaymentStatus
        string Currency
        decimal AmountCaptured
        datetime WebhookProcessedAt
    }

    DISCOUNT_COUPON {
        int Id PK
        string Code
        decimal DiscountPercent
        decimal FixedAmount
        datetime ExpiryDate
        int MaxUses
        int TimesUsed
        bool IsActive
    }

    WISHLIST_ITEM {
        int Id PK
        string UserId FK
        int ProductId FK
        datetime AddedAt
    }
```
