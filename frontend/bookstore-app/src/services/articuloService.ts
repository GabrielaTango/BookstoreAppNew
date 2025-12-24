import api from './api';
import type { Articulo, CreateArticuloDto, UpdateArticuloDto } from '../types/articulo';

export const articuloService = {
  getAll: async (): Promise<Articulo[]> => {
    const response = await api.get<Articulo[]>('/articulos');
    return response.data;
  },

  getById: async (id: number): Promise<Articulo> => {
    const response = await api.get<Articulo>(`/articulos/${id}`);
    return response.data;
  },

  getByCodigo: async (codigo: string): Promise<Articulo> => {
    const response = await api.get<Articulo>(`/articulos/codigo/${codigo}`);
    return response.data;
  },

  create: async (data: CreateArticuloDto): Promise<Articulo> => {
    const response = await api.post<Articulo>('/articulos', data);
    return response.data;
  },

  update: async (id: number, data: UpdateArticuloDto): Promise<Articulo> => {
    const response = await api.put<Articulo>(`/articulos/${id}`, data);
    return response.data;
  },

  delete: async (id: number): Promise<void> => {
    await api.delete(`/articulos/${id}`);
  },
};
