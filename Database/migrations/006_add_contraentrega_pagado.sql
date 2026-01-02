-- Migration: Add ContraEntregaPagado to comprobantes table
-- Date: 2025-12-27
-- Description: Adds ContraEntregaPagado field to track paid amounts for contra entrega

ALTER TABLE comprobantes
ADD COLUMN ContraEntregaPagado DECIMAL(17,2) DEFAULT 0;

-- Update existing records where ContraEntrega was fully paid (assuming if Estado = 'PAG' in first cuota)
-- This is optional, adjust based on your data
