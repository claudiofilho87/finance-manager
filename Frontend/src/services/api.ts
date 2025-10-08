import axios, { AxiosError, InternalAxiosRequestConfig } from "axios";
import { TokenResponseDto, RefreshTokenRequestDto } from "@/types/api.types";

const API_BASE_URL = "/backend";
const api = axios.create({
	baseURL: API_BASE_URL,
	headers: {
		"Content-Type": "application/json",
	},
});

function getUserIdFromAccessToken(token: string): number | null {
	try {
		const payload = token.split(".")[1];
		const decoded = JSON.parse(atob(payload));
		const claimKey = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
		return decoded[claimKey] ? parseInt(decoded[claimKey]) : null;
	} catch (err) {
		console.error("Erro ao decodificar token", err);
		return null;
	}
}

// Request interceptor to add token
api.interceptors.request.use(
	(config: InternalAxiosRequestConfig) => {
		const token = localStorage.getItem("accessToken");
		if (token && config.headers) {
			config.headers.Authorization = `Bearer ${token}`;
		}
		return config;
	},
	(error) => Promise.reject(error)
);

// Response interceptor for token refresh
api.interceptors.response.use(
  (response) => response,
  async (error: AxiosError) => {
    const originalRequest = error.config as InternalAxiosRequestConfig & { _retry?: boolean };

    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;

      try {
        const accessToken = localStorage.getItem('accessToken');
        const refreshToken = localStorage.getItem('refreshToken');

        if (!accessToken || !refreshToken) throw new Error('Tokens não disponíveis');

        const userId = getUserIdFromAccessToken(accessToken);
        if (!userId) throw new Error('Não foi possível obter userId do access token');
        console.log('UserId obtido do access token:', userId);
        const response = await axios.post<{ data: TokenResponseDto }>(
          `${API_BASE_URL}/api/v1/users/refresh-token`,
          { refreshToken, userId }
        );

        const { accessToken: newAccessToken, refreshToken: newRefreshToken } = response.data.data;

        localStorage.setItem('accessToken', newAccessToken);
        localStorage.setItem('refreshToken', newRefreshToken);

        if (originalRequest.headers) {
          originalRequest.headers.Authorization = `Bearer ${newAccessToken}`;
        }

        return api(originalRequest);
      } catch (refreshError) {
        localStorage.removeItem('accessToken');
        localStorage.removeItem('refreshToken');
        window.location.href = '/login';
        return Promise.reject(refreshError);
      }
    }

    return Promise.reject(error);
  }
);

export default api;
