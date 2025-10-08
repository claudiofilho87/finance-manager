import { useState, useEffect } from "react";
import { BillDto, BillCreateDto, RecurrenceTypeDto } from "@/types/api.types";
import { billService } from "@/services/billService";
import { recurrenceTypeService } from "@/services/recurrenceTypeService";
import {
	Dialog,
	DialogContent,
	DialogDescription,
	DialogHeader,
	DialogTitle,
	DialogFooter,
} from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
import { toast } from "sonner";
import { format } from "date-fns";
import {
	Select,
	SelectTrigger,
	SelectValue,
	SelectContent,
	SelectItem,
} from "@/components/ui/select";

interface BillDialogProps {
	open: boolean;
	onOpenChange: (open: boolean) => void;
	bill?: BillDto;
	onSuccess: () => void;
}

const BillDialog = ({ open, onOpenChange, bill, onSuccess }: BillDialogProps) => {
	const [loading, setLoading] = useState(false);
	const [recurrenceTypes, setRecurrenceTypes] = useState<RecurrenceTypeDto[]>([]);
	const [formData, setFormData] = useState<BillCreateDto>({
		name: "",
		description: "",
		value: undefined,
		startDate: format(new Date(), "yyyy-MM-dd"),
		recurrenceTypeId: 4,
		repeatCount: undefined,
	});

	useEffect(() => {
		if (open) {
			recurrenceTypeService
				.getAll()
				.then((types) => setRecurrenceTypes(types))
				.catch((err) => console.error("Erro ao carregar tipos de recorrência:", err));
		}
	}, [open]);

	useEffect(() => {
		if (open) {
			if (bill) {
				// Editando: preenche somente campos que podem ser editados
				setFormData({
					name: bill.name,
					description: bill.description || "",
					value: bill.value,
					startDate: bill.startDate,
					recurrenceTypeId: bill.recurrenceTypeId,
					repeatCount: bill.repeatCount,
				});
			} else {
				// Criando nova conta: campos em branco/valores padrão
				setFormData({
					name: "",
					description: "",
					value: undefined,
					startDate: format(new Date(), "yyyy-MM-dd"),
					recurrenceTypeId: 4,
					repeatCount: undefined,
				});
			}
		}
	}, [open, bill]);

	const handleSubmit = async (e: React.FormEvent) => {
		e.preventDefault();
		if (!formData.name) {
			toast.error("O nome da conta é obrigatório");
			return;
		}

		setLoading(true);
		try {
			if (bill) {
				await billService.update(bill.id, formData);
				toast.success("Conta atualizada com sucesso");
			} else {
				await billService.create(formData);
				toast.success("Conta criada com sucesso");
			}
			onSuccess();
			onOpenChange(false);
		} catch (error: any) {
			console.error(error);
			toast.error(error.response?.data?.message || "Erro ao salvar conta");
		} finally {
			setLoading(false);
		}
	};

	const handleDelete = async () => {
		if (!bill) return;
		if (!window.confirm("Tem certeza que deseja excluir esta conta?")) return;

		setLoading(true);
		try {
			await billService.delete(bill.id);
			toast.success("Conta removida com sucesso");
			onSuccess();
			onOpenChange(false);
		} catch (error) {
			console.error(error);
			toast.error("Erro ao remover conta");
		} finally {
			setLoading(false);
		}
	};

	return (
		<Dialog open={open} onOpenChange={onOpenChange}>
			<DialogContent className="sm:max-w-[500px]">
				<DialogHeader>
					<DialogTitle>{bill ? "Editar Conta" : "Nova Conta"}</DialogTitle>
					<DialogDescription>
						{bill
							? "Edite apenas nome, descrição e valor da conta"
							: "Preencha os dados da nova conta"}
					</DialogDescription>
				</DialogHeader>

				<form onSubmit={handleSubmit} className="space-y-4">
					{/* Nome */}
					<div className="space-y-2">
						<Label htmlFor="name">Nome *</Label>
						<Input
							id="name"
							value={formData.name}
							onChange={(e) => setFormData({ ...formData, name: e.target.value })}
							placeholder="Ex: Aluguel, Conta de luz..."
							disabled={loading}
						/>
					</div>

					{/* Descrição */}
					<div className="space-y-2">
						<Label htmlFor="description">Descrição</Label>
						<Textarea
							id="description"
							value={formData.description}
							onChange={(e) => setFormData({ ...formData, description: e.target.value })}
							placeholder="Descrição adicional (opcional)"
							disabled={loading}
							rows={3}
						/>
					</div>

					{/* Valor */}
					<div className="space-y-2">
						<Label htmlFor="value">Valor R$</Label>
						<Input
							id="value"
							type="number"
							step="0.01"
							value={formData.value || ""}
							onChange={(e) =>
								setFormData({
									...formData,
									value: e.target.value ? parseFloat(e.target.value) : undefined,
								})
							}
							placeholder="0.00"
							disabled={loading}
						/>
					</div>

					{/* Campos apenas no modo criação */}
					{!bill && (
						<>
							<div className="space-y-2">
								<Label htmlFor="startDate">Data de Início</Label>
								<Input
									id="startDate"
									type="date"
									value={formData.startDate}
									onChange={(e) => setFormData({ ...formData, startDate: e.target.value })}
									disabled={loading}
								/>
							</div>

							{/* Select de Recorrência */}
							<div className="space-y-2">
								<Label htmlFor="recurrenceType">Tipo de Recorrência</Label>
								<Select
									value={formData.recurrenceTypeId.toString()}
									onValueChange={(v) =>
										setFormData({ ...formData, recurrenceTypeId: parseInt(v) })
									}
									disabled={loading || recurrenceTypes.length === 0}
								>
									<SelectTrigger>
										<SelectValue placeholder="Selecione..." />
									</SelectTrigger>
									<SelectContent>
										{recurrenceTypes.map((rt) => (
											<SelectItem key={rt.id} value={rt.id.toString()}>
												{rt.type}
											</SelectItem>
										))}
									</SelectContent>
								</Select>
								 
							</div>

							<div className="space-y-2">
								<Label htmlFor="repeatCount">Repetições</Label>
								<Input
									id="repeatCount"
									type="number"
									value={formData.repeatCount || ""}
									onChange={(e) =>
										setFormData({
											...formData,
											repeatCount: e.target.value ? parseInt(e.target.value) : undefined,
										})
									}
									placeholder="Deixe vazio para repetir indefinidamente"
									disabled={loading}
								/>
							</div>
						</>
					)}

					<DialogFooter className="flex justify-between gap-2">
						{bill && (
							<Button variant="destructive" onClick={handleDelete} disabled={loading}>
								Remover
							</Button>
						)}
						<div className="flex gap-2">
							<Button
								type="button"
								variant="outline"
								onClick={() => onOpenChange(false)}
								disabled={loading}
							>
								Cancelar
							</Button>
							<Button type="submit" disabled={loading}>
								{loading ? "Salvando..." : bill ? "Atualizar" : "Criar"}
							</Button>
						</div>
					</DialogFooter>
				</form>
			</DialogContent>
		</Dialog>
	);
};

export default BillDialog;
