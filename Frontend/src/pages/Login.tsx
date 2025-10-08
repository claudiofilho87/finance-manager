import { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import { useAuth } from "@/contexts/AuthContext";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import {
	Card,
	CardContent,
	CardDescription,
	CardHeader,
	CardTitle,
} from "@/components/ui/card";
import { toast } from "sonner";
import { Wallet } from "lucide-react";

const Login = () => {
	const [email, setEmail] = useState("");
	const [password, setPassword] = useState("");
	const [loading, setLoading] = useState(false);
	const { login } = useAuth();
	const navigate = useNavigate();

	const handleSubmit = async (e: React.FormEvent) => {
		e.preventDefault();

		if (!email || !password) {
			toast.error("Por favor, preencha todos os campos");
			return;
		}

		setLoading(true);

		try {
			await login({ email, password });
			toast.success("Login realizado com sucesso!");
			navigate("/dashboard");
		} catch (error: any) {
			console.error("Login error:", error);
			toast.error(
				error.response?.data?.message || "Email ou senha inválidos"
			);
		} finally {
			setLoading(false);
		}
	};

	return (
		<div className="flex min-h-screen items-center justify-center bg-background p-4">
			<Card className="w-full max-w-md">
				<CardHeader className="space-y-3 text-center">
					<div className="mx-auto flex h-12 w-12 items-center justify-center rounded-full bg-primary">
						<Wallet className="h-6 w-6 text-primary-foreground" />
					</div>
					<CardTitle className="text-2xl">Finance Manager</CardTitle>
					<CardDescription>
						Entre com sua conta para gerenciar suas finanças
					</CardDescription>
				</CardHeader>
				<CardContent>
					<form onSubmit={handleSubmit} className="space-y-4">
						<div className="space-y-2">
							<Label htmlFor="email">Email</Label>
							<Input
								id="email"
								type="text"
								value={email}
								onChange={(e) => setEmail(e.target.value)}
								placeholder="Digite o email do seu usuário"
								disabled={loading}
							/>
						</div>
						<div className="space-y-2">
							<Label htmlFor="password">Senha</Label>
							<Input
								id="password"
								type="password"
								value={password}
								onChange={(e) => setPassword(e.target.value)}
								placeholder="Digite sua senha"
								disabled={loading}
							/>
						</div>
						<Button type="submit" className="w-full" disabled={loading}>
							{loading ? "Entrando..." : "Entrar"}
						</Button>
					</form>
					<div className="mt-4 text-center text-sm">
						<span className="text-muted-foreground">Não tem uma conta? </span>
						<Link to="/register" className="text-primary hover:underline">
							Cadastre-se
						</Link>
					</div>
				</CardContent>
			</Card>
		</div>
	);
};

export default Login;
