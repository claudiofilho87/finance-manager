# Finance Manager — Frontend

React 18 SPA built with Vite, TypeScript, TailwindCSS, and shadcn-ui.

---

## Getting Started

### Prerequisites

- [Bun](https://bun.sh/)

### Install dependencies

```sh
bun install
```

### Run development server

```sh
bun run dev
```

App will be available at [http://localhost:5173](http://localhost:5173).

> When running via Docker Compose, the frontend is served through Nginx at [http://localhost:3000](http://localhost:3000).

---

## Scripts

| Command | Description |
|---|---|
| `bun run dev` | Start development server |
| `bun run build` | Build for production |
| `bun run preview` | Preview production build locally |

---

## Folder Structure (`src/`)

- **pages/**: Main pages (Login, Dashboard, Register, etc.)
- **components/**: Reusable components (dialogs, UI, protected routes)
- **contexts/**: React contexts (e.g., authentication)
- **hooks/**: Custom hooks
- **services/**: API communication layer
- **types/**: Shared TypeScript types
- **lib/**: Utility functions
