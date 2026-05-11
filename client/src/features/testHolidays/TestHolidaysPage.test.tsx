import { render, screen, waitFor, within, fireEvent } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import { ThemeProvider } from '@mui/material'
import { vi, describe, it, expect, beforeEach } from 'vitest'
import { theme } from '@/theme/theme'
import { TestHolidaysPage } from './TestHolidaysPage'
import type { TestHoliday } from '@/api/testHolidays'

// ── Mock the API module so all HTTP is controlled without real networking ──────
vi.mock('@/api/testHolidays', () => ({
  getTestHolidays: vi.fn(),
  getTestHolidayById: vi.fn(),
  createTestHoliday: vi.fn(),
  updateTestHoliday: vi.fn(),
  deleteTestHoliday: vi.fn(),
  TEST_DESTINATIONS: ['Mexico', 'Japan', 'New Zealand', 'Iceland', 'Scotland'],
}))

import {
  getTestHolidays,
  createTestHoliday,
  updateTestHoliday,
  deleteTestHoliday,
} from '@/api/testHolidays'

const mockGetTestHolidays = vi.mocked(getTestHolidays)
const mockCreateTestHoliday = vi.mocked(createTestHoliday)
const mockUpdateTestHoliday = vi.mocked(updateTestHoliday)
const mockDeleteTestHoliday = vi.mocked(deleteTestHoliday)

const HOLIDAY_1: TestHoliday = {
  id: '018f4e1a-0000-7000-8000-000000000001',
  name: 'Japan Adventure',
  destination: 'Japan',
  startDate: '2026-07-01',
  endDate: '2026-07-14',
  createdOn: '2026-05-10T00:00:00Z',
  modifiedOn: '2026-05-10T00:00:00Z',
}

const HOLIDAY_2: TestHoliday = {
  id: '018f4e1a-0000-7000-8000-000000000002',
  name: 'Mexico Retreat',
  destination: 'Mexico',
  startDate: '2026-08-01',
  endDate: '2026-08-07',
  createdOn: '2026-05-10T00:00:00Z',
  modifiedOn: '2026-05-10T00:00:00Z',
}

function renderPage() {
  const queryClient = new QueryClient({
    defaultOptions: { queries: { retry: false }, mutations: { retry: false } },
  })
  return render(
    <ThemeProvider theme={theme}>
      <QueryClientProvider client={queryClient}>
        <TestHolidaysPage />
      </QueryClientProvider>
    </ThemeProvider>,
  )
}

beforeEach(() => {
  vi.clearAllMocks()
  mockGetTestHolidays.mockResolvedValue([HOLIDAY_1, HOLIDAY_2])
  mockCreateTestHoliday.mockResolvedValue('new-id')
  mockUpdateTestHoliday.mockResolvedValue(undefined)
  mockDeleteTestHoliday.mockResolvedValue(undefined)
})

// ── Opens the dialog, fills dates and submits — helper ──────────────────────
async function openDialogAndSubmit(
  dialog: HTMLElement,
  overrides: { name?: string; startDate?: string; endDate?: string } = {},
) {
  const { name = 'Test Holiday', startDate = '2027-01-01', endDate = '2027-01-14' } = overrides

  const nameInput = within(dialog).getByRole('textbox', { name: /Name/i })
  await userEvent.clear(nameInput)
  await userEvent.type(nameInput, name)

  // Date inputs need fireEvent.change since jsdom date pickers don't behave like real browsers
  const dateInputs = within(dialog).getAllByDisplayValue(
    (value) => value === '' || /^\d{4}-\d{2}-\d{2}$/.test(value),
  )
  const startInput = dateInputs.find(
    (el) => (el as HTMLInputElement).name === 'startDate',
  ) as HTMLInputElement | undefined
  const endInput = dateInputs.find(
    (el) => (el as HTMLInputElement).name === 'endDate',
  ) as HTMLInputElement | undefined

  if (startInput) fireEvent.change(startInput, { target: { value: startDate } })
  if (endInput) fireEvent.change(endInput, { target: { value: endDate } })

  await userEvent.click(within(dialog).getByRole('button', { name: /Save/i }))
}

