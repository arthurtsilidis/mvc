USE [master]
GO
/****** Object:  Database [PhonesCatalogue]    Script Date: 4/21/2021 11:54:04 AM ******/
CREATE DATABASE [PhonesCatalogue]
GO
ALTER DATABASE [PhonesCatalogue] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [PhonesCatalogue].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [PhonesCatalogue] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [PhonesCatalogue] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [PhonesCatalogue] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [PhonesCatalogue] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [PhonesCatalogue] SET ARITHABORT OFF 
GO
ALTER DATABASE [PhonesCatalogue] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [PhonesCatalogue] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [PhonesCatalogue] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [PhonesCatalogue] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [PhonesCatalogue] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [PhonesCatalogue] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [PhonesCatalogue] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [PhonesCatalogue] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [PhonesCatalogue] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [PhonesCatalogue] SET  DISABLE_BROKER 
GO
ALTER DATABASE [PhonesCatalogue] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [PhonesCatalogue] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [PhonesCatalogue] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [PhonesCatalogue] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [PhonesCatalogue] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [PhonesCatalogue] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [PhonesCatalogue] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [PhonesCatalogue] SET RECOVERY FULL 
GO
ALTER DATABASE [PhonesCatalogue] SET  MULTI_USER 
GO
ALTER DATABASE [PhonesCatalogue] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [PhonesCatalogue] SET DB_CHAINING OFF 
GO
ALTER DATABASE [PhonesCatalogue] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [PhonesCatalogue] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [PhonesCatalogue] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'PhonesCatalogue', N'ON'
GO
ALTER DATABASE [PhonesCatalogue] SET QUERY_STORE = OFF
GO
USE [PhonesCatalogue]
GO
/****** Object:  UserDefinedTableType [dbo].[PhoneNumberType]    Script Date: 4/21/2021 11:54:04 AM ******/
CREATE TYPE [dbo].[PhoneNumberType] AS TABLE(
	[Type] [nvarchar](25) NOT NULL,
	[Number] [nvarchar](20) NOT NULL
)
GO
/****** Object:  Table [dbo].[ClientPhones]    Script Date: 4/21/2021 11:54:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ClientPhones](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClientId] [int] NOT NULL,
	[PhoneTypeId] [int] NOT NULL,
	[PhoneNumber] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_ClientPhones] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Clients]    Script Date: 4/21/2021 11:54:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clients](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LastName] [nvarchar](30) NOT NULL,
	[FirstName] [nvarchar](30) NOT NULL,
	[Address] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Clients] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PhoneTypes]    Script Date: 4/21/2021 11:54:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhoneTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](25) NOT NULL,
 CONSTRAINT [PK_PhoneTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ClientPhones]  WITH CHECK ADD  CONSTRAINT [FK_ClientPhones_Clients] FOREIGN KEY([ClientId])
REFERENCES [dbo].[Clients] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ClientPhones] CHECK CONSTRAINT [FK_ClientPhones_Clients]
GO
ALTER TABLE [dbo].[ClientPhones]  WITH CHECK ADD  CONSTRAINT [FK_ClientPhones_PhoneTypes] FOREIGN KEY([PhoneTypeId])
REFERENCES [dbo].[PhoneTypes] ([Id])
GO
ALTER TABLE [dbo].[ClientPhones] CHECK CONSTRAINT [FK_ClientPhones_PhoneTypes]
GO
/****** Object:  StoredProcedure [dbo].[CreateClient]    Script Date: 4/21/2021 11:54:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
set identity_insert dbo.phoneTypes on;
insert into dbo.phoneTypes (id, [name])
values (1,'Home'),(2,'Mobile'),(3,'Work')
set identity_insert dbo.phoneTypes off;
go
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CreateClient]
	@FirstName nvarchar(30),
	@LastName nvarchar(30),
	@Address nvarchar(100),
	@Email nvarchar(50),
	@PhoneNumbers dbo.PhoneNumberType readonly

AS
BEGIN
	SET NOCOUNT ON;
	if exists (select top 1 1 from @PhoneNumbers where [Type] not in (select [Name] from PhoneTypes))
	begin 
		raiserror ('check phone types', 16, 1  );  
		return;
	end
	declare @id int;
	insert into Clients (LastName, FirstName, [Address], Email)
	select @LastName, @FirstName, @Address, @Email
	select @id = scope_identity();
	insert into ClientPhones(ClientId, PhoneTypeId, PhoneNumber)
	select @id, phoneTypes.Id, phoneNumbers.[Number] 
	from @PhoneNumbers phoneNumbers
	inner join PhoneTypes phoneTypes on phoneNumbers.[Type] = phoneTypes.[Name]

	select clients.Id, clients.FirstName, clients.LastName, clients.Address, clients.Email, PhoneTypes.Name as PhoneTypeName, ClientPhones.PhoneNumber
	from Clients 
	left outer join ClientPhones on Clients.Id = ClientPhones.ClientId
	left outer join PhoneTypes on PhoneTypes.Id = ClientPhones.PhoneTypeId 
	where Clients.Id = @id

END
GO
/****** Object:  StoredProcedure [dbo].[DeleteClient]    Script Date: 4/21/2021 11:54:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DeleteClient]
	@id int
AS
BEGIN
	SET NOCOUNT ON;
	delete from ClientPhones 
	where clientId = @id 
	delete from Clients 
	where Id = @id 
END
GO
/****** Object:  StoredProcedure [dbo].[GetClients]    Script Date: 4/21/2021 11:54:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetClients]
	--@lastName nvarchar(30),
	--@firstName nvarchar(30),
	--@address nvarchar(100),
	--@email nvarchar(50),
	--@phoneNumber nvarchar(20)
AS
BEGIN
	SET NOCOUNT ON;
	select clients.Id, clients.FirstName, clients.LastName, clients.Address, clients.Email, PhoneTypes.Name as PhoneTypeName, ClientPhones.PhoneNumber
	from Clients 
	left outer join ClientPhones on Clients.Id = ClientPhones.ClientId
	left outer join PhoneTypes on PhoneTypes.Id = ClientPhones.PhoneTypeId 

END
GO
/****** Object:  StoredProcedure [dbo].[UpdateClient]    Script Date: 4/21/2021 11:54:04 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UpdateClient]
	@id int,
	@FirstName nvarchar(30),
	@LastName nvarchar(30),
	@Address nvarchar(100),
	@Email nvarchar(50),
	@PhoneNumbers dbo.PhoneNumberType readonly
AS
BEGIN
	SET NOCOUNT ON;
	if not exists (select top 1 1 from clients where id = @id)
	begin
		return;
	end
	if exists (select top 1 1 from @PhoneNumbers where [Type] not in (select [Name] from PhoneTypes))
	begin 
		raiserror ('check phone types', 16, 1  );  
		return;
	end

	update Clients 
	set LastName = @LastName, 
		FirstName = @FirstName, 
		[Address] = @Address, 
		Email = @Email
	where id = @id;

	delete from ClientPhones 
	where clientId = @id;

	insert into ClientPhones(ClientId, PhoneTypeId, PhoneNumber)
	select @id, phoneTypes.Id, phoneNumbers.[Number] 
	from @PhoneNumbers phoneNumbers
	inner join PhoneTypes phoneTypes on phoneNumbers.[Type] = phoneTypes.[Name];

	select clients.Id, clients.FirstName, clients.LastName, clients.Address, clients.Email, PhoneTypes.Name as PhoneTypeName, ClientPhones.PhoneNumber
	from Clients 
	left outer join ClientPhones on Clients.Id = ClientPhones.ClientId
	left outer join PhoneTypes on PhoneTypes.Id = ClientPhones.PhoneTypeId 
	where Clients.Id = @id
END
GO
USE [master]
GO
ALTER DATABASE [PhonesCatalogue] SET  READ_WRITE 
GO
