import { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import { useAuth } from "@/contexts/AuthContext";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { toast } from "sonner";
import { Wallet } from "lucide-react";

const Register = () => {
	const [email, setEmail] = useState("");
	const [username, setUsername] = useState("");
	const [password, setPassword] = useState("");
	const [confirmPassword, setConfirmPassword] = useState("");
	const [loading, setLoading] = useState(false);
	const { register } = useAuth();
	const navigate = useNavigate();

	const handleSubmit = async (e: React.FormEvent) => {
		e.preventDefault();

		if (!email || !username || !password || !confirmPassword) {
			toast.error("Por favor, preencha todos os campos obrigatórios");
			return;
		}

		if (password !== confirmPassword) {
			toast.error("As senhas não coincidem");
			return;
		}

		if (password.length < 6) {
			toast.error("A senha deve ter pelo menos 6 caracteres");
			return;
		}

		setLoading(true);

		try {
			await register({ email, username, password });
			toast.success("Cadastro realizado com sucesso! Faça login para continuar.");
			navigate("/login");
		} catch (error: any) {
			console.error("Register error:", error);
			toast.error(error.response?.data?.message || "Erro ao realizar cadastro");
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
					<CardTitle className="text-2xl">Criar Conta</CardTitle>
					<CardDescription>Cadastre-se para começar a gerenciar suas finanças</CardDescription>
				</CardHeader>
				<CardContent>
					<form onSubmit={handleSubmit} className="space-y-4">
						<div className="space-y-2">
							<Label htmlFor="email">Email *</Label>
							<Input
								id="email"
								type="text"
								value={email}
								onChange={(e) => setEmail(e.target.value)}
								placeholder="Digite seu email"
								disabled={loading}
							/>
						</div>
						<div className="space-y-2">
							<Label htmlFor="username">Usuário *</Label>
							<Input
								id="username"
								type="text"
								value={username}
								onChange={(e) => setUsername(e.target.value)}
								placeholder="Digite seu usuário"
								disabled={loading}
							/>
						</div>
						<div className="space-y-2">
							<Label htmlFor="password">Senha *</Label>
							<Input
								id="password"
								type="password"
								value={password}
								onChange={(e) => setPassword(e.target.value)}
								placeholder="Digite sua senha"
								disabled={loading}
							/>
						</div>
						<div className="space-y-2">
							<Label htmlFor="confirmPassword">Confirmar Senha *</Label>
							<Input
								id="confirmPassword"
								type="password"
								value={confirmPassword}
								onChange={(e) => setConfirmPassword(e.target.value)}
								placeholder="Confirme sua senha"
								disabled={loading}
							/>
						</div>
						<Button type="submit" className="w-full" disabled={loading}>
							{loading ? "Cadastrando..." : "Cadastrar"}
						</Button>
					</form>
					<div className="mt-4 text-center text-sm">
						<span className="text-muted-foreground">Já tem uma conta? </span>
						<Link to="/login" className="text-primary hover:underline">
							Faça login
						</Link>
					</div>
				</CardContent>
			</Card>
		</div>
	);
};

export default Register;
