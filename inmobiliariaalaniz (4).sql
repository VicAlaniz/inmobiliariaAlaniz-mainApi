-- phpMyAdmin SQL Dump
-- version 5.1.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 27-09-2022 a las 17:11:11
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
  `FechaFin` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `contrato`
--

INSERT INTO `contrato` (`Id`, `IdInquilino`, `IdInmueble`, `FechaInicio`, `FechaFin`) VALUES
(4, 4, 3, '2022-09-05', '2022-12-08'),
(5, 2, 1, '2022-09-29', '2023-01-26'),
(7, 4, 4, '2022-09-25', '2023-09-25');

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
  `Coordenadas` varchar(50) NOT NULL,
  `Precio` decimal(10,0) NOT NULL,
  `IdPropietario` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `inmueble`
--

INSERT INTO `inmueble` (`Id`, `Direccion`, `Uso`, `Tipo`, `CantAmbientes`, `Coordenadas`, `Precio`, `IdPropietario`) VALUES
(1, 'Junin 1551', 'Casa', 'Hogar', 4, '6154165, 652625', '30000', 1),
(3, 'Pringles 251', 'Comercio', 'Comercio', 2, '61156465, 5465151', '45000', 3),
(4, 'San Martin 756', 'Comercio', 'Comercio', 2, '2254467, 2265841', '55000', 4);

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
  `Email` varchar(30) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `inquilino`
--

INSERT INTO `inquilino` (`Id`, `Nombre`, `Apellido`, `Dni`, `Telefono`, `Email`) VALUES
(1, 'Maria', 'Alaniz', '31542775', '(266) 455-2265', 'mvicalaniz@gmail.com'),
(2, 'Mariano', 'Luz', '26415235', '351264581', 'mariano@gmail.com'),
(4, 'Candela', 'Hollman', '21456956', '2564458220', 'flkdf@gmail.com');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pago`
--

CREATE TABLE `pago` (
  `Id` int(11) NOT NULL,
  `FechaPago` date NOT NULL,
  `Importe` decimal(10,0) NOT NULL,
  `IdContrato` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `pago`
--

INSERT INTO `pago` (`Id`, `FechaPago`, `Importe`, `IdContrato`) VALUES
(2, '2022-09-22', '45500', 5),
(3, '2022-09-25', '55000', 7);

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
  `Email` varchar(30) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `propietario`
--

INSERT INTO `propietario` (`Id`, `Nombre`, `Apellido`, `Dni`, `Telefono`, `Email`) VALUES
(1, 'Ramiro', 'Alaniz', '27376475', '2664585455', 'ramiro@gmail.com'),
(2, 'Victoria', 'Alaniz', '31542775', '(266) 455-2265', 'mvicalaniz@gmail.com'),
(3, 'Facundo', 'Suarez', '25648522', '26640258426', 'facundo@gmail.com'),
(4, 'Juan', 'Carlos', '21546895', '2664558964', 'juan@mail.com');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuario`
--

CREATE TABLE `usuario` (
  `Id` int(11) NOT NULL,
  `Nombre` varchar(30) NOT NULL,
  `Apellido` varchar(30) NOT NULL,
  `Email` varchar(40) NOT NULL,
  `Clave` varchar(60) NOT NULL,
  `Avatar` varchar(60) DEFAULT NULL,
  `Rol` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Volcado de datos para la tabla `usuario`
--

INSERT INTO `usuario` (`Id`, `Nombre`, `Apellido`, `Email`, `Clave`, `Avatar`, `Rol`) VALUES
(1, 'Victoria', 'Alaniz', 'vic@mail.com', '3A0G2+zJ3luLnlC44+Xe5HGw/9RWJNoyF2XZACvev20=', '/Uploads\\avatar_1.jpg', 1),
(2, 'Gaspar', 'Melto1', 'gaspi@mail.com', 'myl4T6FgkMUdldPQ96rZUnNYn0ho5fyVIc39WWFLd8Y=', '/Uploads\\avatar_2.jpg', 2),
(3, 'Lucho', 'Luz', 'lucho@mail.com', '3A0G2+zJ3luLnlC44+Xe5HGw/9RWJNoyF2XZACvev20=', '/Uploads\\avatar_3.jpg', 2);

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
  ADD KEY `IdPropietario` (`IdPropietario`);

--
-- Indices de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  ADD PRIMARY KEY (`Id`);

--
-- Indices de la tabla `pago`
--
ALTER TABLE `pago`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IdContrato` (`IdContrato`);

--
-- Indices de la tabla `propietario`
--
ALTER TABLE `propietario`
  ADD PRIMARY KEY (`Id`);

--
-- Indices de la tabla `usuario`
--
ALTER TABLE `usuario`
  ADD PRIMARY KEY (`Id`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `contrato`
--
ALTER TABLE `contrato`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `pago`
--
ALTER TABLE `pago`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `propietario`
--
ALTER TABLE `propietario`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `usuario`
--
ALTER TABLE `usuario`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

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
  ADD CONSTRAINT `inmueble_ibfk_1` FOREIGN KEY (`IdPropietario`) REFERENCES `propietario` (`Id`);

--
-- Filtros para la tabla `pago`
--
ALTER TABLE `pago`
  ADD CONSTRAINT `pago_ibfk_1` FOREIGN KEY (`IdContrato`) REFERENCES `contrato` (`Id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
