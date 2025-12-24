import { useState, useEffect } from 'react';
import { categoriaGastoService } from '../services/categoriaGastoService';
import type { CategoriaGasto, CreateCategoriaGastoDto, UpdateCategoriaGastoDto } from '../types/categoriaGasto';
import { PageHeader } from '../components/PageHeader';
import { GradientButton } from '../components/GradientButton';
import { IconButton } from '../components/IconButton';

const CategoriasGastoPage = () => {
  const [categorias, setCategorias] = useState<CategoriaGasto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [showModal, setShowModal] = useState(false);
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [categoriaToDelete, setCategoriaToDelete] = useState<CategoriaGasto | null>(null);
  const [editingCategoria, setEditingCategoria] = useState<CategoriaGasto | null>(null);
  const [formData, setFormData] = useState<CreateCategoriaGastoDto | UpdateCategoriaGastoDto>({
    nombre: '',
    descripcion: '',
  });

  useEffect(() => {
    loadCategorias();
  }, []);

  const loadCategorias = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await categoriaGastoService.getAll();
      setCategorias(data);
    } catch (err) {
      setError('Error al cargar las categorías');
      console.error('Error loading categorias:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleCreate = () => {
    setEditingCategoria(null);
    setFormData({ nombre: '', descripcion: '' });
    setShowModal(true);
  };

  const handleEdit = (categoria: CategoriaGasto) => {
    setEditingCategoria(categoria);
    setFormData({
      nombre: categoria.nombre,
      descripcion: categoria.descripcion || '',
      activo: categoria.activo,
    });
    setShowModal(true);
  };

  const handleDeleteClick = (categoria: CategoriaGasto) => {
    setCategoriaToDelete(categoria);
    setShowDeleteModal(true);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);

    if (!formData.nombre.trim()) {
      setError('El nombre es obligatorio');
      return;
    }

    try {
      setLoading(true);
      if (editingCategoria) {
        await categoriaGastoService.update(editingCategoria.id, formData as UpdateCategoriaGastoDto);
      } else {
        await categoriaGastoService.create(formData as CreateCategoriaGastoDto);
      }
      setShowModal(false);
      await loadCategorias();
    } catch (err: any) {
      setError(err.response?.data?.message || 'Error al guardar la categoría');
      console.error('Error saving categoria:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleDeleteConfirm = async () => {
    if (!categoriaToDelete) return;

    try {
      await categoriaGastoService.delete(categoriaToDelete.id);
      setCategorias(categorias.filter((c) => c.id !== categoriaToDelete.id));
      setShowDeleteModal(false);
      setCategoriaToDelete(null);
    } catch (err) {
      setError('Error al eliminar la categoría');
      console.error('Error deleting categoria:', err);
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value, type } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: type === 'checkbox' ? (e.target as HTMLInputElement).checked : value,
    }));
  };

  return (
    <div>
      <PageHeader
        title="Categorías de Gasto"
        icon="fa-solid fa-tags"
        actions={
          <GradientButton icon="fa-solid fa-plus" onClick={handleCreate}>
            Nueva Categoría
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
                    <th>Nombre</th>
                    <th>Descripción</th>
                    <th>Estado</th>
                    <th>Acciones</th>
                  </tr>
                </thead>
                <tbody>
                  {categorias.map((categoria) => (
                    <tr key={categoria.id}>
                      <td>{categoria.nombre}</td>
                      <td>{categoria.descripcion || '-'}</td>
                      <td>
                        <span className={`badge ${categoria.activo ? 'badge-success' : 'badge-secondary'}`}>
                          {categoria.activo ? 'Activa' : 'Inactiva'}
                        </span>
                      </td>
                      <td>
                        <IconButton
                          icon="fa-solid fa-pen"
                          title="Editar"
                          variant="primary"
                          onClick={() => handleEdit(categoria)}
                        />
                        <IconButton
                          icon="fa-solid fa-trash"
                          title="Eliminar"
                          variant="danger"
                          onClick={() => handleDeleteClick(categoria)}
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
                  {editingCategoria ? 'Editar Categoría' : 'Nueva Categoría'}
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
                      Nombre <span className="text-danger">*</span>
                    </label>
                    <input
                      type="text"
                      className="form-control"
                      name="nombre"
                      value={formData.nombre}
                      onChange={handleChange}
                      placeholder="Nombre de la categoría"
                      maxLength={100}
                      required
                    />
                  </div>
                  <div className="mb-3">
                    <label className="form-label">Descripción</label>
                    <textarea
                      className="form-control"
                      name="descripcion"
                      value={formData.descripcion || ''}
                      onChange={handleChange}
                      placeholder="Descripción (opcional)"
                      rows={3}
                      maxLength={255}
                    />
                  </div>
                  {editingCategoria && (
                    <div className="form-check">
                      <input
                        type="checkbox"
                        className="form-check-input"
                        id="activo"
                        name="activo"
                        checked={(formData as UpdateCategoriaGastoDto).activo ?? true}
                        onChange={handleChange}
                      />
                      <label className="form-check-label" htmlFor="activo">
                        Activa
                      </label>
                    </div>
                  )}
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
                ¿Está seguro de que desea eliminar la categoría{' '}
                <strong>{categoriaToDelete?.nombre}</strong>?
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

export default CategoriasGastoPage;
