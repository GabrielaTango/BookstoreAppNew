/**
 * GradientCard Component
 *
 * Card with gradient header icon and title.
 * Used for section headers in forms and content areas.
 */

import type React from 'react';
import { Icon } from './Icon';

interface GradientCardProps {
  title: string;
  icon?: string; // Bootstrap icon name or Font Awesome class
  children: React.ReactNode;
  className?: string;
}

export const GradientCard: React.FC<GradientCardProps> = ({
  title,
  icon,
  children,
  className = ''
}) => {
  return (
    <div className={`card ${className}`}>
      <div className="card-body">
        {(title || icon) && (
          <div className="card-header-custom">
            {icon && <Icon name={icon} />}
            <h5>{title}</h5>
          </div>
        )}
        {children}
      </div>
    </div>
  );
};

export default GradientCard;
