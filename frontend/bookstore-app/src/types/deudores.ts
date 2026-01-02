export interface CuotaDeudor {
  cuotaId: number;
  periodo: string; // formato MM/YYYY
  importe: number;
  importePagado: number;
  estado: string;
}

export interface DeudorItem {
  comprobanteId: number;
  numeroComprobante: string;
  razonSocial: string;
  codigoVendedor?: string;
  cantidadCuotas: number;
  totalComprobante: number;
  saldo: number;
  anticipo: number;
  contraEntrega: number;
  contraEntregaPagado: number;
  cuotas: CuotaDeudor[];
}

export interface DeudoresReporte {
  mes: number;
  anio: number;
  periodosCuotas: string[]; // Lista de períodos únicos para columnas
  deudores: DeudorItem[];
}
