/**
 * StatCard Component
 *
 * Displays statistics with gradient icon, value, and optional trend indicator.
 * Used in Dashboard and other analytics views.
 */

import type React from 'react';
import { Icon } from './Icon';

interface TrendData {
  value: string;
  direction: 'up' | 'down';
}

interface StatCardProps {
  title: string;
  value: string | number;
  icon: string; // Bootstrap icon name or Font Awesome class
  variant?: 'primary' | 'success' | 'danger' | 'warning';
  trend?: TrendData;
  className?: string;
  onClick?: () => void;
}

export const StatCard: React.FC<StatCardProps> = ({
  title,
  value,
  icon,
  variant = 'primary',
  trend,
  className = '',
  onClick
}) => {
  const cardClass = `stat-card ${variant} ${className}`;
  const isClickable = Boolean(onClick);

  return (
    <div
      className={cardClass}
      onClick={onClick}
      style={isClickable ? { cursor: 'pointer' } : undefined}
    >
      <div className="d-flex justify-content-between align-items-start mb-2">
        <div className="stat-icon">
          <Icon name={icon} />
        </div>
        {trend && (
          <div className={`stat-trend ${trend.direction}`}>
            <Icon name={trend.direction === 'up' ? 'fa-solid fa-arrow-up' : 'fa-solid fa-arrow-down'} />
            {trend.value}
          </div>
        )}
      </div>
      <div className="stat-body">
        <h3 className="text-mono">{value}</h3>
        <p>{title}</p>
      </div>
    </div>
  );
};

export default StatCard;
