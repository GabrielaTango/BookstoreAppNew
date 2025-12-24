import api from './api';
import type { CategoriaGasto, CreateCategoriaGastoDto, UpdateCategoriaGastoDto } from '../types/categoriaGasto';

export const categoriaGastoService = {
  getAll: async (): Promise<CategoriaGasto[]> => {
    const response = await api.get<CategoriaGasto[]>('/categoriasgasto');
    return response.data;
  },

  getActivas: async (): Promise<CategoriaGasto[]> => {
    const response = await api.get<CategoriaGasto[]>('/categoriasgasto/activas');
    return response.data;
  },

  getById: async (id: number): Promise<CategoriaGasto> => {
    const response = await api.get<CategoriaGasto>(`/categoriasgasto/${id}`);
    return response.data;
  },

  create: async (data: CreateCategoriaGastoDto): Promise<CategoriaGasto> => {
    const response = await api.post<CategoriaGasto>('/categoriasgasto', data);
    return response.data;
  },

  update: async (id: number, data: UpdateCategoriaGastoDto): Promise<CategoriaGasto> => {
    const response = await api.put<CategoriaGasto>(`/categoriasgasto/${id}`, data);
    return response.data;
  },

  delete: async (id: number): Promise<void> => {
    await api.delete(`/categoriasgasto/${id}`);
  },
};
