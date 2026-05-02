# ASOMS (Al-Shater Order Management System)

ASOMS is a full-stack order management system with a Vue 3 frontend and an ASP.NET Core backend, built to support order workflows for both end users and administrators.

---

## Overview

ASOMS provides a web-based interface for browsing products, placing orders, and tracking order history, alongside an admin area for managing products, orders, customers, and high-level business reporting.

### Who it's for
- **Users/Customers** who browse items and place/track orders
- **Admins/Operators** who manage products, customers, orders, and reporting

### Problem it solves
It centralizes product browsing, ordering, tracking, and administrative management in a single system—reducing manual coordination and improving visibility across orders and customers.

### High-level architecture
- **Frontend (Vue 3 + Vite):** Single Page Application (SPA) with role-based navigation.
- **Backend (ASP.NET Core .NET 8):** API with JWT-based authentication, database persistence via Entity Framework Core, and realtime notifications.
- **Realtime:** SignalR is used for server-to-client updates.
- **Documents/Reporting:** PDF generation capabilities are included.

> Note: Production URLs, infrastructure details, and operational endpoints are intentionally not documented here.

---

## Features

### User-facing
- Account authentication flows (login, signup, password reset)
- Home/dashboard experience
- Product browsing and product details
- Checkout flow
- Order history
- Profile management

### Admin-facing
- Admin area protected by role-based access
- Orders management
- Customers management
- Products management
- Analytics/reporting views
- Settings area

### Platform capabilities
- Role-based route guarding (user vs admin)
- Realtime notifications/events via SignalR
- Server-side PDF generation (e.g., invoices/documents)

---

## Tech Stack

### Frontend
- Vue 3, Vite
- Vue Router
- Pinia
- Axios
- Tailwind CSS
- Font Awesome
- Chart.js / vue-chartjs
- PWA support (via Vite PWA plugin)
- SignalR JavaScript client

### Backend
- ASP.NET Core (.NET 8)
- Swagger/OpenAPI (enabled in development)
- JWT authentication
- SignalR

### Database / ORM
- PostgreSQL (via Npgsql provider)
- Entity Framework Core

### Tools & Libraries
- QuestPDF / PdfSharpCore (PDF generation)
- CsvHelper / EPPlus (CSV/Excel handling)

### Deployment
- Dockerfile included for backend container builds
- Frontend is compatible with static hosting providers

---

## Project Structure

Simplified structure (key folders only):

```text
.
├── asoms-frontend/           # Vue 3 + Vite frontend (SPA)
│   ├── src/
│   │   ├── router/           # Routes + navigation guards
│   │   ├── stores/           # Pinia stores (auth, etc.)
│   │   ├── views/            # Pages (user + admin)
│   │   └── components/       # Reusable UI components
│   ├── public/
│   ├── package.json
│   └── vite.config.js
├── ASOMS.Cms/                # ASP.NET Core API (host project)
│   ├── Controllers/          # API endpoints (auth, orders, products, analytics, users)
│   ├── Services/             # Business logic + realtime hub + PDF generation
│   ├── appsettings*.json     # Local configuration (do not commit secrets)
│   └── Program.cs            # App startup, DI, middleware
├── ASOMS.DAL/                # Data access layer (EF Core, migrations, seeding)
├── ASOMS.Core/               # Shared contracts, DTOs, interfaces, constants
└── Dockerfile                # Backend container build/publish
```

---

## Installation (Local Development)

### Prerequisites
- Node.js (LTS recommended) + npm
- .NET SDK 8
- PostgreSQL (local instance or container)

### 1) Clone
```bash
git clone https://github.com/alshatermoataz/ASOMS.git
cd ASOMS
```

### 2) Frontend
```bash
cd asoms-frontend
npm install
npm run dev
```

### 3) Backend
```bash
cd ASOMS.Cms
dotnet restore
dotnet run
```

Swagger UI is available in **development mode**.

### 4) Database
- Create a local PostgreSQL database.
- Configure the backend connection string and JWT settings using **environment variables** (recommended) or local-only config.
- Apply migrations according to your workflow.

> This project includes database seeding logic that may run at startup. Use a local/dev database and adjust seed behavior as needed for your environment.

---

## Environment Variables (Safe Placeholders)

### Backend (.NET)

Set secrets through environment variables (recommended). Example placeholders:

```bash
# Connection string (PostgreSQL)
export ConnectionStrings__DefaultConnection="Host=127.0.0.1;Port=5432;Username=your_database_user;Password=your_database_password;Database=your_database_name"

# JWT
export JwtSettings__Secret="your_jwt_secret_at_least_32_chars"
export JwtSettings__Issuer="your_issuer"
export JwtSettings__Audience="your_audience"
export JwtSettings__ExpiryMinutes="1440"
```

### Frontend (Vite)

Create `asoms-frontend/.env.local` (do not commit) with placeholders:

```env
VITE_API_BASE_URL=http://localhost:5000
VITE_SIGNALR_HUB_URL=http://localhost:5000/hubs/notifications
```

---

## Running with Docker (Backend)

A Dockerfile is provided for containerizing the backend. Example (placeholders only):

```bash
docker build -t asoms-backend .

docker run --rm -p 8080:80 \
  -e "ConnectionStrings__DefaultConnection=Host=your_db_host;Port=5432;Username=your_database_user;Password=your_database_password;Database=your_database_name" \
  -e "JwtSettings__Secret=your_jwt_secret_at_least_32_chars" \
  -e "JwtSettings__Issuer=your_issuer" \
  -e "JwtSettings__Audience=your_audience" \
  -e "JwtSettings__ExpiryMinutes=1440" \
  asoms-backend
```

---

## Realtime Notifications

The backend exposes a SignalR hub for realtime events, and the frontend subscribes using the SignalR JS client.

Security best practices (high-level):
- Restrict CORS origins per environment
- Require authentication/authorization where appropriate
- Avoid exposing internal operational actions through public realtime channels

---

## Contributing

1. Create a feature branch
2. Make changes with clear commits
3. Open a Pull Request describing:
   - what changed
   - how to test
   - any configuration required

---

## License

No license file was found in the repository. If you intend this project to be open source, add a `LICENSE` and update this section accordingly.
