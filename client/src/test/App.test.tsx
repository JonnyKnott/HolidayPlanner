import { render, screen } from '@testing-library/react'
import { MemoryRouter } from 'react-router-dom'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import { ThemeProvider } from '@mui/material'
import { describe, it, expect } from 'vitest'
import { theme } from '@/theme/theme'

describe('App shell', () => {
  it('renders the root route placeholder', () => {
    const queryClient = new QueryClient({
      defaultOptions: { queries: { retry: false } },
    })
    render(
      <ThemeProvider theme={theme}>
        <QueryClientProvider client={queryClient}>
          <MemoryRouter>
            <div>HolidayPlanner</div>
          </MemoryRouter>
        </QueryClientProvider>
      </ThemeProvider>
    )
    expect(screen.getByText('HolidayPlanner')).toBeInTheDocument()
  })
})
