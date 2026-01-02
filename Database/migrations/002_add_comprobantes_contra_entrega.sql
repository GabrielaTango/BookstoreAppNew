-- Migration: 002_add_comprobantes_contra_entrega
-- Description: Agrega campo ContraEntrega a la tabla comprobantes
-- Date: 2025-12-27

-- Agregar columna ContraEntrega si no existe
SET @dbname = DATABASE();

SET @columnExists = (
    SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_SCHEMA = @dbname AND TABLE_NAME = 'comprobantes' AND COLUMN_NAME = 'ContraEntrega'
);
SET @sql = IF(@columnExists = 0,
    'ALTER TABLE comprobantes ADD COLUMN ContraEntrega decimal(17,2) DEFAULT 0 AFTER Anticipo',
    'SELECT "Column ContraEntrega already exists"'
);
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;
