# Authentication API Contract — VELARIS

The VELARIS authentication system uses ASP.NET Core Identity with secure password hashing (Argon2 / PBKDF2), anti-forgery CSRF tokens, and role-based access control (`Customer` and `Admin`).

---

## Base URL
`/api/auth` (or Razor Pages `/Account/*`)

---

## Endpoints

### 1. Register User
- **Method:** `POST`
- **Route:** `/api/auth/register`
- **Access Level:** Public
- **Request Body:**
```json
{
  "email": "customer@velaris.com",
  "fullName": "Andreea Predut",
  "password": "SecurePassword123!",
  "confirmPassword": "SecurePassword123!",
  "shippingAddress": "Str. Libertatii nr. 10, Craiova, Dolj"
}
```
- **Responses:**
  - `201 Created`: User account initialized and session cookie established.
  - `400 Bad Request`: Email already in use or validation failure.

---

### 2. Login User
- **Method:** `POST`
- **Route:** `/api/auth/login`
- **Access Level:** Public
- **Request Body:**
```json
{
  "email": "customer@velaris.com",
  "password": "SecurePassword123!",
  "rememberMe": true
}
```
- **Responses:**
  - `200 OK`: Successful authentication, session established.
  - `401 Unauthorized`: Invalid credentials.

---

### 3. Logout
- **Method:** `POST`
- **Route:** `/api/auth/logout`
- **Access Level:** Authenticated
- **Response:**
  - `200 OK`: Session cookie cleared.

---

### 4. Current User Profile (`/me`)
- **Method:** `GET`
- **Route:** `/api/auth/me`
- **Access Level:** Authenticated (`Customer` or `Admin`)
- **Response:**
```json
{
  "id": "usr_7f8c9b2e",
  "email": "customer@velaris.com",
  "fullName": "Andreea Predut",
  "role": "Customer",
  "defaultAddress": "Str. Libertatii nr. 10, Craiova, Dolj",
  "createdAt": "2026-10-16T10:00:00Z"
}
```

---

### 5. Role Enforcement Policy
- Customers attempting to access routes prefixed with `/admin` or `/api/admin/*` receive an immediate `403 Forbidden` at the server middleware level (Requirement **A10**).
