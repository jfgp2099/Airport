USE [PruebaNetactica]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Aerolinea](
	[IdAerolinea] [int]  IDENTITY(1,1) PRIMARY KEY,
	[Nombre] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Aerolinea] PRIMARY KEY CLUSTERED 
(
	[IdAerolinea] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Aeropuerto](
	[IdAeropuerto] [int] IDENTITY(1,1) PRIMARY KEY,
	[Nombre] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Aeropuerto] PRIMARY KEY CLUSTERED 
(
	[IdAeropuerto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reserva](
	[IdReserva] [int]  IDENTITY(1,1) PRIMARY KEY,
	[IdVuelo] [int] NOT NULL,
	[IdCliente] [int] NOT NULL,
	[Precio] [numeric](18, 0) NOT NULL,
 CONSTRAINT [PK_Reserva] PRIMARY KEY CLUSTERED 
(
	[IdReserva] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vuelo](
	[IdVuelo] [int]  IDENTITY(1,1) PRIMARY KEY,
	[FechaSalida] [datetime] NOT NULL,
	[AeropuertoOrigen] [int] NOT NULL,
	[FechaLlegada] [datetime] NOT NULL,
	[AeropuertoDestino] [int] NOT NULL,
	[IdAerolinea] [int] NOT NULL,
 CONSTRAINT [PK_Vuelo] PRIMARY KEY CLUSTERED 
(
	[IdVuelo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


ALTER TABLE [dbo].[Reserva]  WITH CHECK ADD  CONSTRAINT [FK_Reserva_Vuelo] FOREIGN KEY([IdVuelo])
REFERENCES [dbo].[Vuelo] ([IdVuelo])
GO
ALTER TABLE [dbo].[Reserva] CHECK CONSTRAINT [FK_Reserva_Vuelo]
GO
ALTER TABLE [dbo].[Vuelo]  WITH CHECK ADD  CONSTRAINT [FK_Vuelo_Aerolinea] FOREIGN KEY([IdAerolinea])
REFERENCES [dbo].[Aerolinea] ([IdAerolinea])
GO
ALTER TABLE [dbo].[Vuelo] CHECK CONSTRAINT [FK_Vuelo_Aerolinea]
GO
ALTER TABLE [dbo].[Vuelo]  WITH CHECK ADD  CONSTRAINT [FK_Vuelo_Aeropuerto] FOREIGN KEY([AeropuertoDestino])
REFERENCES [dbo].[Aeropuerto] ([IdAeropuerto])
GO
ALTER TABLE [dbo].[Vuelo] CHECK CONSTRAINT [FK_Vuelo_Aeropuerto]
GO
ALTER TABLE [dbo].[Vuelo]  WITH CHECK ADD  CONSTRAINT [FK_Vuelo_Aeropuerto1] FOREIGN KEY([AeropuertoOrigen])
REFERENCES [dbo].[Aeropuerto] ([IdAeropuerto])
GO
ALTER TABLE [dbo].[Vuelo] CHECK CONSTRAINT [FK_Vuelo_Aeropuerto1]

GO
CREATE View [dbo].[ReservaData] as 
select res.IdReserva, res.IdVuelo, vue.FechaLlegada,aerop.Nombre as AeropuertoOrigen, aerop2.Nombre as AeropuertoDestino,
			aerol.Nombre as Aerolinea, res.IdCliente, res.Precio from Aerolinea as aerol, Aeropuerto as aerop,Aeropuerto as aerop2, Reserva as res, Vuelo as vue
	  where  vue.IdVuelo = res.IdVuelo and aerol.IdAerolinea = vue.IdAerolinea and aerop.IdAeropuerto=vue.AeropuertoOrigen and aerop2.IdAeropuerto=vue.AeropuertoDestino and aerol.IdAerolinea = vue.IdAerolinea;
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FilterDataView]
  @Value				VARCHAR(50), 
  @Column				INT 

AS
declare @SQL_QUERY           NVARCHAR(max),
	 @Columname NVARCHAR(max) = (
    SELECT TOP 1 c.name as columname 
	from sys.columns c
	join sys.views v 
		 on v.object_id = c.object_id
		 where  c.column_id = @Column
		 and object_name(c.object_id)= 'ReservaData'
		order by  column_id
)

	SET @SQL_QUERY = Concat('select * from ReservaData where ',@Columname,' =  ''', @Value,'''')
	--print @SQL_QUERY
	EXECUTE sp_executesql @SQL_QUERY;
GO
INSERT [dbo].[Aerolinea] ([Nombre]) VALUES (N'Aerolinea1')
INSERT [dbo].[Aerolinea] ([Nombre]) VALUES (N'Aerolinea2')
INSERT [dbo].[Aeropuerto]  ([Nombre]) VALUES ( N'Aeropuerto1')
INSERT [dbo].[Aeropuerto]  ([Nombre]) VALUES ( N'Aeropuerto2')
GO