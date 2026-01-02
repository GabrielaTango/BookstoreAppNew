-- Migration: Create remitos table
-- Date: 2025-12-27
-- Description: Adds remitos (shipping receipts) table for tracking package shipments

CREATE TABLE IF NOT EXISTS `remitos` (
  `id` int NOT NULL AUTO_INCREMENT,
  `numero` varchar(20) NOT NULL,
  `fecha` datetime DEFAULT CURRENT_TIMESTAMP,
  `cliente_id` int NOT NULL,
  `transporte_id` int NOT NULL,
  `cantidad_bultos` int NOT NULL,
  `valor_declarado` decimal(12,2) NOT NULL,
  `observaciones` text,
  PRIMARY KEY (`id`),
  KEY `FK_remitos_clientes` (`cliente_id`),
  KEY `FK_remitos_transportes` (`transporte_id`),
  CONSTRAINT `FK_remitos_clientes` FOREIGN KEY (`cliente_id`) REFERENCES `clientes` (`Id`),
  CONSTRAINT `FK_remitos_transportes` FOREIGN KEY (`transporte_id`) REFERENCES `transportes` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
