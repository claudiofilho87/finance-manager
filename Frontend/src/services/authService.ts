import api from './api';
import { UserLoginDto, UserDto, TokenResponseDto, ApiResponse, UserCreateDto } from '@/types/api.types';

export const authService = {
  async login(credentials: UserLoginDto): Promise<TokenResponseDto> {
    const response = await api.post<ApiResponse<TokenResponseDto>>(
      '/api/v1/users/login',
      credentials
    );
    
    const { accessToken, refreshToken } = response.data.data;
    
    localStorage.setItem('accessToken', accessToken);
    localStorage.setItem('refreshToken', refreshToken);
    // localStorage.setItem('tokenExpiresIn', expiresIn.toString());
    
    return response.data.data;
  },

  async register(userCreateData: UserCreateDto): Promise<UserDto> {
    const response = await api.post<ApiResponse<UserDto>>(
      '/api/v1/users/register',
      userCreateData
    );
    return response.data.data;
  },

  logout() {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    localStorage.removeItem('tokenExpiresIn');
  },

  isAuthenticated(): boolean {
    return !!localStorage.getItem('accessToken');
  },

  getAccessToken(): string | null {
    return localStorage.getItem('accessToken');
  }
};
