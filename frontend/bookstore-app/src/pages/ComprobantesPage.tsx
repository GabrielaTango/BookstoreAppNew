import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { comprobanteService } from '../services/comprobanteService';
import type { Comprobante } from '../types/comprobante';
import { showSuccessAlert, showErrorAlert, showDeleteConfirmDialog } from '../utils/sweetalert';
import { PageHeader } from '../components/PageHeader';
import { GradientButton } from '../components/GradientButton';
import { IconButton } from '../components/IconButton';

const ComprobantesPage = () => {
  const [comprobantes, setComprobantes] = useState<Comprobante[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    loadComprobantes();
  }, []);

  const loadComprobantes = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await comprobanteService.getAll();
      setComprobantes(data);
    } catch (err) {
      setError('Error al cargar los comprobantes. Verifique que la API esté ejecutándose.');
      console.error('Error loading comprobantes:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleDeleteClick = async (comprobante: Comprobante) => {
    const itemName = `comprobante ${comprobante.numeroComprobante} del cliente ${comprobante.clienteNombre}`;
    const result = await showDeleteConfirmDialog(itemName);

    if (result.isConfirmed) {
      try {
        await comprobanteService.delete(comprobante.id);
        setComprobantes(comprobantes.filter((c) => c.id !== comprobante.id));
        await showSuccessAlert('Comprobante eliminado', `El comprobante ${comprobante.numeroComprobante} ha sido eliminado correctamente`);
      } catch (err) {
        await showErrorAlert('Error', 'No se pudo eliminar el comprobante');
        console.error('Error deleting comprobante:', err);
      }
    }
  };

  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    return date.toLocaleDateString('es-AR');
  };

  return (
    <div>
      <PageHeader
        title="Gestión de Comprobantes"
        icon="fa-solid fa-receipt"
        actions={
          <Link to="/comprobantes/nuevo">
            <GradientButton icon="fa-solid fa-plus">
              Nuevo Comprobante
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
                    <th>Fecha</th>
                    <th>Tipo</th>
                    <th>Número</th>
                    <th>Cliente</th>
                    <th>Vendedor</th>
                    <th className="text-end">Total</th>
                    <th className="text-center">Acciones</th>
                  </tr>
                </thead>
                <tbody>
                  {comprobantes.map((comprobante) => (
                    <tr key={comprobante.id}>
                      <td>{formatDate(comprobante.fecha)}</td>
                      <td>{comprobante.tipoComprobante || '-'}</td>
                      <td>{comprobante.numeroComprobante || '-'}</td>
                      <td>{comprobante.clienteNombre || '-'}</td>
                      <td>{comprobante.vendedorNombre || '-'}</td>
                      <td className="text-end">${comprobante.total.toFixed(2)}</td>
                      <td className="text-center">
                        <IconButton
                          icon="fa-solid fa-file-pdf"
                          title="Ver PDF"
                          variant="info"
                          onClick={() => comprobanteService.openPdf(comprobante.id)}
                        />
                        <IconButton
                          icon="fa-solid fa-list"
                          title="Cupones"
                          variant="secondary"
                          onClick={() => comprobanteService.openCuponesPdf(comprobante.id)}
                        />
                        <Link to={`/comprobantes/editar/${comprobante.id}`}>
                          <IconButton icon="fa-solid fa-pen" title="Editar" variant="primary" />
                        </Link>
                        <IconButton
                          icon="fa-solid fa-trash"
                          title="Eliminar"
                          variant="danger"
                          onClick={() => handleDeleteClick(comprobante)}
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

export default ComprobantesPage;
