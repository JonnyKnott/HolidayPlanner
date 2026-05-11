import type { AxiosResponse } from 'axios'
import { apiClient } from './client'

export const TEST_DESTINATIONS = [
  'Mexico',
  'Japan',
  'New Zealand',
  'Iceland',
  'Scotland',
] as const

export type TestDestination = (typeof TEST_DESTINATIONS)[number]

export interface TestHoliday {
  id: string
  name: string
  destination: string
  startDate: string
  endDate: string
  createdOn: string
  modifiedOn: string
}

export interface CreateTestHolidayRequest {
  name: string
  destination: string
  startDate: string
  endDate: string
}

export type UpdateTestHolidayRequest = CreateTestHolidayRequest

const BASE = '/v1/test/holidays'

export async function getTestHolidays(): Promise<TestHoliday[]> {
  const { data } = await apiClient.get<TestHoliday[]>(BASE)
  return data
}

export async function getTestHolidayById(id: string): Promise<TestHoliday> {
  const { data } = await apiClient.get<TestHoliday>(`${BASE}/${id}`)
  return data
}

export async function createTestHoliday(body: CreateTestHolidayRequest): Promise<string> {
  const response: AxiosResponse<void> = await apiClient.post(BASE, body)
  const location = response.headers['location'] as string | undefined
  return location?.split('/').pop() ?? ''
}

export async function updateTestHoliday(
  id: string,
  body: UpdateTestHolidayRequest,
): Promise<void> {
  await apiClient.put(`${BASE}/${id}`, body)
}

export async function deleteTestHoliday(id: string): Promise<void> {
  await apiClient.delete(`${BASE}/${id}`)
}
