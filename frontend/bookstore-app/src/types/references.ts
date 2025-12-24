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
}

export interface CreateSubZonaDto {
  codigo: string;
  descripcion: string;
}

export interface UpdateSubZonaDto {
  codigo: string;
  descripcion: string;
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
