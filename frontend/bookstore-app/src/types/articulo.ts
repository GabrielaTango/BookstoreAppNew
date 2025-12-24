export interface Articulo {
  id: number;
  codigo?: string;
  descripcion?: string;
  codBarras?: string;
  observaciones?: string;
  tomos?: number;
  tema?: string;
  precio?: number;
}

export interface CreateArticuloDto {
  codigo?: string;
  descripcion: string;
  codBarras?: string;
  observaciones?: string;
  tomos?: number;
  tema?: string;
  precio?: number;
}

export interface UpdateArticuloDto {
  codigo?: string;
  descripcion: string;
  codBarras?: string;
  observaciones?: string;
  tomos?: number;
  tema?: string;
  precio?: number;
}
