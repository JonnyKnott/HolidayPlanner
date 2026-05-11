import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import {
  createTestHoliday,
  deleteTestHoliday,
  getTestHolidays,
  updateTestHoliday,
  type CreateTestHolidayRequest,
  type UpdateTestHolidayRequest,
} from '@/api/testHolidays'

export const TEST_HOLIDAYS_QUERY_KEY = ['testHolidays'] as const

export function useTestHolidays() {
  return useQuery({
    queryKey: TEST_HOLIDAYS_QUERY_KEY,
    queryFn: getTestHolidays,
  })
}

export function useCreateTestHoliday() {
  const queryClient = useQueryClient()
  return useMutation({
    mutationFn: (body: CreateTestHolidayRequest) => createTestHoliday(body),
    onSuccess: () => {
      void queryClient.invalidateQueries({ queryKey: TEST_HOLIDAYS_QUERY_KEY })
    },
  })
}

export function useUpdateTestHoliday() {
  const queryClient = useQueryClient()
  return useMutation({
    mutationFn: ({ id, body }: { id: string; body: UpdateTestHolidayRequest }) =>
      updateTestHoliday(id, body),
    onSuccess: () => {
      void queryClient.invalidateQueries({ queryKey: TEST_HOLIDAYS_QUERY_KEY })
    },
  })
}

export function useDeleteTestHoliday() {
  const queryClient = useQueryClient()
  return useMutation({
    mutationFn: (id: string) => deleteTestHoliday(id),
    onSuccess: () => {
      void queryClient.invalidateQueries({ queryKey: TEST_HOLIDAYS_QUERY_KEY })
    },
  })
}
