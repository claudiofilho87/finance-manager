# Finance Manager

Manage your personal finances simply and efficiently!  
This project consists of a .NET backend, a React + Vite + TailwindCSS frontend, and Docker infrastructure to facilitate development and deployment.

---

## Project Structure

```
proj-finance-manager/
├── Docker/           # Orchestration and infrastructure (Docker, Nginx)
├── FinanceManager/   # Backend (.NET 8, WebAPI)
└── Frontend/         # Frontend (React, Vite, TailwindCSS)
```

---

## Technologies Used

- **Frontend:** React, Vite, TypeScript, TailwindCSS
- **Backend:** .NET 8, ASP.NET Core WebAPI, Entity Framework Core
- **Infrastructure:** Docker, Docker Compose, Nginx

---

## How to Run Locally

### Prerequisites

- [Docker](https://www.docker.com/)
- [Node.js](https://nodejs.org/) (optional, to run the frontend outside Docker)
- [.NET 8 SDK](https://dotnet.microsoft.com/download) (optional, to run the backend outside Docker)

### 1. Environment Variables Setup

Copy the example files and adjust as needed:

```sh
cp Docker/.env.example Docker/.env
```

### 2. Running Everything with Docker

From the project root, run:

```sh
cd Docker
docker compose up --build
```

- The frontend will be available at: [http://localhost:3000](http://localhost:3000)
- The backend will be available at: [http://localhost:5000](http://localhost:5000) (or as configured)
- Nginx can be used as a reverse proxy, as configured.

### 3. Running Manually (without Docker)

#### Backend

```sh
cd FinanceManager
dotnet build
dotnet run
```

#### Frontend

```sh
cd Frontend
npm install
npm run dev
```

---

## Folder Structure

### Frontend

- **src/pages/**: Main pages (Login, Dashboard, Register, etc)
- **src/components/**: Reusable components (dialogs, UI, protected routes)
- **src/contexts/**: React contexts (e.g., authentication)
- **src/services/**: Services for API communication
- **src/types/**: Shared TypeScript types
- **public/**: Static files

### Backend (FinanceManager)

- **Controllers/**: REST API controllers
- **DTOs/**: Data Transfer Objects
- **Models/**: Data models and enums
- **Services/**: Business logic and interfaces
- **Data/**: Entity Framework context
- **Migrations/**: Database migrations
- **Logs/**: Execution logs
- **Shared/**: Utility classes and standard responses

### Docker

- **docker-compose.yaml**: Service orchestration
- **Backend/**: Backend Dockerfile
- **Frontend/**: Frontend Dockerfile
- **nginx/**: Nginx configuration

---

## Useful Scripts

### Frontend

- `npm run dev` — starts the development server
- `npm run build` — generates the production build

### Backend

- `dotnet run` — starts the API
- `dotnet ef database update` — applies migrations

---

## Notes

- Adjust environment variables according to your development environment.
- The project uses JWT authentication.
- The frontend consumes the backend API via REST endpoints.

---

## License

This project is licensed under the MIT License.