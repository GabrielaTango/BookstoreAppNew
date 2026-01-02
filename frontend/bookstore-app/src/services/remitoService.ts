import api from './api';
import type { Remito, CreateRemitoDto, UpdateRemitoDto } from '../types/remito';

const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5184/api';

export const remitoService = {
  getAll: async (): Promise<Remito[]> => {
    const response = await api.get<Remito[]>('/remitos');
    return response.data;
  },

  getById: async (id: number): Promise<Remito> => {
    const response = await api.get<Remito>(`/remitos/${id}`);
    return response.data;
  },

  create: async (data: CreateRemitoDto): Promise<Remito> => {
    const response = await api.post<Remito>('/remitos', data);
    return response.data;
  },

  update: async (id: number, data: UpdateRemitoDto): Promise<Remito> => {
    const response = await api.put<Remito>(`/remitos/${id}`, data);
    return response.data;
  },

  delete: async (id: number): Promise<void> => {
    await api.delete(`/remitos/${id}`);
  },

  openPdf: (id: number): void => {
    window.open(`${API_BASE_URL}/remitos/${id}/pdf`, '_blank');
  },

  openEtiquetasPdf: (id: number): void => {
    window.open(`${API_BASE_URL}/remitos/${id}/etiquetas-pdf`, '_blank');
  },

  openCompletoPdf: (id: number): void => {
    window.open(`${API_BASE_URL}/remitos/${id}/completo-pdf`, '_blank');
  },
};
