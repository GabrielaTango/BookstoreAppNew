export interface Remito {
  id: number;
  numero: string;
  fecha: string;
  clienteId: number;
  clienteNombre?: string;
  clienteDomicilio?: string;
  clienteLocalidad?: string;
  clienteProvincia?: string;
  transporteId: number;
  transporteNombre?: string;
  transporteDireccion?: string;
  transporteCuit?: string;
  cantidadBultos: number;
  valorDeclarado: number;
  observaciones?: string;
}

export interface CreateRemitoDto {
  clienteId: number;
  transporteId: number;
  cantidadBultos: number;
  valorDeclarado: number;
  observaciones?: string;
}

export interface UpdateRemitoDto {
  clienteId: number;
  transporteId: number;
  cantidadBultos: number;
  valorDeclarado: number;
  observaciones?: string;
}
