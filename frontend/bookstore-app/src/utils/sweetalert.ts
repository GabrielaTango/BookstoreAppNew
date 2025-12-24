import Swal from 'sweetalert2';

export const showSuccessAlert = (title: string, text?: string) => {
  return Swal.fire({
    title,
    text,
    icon: 'success',
    confirmButtonText: 'Aceptar',
    confirmButtonColor: '#28a745'
  });
};

export const showErrorAlert = (title: string, text?: string) => {
  return Swal.fire({
    title,
    text,
    icon: 'error',
    confirmButtonText: 'Aceptar',
    confirmButtonColor: '#dc3545'
  });
};

export const showWarningAlert = (title: string, text?: string) => {
  return Swal.fire({
    title,
    text,
    icon: 'warning',
    confirmButtonText: 'Aceptar',
    confirmButtonColor: '#ffc107'
  });
};

export const showInfoAlert = (title: string, text?: string) => {
  return Swal.fire({
    title,
    text,
    icon: 'info',
    confirmButtonText: 'Aceptar',
    confirmButtonColor: '#17a2b8'
  });
};

export const showConfirmDialog = (title: string, text?: string) => {
  return Swal.fire({
    title,
    text,
    icon: 'question',
    showCancelButton: true,
    confirmButtonText: 'Confirmar',
    cancelButtonText: 'Cancelar',
    confirmButtonColor: '#007bff',
    cancelButtonColor: '#6c757d'
  });
};

export const showDeleteConfirmDialog = (itemName: string) => {
  return Swal.fire({
    title: '¿Está seguro?',
    text: `¿Desea eliminar ${itemName}? Esta acción no se puede deshacer.`,
    icon: 'warning',
    showCancelButton: true,
    confirmButtonText: 'Sí, eliminar',
    cancelButtonText: 'Cancelar',
    confirmButtonColor: '#dc3545',
    cancelButtonColor: '#6c757d'
  });
};

export const showLoadingAlert = (title: string = 'Procesando...') => {
  Swal.fire({
    title,
    allowOutsideClick: false,
    didOpen: () => {
      Swal.showLoading();
    }
  });
};

export const closeAlert = () => {
  Swal.close();
};
