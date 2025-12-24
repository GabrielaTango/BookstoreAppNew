export interface DashboardStats {
  totalComprobantes: number;
  totalClientes: number;
  totalArticulos: number;
  ventasHoy: number;
}

export interface IvaVenta {
  fecha: string;
  numeroComprobante: string;
  nombre: string;
  nroDocumento: string;
  total: number;
}

export interface ActividadReciente {
  tipo: string;
  icono: string;
  color: string;
  descripcion: string;
  fecha: string;
}
