-- Migration: 001_add_subzonas_provincia_cp_localidad
-- Description: Agrega campos Provincia, Código Postal y Localidad a la tabla subzonas
-- Date: 2025-12-27

-- Verificar si las columnas ya existen antes de agregarlas
SET @dbname = DATABASE();

-- Agregar columna provincia_id si no existe
SET @columnExists = (
    SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_SCHEMA = @dbname AND TABLE_NAME = 'subzonas' AND COLUMN_NAME = 'provincia_id'
);
SET @sql = IF(@columnExists = 0,
    'ALTER TABLE subzonas ADD COLUMN provincia_id INT NULL',
    'SELECT "Column provincia_id already exists"'
);
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- Agregar columna codigo_postal si no existe
SET @columnExists = (
    SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_SCHEMA = @dbname AND TABLE_NAME = 'subzonas' AND COLUMN_NAME = 'codigo_postal'
);
SET @sql = IF(@columnExists = 0,
    'ALTER TABLE subzonas ADD COLUMN codigo_postal VARCHAR(10) NULL',
    'SELECT "Column codigo_postal already exists"'
);
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- Agregar columna localidad si no existe
SET @columnExists = (
    SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_SCHEMA = @dbname AND TABLE_NAME = 'subzonas' AND COLUMN_NAME = 'localidad'
);
SET @sql = IF(@columnExists = 0,
    'ALTER TABLE subzonas ADD COLUMN localidad VARCHAR(100) NULL',
    'SELECT "Column localidad already exists"'
);
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- Agregar FK si no existe
SET @fkExists = (
    SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
    WHERE TABLE_SCHEMA = @dbname AND TABLE_NAME = 'subzonas' AND CONSTRAINT_NAME = 'FK_subzonas_provincias'
);
SET @sql = IF(@fkExists = 0,
    'ALTER TABLE subzonas ADD CONSTRAINT FK_subzonas_provincias FOREIGN KEY (provincia_id) REFERENCES provincias(id)',
    'SELECT "FK_subzonas_provincias already exists"'
);
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- Agregar índice si no existe
SET @indexExists = (
    SELECT COUNT(*) FROM INFORMATION_SCHEMA.STATISTICS
    WHERE TABLE_SCHEMA = @dbname AND TABLE_NAME = 'subzonas' AND INDEX_NAME = 'FK_subzonas_provincias'
);
SET @sql = IF(@indexExists = 0,
    'ALTER TABLE subzonas ADD INDEX FK_subzonas_provincias (provincia_id)',
    'SELECT "Index FK_subzonas_provincias already exists"'
);
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;