describe('TestHolidaysPage', () => {
  it('shows a loading indicator while data is fetching', () => {
    mockGetTestHolidays.mockImplementation(() => new Promise(() => { /* never resolves */ }))

    renderPage()

    expect(screen.getByLabelText('Loading holidays')).toBeInTheDocument()
  })

  it('shows holiday rows after data loads', async () => {
    renderPage()

    await waitFor(() => {
      expect(screen.getByText('Japan Adventure')).toBeInTheDocument()
    })
    expect(screen.getByText('Mexico Retreat')).toBeInTheDocument()
  })

  it('shows the empty state when there are no holidays', async () => {
    mockGetTestHolidays.mockResolvedValue([])

    renderPage()

    await waitFor(() => {
      expect(screen.getByText(/No holidays yet/i)).toBeInTheDocument()
    })
  })

  it('shows an error alert when the fetch fails', async () => {
    mockGetTestHolidays.mockRejectedValue(new Error('Server error'))

    renderPage()

    await waitFor(() => {
      expect(screen.getByRole('alert')).toBeInTheDocument()
    })
  })

  it('opens the Add Holiday dialog when the Add button is clicked', async () => {
    renderPage()

    await waitFor(() => screen.getByText('Japan Adventure'))

    await userEvent.click(screen.getByRole('button', { name: /Add Holiday/i }))

    const dialog = screen.getByRole('dialog')
    expect(dialog).toBeInTheDocument()
    expect(within(dialog).getByRole('heading', { name: 'Add Holiday' })).toBeInTheDocument()
  })

  it('calls createTestHoliday when a valid new holiday is submitted', async () => {
    renderPage()

    await waitFor(() => screen.getByText('Japan Adventure'))
    await userEvent.click(screen.getByRole('button', { name: /Add Holiday/i }))

    const dialog = screen.getByRole('dialog')
    await openDialogAndSubmit(dialog, { name: 'New Trip' })

    await waitFor(() => expect(mockCreateTestHoliday).toHaveBeenCalledWith(
      expect.objectContaining({ name: 'New Trip' }),
    ))
  })

  it('shows a validation error when Name is empty on submit', async () => {
    renderPage()

    await waitFor(() => screen.getByText('Japan Adventure'))
    await userEvent.click(screen.getByRole('button', { name: /Add Holiday/i }))

    const dialog = screen.getByRole('dialog')
    // Submit without filling in the Name field
    await userEvent.click(within(dialog).getByRole('button', { name: /Save/i }))

    await waitFor(() => {
      expect(within(dialog).getByText('Name is required')).toBeInTheDocument()
    })
  })

  it('opens the Edit dialog pre-filled when the Edit button is clicked', async () => {
    renderPage()

    await waitFor(() => screen.getByText('Japan Adventure'))
    await userEvent.click(screen.getByLabelText('Edit Japan Adventure'))

    await waitFor(() => {
      const dialog = screen.getByRole('dialog')
      expect(within(dialog).getByRole('heading', { name: 'Edit Holiday' })).toBeInTheDocument()
      const nameInput = within(dialog).getByRole('textbox', { name: /Name/i }) as HTMLInputElement
      expect(nameInput.value).toBe('Japan Adventure')
    })
  })

  it('calls updateTestHoliday when an edited holiday is saved', async () => {
    renderPage()

    await waitFor(() => screen.getByText('Japan Adventure'))
    await userEvent.click(screen.getByLabelText('Edit Japan Adventure'))

    await waitFor(() => screen.getByRole('dialog'))
    const dialog = screen.getByRole('dialog')

    const nameInput = within(dialog).getByRole('textbox', { name: /Name/i }) as HTMLInputElement
    await userEvent.clear(nameInput)
    await userEvent.type(nameInput, 'Japan Adventure Updated')

    await userEvent.click(within(dialog).getByRole('button', { name: /Save/i }))

    await waitFor(() =>
      expect(mockUpdateTestHoliday).toHaveBeenCalledWith(
        HOLIDAY_1.id,
        expect.objectContaining({ name: 'Japan Adventure Updated' }),
      ),
    )
  })

  it('calls deleteTestHoliday when the Delete button is clicked', async () => {
    renderPage()

    await waitFor(() => screen.getByText('Japan Adventure'))
    await userEvent.click(screen.getByLabelText('Delete Japan Adventure'))

    await waitFor(() =>
      expect(mockDeleteTestHoliday).toHaveBeenCalledWith(HOLIDAY_1.id),
    )
  })
})
