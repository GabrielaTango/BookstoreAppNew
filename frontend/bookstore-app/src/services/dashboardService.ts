import api from './api';
import type { DashboardStats, ActividadReciente } from '../types/dashboard';

export const dashboardService = {
  getStats: async (): Promise<DashboardStats> => {
    const response = await api.get<DashboardStats>('/dashboard/stats');
    return response.data;
  },

  getActividadesRecientes: async (cantidad: number = 10): Promise<ActividadReciente[]> => {
    const response = await api.get<ActividadReciente[]>(`/dashboard/actividades-recientes?cantidad=${cantidad}`);
    return response.data;
  }
};
