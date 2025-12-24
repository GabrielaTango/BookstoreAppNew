import api from './api';
import type { Cliente, CreateClienteDto, UpdateClienteDto } from '../types/cliente';

export const clienteService = {
  getAll: async (): Promise<Cliente[]> => {
    const response = await api.get<Cliente[]>('/clientes');
    return response.data;
  },

  getById: async (id: number): Promise<Cliente> => {
    const response = await api.get<Cliente>(`/clientes/${id}`);
    return response.data;
  },

  getByCodigo: async (codigo: string): Promise<Cliente> => {
    const response = await api.get<Cliente>(`/clientes/codigo/${codigo}`);
    return response.data;
  },

  create: async (data: CreateClienteDto): Promise<Cliente> => {
    const response = await api.post<Cliente>('/clientes', data);
    return response.data;
  },

  update: async (id: number, data: UpdateClienteDto): Promise<Cliente> => {
    const response = await api.put<Cliente>(`/clientes/${id}`, data);
    return response.data;
  },

  delete: async (id: number): Promise<void> => {
    await api.delete(`/clientes/${id}`);
  },
};
