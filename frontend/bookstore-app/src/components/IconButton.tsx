/**
 * IconButton Component
 *
 * Small square button with just an icon.
 * Used for table actions (edit, delete, view, etc.)
 */

import type React from 'react';
import { Icon } from './Icon';

interface IconButtonProps {
  icon: string; // Bootstrap icon name or Font Awesome class
  onClick?: () => void;
  variant?: 'default' | 'primary' | 'secondary' | 'danger' | 'warning' | 'success' | 'info';
  title?: string; // Tooltip text
  disabled?: boolean;
  className?: string;
}

export const IconButton: React.FC<IconButtonProps> = ({
  icon,
  onClick,
  variant = 'default',
  title,
  disabled = false,
  className = ''
}) => {
  let variantClass = '';

  if (variant === 'danger') {
    variantClass = 'text-danger';
  } else if (variant === 'warning') {
    variantClass = 'text-warning';
  } else if (variant === 'success') {
    variantClass = 'text-success';
  } else if (variant === 'primary') {
    variantClass = 'text-primary';
  } else if (variant === 'secondary') {
    variantClass = 'text-secondary';
  } else if (variant === 'info') {
    variantClass = 'text-info';
  }

  return (
    <button
      className={`btn-icon ${variantClass} ${className}`}
      onClick={onClick}
      title={title}
      disabled={disabled}
      type="button"
    >
      <Icon name={icon} />
    </button>
  );
};

export default IconButton;
