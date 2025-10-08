import { useState, useEffect } from "react";
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogFooter } from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Select, SelectTrigger, SelectValue, SelectContent, SelectItem } from "@/components/ui/select";
import { toast } from "sonner";
import { BillOcorrenceDto, BillOcorrenceCreateDto, BillDto } from "@/types/api.types";
import { billService } from "@/services/billService";
import { billOcorrenceService } from "@/services/billOcorrenceService";
import BillDialog from "@/components/BillDialog";

interface BillOcorrenceDialogProps {
	open: boolean;
	onOpenChange: (open: boolean) => void;
	ocorrence?: BillOcorrenceDto;
	onSuccess: () => void;
}

const BillOcorrenceDialog = ({ open, onOpenChange, ocorrence, onSuccess }: BillOcorrenceDialogProps) => {
	const [date, setDate] = useState("");
	const [status, setStatus] = useState<number>(0);
	const [bill, setBill] = useState<BillDto | undefined>();
	const [loading, setLoading] = useState(false);
	const [billDialogOpen, setBillDialogOpen] = useState(false);

	useEffect(() => {
		const loadBill = async () => {
			if (!ocorrence) return;
			setDate(ocorrence.date);
			setStatus(ocorrence.status);

			try {
				const bills = await billService.getAll();
				const found = bills.find(b => b.name === ocorrence.bill);
				if (found) setBill(found);
			} catch (error) {
				console.error("Erro ao carregar conta da ocorrência:", error);
			}
		};

		loadBill();
	}, [ocorrence]);

	const handleSave = async () => {
		if (!ocorrence || !bill) return;

		try {
			setLoading(true);

			const payload: BillOcorrenceCreateDto = {
				billId: bill.id,
				date,
				status,
			};

			await billOcorrenceService.update(ocorrence.id, payload);
			toast.success("Ocorrência atualizada com sucesso");
			onSuccess();
			onOpenChange(false);
		} catch (error) {
			console.error(error);
			toast.error("Erro ao atualizar ocorrência");
		} finally {
			setLoading(false);
		}
	};

	return (
		<>
			<Dialog open={open} onOpenChange={onOpenChange}>
				<DialogContent>
					<DialogHeader>
						<DialogTitle>Editar Ocorrência</DialogTitle>
					</DialogHeader>

					<div className="space-y-4 mt-2">
						<div>
							<label className="block text-sm font-medium mb-1">Conta</label>
							<p className="text-muted-foreground">{bill?.name || "Carregando..."}</p>
							<Button
								variant="outline"
								size="sm"
								className="mt-2"
								onClick={() => setBillDialogOpen(true)}
							>
								Editar Conta
							</Button>
						</div>

						<div>
							<label className="block text-sm font-medium mb-1">Data</label>
							<Input
								type="date"
								value={date}
								onChange={(e) => setDate(e.target.value)}
							/>
						</div>

						<div>
							<label className="block text-sm font-medium mb-1">Status</label>
							<Select value={status.toString()} onValueChange={(v) => setStatus(parseInt(v))}>
								<SelectTrigger className="w-full">
									<SelectValue />
								</SelectTrigger>
								<SelectContent>
									<SelectItem value="0">Pendente</SelectItem>
									<SelectItem value="1">Pago</SelectItem>
								</SelectContent>
							</Select>
						</div>
					</div>

					<DialogFooter className="mt-4 flex justify-end gap-2">
						<Button variant="outline" onClick={() => onOpenChange(false)}>Cancelar</Button>
						<Button onClick={handleSave} disabled={loading}>
							{loading ? "Salvando..." : "Salvar"}
						</Button>
					</DialogFooter>
				</DialogContent>
			</Dialog>

			<BillDialog
				open={billDialogOpen}
				onOpenChange={setBillDialogOpen}
				bill={bill}
				onSuccess={onSuccess}
			/>
		</>
	);
};

export default BillOcorrenceDialog;