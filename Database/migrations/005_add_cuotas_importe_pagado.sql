-- Migration: Add importe_pagado to cuotas table
-- Date: 2025-12-27
-- Description: Adds ImportePagado field to track paid amounts for installments

ALTER TABLE cuotas
ADD COLUMN importe_pagado DECIMAL(17,2) DEFAULT 0;

-- Update existing paid installments to have importe_pagado = importe
UPDATE cuotas SET importe_pagado = Importe WHERE Estado = 'PAG';
