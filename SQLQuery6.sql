-- 1. Crear y seleccionar la base de datos
CREATE DATABASE SistemaLoginDB;
GO

USE SistemaLoginDB;
GO

-- 2. Crear la tabla de usuarios
CREATE TABLE Usuarios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Correo NVARCHAR(100) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL
);
GO

-- 3. Insertar credenciales de prueba 
INSERT INTO Usuarios (Nombre, Correo, Password) VALUES 
('Ricardo Rodriguez Larios', 'strrod14@upana.edu.gt', '@Gentildonna..2026');
GO