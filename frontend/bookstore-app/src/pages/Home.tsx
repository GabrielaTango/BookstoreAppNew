import { Link } from 'react-router-dom';

const Home = () => {
  return (
    <div>
      <h1 className="mb-4">Sistema de Facturación - Bookstore</h1>
      <p className="lead">Bienvenido al sistema de gestión y facturación</p>

      <div className="row mt-5">
        <div className="col-md-4">
          <div className="card h-100 shadow-sm">
            <div className="card-body">
              <h5 className="card-title">
                <i className="bi bi-people-fill me-2"></i>
                Clientes
              </h5>
              <p className="card-text">
                Gestiona la información de tus clientes, incluyendo datos de contacto,
                condiciones de pago y más.
              </p>
              <Link to="/clientes" className="btn btn-primary">
                Ir a Clientes
              </Link>
            </div>
          </div>
        </div>
        <div className="col-md-4">
          <div className="card h-100 shadow-sm">
            <div className="card-body">
              <h5 className="card-title">
                <i className="bi bi-box-seam me-2"></i>
                Artículos
              </h5>
              <p className="card-text">
                Administra el catálogo de productos y artículos disponibles para la venta.
              </p>
              <Link to="/articulos" className="btn btn-primary">
                Ir a Artículos
              </Link>
            </div>
          </div>
        </div>
        <div className="col-md-4">
          <div className="card h-100 shadow-sm">
            <div className="card-body">
              <h5 className="card-title">
                <i className="bi bi-receipt me-2"></i>
                Facturación
              </h5>
              <p className="card-text">
                Genera y administra comprobantes, facturas y controla las ventas con cálculo de cuotas.
              </p>
              <Link to="/comprobantes" className="btn btn-primary">
                Ir a Facturación
              </Link>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Home;
