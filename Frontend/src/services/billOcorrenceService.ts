import api from "./api";
import { BillOcorrenceDto, BillOcorrenceCreateDto, ApiResponse } from "@/types/api.types";

export const billOcorrenceService = {
	async getAll(): Promise<BillOcorrenceDto[]> {
		const response = await api.get<ApiResponse<BillOcorrenceDto[]>>("/api/v1/bill/ocorrences");
		return response.data.data;
	},

	async getById(id: number): Promise<BillOcorrenceDto> {
		const response = await api.get<ApiResponse<BillOcorrenceDto>>(
			`/api/v1/bill/ocorrences/${id}`
		);
		return response.data.data;
	},

	async create(ocorrence: BillOcorrenceCreateDto): Promise<BillOcorrenceDto> {
		const response = await api.post<ApiResponse<BillOcorrenceDto>>(
			"/api/v1/bill/ocorrences",
			ocorrence
		);
		return response.data.data;
	},

	async update(id: number, ocorrence: BillOcorrenceCreateDto): Promise<BillOcorrenceDto> {
		const response = await api.put<ApiResponse<BillOcorrenceDto>>(
			`/api/v1/bill/ocorrences/${id}`,
			ocorrence
		);
		return response.data.data;
	},

	async delete(id: number): Promise<void> {
		await api.delete(`/api/v1/bill/ocorrences/${id}`);
	},
};
