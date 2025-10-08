import React, { createContext, useContext, useState, useEffect } from "react";
import { authService } from "@/services/authService";
import { UserLoginDto, UserDto, UserCreateDto } from "@/types/api.types";

interface AuthContextType {
	isAuthenticated: boolean;
	login: (credentials: UserLoginDto) => Promise<void>;
	register: (userData: UserDto) => Promise<void>;
	logout: () => void;
	loading: boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
	const [isAuthenticated, setIsAuthenticated] = useState(false);
	const [loading, setLoading] = useState(true);

	useEffect(() => {
		const checkAuth = () => {
			const authenticated = authService.isAuthenticated();
			setIsAuthenticated(authenticated);
			setLoading(false);
		};

		checkAuth();
	}, []);

	const login = async (credentials: UserLoginDto) => {
		await authService.login(credentials);
		setIsAuthenticated(true);
	};

	const register = async (userCreateData: UserCreateDto) => {
		await authService.register(userCreateData);
	};

	const logout = () => {
		authService.logout();
		setIsAuthenticated(false);
	};

	return (
		<AuthContext.Provider value={{ isAuthenticated, login, register, logout, loading }}>
			{children}
		</AuthContext.Provider>
	);
};

export const useAuth = () => {
	const context = useContext(AuthContext);
	if (context === undefined) {
		throw new Error("useAuth must be used within an AuthProvider");
	}
	return context;
};
