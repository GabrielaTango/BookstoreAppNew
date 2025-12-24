# Bookstore App - Sistema de Facturación

Sistema completo de gestión y facturación para librerías, desarrollado con React + TypeScript en el frontend y ASP.NET Core 8 en el backend.

## Tecnologías Utilizadas

### Backend
- ASP.NET Core 8 Web API
- Dapper (micro-ORM)
- MySQL
- C#

### Frontend
- React 18
- TypeScript
- Bootstrap 5
- React Router
- Axios
- Vite

## Requisitos Previos

- .NET 8 SDK
- Node.js 20.19+ o 22.12+
- MySQL Server (ya configurado en servidor remoto)

## Estructura del Proyecto

```
BookstoreAppIA/
├── backend/BookstoreAPI/        # API REST en ASP.NET Core 8
│   ├── Controllers/             # Endpoints de la API
│   ├── Services/                # Lógica de negocio
│   ├── Repositories/            # Acceso a datos con Dapper
│   ├── Models/                  # Entidades del dominio
│   ├── DTOs/                    # Data Transfer Objects
│   └── Data/                    # Contexto de base de datos
├── frontend/bookstore-app/      # Aplicación React
│   └── src/
│       ├── components/          # Componentes reutilizables
│       ├── pages/               # Páginas de la aplicación
│       ├── services/            # Servicios API
│       ├── types/               # Definiciones TypeScript
│       └── layouts/             # Layouts principales
└── Database/                    # Scripts SQL
    └── db.sql                   # Schema de la base de datos
```

## Configuración y Ejecución

### 1. Backend (ASP.NET Core 8)

```bash
# Navegar a la carpeta del backend
cd backend/BookstoreAPI

# Restaurar dependencias
dotnet restore

# Ejecutar la API
dotnet run
```

La API estará disponible en:
- **HTTP**: http://localhost:5000
- **HTTPS**: https://localhost:5001
- **Swagger**: http://localhost:5000/swagger

### 2. Frontend (React + TypeScript)

```bash
# Navegar a la carpeta del frontend
cd frontend/bookstore-app

# Instalar dependencias
npm install

# Ejecutar en modo desarrollo
npm run dev
```

La aplicación estará disponible en:
- **URL**: http://localhost:5173

## Funcionalidades Implementadas

### ABM de Clientes ✅
- Listado de clientes con tabla responsive
- Crear nuevo cliente con validaciones
- Editar cliente existente
- Eliminar cliente con confirmación
- Búsqueda por ID y código

### Campos de Cliente
- Información general (código, nombre, contacto)
- Datos fiscales (CUIT/DNI, IIBB, categoría IVA)
- Contacto (teléfonos, email)
- Domicilios (comercial y particular)
- Condiciones comerciales (forma de pago, descuentos)
- Observaciones

## API Endpoints

### Clientes

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/clientes` | Obtener todos los clientes |
| GET | `/api/clientes/{id}` | Obtener cliente por ID |
| GET | `/api/clientes/codigo/{codigo}` | Obtener cliente por código |
| POST | `/api/clientes` | Crear nuevo cliente |
| PUT | `/api/clientes/{id}` | Actualizar cliente |
| DELETE | `/api/clientes/{id}` | Eliminar cliente |

### Health Check

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/health` | Verificar estado de la API |
| GET | `/api/health/database` | Verificar conexión a base de datos |

## Conexión a Base de Datos

La aplicación está configurada para conectarse a:
- **Servidor**: 179.43.121.202
- **Base de datos**: BookstoreApp
- **Usuario**: chami

La cadena de conexión está configurada en:
- `backend/BookstoreAPI/appsettings.json`
- `backend/BookstoreAPI/appsettings.Development.json`

## Próximos Desarrollos

- [ ] ABM de Artículos
- [ ] ABM de Vendedores
- [ ] Gestión de Zonas y Provincias
- [ ] Módulo de Facturación
- [ ] Gestión de Comprobantes
- [ ] Control de Cuotas y Pagos
- [ ] Reportes y estadísticas
- [ ] Autenticación y autorización

## Comandos Útiles

### Backend
```bash
# Compilar el proyecto
dotnet build

# Ejecutar tests
dotnet test

# Publicar para producción
dotnet publish -c Release
```

### Frontend
```bash
# Compilar para producción
npm run build

# Preview de producción
npm run preview

# Linter
npm run lint
```

## Notas de Desarrollo

- El backend usa el patrón Repository + Service
- La validación se realiza con Data Annotations en los DTOs
- El frontend usa React Hooks para manejo de estado
- Bootstrap se importa globalmente desde main.tsx
- CORS está configurado para los puertos 5173 y 5174 de Vite

## Solución de Problemas

### Error de conexión a la API desde el frontend
Verificar que:
1. El backend esté ejecutándose en http://localhost:5000
2. CORS esté correctamente configurado
3. La URL base en `frontend/src/services/api.ts` sea correcta

### Error de conexión a base de datos
1. Verificar que el servidor MySQL sea accesible
2. Validar credenciales en appsettings.json
3. Probar con el endpoint `/api/health/database`

## Licencia

Proyecto privado - Todos los derechos reservados
