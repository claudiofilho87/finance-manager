import api from './api';
import { BillDto, BillCreateDto, ApiResponse } from '@/types/api.types';

export const billService = {
  async getAll(): Promise<BillDto[]> {
    const response = await api.get<ApiResponse<BillDto[]>>('/api/v1/bills');
    return response.data.data;
  },

  async getById(id: number): Promise<BillDto> {
    const response = await api.get<ApiResponse<BillDto>>(`/api/v1/bills/${id}`);
    return response.data.data;
  },

  async create(bill: BillCreateDto): Promise<BillDto> {
    const response = await api.post<ApiResponse<BillDto>>('/api/v1/bills', bill);
    return response.data.data;
  },

  async update(id: number, bill: BillCreateDto): Promise<BillDto> {
    const response = await api.put<ApiResponse<BillDto>>(`/api/v1/bills/${id}`, bill);
    return response.data.data;
  },

  async delete(id: number): Promise<void> {
    await api.delete(`/api/v1/bills/${id}`);
  }
};
