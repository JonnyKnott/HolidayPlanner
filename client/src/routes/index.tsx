// React import not needed — automatic JSX transform is enabled via tsconfig "jsx": "react-jsx"
import { createBrowserRouter } from 'react-router-dom'

export const router = createBrowserRouter([
  {
    path: '/',
    element: <div>HolidayPlanner</div>,
  },
])
