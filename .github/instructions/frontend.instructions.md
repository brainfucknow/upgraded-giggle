---
applyTo: "frontend/**/*.ts,frontend/**/*.tsx,frontend/**/*.css"
---

## Folder Structure

- `/frontend`: React + TypeScript frontend with Vite build tool

## Tech Stack

### Frontend
- **React 19** with TypeScript and Vite
- **TanStack Query (React Query)** for server state management and API calls
- **React Router** for client-side routing
- **Tailwind CSS** for styling (to be configured)
- **ESLint** for code quality

## Frontend Architecture Guidelines

### Component Structure
- Use **functional components** with hooks exclusively
- Prefer **composition over inheritance**
- Create reusable UI components in `/frontend/src/components/ui/`
- Feature-based folder organization: `/frontend/src/features/{feature-name}/`
- Shared utilities in `/frontend/src/lib/` and `/frontend/src/utils/`

### State Management
- Use **React Query** for server state (API data, caching, synchronization)
- Use **React hooks** (useState, useReducer) for local component state
- Use **React Context** only for truly global UI state (theme, auth status)
- Avoid prop drilling - prefer composition and React Query

### API Integration
- All API calls through **React Query hooks**
- Create custom hooks for each API endpoint (e.g., `useGetPosts`, `useCreatePost`)
- Implement proper **error handling** and **loading states**
- Use **TypeScript interfaces** for all API responses
- Implement **optimistic updates** for better UX

## TypeScript Coding Standards

### Code Style
- Use **semicolons** at the end of statements
- Use **single quotes** for strings
- Use **template literals** for string interpolation
- **2-space indentation**
- **Trailing commas** in objects and arrays
- **Arrow functions** for callbacks and short functions

### TypeScript Specific
- **Strict TypeScript** configuration enabled
- Define **interfaces** for all props, API responses, and data structures
- Use **type-only imports** when importing types: `import type { User } from './types'`
- Prefer **const assertions** for readonly data: `as const`
- Use **generic types** appropriately for reusable components
- Avoid `any` type - use `unknown` if necessary

### React Best Practices
- Use **function declarations** for components: `function ComponentName() {}`
- Use **named exports** for components
- Implement **proper error boundaries** for error handling
- Use **React.memo()** for performance optimization when needed
- Use **custom hooks** to extract and reuse stateful logic
- Implement **proper loading and error states** in all components

### File Naming Conventions
- **PascalCase** for component files: `BlogPost.tsx`
- **camelCase** for utility files: `apiClient.ts`
- **kebab-case** for style files: `blog-post.module.css`
- Use **index.ts** files for clean imports

### Component Guidelines
- **Single responsibility** - one component per file
- **Props interface** defined above each component
- **Default props** using ES6 default parameters
- **Conditional rendering** using logical operators or ternary
- **Event handlers** prefixed with `handle`: `handleSubmit`, `handleClick`

### Performance
- Use **React.lazy()** and **Suspense** for code splitting
- Implement **virtual scrolling** for large lists
- Use **useMemo** and **useCallback** judiciously (not everywhere)
- Optimize **re-renders** with proper dependency arrays

### Accessibility
- Use **semantic HTML** elements
- Include **ARIA attributes** where needed
- Ensure **keyboard navigation** works properly
- Maintain **color contrast** standards
- Use **screen reader** friendly descriptions

### Testing (when implemented)
- Use **React Testing Library** for component testing
- Test **user interactions** not implementation details
- Write **integration tests** for critical user flows
- Use **MSW** for API mocking in tests
