import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { remitoService } from '../services/remitoService';
import type { Remito } from '../types/remito';
import { PageHeader } from '../components/PageHeader';
import { GradientButton } from '../components/GradientButton';
import { IconButton } from '../components/IconButton';

const RemitosPage = () => {
  const navigate = useNavigate();
  const [remitos, setRemitos] = useState<Remito[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [remitoToDelete, setRemitoToDelete] = useState<Remito | null>(null);

  useEffect(() => {
    loadRemitos();
  }, []);

  const loadRemitos = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await remitoService.getAll();
      setRemitos(data);
    } catch (err) {
      setError('Error al cargar los remitos');
      console.error('Error loading remitos:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleCreate = () => {
    navigate('/remitos/nuevo');
  };

  const handleEdit = (id: number) => {
    navigate(`/remitos/editar/${id}`);
  };

  const handleDeleteClick = (remito: Remito) => {
    setRemitoToDelete(remito);
    setShowDeleteModal(true);
  };

  const handleDeleteConfirm = async () => {
    if (!remitoToDelete) return;

    try {
      await remitoService.delete(remitoToDelete.id);
      setRemitos(remitos.filter((r) => r.id !== remitoToDelete.id));
      setShowDeleteModal(false);
      setRemitoToDelete(null);
    } catch (err) {
      setError('Error al eliminar el remito');
      console.error('Error deleting remito:', err);
    }
  };

  const handleViewPdf = (id: number) => {
    remitoService.openPdf(id);
  };

  const handleViewEtiquetas = (id: number) => {
    remitoService.openEtiquetasPdf(id);
  };

  const handlePrintCompleto = (id: number) => {
    remitoService.openCompletoPdf(id);
  };

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString('es-AR', {
      day: '2-digit',
      month: '2-digit',
      year: 'numeric',
    });
  };

  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat('es-AR', {
      style: 'currency',
      currency: 'ARS',
    }).format(value);
  };

  return (
    <div>
      <PageHeader
        title="Gestión de Remitos"
        icon="fa-solid fa-file-invoice"
        actions={
          <GradientButton icon="fa-solid fa-plus" onClick={handleCreate}>
            Nuevo Remito
          </GradientButton>
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
                    <th>Número</th>
                    <th>Fecha</th>
                    <th>Cliente</th>
                    <th>Transporte</th>
                    <th>Bultos</th>
                    <th>Valor</th>
                    <th>Acciones</th>
                  </tr>
                </thead>
                <tbody>
                  {remitos.length === 0 ? (
                    <tr>
                      <td colSpan={7} className="text-center py-4">
                        No hay remitos registrados
                      </td>
                    </tr>
                  ) : (
                    remitos.map((remito) => (
                      <tr key={remito.id}>
                        <td>{remito.numero}</td>
                        <td>{formatDate(remito.fecha)}</td>
                        <td>{remito.clienteNombre || '-'}</td>
                        <td>{remito.transporteNombre || '-'}</td>
                        <td className="text-center">{remito.cantidadBultos}</td>
                        <td className="text-end">{formatCurrency(remito.valorDeclarado)}</td>
                        <td>
                          <IconButton
                            icon="fa-solid fa-print"
                            title="Imprimir Completo (Remito x3 + Etiquetas)"
                            variant="success"
                            onClick={() => handlePrintCompleto(remito.id)}
                          />
                          <IconButton
                            icon="fa-solid fa-file-pdf"
                            title="Ver Remito"
                            variant="info"
                            onClick={() => handleViewPdf(remito.id)}
                          />
                          <IconButton
                            icon="fa-solid fa-tags"
                            title="Ver Etiquetas"
                            variant="secondary"
                            onClick={() => handleViewEtiquetas(remito.id)}
                          />
                          <IconButton
                            icon="fa-solid fa-pen"
                            title="Editar"
                            variant="primary"
                            onClick={() => handleEdit(remito.id)}
                          />
                          <IconButton
                            icon="fa-solid fa-trash"
                            title="Eliminar"
                            variant="danger"
                            onClick={() => handleDeleteClick(remito)}
                          />
                        </td>
                      </tr>
                    ))
                  )}
                </tbody>
              </table>
            </div>
          </div>
        </div>
      )}

      {/* Modal para eliminar */}
      {showDeleteModal && (
        <div className="modal show d-block" tabIndex={-1} style={{ backgroundColor: 'rgba(0,0,0,0.5)' }}>
          <div className="modal-dialog">
            <div className="modal-content">
              <div className="modal-header">
                <h5 className="modal-title">Confirmar Eliminación</h5>
                <button
                  type="button"
                  className="btn-close"
                  onClick={() => setShowDeleteModal(false)}
                  aria-label="Close"
                ></button>
              </div>
              <div className="modal-body">
                ¿Está seguro de que desea eliminar el remito{' '}
                <strong>{remitoToDelete?.numero}</strong>?
              </div>
              <div className="modal-footer">
                <button
                  type="button"
                  className="btn btn-secondary"
                  onClick={() => setShowDeleteModal(false)}
                >
                  Cancelar
                </button>
                <button
                  type="button"
                  className="btn btn-danger"
                  onClick={handleDeleteConfirm}
                >
                  Eliminar
                </button>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default RemitosPage;
