import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { gastoService } from '../services/gastoService';
import type { Gasto } from '../types/gasto';
import { showSuccessAlert, showErrorAlert, showDeleteConfirmDialog } from '../utils/sweetalert';
import { PageHeader } from '../components/PageHeader';
import { GradientButton } from '../components/GradientButton';
import { IconButton } from '../components/IconButton';

const GastosPage = () => {
  const [gastos, setGastos] = useState<Gasto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    loadGastos();
  }, []);

  const loadGastos = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await gastoService.getAll();
      setGastos(data);
    } catch (err) {
      setError('Error al cargar los gastos. Verifique que la API esté ejecutándose.');
      console.error('Error loading gastos:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleDeleteClick = async (gasto: Gasto) => {
    const result = await showDeleteConfirmDialog(`el gasto ${gasto.nroComprobante}`);

    if (result.isConfirmed) {
      try {
        await gastoService.delete(gasto.id);
        setGastos(gastos.filter((g) => g.id !== gasto.id));
        await showSuccessAlert('Gasto eliminado', `El gasto ${gasto.nroComprobante} ha sido eliminado correctamente`);
      } catch (err) {
        await showErrorAlert('Error', 'No se pudo eliminar el gasto');
        console.error('Error deleting gasto:', err);
      }
    }
  };

  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    return date.toLocaleDateString('es-AR');
  };

  const formatCurrency = (amount: number) => {
    return new Intl.NumberFormat('es-AR', {
      style: 'currency',
      currency: 'ARS'
    }).format(amount);
  };

  return (
    <div>
      <PageHeader
        title="Gestión de Gastos"
        icon="fa-solid fa-receipt"
        actions={
          <Link to="/gastos/nuevo">
            <GradientButton icon="fa-solid fa-plus">
              Nuevo Gasto
            </GradientButton>
          </Link>
        }
      />

      {error && <div className="alert alert-danger">{error}</div>}

      {loading ? (
        <div className="text-center py-5">
          <div className="spinner-gradient" />
        </div>
      ) : (
        <div className="card">
          <div className="card-body">
            <div className="table-responsive">
              <table className="custom-table">
                <thead>
                  <tr>
                    <th>Nro. Comprobante</th>
                    <th>Fecha</th>
                    <th>Categoría</th>
                    <th>Descripción</th>
                    <th>Importe</th>
                    <th>Acciones</th>
                  </tr>
                </thead>
                <tbody>
                  {gastos.map((gasto) => (
                    <tr key={gasto.id}>
                      <td>{gasto.nroComprobante}</td>
                      <td>{formatDate(gasto.fecha)}</td>
                      <td>
                        <span className="badge-categoria">{gasto.categoria}</span>
                      </td>
                      <td>{gasto.descripcion}</td>
                      <td className="text-end fw-bold">{formatCurrency(gasto.importe)}</td>
                      <td>
                        <Link to={`/gastos/editar/${gasto.id}`}>
                          <IconButton icon="fa-solid fa-pen" title="Editar" variant="primary" />
                        </Link>
                        <IconButton
                          icon="fa-solid fa-trash"
                          title="Eliminar"
                          variant="danger"
                          onClick={() => handleDeleteClick(gasto)}
                        />
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default GastosPage;
