export interface Comprobante {
  id: number;
  cliente_Id: number;
  clienteNombre?: string;
  fecha: string;
  tipoComprobante?: string;
  numeroComprobante?: string;
  total: number;
  cae?: string;
  vto?: string;
  bonificacion?: number;
  porcentajeBonif?: number;
  anticipo?: number;
  cuotas?: number;
  valorCuota?: number;
  vendedor_Id?: number;
  vendedorNombre?: string;
  detalles: ComprobanteDetalle[];
}

export interface ComprobanteDetalle {
  id?: number;
  articulo_Id: number;
  articuloCodigo?: string;
  articuloDescripcion?: string;
  cantidad: number;
  precio_Unitario: number;
  subtotal: number;
}

export interface CreateComprobanteDto {
  cliente_Id: number;
  fecha: string;
  tipoComprobante?: string;
  numeroComprobante?: string;
  total: number;
  cae?: string;
  vto?: string;
  bonificacion?: number;
  porcentajeBonif?: number;
  anticipo?: number;
  cuotas?: number;
  valorCuota?: number;
  vendedor_Id?: number;
  detalles: ComprobanteDetalleDto[];
}

export interface UpdateComprobanteDto {
  cliente_Id: number;
  fecha: string;
  tipoComprobante?: string;
  numeroComprobante?: string;
  total: number;
  cae?: string;
  vto?: string;
  bonificacion?: number;
  porcentajeBonif?: number;
  anticipo?: number;
  cuotas?: number;
  valorCuota?: number;
  vendedor_Id?: number;
  detalles: ComprobanteDetalleDto[];
}

export interface ComprobanteDetalleDto {
  articulo_Id: number;
  cantidad: number;
  precio_Unitario: number;
  subtotal: number;
}
