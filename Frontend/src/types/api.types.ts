// API Response wrapper
export interface ApiResponse<T> {
	data: T;
	success: boolean;
	message?: string;
}

// Auth types
export interface UserCreateDto {
	email: string;
	username: string;
	password: string;
	role?: string;
}

export interface UserLoginDto {
	username: string;
	password: string;
}

export interface UserDto {
	email: string;
	username: string;
	role?: string;
}

export interface TokenResponseDto {
	accessToken: string;
	refreshToken: string;
}

export interface RefreshTokenRequestDto {
	userId: number;
	refreshToken: string;
}

// Bill
export interface BillDto {
	id: number;
	name: string;
	description?: string;
	value?: number;
	startDate: string; // YYYY-MM-DD
	recurrenceTypeId: number;
	repeatCount?: number;
}

// BillCreateDto
export interface BillCreateDto {
	name: string;
	description?: string;
	value?: number;
	startDate: string; // YYYY-MM-DD
	recurrenceTypeId: number;
	repeatCount?: number;
}

// Bill Ocorrence

export interface BillOcorrenceDto {
	id: number;
	bill: string;
	date: string; // YYYY-MM-DD
	status: number;
}

export interface BillOcorrenceCreateDto {
	billId: number;
	date: string; // YYYY-MM-DD
	status: number;
}

// Recurrence Type
export interface RecurrenceTypeDto {
	id: number;
	type: string;
	daysInterval: number;
}
