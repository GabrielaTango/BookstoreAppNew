export interface CuotaListado {
  id: number;
  comprobanteId: number;
  numeroComprobante?: string;
  fechaComprobante?: string;
  clienteId: number;
  clienteNombre?: string;
  zonaId?: number;
  zonaNombre?: string;
  fechaCuota?: string;
  importe: number;
  importePagado: number;
  estado?: string;
  esCuotaCero: boolean;
}

export interface UpdateImportePagadoDto {
  importePagado: number;
  esCuotaCero: boolean;
  comprobanteId?: number;
}
