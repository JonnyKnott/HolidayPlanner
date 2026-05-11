// React import not needed — automatic JSX transform is enabled via tsconfig "jsx": "react-jsx"
import { lazy, Suspense } from 'react'
import { createBrowserRouter } from 'react-router-dom'
import { CircularProgress, Box } from '@mui/material'

// Lazy-load the test page so it is not bundled into the main chunk.
// This route is intentionally NOT linked from any permanent navigation element.
const TestHolidaysPage = lazy(() =>
  import('@/features/testHolidays/TestHolidaysPage').then((m) => ({
    default: m.TestHolidaysPage,
  }))
)

function PageFallback() {
  return (
    <Box display="flex" justifyContent="center" alignItems="center" minHeight="50vh">
      <CircularProgress />
    </Box>
  )
}

export function createRouter() {
  return createBrowserRouter([
    {
      path: '/',
      element: <div>HolidayPlanner</div>,
    },
    {
      path: '/test/holidays',
      element: (
        <Suspense fallback={<PageFallback />}>
          <TestHolidaysPage />
        </Suspense>
      ),
    },
  ])
}
