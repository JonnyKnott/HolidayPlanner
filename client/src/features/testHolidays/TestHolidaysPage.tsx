import { useState } from 'react'
import {
  Alert,
  Box,
  Button,
  CircularProgress,
  Container,
  IconButton,
  Typography,
} from '@mui/material'
import AddIcon from '@mui/icons-material/Add'
import DeleteIcon from '@mui/icons-material/Delete'
import EditIcon from '@mui/icons-material/Edit'
import { DataGrid } from '@mui/x-data-grid'
import type { GridColDef } from '@mui/x-data-grid'
import type { TestHoliday } from '@/api/testHolidays'
import {
  useCreateTestHoliday,
  useDeleteTestHoliday,
  useTestHolidays,
  useUpdateTestHoliday,
} from './hooks'
import { TestHolidayDialog } from './TestHolidayDialog'
import type { HolidayFormValues } from './TestHolidayDialog'

export function TestHolidaysPage() {
  const [dialogOpen, setDialogOpen] = useState(false)
  const [editingHoliday, setEditingHoliday] = useState<TestHoliday | null>(null)
  const [submitError, setSubmitError] = useState<string | null>(null)

  const { data: holidays, isLoading, isError, error } = useTestHolidays()
  const createMutation = useCreateTestHoliday()
  const updateMutation = useUpdateTestHoliday()
  const deleteMutation = useDeleteTestHoliday()

  const openAdd = () => {
    setEditingHoliday(null)
    setSubmitError(null)
    setDialogOpen(true)
  }

  const openEdit = (holiday: TestHoliday) => {
    setEditingHoliday(holiday)
    setSubmitError(null)
    setDialogOpen(true)
  }

  const closeDialog = () => {
    setDialogOpen(false)
    setEditingHoliday(null)
    setSubmitError(null)
  }

  const handleSubmit = async (values: HolidayFormValues) => {
    setSubmitError(null)
    try {
      if (editingHoliday) {
        await updateMutation.mutateAsync({ id: editingHoliday.id, body: values })
      } else {
        await createMutation.mutateAsync(values)
      }
      closeDialog()
    } catch {
      setSubmitError('An error occurred. Please try again.')
    }
  }

  const handleDelete = (id: string) => {
    deleteMutation.mutate(id)
  }

  const isSubmitting = createMutation.isPending || updateMutation.isPending

  const columns: GridColDef<TestHoliday>[] = [
    { field: 'name', headerName: 'Name', flex: 1, minWidth: 150 },
    { field: 'destination', headerName: 'Destination', width: 140 },
    { field: 'startDate', headerName: 'Start Date', width: 120 },
    { field: 'endDate', headerName: 'End Date', width: 120 },
    {
      field: 'actions',
      headerName: 'Actions',
      width: 120,
      sortable: false,
      renderCell: ({ row }) => (
        <Box>
          <IconButton
            aria-label={`Edit ${row.name}`}
            size="small"
            onClick={() => openEdit(row)}
          >
            <EditIcon fontSize="small" />
          </IconButton>
          <IconButton
            aria-label={`Delete ${row.name}`}
            size="small"
            color="error"
            onClick={() => handleDelete(row.id)}
          >
            <DeleteIcon fontSize="small" />
          </IconButton>
        </Box>
      ),
    },
  ]

  return (
    <Container maxWidth="lg" sx={{ py: 4 }}>
      <Box display="flex" justifyContent="space-between" alignItems="center" mb={3}>
        <Typography variant="h4" component="h1">
          Test Holidays
        </Typography>
        <Button variant="contained" startIcon={<AddIcon />} onClick={openAdd}>
          Add Holiday
        </Button>
      </Box>

      {isLoading && (
        <Box display="flex" justifyContent="center" py={8}>
          <CircularProgress aria-label="Loading holidays" />
        </Box>
      )}

      {isError && (
        <Alert severity="error" sx={{ mb: 2 }}>
          {error instanceof Error ? error.message : 'Failed to load holidays'}
        </Alert>
      )}

      {!isLoading && !isError && holidays !== undefined && holidays.length === 0 && (
        <Box py={8} textAlign="center">
          <Typography color="text.secondary">
            No holidays yet. Click &quot;Add Holiday&quot; to create one.
          </Typography>
        </Box>
      )}

      {!isLoading && !isError && holidays !== undefined && holidays.length > 0 && (
        <DataGrid
          rows={holidays}
          columns={columns}
          getRowId={(row) => row.id}
          autoHeight
          pageSizeOptions={[10, 25, 50]}
          initialState={{ pagination: { paginationModel: { pageSize: 10 } } }}
          disableRowSelectionOnClick
        />
      )}

      <TestHolidayDialog
        open={dialogOpen}
        onClose={closeDialog}
        onSubmit={handleSubmit}
        editingHoliday={editingHoliday}
        isSubmitting={isSubmitting}
        error={submitError}
      />
    </Container>
  )
}
