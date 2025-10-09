import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "@/contexts/AuthContext";
import { billService } from "@/services/billService";
import { billOcorrenceService } from "@/services/billOcorrenceService";
import { BillDto, BillOcorrenceDto, BillOcorrenceCreateDto } from "@/types/api.types";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import {
	Select,
	SelectContent,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from "@/components/ui/select";
import { Badge } from "@/components/ui/badge";
import { toast } from "sonner";
import {
	LogOut,
	Plus,
	Calendar,
	DollarSign,
	CheckCircle2,
	XCircle,
	Edit,
	Trash2,
} from "lucide-react";
import BillDialog from "@/components/BillDialog";
import BillOcorrenceDialog from "@/components/BillOcorrenceDialog";
import { format, parseISO } from "date-fns";
import { ptBR } from "date-fns/locale";

const Dashboard = () => {
	const { logout } = useAuth();
	const navigate = useNavigate();
	const [bills, setBills] = useState<BillDto[]>([]);
	const [ocorrences, setOcorrences] = useState<BillOcorrenceDto[]>([]);
	const [loading, setLoading] = useState(true);
	const [selectedMonth, setSelectedMonth] = useState(new Date().getMonth() + 1);
	const [selectedYear, setSelectedYear] = useState(new Date().getFullYear());
	const [dialogOpen, setDialogOpen] = useState(false);
	const [editingBill, setEditingBill] = useState<BillDto | undefined>();
	const [editingOcorrence, setEditingOcorrence] = useState<BillOcorrenceDto | undefined>();
	const [occurrenceDialogOpen, setOccurrenceDialogOpen] = useState(false);
	const [selectedStatus, setSelectedStatus] = useState<string>("all");

	// Carrega contas e ocorrências
	const loadData = async () => {
		try {
			setLoading(true);
			const [billsData, ocorrencesData] = await Promise.all([
				billService.getAll(),
				billOcorrenceService.getAll(),
			]);

			setBills(billsData);
			setOcorrences(ocorrencesData);
		} catch (error) {
			console.error("Erro ao carregar dados:", error);
			toast.error("Erro ao carregar dados");
		} finally {
			setLoading(false);
		}
	};

	useEffect(() => {
		loadData();
	}, []);

	const handleLogout = () => {
		logout();
		navigate("/login");
	};

	const handleCreateBill = () => {
		setEditingBill(undefined);
		setDialogOpen(true);
	};

	const handleEditOcorrence = (occ: BillOcorrenceDto) => {
		setEditingOcorrence(occ);
		setOccurrenceDialogOpen(true);
	};

	const handleDeleteOcorrence = async (occ: BillOcorrenceDto) => {
		if (!window.confirm("Tem certeza que deseja excluir esta ocorrência?")) return;

		try {
			await billOcorrenceService.delete(occ.id);
			toast.success("Ocorrência excluída com sucesso");
			loadData();
		} catch (error) {
			console.error("Erro ao excluir ocorrência:", error);
			toast.error("Erro ao excluir ocorrência");
		}
	};

	const handleDeleteBill = async (id: number) => {
		if (!window.confirm("Tem certeza que deseja excluir esta conta?")) return;

		try {
			await billService.delete(id);
			toast.success("Conta excluída com sucesso");
			loadData();
		} catch (error) {
			console.error("Erro ao excluir conta:", error);
			toast.error("Erro ao excluir conta");
		}
	};

	const handleToggleOcorrenceStatus = async (occ: BillOcorrenceDto) => {
		try {
			const bill = bills.find((b) => b.name === occ.bill);
			if (!bill) throw new Error("Conta não encontrada");

			const newStatus: number = occ.status === 1 ? 0 : 1;

			const payload: BillOcorrenceCreateDto = {
				billId: bill.id,
				date: occ.date,
				status: newStatus,
			};

			await billOcorrenceService.update(occ.id, payload);

			toast.success(`Status alterado para ${newStatus === 1 ? "pago" : "pendente"}`);
			loadData();
		} catch (error) {
			console.error("Erro ao atualizar ocorrência:", error);
			toast.error("Erro ao atualizar status");
		}
	};

	const monthFilteredOcorrences = ocorrences.filter((occ) => {
		const occDate = parseISO(occ.date);
		return occDate.getMonth() + 1 === selectedMonth && occDate.getFullYear() === selectedYear;
	});

	const filteredOcorrences = monthFilteredOcorrences.filter((occ) => {
		return (
			selectedStatus === "all" ||
			(selectedStatus === "paid" && occ.status === 1) ||
			(selectedStatus === "pending" && occ.status === 0)
		);
	});

	const totalValue = monthFilteredOcorrences.reduce((sum, occ) => {
		return sum + (occ.value ?? bills.find((b) => b.name === occ.bill)?.value ?? 0);
	}, 0);

	const paidValue = monthFilteredOcorrences.reduce((sum, occ) => {
		const value = occ.value ?? bills.find((b) => b.name === occ.bill)?.value ?? 0;
		return sum + (occ.status === 1 ? value : 0);
	}, 0);

	const pendingValue = totalValue - paidValue;

	const months = [
		{ value: 1, label: "Janeiro" },
		{ value: 2, label: "Fevereiro" },
		{ value: 3, label: "Março" },
		{ value: 4, label: "Abril" },
		{ value: 5, label: "Maio" },
		{ value: 6, label: "Junho" },
		{ value: 7, label: "Julho" },
		{ value: 8, label: "Agosto" },
		{ value: 9, label: "Setembro" },
		{ value: 10, label: "Outubro" },
		{ value: 11, label: "Novembro" },
		{ value: 12, label: "Dezembro" },
	];

	const years = Array.from({ length: 5 }, (_, i) => new Date().getFullYear() - 2 + i);

	if (loading) {
		return (
			<div className="flex min-h-screen items-center justify-center">
				<div className="text-center">
					<div className="h-12 w-12 animate-spin rounded-full border-4 border-primary border-t-transparent mx-auto mb-4"></div>
					<p className="text-muted-foreground">Carregando...</p>
				</div>
			</div>
		);
	}

	return (
		<div className="min-h-screen bg-background">
			<header className="border-b bg-card">
				<div className="container mx-auto flex items-center justify-between px-4 py-4">
					<h1 className="text-2xl font-bold">Finance Manager</h1>
					<Button variant="ghost" size="sm" onClick={handleLogout}>
						<LogOut className="mr-2 h-4 w-4" />
						Sair
					</Button>
				</div>
			</header>

			<main className="container mx-auto p-4 space-y-6">
				{/* Filtros e criar conta */}
				<div className="flex flex-col sm:flex-row gap-4 items-start sm:items-center justify-between">
					<div className="flex gap-2">
						<Select
							value={selectedMonth.toString()}
							onValueChange={(v) => setSelectedMonth(parseInt(v))}
						>
							<SelectTrigger className="w-[140px]">
								<SelectValue />
							</SelectTrigger>
							<SelectContent>
								{months.map((m) => (
									<SelectItem key={m.value} value={m.value.toString()}>
										{m.label}
									</SelectItem>
								))}
							</SelectContent>
						</Select>
						<Select
							value={selectedYear.toString()}
							onValueChange={(v) => setSelectedYear(parseInt(v))}
						>
							<SelectTrigger className="w-[100px]">
								<SelectValue />
							</SelectTrigger>
							<SelectContent>
								{years.map((y) => (
									<SelectItem key={y} value={y.toString()}>
										{y}
									</SelectItem>
								))}
							</SelectContent>
						</Select>
						<Select value={selectedStatus} onValueChange={(v) => setSelectedStatus(v)}>
							<SelectTrigger className="w-[130px]">
								<SelectValue placeholder="Status" />
							</SelectTrigger>
							<SelectContent>
								<SelectItem value="all">Todos</SelectItem>
								<SelectItem value="paid">Pagos</SelectItem>
								<SelectItem value="pending">Pendentes</SelectItem>
							</SelectContent>
						</Select>
					</div>
					<Button onClick={handleCreateBill}>
						<Plus className="mr-2 h-4 w-4" />
						Nova Conta
					</Button>
				</div>

				{/* Cards de resumo */}
				<div className="grid gap-4 md:grid-cols-3">
					<Card>
						<CardHeader className="flex items-center justify-between pb-2">
							<CardTitle className="text-sm font-medium">Total do Mês</CardTitle>
							<DollarSign className="h-4 w-4 text-muted-foreground" />
						</CardHeader>
						<CardContent>
							<div className="text-2xl font-bold">
								R${" "}
								{totalValue.toLocaleString("pt-BR", {
									minimumFractionDigits: 2,
									maximumFractionDigits: 2,
								})}
							</div>
						</CardContent>
					</Card>
					<Card>
						<CardHeader className="flex items-center justify-between pb-2">
							<CardTitle className="text-sm font-medium">Pago</CardTitle>
							<CheckCircle2 className="h-4 w-4 text-success" />
						</CardHeader>
						<CardContent>
							<div className="text-2xl font-bold text-success">
								R${" "}
								{paidValue.toLocaleString("pt-BR", {
									minimumFractionDigits: 2,
									maximumFractionDigits: 2,
								})}
							</div>
						</CardContent>
					</Card>
					<Card>
						<CardHeader className="flex items-center justify-between pb-2">
							<CardTitle className="text-sm font-medium">Pendente</CardTitle>
							<XCircle className="h-4 w-4 text-destructive" />
						</CardHeader>
						<CardContent>
							<div className="text-2xl font-bold text-destructive">
								R${" "}
								{pendingValue.toLocaleString("pt-BR", {
									minimumFractionDigits: 2,
									maximumFractionDigits: 2,
								})}
							</div>
						</CardContent>
					</Card>
				</div>

				{/* Lista de ocorrências */}
				<div className="space-y-4">
					<h2 className="text-xl font-semibold">Contas do Período</h2>
					{filteredOcorrences.length === 0 ? (
						<Card>
							<CardContent className="flex flex-col items-center justify-center py-10">
								<Calendar className="h-12 w-12 text-muted-foreground mb-4" />
								<p className="text-muted-foreground text-center">
									Nenhuma conta encontrada para este período
								</p>
							</CardContent>
						</Card>
					) : (
						<div className="grid gap-4">
							{filteredOcorrences.map((occ) => {
								const bill = bills.find((b) => b.name === occ.bill);
								const isPaid = occ.status === 1;
								const paymentDate = isPaid ? occ.date : undefined;

								return (
									<Card key={occ.id}>
										<CardContent className="flex flex-col sm:flex-row items-start sm:items-center justify-between p-4 gap-4">
											<div className="flex-1 space-y-1">
												<div className="flex items-center gap-2">
													<h3 className="font-semibold">{occ.bill}</h3>
													<Badge variant={isPaid ? "default" : "destructive"}>
														{isPaid ? "Pago" : "Pendente"}
													</Badge>
												</div>
												{bill?.description && (
													<p className="text-sm text-muted-foreground">
														{bill.description}
													</p>
												)}
												{occ.observation && (
													<p className="text-sm text-destructive">
														Observação: {occ.observation}
													</p>
												)}
												<p className="text-sm text-muted-foreground">
													Vencimento:{" "}
													{format(parseISO(occ.date), "dd/MM/yyyy", {
														locale: ptBR,
													})}
												</p>
											</div>

											<div className="flex items-center gap-4">
												<div className="text-right">
													<p className="text-xl font-bold">
														R${" "}
														{(occ.value ?? bill?.value ?? 0).toLocaleString("pt-BR", {
															minimumFractionDigits: 2,
															maximumFractionDigits: 2,
														})}
													</p>
													{paymentDate && (
														<p className="text-xs text-muted-foreground">
															Pago em{" "}
															{format(parseISO(paymentDate), "dd/MM/yyyy", {
																locale: ptBR,
															})}
														</p>
													)}
												</div>
												<div className="flex gap-2">
													<Button
														size="sm"
														variant={isPaid ? "outline" : "default"}
														onClick={() => handleToggleOcorrenceStatus(occ)}
													>
														{isPaid ? (
															<XCircle className="h-4 w-4" />
														) : (
															<CheckCircle2 className="h-4 w-4" />
														)}
													</Button>
													{bill && (
														<>
															<Button
																size="sm"
																variant="outline"
																onClick={() => handleEditOcorrence(occ)}
															>
																<Edit className="h-4 w-4" />
															</Button>
															<Button
																size="sm"
																variant="destructive"
																onClick={() => handleDeleteOcorrence(occ)}
															>
																<Trash2 className="h-4 w-4" />
															</Button>
														</>
													)}
												</div>
											</div>
										</CardContent>
									</Card>
								);
							})}
						</div>
					)}
				</div>
			</main>

			<BillDialog
				open={dialogOpen}
				onOpenChange={setDialogOpen}
				bill={editingBill}
				onSuccess={loadData}
			/>

			<BillOcorrenceDialog
				open={occurrenceDialogOpen}
				onOpenChange={setOccurrenceDialogOpen}
				ocorrence={editingOcorrence}
				onSuccess={loadData}
			/>
		</div>
	);
};

export default Dashboard;
