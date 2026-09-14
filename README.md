# Finance Manager

Manage your personal finances simply and efficiently!
This project consists of a .NET 8 backend (Clean Architecture), a React + Vite + TailwindCSS frontend, and Docker infrastructure to facilitate development and deployment.

---

## Project Structure

```
finance-manager/
├── .github/workflows/    # CI/CD pipeline (GitHub Actions)
├── Backend/              # Backend (.NET 8, Clean Architecture)
│   ├── src/
│   │   ├── FinanceManager.API/            # Controllers, Middlewares, entry point
│   │   ├── FinanceManager.Application/    # DTOs, Services, Interfaces, Mappings
│   │   ├── FinanceManager.Domain/         # Entities, Enums, Factories
│   │   └── FinanceManager.Infrastructure/ # EF Core, Migrations, Repositories
│   └── FinanceManager.sln
├── Docker/               # Dockerfiles and Nginx config
├── Frontend/             # Frontend (React, Vite, TypeScript, TailwindCSS)
├── docker-compose.yaml            # Development environment
└── docker-compose.production.yaml # Production environment (pulls images built by CI)
```

---

## Technologies Used

- **Frontend:** React 18, Vite, TypeScript, TailwindCSS, shadcn-ui
- **Backend:** .NET 8, ASP.NET Core WebAPI, Entity Framework Core, PostgreSQL 17
- **Infrastructure:** Docker, Docker Compose, Nginx
- **CI/CD:** GitHub Actions, GitHub Container Registry (GHCR)
- **Auth:** JWT with refresh token flow

---

## How to Run Locally

### Prerequisites

- [Docker](https://www.docker.com/)
- [Bun](https://bun.sh/) (optional, to run the frontend outside Docker)
- [.NET 8 SDK](https://dotnet.microsoft.com/download) (optional, to run the backend outside Docker)

### 1. Environment Variables Setup

Copy the example file and adjust as needed:

```sh
cp Docker/.env.example Docker/.env
```

### 2. Running Everything with Docker

From the project root:

```sh
docker compose up -d
```

- Frontend: [http://localhost:3000](http://localhost:3000)
- Backend API: [http://localhost:5050](http://localhost:5050)
- Swagger: [http://localhost:5050/swagger](http://localhost:5050/swagger)

### 3. Running Manually (without Docker)

#### Backend

```sh
cd Backend
dotnet run --project src/FinanceManager.API
```

#### Frontend

```sh
cd Frontend
bun install
bun run dev
```

---

## CI/CD

On every push to `main` (or manually via `workflow_dispatch`), [`.github/workflows/docker-publish.yml`](.github/workflows/docker-publish.yml) builds the backend and frontend Docker images and pushes them to the GitHub Container Registry:

- `ghcr.io/claudiofilho87/finance-manager-backend:latest`
- `ghcr.io/claudiofilho87/finance-manager-frontend:latest`

`docker-compose.production.yaml` pulls these prebuilt images directly instead of building locally. To deploy:

```sh
docker compose -f docker-compose.production.yaml up -d
```

In production, the frontend container is exposed on port `80` (behind a Cloudflare proxy), and the backend on port `5050`.

---

## Folder Structure

### Frontend (`Frontend/src/`)

- **pages/**: Main pages (Login, Dashboard, Register, etc.)
- **components/**: Reusable components (dialogs, UI, protected routes)
- **contexts/**: React contexts (e.g., authentication)
- **hooks/**: Custom hooks
- **services/**: API communication layer
- **types/**: Shared TypeScript types
- **lib/**: Utility functions

### Backend (`Backend/src/`)

- **FinanceManager.API**: Controllers, Middlewares, Helpers, entry point (`Program.cs`)
- **FinanceManager.Application**: DTOs, Services, Interfaces, Mappings, shared responses
- **FinanceManager.Domain**: Entities, Enums, Factories and domain interfaces
- **FinanceManager.Infrastructure**: EF Core context, Migrations, Repositories, background jobs, authentication

### Docker (`Docker/`)

- **Backend/**: Backend Dockerfile
- **Frontend/**: Frontend Dockerfile
- **nginx/**: Nginx configuration (reverse proxy)

---

## Useful Scripts

### Frontend

- `bun run dev` — starts the development server
- `bun run build` — generates the production build

### Backend

- `dotnet run --project src/FinanceManager.API` — starts the API
- `dotnet ef database update --project src/FinanceManager.Infrastructure` — applies migrations

---

## Notes

- Monetary values are stored as **cents (long)** to avoid floating-point issues.
- All API responses are wrapped in `ApiResponse<T>`.
- All queries are scoped by the authenticated user (multi-tenant isolation).
- EF Core migrations are applied automatically on startup.

---

## License

This project is licensed under the MIT License.
