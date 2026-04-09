# Finance Manager

Personal finance management application for tracking recurring bills and their occurrences.

## Project Structure

- `Backend/` — Backend API (.NET 8, C#, PostgreSQL, Clean Architecture)
  - `src/` — Domain, Application, Infrastructure, API projects
  - `FinanceManager.sln` — Solution file
- `Frontend/` — Frontend SPA (React 18, TypeScript, Vite)
- `Docker/` — Dockerfiles and nginx config
- `docker-compose.yaml` — Development environment
- `docker-compose.production.yaml` — Production environment

## Running Locally

```bash
# Start all services (backend, frontend, postgres)
docker compose up -d

# Frontend: http://localhost:3000
# Backend API: http://localhost:5050
# Swagger: http://localhost:5050/swagger
```

## Architecture Overview

- **Nginx** reverse proxy on port 3000, routes `/backend/` to the .NET API
- **JWT authentication** with refresh token flow
- **PostgreSQL 17** database with EF Core migrations (auto-applied on startup)
- Monetary values stored as **cents (long)** to avoid floating-point issues

## Conventions

- Language: code in English, UI labels and user-facing text in **Brazilian Portuguese (pt-BR)**
- Commit messages in English, prefixed with type: `fix:`, `feat:`, `refactor:`, `docs:`, `chore:`
- All API responses wrapped in `ApiResponse<T>`
- All queries scoped by authenticated UserId (multi-tenant isolation)
