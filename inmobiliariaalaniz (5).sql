-- phpMyAdmin SQL Dump
-- version 5.1.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 30-10-2022 a las 22:55:48
-- Versión del servidor: 10.4.21-MariaDB
-- Versión de PHP: 7.4.24

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `inmobiliariaalaniz`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contrato`
--

CREATE TABLE `contrato` (
  `Id` int(11) NOT NULL,
  `IdInquilino` int(11) NOT NULL,
  `IdInmueble` int(11) NOT NULL,
  `FechaInicio` date NOT NULL,
  `FechaFin` date NOT NULL,
  `MontoAlquiler` double NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `contrato`
--

INSERT INTO `contrato` (`Id`, `IdInquilino`, `IdInmueble`, `FechaInicio`, `FechaFin`, `MontoAlquiler`) VALUES
(10, 4, 17, '2022-08-16', '2023-08-16', 35000),
(11, 2, 18, '2021-06-22', '2023-06-22', 60000);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmueble`
--

CREATE TABLE `inmueble` (
  `Id` int(11) NOT NULL,
  `Direccion` varchar(45) NOT NULL,
  `Uso` varchar(30) NOT NULL,
  `Tipo` varchar(30) NOT NULL,
  `CantAmbientes` int(11) NOT NULL,
  `Precio` decimal(10,0) NOT NULL,
  `PropietarioId` int(11) NOT NULL,
  `Estado` tinyint(1) NOT NULL,
  `Imagen` varchar(100) DEFAULT NULL,
  `CoordenadaN` decimal(10,5) NOT NULL,
  `CoordenadaE` decimal(10,5) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `inmueble`
--

INSERT INTO `inmueble` (`Id`, `Direccion`, `Uso`, `Tipo`, `CantAmbientes`, `Precio`, `PropietarioId`, `Estado`, `Imagen`, `CoordenadaN`, `CoordenadaE`) VALUES
(17, 'Junín 1551', 'Hogar', 'Casa', 4, '35000', 4, 0, 'http://192.168.2.53:5000/Uploads/inmueble_489664.jpg', '-66.33584', '-33.25225'),
(18, 'Pringles 827', 'Comercio', 'Comercio', 2, '60000', 4, 1, 'http://192.168.2.53:5000/Uploads/inmueble_450964.jpg', '-66.33584', '-33.25225'),
(19, 'Solares 123', 'Hogar', 'Depto', 3, '40000', 4, 1, 'http://192.168.2.53:5000/Uploads/inmueble_437216.jpg', '-66.33582', '-33.25225');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilino`
--

CREATE TABLE `inquilino` (
  `Id` int(11) NOT NULL,
  `Nombre` varchar(20) NOT NULL,
  `Apellido` varchar(20) NOT NULL,
  `Dni` varchar(15) NOT NULL,
  `Telefono` varchar(20) NOT NULL,
  `Email` varchar(30) NOT NULL,
  `NombreGarante` varchar(60) NOT NULL,
  `TelefonoGarante` varchar(30) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `inquilino`
--

INSERT INTO `inquilino` (`Id`, `Nombre`, `Apellido`, `Dni`, `Telefono`, `Email`, `NombreGarante`, `TelefonoGarante`) VALUES
(1, 'Maria', 'Alaniz', '31542775', '(266) 455-2265', 'mvicalaniz@gmail.com', '', ''),
(2, 'Mariano', 'Luz', '26415235', '351264581', 'mariano@gmail.com', 'Luis Mercado', '2664231565'),
(4, 'Candela', 'Hollman', '21456956', '2564458220', 'flkdf@gmail.com', 'Agustin Martinez', '2657298545');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietario`
--

CREATE TABLE `propietario` (
  `Id` int(11) NOT NULL,
  `Nombre` varchar(30) NOT NULL,
  `Apellido` varchar(30) NOT NULL,
  `Dni` varchar(20) NOT NULL,
  `Telefono` varchar(20) NOT NULL,
  `Email` varchar(30) NOT NULL,
  `Clave` varchar(60) NOT NULL,
  `ImgPerfil` varchar(60) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `propietario`
--

INSERT INTO `propietario` (`Id`, `Nombre`, `Apellido`, `Dni`, `Telefono`, `Email`, `Clave`, `ImgPerfil`) VALUES
(1, 'Ramiro', 'Alaniz', '27376475', '2664585455', 'ramiro@gmail.com', '', ''),
(2, 'Victoria', 'Alaniz', '31542775', '(266) 455-2265', 'mvicalaniz@gmail.com', '', ''),
(3, 'Facundo', 'Suarez', '25648522', '26640258426', 'facundo@gmail.com', '', ''),
(4, 'Juan Manuel', 'Funes', '21546895', '2664558964', 'juan@mail.com', 'yn08Pgd3EoDXFX1+4H4BAulWKLJ0DVX1At/oJwzqx2c=', 'http://192.168.2.53:5000/Uploads/avatar_1.jpg');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `IdInmueble` (`IdInmueble`) USING BTREE,
  ADD KEY `IdInquilino` (`IdInquilino`) USING BTREE;

--
-- Indices de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IdPropietario` (`PropietarioId`);

--
-- Indices de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  ADD PRIMARY KEY (`Id`);

--
-- Indices de la tabla `propietario`
--
ALTER TABLE `propietario`
  ADD PRIMARY KEY (`Id`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `contrato`
--
ALTER TABLE `contrato`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=20;

--
-- AUTO_INCREMENT de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `propietario`
--
ALTER TABLE `propietario`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD CONSTRAINT `contrato_ibfk_1` FOREIGN KEY (`IdInmueble`) REFERENCES `inmueble` (`Id`),
  ADD CONSTRAINT `contrato_ibfk_2` FOREIGN KEY (`IdInquilino`) REFERENCES `inquilino` (`Id`);

--
-- Filtros para la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD CONSTRAINT `inmueble_ibfk_1` FOREIGN KEY (`PropietarioId`) REFERENCES `propietario` (`Id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
