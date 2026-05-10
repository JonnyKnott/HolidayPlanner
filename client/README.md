# HolidayPlanner — Client

React/TypeScript frontend for the HolidayPlanner application.

## Tech stack

- **Vite** — build tool and dev server
- **React 18** + **TypeScript** — UI framework, strict mode
- **MUI v6** — component library (Material Design)
- **React Router v6** — client-side routing
- **TanStack Query v5** — server state management
- **React Hook Form** + **Zod** — form handling and validation
- **Axios** — HTTP client (proxied to .NET API in dev)
- **Vitest** + **React Testing Library** + **MSW** — testing

## Getting started

Start the .NET API first (see root README), then:

```bash
npm install
npm run dev
```

App available at `http://localhost:5173`.

## Scripts

| Command | Description |
|---|---|
| `npm run dev` | Start dev server |
| `npm run build` | Production build |
| `npm run lint` | Run ESLint |
| `npm run format` | Format with Prettier |
| `npm run test` | Run tests in watch mode |
| `npm run test:run` | Run tests once |

## API proxy

The dev server proxies `/api` requests to `https://localhost:7169` (the .NET API). Ensure the API is running before making data requests.
