import api from './api';
import type { Comprobante, CreateComprobanteDto, UpdateComprobanteDto } from '../types/comprobante';
import type { IvaVenta } from '../types/dashboard';

const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5184/api';

export const comprobanteService = {
  getAll: async (): Promise<Comprobante[]> => {
    const response = await api.get<Comprobante[]>('/comprobantes');
    return response.data;
  },

  getById: async (id: number): Promise<Comprobante> => {
    const response = await api.get<Comprobante>(`/comprobantes/${id}`);
    return response.data;
  },

  create: async (data: CreateComprobanteDto): Promise<Comprobante> => {
    const response = await api.post<Comprobante>('/comprobantes', data);
    return response.data;
  },

  update: async (id: number, data: UpdateComprobanteDto): Promise<Comprobante> => {
    const response = await api.put<Comprobante>(`/comprobantes/${id}`, data);
    return response.data;
  },

  delete: async (id: number): Promise<void> => {
    await api.delete(`/comprobantes/${id}`);
  },

  openPdf: (id: number): void => {
    const pdfUrl = `${API_BASE_URL}/comprobantes/${id}/pdf`;
    window.open(pdfUrl, '_blank');
  },

  openCuponesPdf: (id: number): void => {
    const pdfUrl = `${API_BASE_URL}/comprobantes/${id}/cupones-pdf`;
    window.open(pdfUrl, '_blank');
  },

  getIvaVentas: async (fechaDesde: string, fechaHasta: string): Promise<IvaVenta[]> => {
    const response = await api.get<IvaVenta[]>(`/comprobantes/iva-ventas?fechaDesde=${fechaDesde}&fechaHasta=${fechaHasta}`);
    return response.data;
  },

  openIvaVentasPdf: (fechaDesde: string, fechaHasta: string): void => {
    const pdfUrl = `${API_BASE_URL}/comprobantes/iva-ventas-pdf?fechaDesde=${fechaDesde}&fechaHasta=${fechaHasta}`;
    window.open(pdfUrl, '_blank');
  },
};
