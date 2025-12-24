import api from './api';
import type {
  Zona, SubZona, Provincia, Vendedor,
  CreateZonaDto, UpdateZonaDto,
  CreateSubZonaDto, UpdateSubZonaDto,
  CreateProvinciaDto, UpdateProvinciaDto,
  CreateVendedorDto, UpdateVendedorDto
} from '../types/references';

export const referenceService = {
  // Zonas
  getZonas: async (): Promise<Zona[]> => {
    const response = await api.get<Zona[]>('/references/zonas');
    return response.data;
  },

  getZonaById: async (id: number): Promise<Zona> => {
    const response = await api.get<Zona>(`/references/zonas/${id}`);
    return response.data;
  },

  createZona: async (data: CreateZonaDto): Promise<Zona> => {
    const response = await api.post<Zona>('/references/zonas', data);
    return response.data;
  },

  updateZona: async (id: number, data: UpdateZonaDto): Promise<Zona> => {
    const response = await api.put<Zona>(`/references/zonas/${id}`, data);
    return response.data;
  },

  deleteZona: async (id: number): Promise<void> => {
    await api.delete(`/references/zonas/${id}`);
  },

  // SubZonas
  getSubZonas: async (): Promise<SubZona[]> => {
    const response = await api.get<SubZona[]>('/references/subzonas');
    return response.data;
  },

  getSubZonaById: async (id: number): Promise<SubZona> => {
    const response = await api.get<SubZona>(`/references/subzonas/${id}`);
    return response.data;
  },

  createSubZona: async (data: CreateSubZonaDto): Promise<SubZona> => {
    const response = await api.post<SubZona>('/references/subzonas', data);
    return response.data;
  },

  updateSubZona: async (id: number, data: UpdateSubZonaDto): Promise<SubZona> => {
    const response = await api.put<SubZona>(`/references/subzonas/${id}`, data);
    return response.data;
  },

  deleteSubZona: async (id: number): Promise<void> => {
    await api.delete(`/references/subzonas/${id}`);
  },

  // Provincias
  getProvincias: async (): Promise<Provincia[]> => {
    const response = await api.get<Provincia[]>('/references/provincias');
    return response.data;
  },

  getProvinciaById: async (id: number): Promise<Provincia> => {
    const response = await api.get<Provincia>(`/references/provincias/${id}`);
    return response.data;
  },

  createProvincia: async (data: CreateProvinciaDto): Promise<Provincia> => {
    const response = await api.post<Provincia>('/references/provincias', data);
    return response.data;
  },

  updateProvincia: async (id: number, data: UpdateProvinciaDto): Promise<Provincia> => {
    const response = await api.put<Provincia>(`/references/provincias/${id}`, data);
    return response.data;
  },

  deleteProvincia: async (id: number): Promise<void> => {
    await api.delete(`/references/provincias/${id}`);
  },

  // Vendedores
  getVendedores: async (): Promise<Vendedor[]> => {
    const response = await api.get<Vendedor[]>('/references/vendedores');
    return response.data;
  },

  getVendedorById: async (id: number): Promise<Vendedor> => {
    const response = await api.get<Vendedor>(`/references/vendedores/${id}`);
    return response.data;
  },

  createVendedor: async (data: CreateVendedorDto): Promise<Vendedor> => {
    const response = await api.post<Vendedor>('/references/vendedores', data);
    return response.data;
  },

  updateVendedor: async (id: number, data: UpdateVendedorDto): Promise<Vendedor> => {
    const response = await api.put<Vendedor>(`/references/vendedores/${id}`, data);
    return response.data;
  },

  deleteVendedor: async (id: number): Promise<void> => {
    await api.delete(`/references/vendedores/${id}`);
  },
};
