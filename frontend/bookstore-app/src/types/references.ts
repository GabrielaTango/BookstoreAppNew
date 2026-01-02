// Zona
export interface Zona {
  id: number;
  codigo?: string;
  descripcion?: string;
}

export interface CreateZonaDto {
  codigo: string;
  descripcion: string;
}

export interface UpdateZonaDto {
  codigo: string;
  descripcion: string;
}

// SubZona
export interface SubZona {
  id: number;
  codigo?: string;
  descripcion?: string;
  provinciaId: number;
  codigoPostal?: string;
  localidad?: string;
  provinciaDescripcion?: string;
}

export interface CreateSubZonaDto {
  codigo: string;
  descripcion: string;
  provinciaId: number;
  codigoPostal: string;
  localidad: string;
}

export interface UpdateSubZonaDto {
  codigo: string;
  descripcion: string;
  provinciaId: number;
  codigoPostal: string;
  localidad: string;
}

// Provincia
export interface Provincia {
  id: number;
  codigo?: string;
  descripcion?: string;
}

export interface CreateProvinciaDto {
  codigo: string;
  descripcion: string;
}

export interface UpdateProvinciaDto {
  codigo: string;
  descripcion: string;
}

// Vendedor
export interface Vendedor {
  id: number;
  codigo?: string;
  descripcion?: string;
}

export interface CreateVendedorDto {
  codigo: string;
  descripcion: string;
}

export interface UpdateVendedorDto {
  codigo: string;
  descripcion: string;
}

// Transporte
export interface Transporte {
  id: number;
  codigo?: string;
  nombre: string;
  direccion?: string;
  localidad?: string;
  provinciaId?: number;
  provinciaDescripcion?: string;
  cuit?: string;
}

export interface CreateTransporteDto {
  codigo?: string;
  nombre: string;
  direccion?: string;
  localidad?: string;
  provinciaId?: number;
  cuit?: string;
}

export interface UpdateTransporteDto {
  codigo?: string;
  nombre: string;
  direccion?: string;
  localidad?: string;
  provinciaId?: number;
  cuit?: string;
}
