import { useState, useEffect } from 'react';
import { useNavigate, useParams, Link } from 'react-router-dom';
import { comprobanteService } from '../services/comprobanteService';
import { clienteService } from '../services/clienteService';
import { articuloService } from '../services/articuloService';
import { referenceService } from '../services/referenceService';
import { PageHeader } from '../components/PageHeader';
import { GradientButton } from '../components/GradientButton';
import { GradientCard } from '../components/GradientCard';
import { FormGroup } from '../components/FormGroup';
import { Icon } from '../components/Icon';
import type { CreateComprobanteDto, UpdateComprobanteDto, ComprobanteDetalleDto } from '../types/comprobante';
import type { Cliente } from '../types/cliente';
import type { Articulo } from '../types/articulo';
import type { Vendedor } from '../types/references';
import IconButton from '../components/IconButton';

interface ItemTemp extends ComprobanteDetalleDto {
  tempId: number;
  articuloDescripcion?: string;
  articuloCodigo?: string;
}

const ComprobanteFormPage = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const isEditMode = !!id;

  const [clientes, setClientes] = useState<Cliente[]>([]);
  const [articulos, setArticulos] = useState<Articulo[]>([]);
  const [vendedores, setVendedores] = useState<Vendedor[]>([]);

  const [selectedClienteId, setSelectedClienteId] = useState<number | null>(null);
  const [selectedCliente, setSelectedCliente] = useState<Cliente | null>(null);
  const [selectedVendedorId, setSelectedVendedorId] = useState<number | null>(null);

  const [items, setItems] = useState<ItemTemp[]>([]);
  const [nextTempId, setNextTempId] = useState(1);

  // Formulario de item
  const [showItemModal, setShowItemModal] = useState(false);
  const [editingItem, setEditingItem] = useState<ItemTemp | null>(null);
  const [itemForm, setItemForm] = useState({
    articulo_Id: 0,
    cantidad: 1,
    precio_Unitario: 0,
  });

  // Cálculo de cuotas (se define ANTES de cargar items)
  const [anticipo, setAnticipo] = useState<number>(0);
  const [contraEntrega, setContraEntrega] = useState<number>(0);
  const [cantidadCuotas, setCantidadCuotas] = useState<number>(1);
  const [valorCuota, setValorCuota] = useState<number>(0);

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    loadInitialData();
  }, []);

  useEffect(() => {
    if (isEditMode && id) {
      loadComprobante(parseInt(id));
    }
  }, [id, isEditMode]);

  useEffect(() => {
    if (selectedClienteId) {
      loadCliente(selectedClienteId);
    }
  }, [selectedClienteId]);

  // El total se calcula automáticamente
  const calcularTotalComprobante = (): number => {
    return anticipo + contraEntrega + (cantidadCuotas * valorCuota);
  };

  const loadInitialData = async () => {
    try {
      setLoading(true);
      const [clientesData, articulosData, vendedoresData] = await Promise.all([
        clienteService.getAll(),
        articuloService.getAll(),
        referenceService.getVendedores(),
      ]);
      setClientes(clientesData);
      setArticulos(articulosData);
      setVendedores(vendedoresData);
    } catch (err) {
      setError('Error al cargar datos iniciales');
      console.error('Error loading initial data:', err);
    } finally {
      setLoading(false);
    }
  };

  const loadComprobante = async (comprobanteId: number) => {
    try {
      setLoading(true);
      const comprobante = await comprobanteService.getById(comprobanteId);
      setSelectedClienteId(comprobante.cliente_Id);
      setSelectedVendedorId(comprobante.vendedor_Id || null);
      setAnticipo(comprobante.anticipo || 0);
      setContraEntrega(comprobante.contraEntrega || 0);
      setCantidadCuotas(comprobante.cuotas || 1);
      setValorCuota(comprobante.valorCuota || 0);

      const itemsTemp: ItemTemp[] = comprobante.detalles.map((d, index) => ({
        tempId: index + 1,
        articulo_Id: d.articulo_Id,
        articuloCodigo: d.articuloCodigo,
        articuloDescripcion: d.articuloDescripcion,
        cantidad: d.cantidad,
        precio_Unitario: d.precio_Unitario,
        subtotal: d.subtotal,
      }));
      setItems(itemsTemp);
      setNextTempId(itemsTemp.length + 1);
    } catch (err) {
      setError('Error al cargar el comprobante');
      console.error('Error loading comprobante:', err);
    } finally {
      setLoading(false);
    }
  };

  const loadCliente = async (clienteId: number) => {
    try {
      const cliente = await clienteService.getById(clienteId);
      setSelectedCliente(cliente);
      // Establecer vendedor del cliente como predeterminado
      if (cliente.vendedor_Id && !selectedVendedorId) {
        setSelectedVendedorId(cliente.vendedor_Id);
      }
    } catch (err) {
      console.error('Error loading cliente:', err);
    }
  };

  const handleClienteChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const clienteId = parseInt(e.target.value);
    setSelectedClienteId(clienteId);
  };

  const handleVendedorChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const vendedorId = parseInt(e.target.value);
    setSelectedVendedorId(vendedorId || null);
  };

  const handleAddItem = () => {
    setEditingItem(null);
    setItemForm({
      articulo_Id: 0,
      cantidad: 1,
      precio_Unitario: 0,
    });
    setShowItemModal(true);
  };

  const handleEditItem = (item: ItemTemp) => {
    setEditingItem(item);
    setItemForm({
      articulo_Id: item.articulo_Id,
      cantidad: item.cantidad,
      precio_Unitario: item.precio_Unitario,
    });
    setShowItemModal(true);
  };

  const handleDeleteItem = (tempId: number) => {
    setItems(items.filter(item => item.tempId !== tempId));
  };

  const handleArticuloSelect = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const articuloId = parseInt(e.target.value);
    const articulo = articulos.find(a => a.id === articuloId);

    setItemForm({
      ...itemForm,
      articulo_Id: articuloId,
      precio_Unitario: articulo?.precio || 0,
    });
  };

  const handleItemFormChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setItemForm({
      ...itemForm,
      [name]: parseFloat(value) || 0,
    });
  };

  const handleSaveItem = () => {
    if (itemForm.articulo_Id === 0) {
      setError('Debe seleccionar un artículo');
      return;
    }

    if (itemForm.cantidad <= 0) {
      setError('La cantidad debe ser mayor a 0');
      return;
    }

    const articulo = articulos.find(a => a.id === itemForm.articulo_Id);
    const subtotal = itemForm.cantidad * itemForm.precio_Unitario;

    const newItem: ItemTemp = {
      tempId: editingItem ? editingItem.tempId : nextTempId,
      articulo_Id: itemForm.articulo_Id,
      articuloCodigo: articulo?.codigo,
      articuloDescripcion: articulo?.descripcion,
      cantidad: itemForm.cantidad,
      precio_Unitario: itemForm.precio_Unitario,
      subtotal: subtotal,
    };

    if (editingItem) {
      setItems(items.map(item => item.tempId === editingItem.tempId ? newItem : item));
    } else {
      setItems([...items, newItem]);
      setNextTempId(nextTempId + 1);
    }

    setShowItemModal(false);
    setError(null);
  };

  const calcularTotalItems = (): number => {
    return items.reduce((sum, item) => sum + item.subtotal, 0);
  };

  const validarTotales = (): boolean => {
    const totalItems = calcularTotalItems();
    const totalComprobante = calcularTotalComprobante();
    return Math.abs(totalItems - totalComprobante) < 0.01; // Tolerancia de centavos
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);

    if (!selectedClienteId) {
      setError('Debe seleccionar un cliente');
      return;
    }

    const totalComprobante = calcularTotalComprobante();

    if (totalComprobante <= 0) {
      setError('El total del comprobante debe ser mayor a 0');
      return;
    }

    if (items.length === 0) {
      setError('Debe agregar al menos un item');
      return;
    }

    if (!validarTotales()) {
      const totalItems = calcularTotalItems();
      const diferencia = totalComprobante - totalItems;
      setError(`El total de items ($${totalItems.toFixed(2)}) no coincide con el total calculado ($${totalComprobante.toFixed(2)}). Diferencia: $${diferencia.toFixed(2)}`);
      return;
    }

    const detalles: ComprobanteDetalleDto[] = items.map(item => ({
      articulo_Id: item.articulo_Id,
      cantidad: item.cantidad,
      precio_Unitario: item.precio_Unitario,
      subtotal: item.subtotal,
    }));

    const dto: CreateComprobanteDto | UpdateComprobanteDto = {
      cliente_Id: selectedClienteId,
      fecha: new Date().toISOString(),
      tipoComprobante: 'FC',
      total: totalComprobante,
      vendedor_Id: selectedVendedorId || undefined,
      anticipo: anticipo,
      contraEntrega: contraEntrega,
      cuotas: cantidadCuotas,
      valorCuota: valorCuota,
      detalles: detalles,
    };

    try {
      setLoading(true);
      if (isEditMode && id) {
        await comprobanteService.update(parseInt(id), dto as UpdateComprobanteDto);
      } else {
        await comprobanteService.create(dto as CreateComprobanteDto);
      }
      navigate('/comprobantes');
    } catch (err: any) {
      setError(
        err.response?.data?.message ||
        `Error al ${isEditMode ? 'actualizar' : 'crear'} el comprobante`
      );
      console.error('Error saving comprobante:', err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <PageHeader
        title={isEditMode ? 'Editar Comprobante' : 'Nuevo Comprobante'}
        icon="fa-solid fa-receipt"
        actions={
          <Link to="/comprobantes" className="btn-secondary-action">
            <Icon name="fa-solid fa-arrow-left" />
            Volver
          </Link>
        }
      />

      {error && (
        <div className="alert alert-danger alert-dismissible fade show" role="alert">
          <Icon name="fa-solid fa-triangle-exclamation" />
          {error}
          <button
            type="button"
            className="btn-close"
            onClick={() => setError(null)}
            aria-label="Close"
          ></button>
        </div>
      )}

      <form onSubmit={handleSubmit}>
        <GradientCard title="Datos del Cliente" icon="fa-solid fa-circle-user">
          <div className="row">
            <div className="col-md-6">
              <FormGroup label="Cliente" required>
                <select
                  className="form-select"
                  value={selectedClienteId || ''}
                  onChange={handleClienteChange}
                  required
                >
                  <option value="">Seleccione un cliente</option>
                  {clientes.map((cliente) => (
                    <option key={cliente.id} value={cliente.id}>
                      {cliente.nombre} - {cliente.nroDocumento}
                    </option>
                  ))}
                </select>
              </FormGroup>
            </div>

            {selectedCliente && (
              <>
                <div className="col-md-6">
                  <FormGroup label="Documento">
                    <input
                      type="text"
                      className="form-control"
                      value={`${selectedCliente.tipoDocumento || ''} ${selectedCliente.nroDocumento || ''}`}
                      readOnly
                    />
                  </FormGroup>
                </div>
                <div className="col-md-6">
                  <FormGroup label="Domicilio">
                    <input
                      type="text"
                      className="form-control"
                      value={selectedCliente.domicilioComercial || '-'}
                      readOnly
                    />
                  </FormGroup>
                </div>
                <div className="col-md-3">
                  <FormGroup label="Teléfono">
                    <input
                      type="text"
                      className="form-control"
                      value={selectedCliente.telefono || '-'}
                      readOnly
                    />
                  </FormGroup>
                </div>
                <div className="col-md-3">
                  <FormGroup label="Condición IVA">
                    <input
                      type="text"
                      className="form-control"
                      value={selectedCliente.categoriaIva || '-'}
                      readOnly
                    />
                  </FormGroup>
                </div>
              </>
            )}
          </div>
        </GradientCard>

        <GradientCard title="Vendedor" icon="fa-solid fa-user-tie" className="mt-4">
          <div className="row">
            <div className="col-md-6">
              <FormGroup label="Vendedor Asignado">
                <select
                  className="form-select"
                  value={selectedVendedorId || ''}
                  onChange={handleVendedorChange}
                >
                  <option value="">Sin vendedor</option>
                  {vendedores.map((vendedor) => (
                    <option key={vendedor.id} value={vendedor.id}>
                      {vendedor.descripcion}
                    </option>
                  ))}
                </select>
              </FormGroup>
            </div>
          </div>
        </GradientCard>

        <GradientCard title="Cálculo de Cuotas" icon="fa-solid fa-calculator" className="mt-4">
          <div className="row">
            <div className="col-md-2">
              <FormGroup label="Anticipo">
                <div className="input-group">
                  <span className="input-group-text">$</span>
                  <input
                    type="number"
                    className="form-control"
                    value={anticipo}
                    onChange={(e) => setAnticipo(parseFloat(e.target.value) || 0)}
                    min="0"
                    step="0.01"
                  />
                </div>
                <small className="form-text text-muted">Pago anticipado</small>
              </FormGroup>
            </div>
            <div className="col-md-2">
              <FormGroup label="Contra Entrega">
                <div className="input-group">
                  <span className="input-group-text">$</span>
                  <input
                    type="number"
                    className="form-control"
                    value={contraEntrega}
                    onChange={(e) => setContraEntrega(parseFloat(e.target.value) || 0)}
                    min="0"
                    step="0.01"
                  />
                </div>
                <small className="form-text text-muted">Factura</small>
              </FormGroup>
            </div>
            <div className="col-md-2">
              <FormGroup label="Cuotas">
                <input
                  type="number"
                  className="form-control"
                  value={cantidadCuotas}
                  onChange={(e) => setCantidadCuotas(parseInt(e.target.value) || 1)}
                  min="1"
                  max="60"
                />
              </FormGroup>
            </div>
            <div className="col-md-2">
              <FormGroup label="Valor Cuota">
                <div className="input-group">
                  <span className="input-group-text">$</span>
                  <input
                    type="number"
                    className="form-control"
                    value={valorCuota}
                    onChange={(e) => setValorCuota(parseFloat(e.target.value) || 0)}
                    min="0"
                    step="0.01"
                  />
                </div>
              </FormGroup>
            </div>
            <div className="col-md-4">
              <FormGroup label="TOTAL">
                <div className="input-group">
                  <span className="input-group-text">$</span>
                  <input
                    type="text"
                    className="form-control form-control-lg fw-bold text-end bg-light"
                    value={calcularTotalComprobante().toFixed(2)}
                    readOnly
                  />
                </div>
                <small className="form-text text-muted">
                  Anticipo + Contra Entrega + ({cantidadCuotas} × ${valorCuota.toFixed(2)})
                </small>
              </FormGroup>
            </div>
          </div>
        </GradientCard>

        <GradientCard title="Items del Comprobante" icon="fa-solid fa-box" className="mt-4">
          <div className="d-flex justify-content-end">
            <GradientButton
              className='btn-primary-action'
              type="button"
              icon="fa-solid fa-circle-plus"
              onClick={handleAddItem}
            >
              Agregar Item
            </GradientButton>
          </div>
          {items.length === 0 ? (
            <div className="alert alert-info mt-3 d-flex align-items-center justify-content-center" onClick={handleAddItem}>
              No hay items agregados. Haga clic en
              <Link to={"#"} onClick={handleAddItem}>Agregar Item</Link>
              para comenzar.
            </div>
          ) : (
            <div className="table-responsive mt-3">
              <table className="table table-hover mb-0">
                <thead>
                  <tr>
                    <th>Código</th>
                    <th>Descripción</th>
                    <th className="text-end">Cantidad</th>
                    <th className="text-end">Precio Unit.</th>
                    <th className="text-end">Subtotal</th>
                    <th className="text-center">Acciones</th>
                  </tr>
                </thead>
                <tbody>
                  {items.map((item) => (
                    <tr key={item.tempId}>
                      <td>{item.articuloCodigo || '-'}</td>
                      <td>{item.articuloDescripcion}</td>
                      <td className="text-end">{item.cantidad}</td>
                      <td className="text-end">${item.precio_Unitario.toFixed(2)}</td>
                      <td className="text-end">${item.subtotal.toFixed(2)}</td>
                      <td className="text-center">
                        <IconButton
                          className='me-2'
                          icon="fa-solid fa-pen"
                          title="Ver PDF"
                          variant="primary"
                          onClick={() => handleEditItem(item)}
                        />
                        <IconButton
                          icon="fa-solid fa-trash"
                          title="Eliminar"
                          variant="danger"
                          onClick={() => handleDeleteItem(item.tempId)}
                        />
                      </td>
                    </tr>
                  ))}
                  <tr className="table-primary">
                    <td colSpan={4} className="text-end"><strong>TOTAL ITEMS:</strong></td>
                    <td className="text-end"><strong>${calcularTotalItems().toFixed(2)}</strong></td>
                    <td></td>
                  </tr>
                </tbody>
              </table>
            </div>
          )}
          {items.length > 0 && calcularTotalComprobante() > 0 && (
            <div className={`alert mt-3 mb-0 ${validarTotales() ? 'alert-success' : 'alert-warning'}`}>
              <Icon name={validarTotales() ? 'fa-solid fa-circle-check' : 'fa-solid fa-triangle-exclamation'} />
              {validarTotales() ? (
                <strong>Los totales coinciden correctamente</strong>
              ) : (
                <>
                  <strong>Diferencia:</strong> ${(calcularTotalComprobante() - calcularTotalItems()).toFixed(2)}
                  <span className="ms-2">(Total calculado: ${calcularTotalComprobante().toFixed(2)} - Total items: ${calcularTotalItems().toFixed(2)})</span>
                </>
              )}
            </div>
          )}
        </GradientCard>

        <div className="d-flex gap-2 mt-4">
          <GradientButton
            type="submit"
            icon="fa-solid fa-floppy-disk"
            disabled={loading || items.length === 0}
          >
            {loading ? 'Guardando...' : isEditMode ? 'Actualizar' : 'Crear'}
          </GradientButton>
          <Link to="/comprobantes" className="btn-secondary-action">
            Cancelar
          </Link>
        </div>
      </form>

      {showItemModal && (
        <div className="modal show d-block" tabIndex={-1} style={{ backgroundColor: 'rgba(0,0,0,0.5)' }}>
          <div className="modal-dialog modal-dialog-centered">
            <div className="modal-content">
              <div className="modal-header">
                <h5 className="modal-title">
                  <Icon name="fa-solid fa-box" />
                  {editingItem ? 'Editar Item' : 'Agregar Item'}
                </h5>
                <button
                  type="button"
                  className="btn-close"
                  onClick={() => setShowItemModal(false)}
                  aria-label="Close"
                ></button>
              </div>
              <div className="modal-body">
                <FormGroup label="Artículo" required>
                  <select
                    className="form-select"
                    value={itemForm.articulo_Id}
                    onChange={handleArticuloSelect}
                    required
                  >
                    <option value="0">Seleccione un artículo</option>
                    {articulos.map((articulo) => (
                      <option key={articulo.id} value={articulo.id}>
                        {articulo.codigo} - {articulo.descripcion} (${articulo.precio?.toFixed(2)})
                      </option>
                    ))}
                  </select>
                </FormGroup>

                <FormGroup label="Cantidad" required>
                  <input
                    type="number"
                    className="form-control"
                    name="cantidad"
                    value={itemForm.cantidad}
                    onChange={handleItemFormChange}
                    min="1"
                    required
                  />
                </FormGroup>

                <FormGroup label="Precio Unitario" required>
                  <div className="input-group">
                    <span className="input-group-text">$</span>
                    <input
                      type="number"
                      className="form-control"
                      name="precio_Unitario"
                      value={itemForm.precio_Unitario}
                      onChange={handleItemFormChange}
                      min="0"
                      step="0.01"
                      required
                    />
                  </div>
                </FormGroup>

                <div className="alert alert-info mb-0">
                  <Icon name="fa-solid fa-calculator" />
                  <strong>Subtotal:</strong> ${(itemForm.cantidad * itemForm.precio_Unitario).toFixed(2)}
                </div>
              </div>
              <div className="modal-footer">
                <button
                  type="button"
                  className="btn-secondary-action"
                  onClick={() => setShowItemModal(false)}
                >
                  Cancelar
                </button>
                <GradientButton
                  onClick={handleSaveItem}
                  icon={editingItem ? 'fa-solid fa-circle-check' : 'fa-solid fa-circle-plus'}
                >
                  {editingItem ? 'Actualizar' : 'Agregar'}
                </GradientButton>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default ComprobanteFormPage;
