import api from './api';
import { RecurrenceTypeDto, ApiResponse } from '@/types/api.types';

export const recurrenceTypeService = {
  async getAll(): Promise<RecurrenceTypeDto[]> {
    const response = await api.get<ApiResponse<RecurrenceTypeDto[]>>('/api/v1/recurrence-types');
    return response.data.data;
  },

  async getById(id: number): Promise<RecurrenceTypeDto> {
    const response = await api.get<ApiResponse<RecurrenceTypeDto>>(`/api/v1/recurrence-types/${id}`);
    return response.data.data;
  }
};
