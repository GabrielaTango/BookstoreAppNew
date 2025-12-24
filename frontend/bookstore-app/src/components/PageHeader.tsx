/**
 * PageHeader Component
 *
 * Reusable page header with icon, title, subtitle and action buttons.
 * Used consistently across all pages for uniform look and feel.
 */

import type React from 'react';
import { Icon } from './Icon';

interface PageHeaderProps {
  title: string;
  icon?: string; // Bootstrap icon name (bi-*) or Font Awesome class
  subtitle?: string;
  actions?: React.ReactNode; // Buttons, search box, or other action elements
  className?: string;
}

export const PageHeader: React.FC<PageHeaderProps> = ({
  title,
  icon,
  subtitle,
  actions,
  className = ''
}) => {
  return (
    <div className={`page-header ${className}`}>
      <div>
        <h1>
          {icon && (
            <div className="icon-circle">
              <Icon name={icon} />
            </div>
          )}
          {title}
        </h1>
        {subtitle && <p className="text-secondary mb-0 mt-2">{subtitle}</p>}
      </div>

      {actions && (
        <div className="d-flex gap-2 align-items-center">
          {actions}
        </div>
      )}
    </div>
  );
};

export default PageHeader;
