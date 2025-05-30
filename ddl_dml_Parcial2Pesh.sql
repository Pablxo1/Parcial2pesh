CREATE DATABASE Parcial2Pesh;
GO
USE [master]
GO
CREATE LOGIN [usrparcial2] WITH PASSWORD = N'12345678',
	DEFAULT_DATABASE = [Parcial2Pesh],
	CHECK_EXPIRATION = OFF,
	CHECK_POLICY = ON
GO
USE [Parcial2Pesh]
GO
CREATE USER [usrparcial2] FOR LOGIN [usrparcial2]
GO
ALTER ROLE [db_owner] ADD MEMBER [usrparcial2]
GO

CREATE TABLE Serie (
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  titulo VARCHAR(250) NOT NULL,
  sinopsis VARCHAR(5000) NOT NULL,
  director VARCHAR(100) NOT NULL,
  episodios INT NOT NULL,
  fechaEstreno DATE NOT NULL,
  estado SMALLINT NOT NULL DEFAULT 1
);


-- DML
INSERT INTO Serie (titulo, sinopsis, director, episodios, fechaEstreno, estado)
VALUES
('Breaking Bad', 'Un profesor de química se convierte en fabricante de metanfetaminas tras ser diagnosticado con cáncer.', 'Vince Gilligan', 62, '2008-01-20', 1),
('Game of Thrones', 'Nobles luchan por el control del Trono de Hierro en un mundo de fantasía medieval.', 'David Benioff, D.B. Weiss', 73, '2011-04-17', 1),
('Stranger Things', 'Un grupo de niños investiga la desaparición de su amigo y descubre fenómenos paranormales.', 'The Duffer Brothers', 34, '2016-07-15', 1);