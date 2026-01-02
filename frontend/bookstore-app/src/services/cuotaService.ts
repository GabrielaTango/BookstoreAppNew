import api from './api';
import type { CuotaListado, UpdateImportePagadoDto } from '../types/cuota';

export const cuotaService = {
  getAll: async (zonaId?: number): Promise<CuotaListado[]> => {
    const params = zonaId ? `?zonaId=${zonaId}` : '';
    const response = await api.get<CuotaListado[]>(`/cuotas${params}`);
    return response.data;
  },

  updateImportePagado: async (id: number, dto: UpdateImportePagadoDto): Promise<void> => {
    await api.put(`/cuotas/${id}/importe-pagado`, dto);
  },
};
