import { useEffect } from 'react'
import {
  Alert,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  FormControl,
  FormHelperText,
  InputLabel,
  MenuItem,
  Select,
  Stack,
  TextField,
} from '@mui/material'
import { Controller, useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { z } from 'zod'
import { TEST_DESTINATIONS } from '@/api/testHolidays'
import type { TestHoliday } from '@/api/testHolidays'

const schema = z
  .object({
    name: z.string().min(1, 'Name is required'),
    destination: z.enum(['Mexico', 'Japan', 'New Zealand', 'Iceland', 'Scotland']),
    startDate: z.string().min(1, 'Start date is required'),
    endDate: z.string().min(1, 'End date is required'),
  })
  .superRefine((data, ctx) => {
    if (data.startDate && data.endDate && data.endDate <= data.startDate) {
      ctx.addIssue({
        code: z.ZodIssueCode.custom,
        message: 'End date must be after start date',
        path: ['endDate'],
      })
    }
  })

export type HolidayFormValues = z.infer<typeof schema>

interface Props {
  open: boolean
  onClose: () => void
  onSubmit: (values: HolidayFormValues) => Promise<void>
  editingHoliday: TestHoliday | null
  isSubmitting: boolean
  error: string | null
}

export function TestHolidayDialog({
  open,
  onClose,
  onSubmit,
  editingHoliday,
  isSubmitting,
  error,
}: Props) {
  const {
    control,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<HolidayFormValues>({
    resolver: zodResolver(schema),
    defaultValues: {
      name: '',
      destination: 'Mexico',
      startDate: '',
      endDate: '',
    },
  })

  useEffect(() => {
    if (open) {
      if (editingHoliday) {
        reset({
          name: editingHoliday.name,
          destination:
            editingHoliday.destination as HolidayFormValues['destination'],
          startDate: editingHoliday.startDate,
          endDate: editingHoliday.endDate,
        })
      } else {
        reset({ name: '', destination: 'Mexico', startDate: '', endDate: '' })
      }
    }
  }, [open, editingHoliday, reset])

  const handleClose = () => {
    reset()
    onClose()
  }

  return (
    <Dialog open={open} onClose={handleClose} maxWidth="sm" fullWidth>
      <form onSubmit={handleSubmit(onSubmit)} noValidate>
        <DialogTitle>{editingHoliday ? 'Edit Holiday' : 'Add Holiday'}</DialogTitle>
        <DialogContent>
          <Stack spacing={2} sx={{ mt: 1 }}>
            {error && <Alert severity="error">{error}</Alert>}

            <Controller
              name="name"
              control={control}
              render={({ field }) => (
                <TextField
                  {...field}
                  label="Name"
                  error={!!errors.name}
                  helperText={errors.name?.message}
                  fullWidth
                  required
                />
              )}
            />

            <Controller
              name="destination"
              control={control}
              render={({ field }) => (
                <FormControl fullWidth required error={!!errors.destination}>
                  <InputLabel id="destination-label">Destination</InputLabel>
                  <Select
                    {...field}
                    labelId="destination-label"
                    label="Destination"
                    inputProps={{ 'aria-label': 'Destination' }}
                  >
                    {TEST_DESTINATIONS.map((dest) => (
                      <MenuItem key={dest} value={dest}>
                        {dest}
                      </MenuItem>
                    ))}
                  </Select>
                  {errors.destination && (
                    <FormHelperText>{errors.destination.message}</FormHelperText>
                  )}
                </FormControl>
              )}
            />

            <Controller
              name="startDate"
              control={control}
              render={({ field }) => (
                <TextField
                  {...field}
                  label="Start Date"
                  type="date"
                  slotProps={{ inputLabel: { shrink: true } }}
                  error={!!errors.startDate}
                  helperText={errors.startDate?.message}
                  fullWidth
                  required
                />
              )}
            />

            <Controller
              name="endDate"
              control={control}
              render={({ field }) => (
                <TextField
                  {...field}
                  label="End Date"
                  type="date"
                  slotProps={{ inputLabel: { shrink: true } }}
                  error={!!errors.endDate}
                  helperText={errors.endDate?.message}
                  fullWidth
                  required
                />
              )}
            />
          </Stack>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleClose} disabled={isSubmitting}>
            Cancel
          </Button>
          <Button type="submit" variant="contained" disabled={isSubmitting}>
            {isSubmitting ? 'Saving…' : 'Save'}
          </Button>
        </DialogActions>
      </form>
    </Dialog>
  )
}
