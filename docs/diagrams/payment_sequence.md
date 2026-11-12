# Critical Flow: Payment, Webhook & Concurrency Sequence Diagram

This diagram details the required critical path (Requirement **A6**, **A7**): checkout submission, payment processing, asynchronous webhook receipt, order status transition, and atomic stock decrement.

```mermaid
sequenceDiagram
    autonumber
    actor Customer as Customer (Browser)
    participant App as VELARIS Web App
    participant Stripe as Stripe Payment Gateway
    participant Webhook as Webhook Controller
    participant DB as SQLite DB (EF Core)

    Customer->>App: 1. Click "Proceed to Checkout"
    App->>DB: 2. Validate cart items & preliminary stock levels
    DB-->>App: 3. Stock verified
    App->>Stripe: 4. Create Stripe Checkout Session (Amount, Order Number metadata)
    Stripe-->>App: 5. Return session URL
    App-->>Customer: 6. Redirect to Stripe Hosted Checkout
    Customer->>Stripe: 7. Enter Sandbox Test Card (4242 4242...) & Pay

    alt Payment Succeeded
        Stripe-->>Customer: 8a. Redirect to /checkout/confirmation (Order status still "New")
        Stripe->)Webhook: 9. Async Webhook POST: checkout.session.completed
        Note over Webhook: Webhook signature cryptographic verification
        Webhook->>DB: 10. Start DB Transaction: Atomic Stock Decrement & Concurrency Check
        DB-->>Webhook: 11. Stock decreased, RowVersion updated
        Webhook->>DB: 12. Update Order Status: New -> Paid
        Webhook-->>Stripe: 13. HTTP 200 OK (Event Acknowledged)
        Customer->>App: 14. Refresh confirmation page -> Order Status: PAID
    else Payment Declined / Canceled
        Stripe-->>Customer: 8b. Payment Failed / Canceled redirect
        App->>DB: Update Order Status: Unpaid
        Customer-->>App: View Failure notification with option to retry
    end
```
