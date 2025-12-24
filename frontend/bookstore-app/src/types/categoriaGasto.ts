export interface CategoriaGasto {
  id: number;
  nombre: string;
  descripcion?: string;
  activo: boolean;
}

export interface CreateCategoriaGastoDto {
  nombre: string;
  descripcion?: string;
}

export interface UpdateCategoriaGastoDto {
  nombre: string;
  descripcion?: string;
  activo: boolean;
}
