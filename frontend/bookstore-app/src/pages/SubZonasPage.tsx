import { useState, useEffect } from 'react';
import { referenceService } from '../services/referenceService';
import type { SubZona, CreateSubZonaDto, UpdateSubZonaDto } from '../types/references';
import { PageHeader } from '../components/PageHeader';
import { GradientButton } from '../components/GradientButton';
import { IconButton } from '../components/IconButton';

const SubZonasPage = () => {
  const [subzonas, setSubZonas] = useState<SubZona[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [showModal, setShowModal] = useState(false);
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [subzonaToDelete, setSubZonaToDelete] = useState<SubZona | null>(null);
  const [editingSubZona, setEditingSubZona] = useState<SubZona | null>(null);
  const [formData, setFormData] = useState<CreateSubZonaDto | UpdateSubZonaDto>({
    codigo: '',
    descripcion: '',
  });

  useEffect(() => {
    loadSubZonas();
  }, []);

  const loadSubZonas = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await referenceService.getSubZonas();
      setSubZonas(data);
    } catch (err) {
      setError('Error al cargar las subzonas');
      console.error('Error loading subzonas:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleCreate = () => {
    setEditingSubZona(null);
    setFormData({ codigo: '', descripcion: '' });
    setShowModal(true);
  };

  const handleEdit = (subzona: SubZona) => {
    setEditingSubZona(subzona);
    setFormData({
      codigo: subzona.codigo || '',
      descripcion: subzona.descripcion || '',
    });
    setShowModal(true);
  };

  const handleDeleteClick = (subzona: SubZona) => {
    setSubZonaToDelete(subzona);
    setShowDeleteModal(true);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);

    if (!formData.codigo.trim() || !formData.descripcion.trim()) {
      setError('Código y descripción son obligatorios');
      return;
    }

    try {
      setLoading(true);
      if (editingSubZona) {
        await referenceService.updateSubZona(editingSubZona.id, formData as UpdateSubZonaDto);
      } else {
        await referenceService.createSubZona(formData as CreateSubZonaDto);
      }
      setShowModal(false);
      await loadSubZonas();
    } catch (err: any) {
      setError(err.response?.data?.message || 'Error al guardar la subzona');
      console.error('Error saving subzona:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleDeleteConfirm = async () => {
    if (!subzonaToDelete) return;

    try {
      await referenceService.deleteSubZona(subzonaToDelete.id);
      setSubZonas(subzonas.filter((s) => s.id !== subzonaToDelete.id));
      setShowDeleteModal(false);
      setSubZonaToDelete(null);
    } catch (err) {
      setError('Error al eliminar la subzona');
      console.error('Error deleting subzona:', err);
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  return (
    <div>
      <PageHeader
        title="Gestión de SubZonas"
        icon="fa-solid fa-map-location-dot"
        actions={
          <GradientButton icon="fa-solid fa-plus" onClick={handleCreate}>
            Nueva SubZona
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
                    <th>Código</th>
                    <th>Descripción</th>
                    <th>Acciones</th>
                  </tr>
                </thead>
                <tbody>
                  {subzonas.map((subzona) => (
                    <tr key={subzona.id}>
                      <td>{subzona.codigo || '-'}</td>
                      <td>{subzona.descripcion || '-'}</td>
                      <td>
                        <IconButton
                          icon="fa-solid fa-pen"
                          title="Editar"
                          variant="primary"
                          onClick={() => handleEdit(subzona)}
                        />
                        <IconButton
                          icon="fa-solid fa-trash"
                          title="Eliminar"
                          variant="danger"
                          onClick={() => handleDeleteClick(subzona)}
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

      {/* Modal para crear/editar */}
      {showModal && (
        <div className="modal show d-block" tabIndex={-1} style={{ backgroundColor: 'rgba(0,0,0,0.5)' }}>
          <div className="modal-dialog">
            <div className="modal-content">
              <div className="modal-header">
                <h5 className="modal-title">
                  {editingSubZona ? 'Editar SubZona' : 'Nueva SubZona'}
                </h5>
                <button
                  type="button"
                  className="btn-close"
                  onClick={() => setShowModal(false)}
                  aria-label="Close"
                ></button>
              </div>
              <form onSubmit={handleSubmit}>
                <div className="modal-body">
                  <div className="mb-3">
                    <label className="form-label">
                      Código <span className="text-danger">*</span>
                    </label>
                    <input
                      type="text"
                      className="form-control"
                      name="codigo"
                      value={formData.codigo}
                      onChange={handleChange}
                      placeholder="Código"
                      maxLength={25}
                      required
                    />
                  </div>
                  <div className="mb-3">
                    <label className="form-label">
                      Descripción <span className="text-danger">*</span>
                    </label>
                    <input
                      type="text"
                      className="form-control"
                      name="descripcion"
                      value={formData.descripcion}
                      onChange={handleChange}
                      placeholder="Descripción"
                      maxLength={100}
                      required
                    />
                  </div>
                </div>
                <div className="modal-footer">
                  <button
                    type="button"
                    className="btn btn-secondary"
                    onClick={() => setShowModal(false)}
                  >
                    Cancelar
                  </button>
                  <button type="submit" className="btn btn-primary" disabled={loading}>
                    {loading ? 'Guardando...' : 'Guardar'}
                  </button>
                </div>
              </form>
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
                ¿Está seguro de que desea eliminar la subzona{' '}
                <strong>{subzonaToDelete?.descripcion}</strong>?
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

export default SubZonasPage;
