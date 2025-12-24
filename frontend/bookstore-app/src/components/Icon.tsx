/**
 * Icon Component
 *
 * Wrapper component for migrating from Bootstrap Icons to Font Awesome.
 * Maps Bootstrap Icon names (bi-*) to Font Awesome classes (fa-solid fa-*).
 * Allows gradual migration without changing all icon references at once.
 */

import type React from 'react';

interface IconProps {
  name: string; // Can be Bootstrap Icon name (bi-receipt) or Font Awesome class
  className?: string;
  style?: React.CSSProperties;
}

// Mapping from Bootstrap Icons to Font Awesome
const ICON_MAP: Record<string, string> = {
  // Common icons
  'bi-receipt': 'fa-solid fa-receipt',
  'bi-people': 'fa-solid fa-users',
  'bi-box': 'fa-solid fa-box',
  'bi-cart': 'fa-solid fa-cart-shopping',
  'bi-file-earmark': 'fa-solid fa-file',
  'bi-file-text': 'fa-solid fa-file-lines',
  'bi-file-earmark-text': 'fa-solid fa-file-lines',
  'bi-file-earmark-plus': 'fa-solid fa-file-circle-plus',
  'bi-plus': 'fa-solid fa-plus',
  'bi-plus-circle': 'fa-solid fa-circle-plus',
  'bi-pencil': 'fa-solid fa-pen',
  'bi-pencil-square': 'fa-solid fa-pen-to-square',
  'bi-trash': 'fa-solid fa-trash',
  'bi-trash3': 'fa-solid fa-trash-can',
  'bi-eye': 'fa-solid fa-eye',
  'bi-eye-slash': 'fa-solid fa-eye-slash',
  'bi-download': 'fa-solid fa-download',
  'bi-upload': 'fa-solid fa-upload',
  'bi-search': 'fa-solid fa-magnifying-glass',
  'bi-filter': 'fa-solid fa-filter',
  'bi-x': 'fa-solid fa-xmark',
  'bi-x-circle': 'fa-solid fa-circle-xmark',
  'bi-check': 'fa-solid fa-check',
  'bi-check-circle': 'fa-solid fa-circle-check',
  'bi-exclamation-triangle': 'fa-solid fa-triangle-exclamation',
  'bi-info-circle': 'fa-solid fa-circle-info',
  'bi-question-circle': 'fa-solid fa-circle-question',

  // Dashboard icons
  'bi-speedometer2': 'fa-solid fa-gauge',
  'bi-grid': 'fa-solid fa-grip',
  'bi-graph-up': 'fa-solid fa-chart-line',
  'bi-bar-chart': 'fa-solid fa-chart-bar',
  'bi-pie-chart': 'fa-solid fa-chart-pie',

  // Navigation icons
  'bi-house': 'fa-solid fa-house',
  'bi-house-door': 'fa-solid fa-house',
  'bi-gear': 'fa-solid fa-gear',
  'bi-sliders': 'fa-solid fa-sliders',
  'bi-list': 'fa-solid fa-list',
  'bi-grid-3x3': 'fa-solid fa-table-cells',

  // User icons
  'bi-person': 'fa-solid fa-user',
  'bi-person-circle': 'fa-solid fa-circle-user',
  'bi-person-plus': 'fa-solid fa-user-plus',

  // Business icons
  'bi-currency-dollar': 'fa-solid fa-dollar-sign',
  'bi-cash': 'fa-solid fa-money-bills',
  'bi-credit-card': 'fa-solid fa-credit-card',
  'bi-wallet': 'fa-solid fa-wallet',
  'bi-tag': 'fa-solid fa-tag',
  'bi-tags': 'fa-solid fa-tags',
  'bi-percent': 'fa-solid fa-percent',

  // Location icons
  'bi-geo-alt': 'fa-solid fa-location-dot',
  'bi-map': 'fa-solid fa-map',
  'bi-building': 'fa-solid fa-building',

  // Communication icons
  'bi-envelope': 'fa-solid fa-envelope',
  'bi-telephone': 'fa-solid fa-phone',
  'bi-chat': 'fa-solid fa-message',
  'bi-bell': 'fa-solid fa-bell',

  // Document icons
  'bi-file-earmark-pdf': 'fa-solid fa-file-pdf',
  'bi-file-earmark-excel': 'fa-solid fa-file-excel',
  'bi-file-earmark-word': 'fa-solid fa-file-word',
  'bi-printer': 'fa-solid fa-print',
  'bi-clipboard': 'fa-solid fa-clipboard',

  // Arrow icons
  'bi-arrow-up': 'fa-solid fa-arrow-up',
  'bi-arrow-down': 'fa-solid fa-arrow-down',
  'bi-arrow-left': 'fa-solid fa-arrow-left',
  'bi-arrow-right': 'fa-solid fa-arrow-right',
  'bi-chevron-up': 'fa-solid fa-chevron-up',
  'bi-chevron-down': 'fa-solid fa-chevron-down',
  'bi-chevron-left': 'fa-solid fa-chevron-left',
  'bi-chevron-right': 'fa-solid fa-chevron-right',

  // Time icons
  'bi-clock': 'fa-solid fa-clock',
  'bi-clock-history': 'fa-solid fa-clock-rotate-left',
  'bi-calendar': 'fa-solid fa-calendar',
  'bi-calendar-event': 'fa-solid fa-calendar-days',

  // Action icons
  'bi-save': 'fa-solid fa-floppy-disk',
  'bi-send': 'fa-solid fa-paper-plane',
  'bi-share': 'fa-solid fa-share-nodes',
  'bi-link': 'fa-solid fa-link',
  'bi-clipboard-check': 'fa-solid fa-clipboard-check',

  // Status icons
  'bi-star': 'fa-solid fa-star',
  'bi-heart': 'fa-solid fa-heart',
  'bi-bookmark': 'fa-solid fa-bookmark',
  'bi-flag': 'fa-solid fa-flag',
  'bi-shield': 'fa-solid fa-shield',
  'bi-lock': 'fa-solid fa-lock',
  'bi-unlock': 'fa-solid fa-unlock',
};

export const Icon: React.FC<IconProps> = ({ name, className = '', style }) => {
  // If name starts with 'bi-', map it to Font Awesome
  // Otherwise, assume it's already a Font Awesome class
  const iconClass = name.startsWith('bi-')
    ? (ICON_MAP[name] || 'fa-solid fa-circle-question') // fallback icon if not mapped
    : name;

  return <i className={`${iconClass} ${className}`} style={style} />;
};

export default Icon;
