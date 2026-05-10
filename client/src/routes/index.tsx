// React import not needed — automatic JSX transform is enabled via tsconfig "jsx": "react-jsx"
import { createBrowserRouter } from 'react-router-dom'

export function createRouter() {
  return createBrowserRouter([
    {
      path: '/',
      element: <div>HolidayPlanner</div>,
    },
  ])
}
