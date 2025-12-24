/**
 * FormGroup Component
 *
 * Wrapper for form inputs with label, icon, and consistent styling.
 * Provides uniform spacing and theme-consistent design.
 */

import type React from 'react';
import { Icon } from './Icon';

interface FormGroupProps {
  label: string;
  children: React.ReactNode;
  icon?: string; // Optional icon (Bootstrap icon name or Font Awesome class)
  required?: boolean;
  error?: string;
  className?: string;
}

export const FormGroup: React.FC<FormGroupProps> = ({
  label,
  children,
  icon,
  required = false,
  error,
  className = ''
}) => {
  return (
    <div className={`form-group ${className}`}>
      <label>
        {label}
        {required && <span className="required ms-1">*</span>}
      </label>

      {icon ? (
        <div className="input-group">
          <Icon name={icon} />
          {children}
        </div>
      ) : (
        children
      )}

      {error && (
        <div className="text-danger mt-1" style={{ fontSize: '0.875rem' }}>
          {error}
        </div>
      )}
    </div>
  );
};

export default FormGroup;
