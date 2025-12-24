/**
 * DataTable Component
 *
 * Styled table wrapper with theme-consistent design.
 * Handles responsive display and applies custom table styles.
 */

import type React from 'react';

interface Column {
  header: string;
  key: string;
  render?: (item: any) => React.ReactNode;
}

interface DataTableProps {
  columns: Column[];
  data: any[];
  emptyMessage?: string;
  className?: string;
}

export const DataTable: React.FC<DataTableProps> = ({
  columns,
  data,
  emptyMessage = 'No hay datos para mostrar',
  className = ''
}) => {
  if (data.length === 0) {
    return (
      <div className="text-center py-5 text-secondary">
        <p className="mb-0">{emptyMessage}</p>
      </div>
    );
  }

  return (
    <div className={`table-responsive ${className}`}>
      <table className="custom-table">
        <thead>
          <tr>
            {columns.map((column) => (
              <th key={column.key}>{column.header}</th>
            ))}
          </tr>
        </thead>
        <tbody>
          {data.map((item, index) => (
            <tr key={item.id || index}>
              {columns.map((column) => (
                <td key={column.key}>
                  {column.render ? column.render(item) : item[column.key]}
                </td>
              ))}
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default DataTable;
