# Frontend — React + TypeScript + Vite

## Quick Reference

```bash
# Install dependencies
npm install

# Dev server
npm run dev

# Build
npm run build

# Lint
npm run lint
```

## Architecture

```
src/
├── pages/          → Route-level components (Dashboard, Login, Register)
├── components/     → App-specific components (BillDialog, ProtectedRoute)
│   └── ui/         → shadcn/ui primitives (do not edit manually — use shadcn CLI)
├── contexts/       → React Context providers (AuthContext)
├── services/       → API service layer (axios-based)
├── types/          → TypeScript type definitions (api.types.ts)
├── hooks/          → Custom React hooks
├── lib/            → Utilities (utils.ts — cn() helper)
├── App.tsx         → Root component, routing, providers
└── main.tsx        → Entry point
```

## Key Patterns

- **AuthContext** — global auth state via Context API + `useAuth()` hook
- **Service layer** — all API calls centralized in `services/` (never call axios directly from components)
- **Protected routes** — `ProtectedRoute` wrapper redirects unauthenticated users
- **Axios interceptors** in `api.ts` — auto-inject JWT, auto-refresh on 401
- **Token storage** — localStorage (`token`, `refreshToken`)
- **Form handling** — React Hook Form + Zod schema validation
- **State management** — TanStack React Query for server state, useState for local UI state

## UI Stack

- **shadcn/ui** components (Radix UI + Tailwind) — 48 primitives in `components/ui/`
- **Tailwind CSS** — utility-first styling, dark mode support
- **Lucide React** — icon library
- **Recharts** — charts/graphs
- **Sonner** — toast notifications
- **date-fns** with `pt-BR` locale for date formatting

## Conventions

- Functional components with hooks (no class components)
- TypeScript strict mode
- UI text in Brazilian Portuguese (pt-BR)
- API base URL: `/backend` (proxied by nginx to .NET API)
- Monetary values come from API as cents — convert for display
- Path alias: `@/` maps to `src/`

## Adding shadcn/ui Components

```bash
npx shadcn-ui@latest add <component-name>
```

Do not manually edit files in `components/ui/` — they are managed by shadcn CLI.
