USE [AhlamCoDb]
GO
/****** Object:  UserDefinedFunction [dbo].[GetTotalContactAccountPayments]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GetTotalContactAccountPayments] (@Id INT,@PayoutTransaction BIT) RETURNS INT AS BEGIN RETURN (SELECT SUM(PaymentValue) FROM dbo.ContactAccountPayment WHERE DeletedDate IS NULL AND PayoutTransaction = @PayoutTransaction AND ContactAccountId = @Id); END
GO
/****** Object:  UserDefinedFunction [dbo].[GetTotalPaymentByPaymentTypeId]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GetTotalPaymentByPaymentTypeId] (@PaymentTypeId INT)RETURNS INT AS BEGIN RETURN (SELECT ISNULL(SUM(PaymentValue), 0) AS TotalPayments FROM dbo.Payments WHERE PaymentTypeId = @PaymentTypeId AND dbo.Payments.Paid = 1);END
GO
/****** Object:  UserDefinedFunction [dbo].[GetTotalPaymentsByPatientOperationId]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GetTotalPaymentsByPatientOperationId] (@PatientOperationId INT)RETURNS INT AS BEGIN RETURN (SELECT ISNULL(SUM(PaymentValue),0) FROM dbo.PatientOperationSessions WHERE PatientOperationId = @PatientOperationId);END
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccountPaymentCategories]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountPaymentCategories](
	[Id] [int] NOT NULL,
	[CategoryName] [nvarchar](200) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[LastModifiedDate] [datetime2](7) NULL,
	[DeletedDate] [datetime2](7) NULL,
	[DeletedBy] [nvarchar](max) NULL,
 CONSTRAINT [PK_AccountPaymentCategories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AppLanguage]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppLanguage](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LanguageCode] [nvarchar](max) NULL,
	[DeviceName] [nvarchar](max) NULL,
	[PrinterName] [nvarchar](200) NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[LastModifiedDate] [datetime2](7) NULL,
	[WaitingNext] [bit] NOT NULL,
 CONSTRAINT [PK_AppLanguage] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CartoonLabels]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CartoonLabels](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CartoonNameId] [int] NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[LastModifiedDate] [datetime2](7) NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
	[ThePaperCode] [int] NOT NULL,
	[Entry] [int] NULL,
	[Extry] [int] NULL,
	[FirstDayValue] [int] NOT NULL,
	[MinimumValue] [int] NOT NULL,
	[Note] [nvarchar](max) NULL,
	[DEntry] [float] NOT NULL,
	[DExtry] [float] NOT NULL,
	[CurrentValue] [float] NOT NULL,
 CONSTRAINT [PK_CartoonLabels] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContactAccountPayment]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContactAccountPayment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ContactAccountId] [int] NULL,
	[PaymentDate] [date] NOT NULL,
	[PaymentValue] [int] NOT NULL,
	[Notes] [nvarchar](400) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
	[PayoutTransaction] [bit] NOT NULL,
	[CategoryId] [int] NOT NULL,
	[ContactId] [int] NULL,
 CONSTRAINT [PK_ContactAccountPayment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContactAccounts]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContactAccounts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ContactId] [int] NOT NULL,
	[AccountName] [nvarchar](75) NOT NULL,
	[TotalCost] [int] NOT NULL,
	[Notes] [nvarchar](400) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
	[PayOutAccount] [bit] NOT NULL,
	[Remaining]  AS ([TotalCost]-isnull([dbo].[GetTotalContactAccountPayments]([Id],[PayOutAccount]),(0))),
 CONSTRAINT [PK_ContactAccounts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Contacts]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contacts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ContactName] [nvarchar](75) NOT NULL,
	[ContactPhones] [nvarchar](400) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[Address] [nvarchar](200) NOT NULL,
	[Phone] [nvarchar](200) NOT NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
	[CompanyType] [nvarchar](max) NULL,
	[EmailAdress] [nvarchar](max) NULL,
	[WebSite] [nvarchar](max) NULL,
	[note] [nvarchar](max) NULL,
 CONSTRAINT [PK_Contacts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[Id] [int] NOT NULL,
	[ContactId] [int] NULL,
	[AccountName] [nvarchar](max) NULL,
	[Notes] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedDate] [datetime] NULL,
	[DeletedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customers]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ContactId] [int] NOT NULL,
	[AccountName] [nvarchar](max) NULL,
	[Notes] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[LastModifiedDate] [datetime2](7) NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
	[PayOutAccount] [bit] NOT NULL,
	[Remaining] [int] NOT NULL,
 CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DeniedUsers]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DeniedUsers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](max) NULL,
	[DeniedDate] [datetime2](7) NOT NULL,
	[DeviceName] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[LastModifiedDate] [datetime2](7) NULL,
	[DeletedDate] [datetime2](7) NULL,
	[DeletedBy] [nvarchar](max) NULL,
 CONSTRAINT [PK_DeniedUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Diagnosis]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Diagnosis](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EnName] [nvarchar](75) NOT NULL,
	[ArName] [nvarchar](75) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[Code] [nvarchar](200) NOT NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Diagnosis] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Diseases]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Diseases](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EnName] [nvarchar](200) NULL,
	[ArName] [nvarchar](75) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
	[Enrty] [nvarchar](max) NULL,
	[Extry] [nvarchar](max) NULL,
	[FirstDayValue] [nvarchar](max) NULL,
	[Note] [nvarchar](max) NULL,
	[ThePNumber] [int] NOT NULL,
	[FirstValue] [float] NOT NULL,
	[CurrentValue] [float] NOT NULL,
 CONSTRAINT [PK_Diseases] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EyeTests]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EyeTests](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EnName] [nvarchar](75) NOT NULL,
	[ArName] [nvarchar](75) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[Code] [nvarchar](200) NOT NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_EyeTests] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Glasses]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Glasses](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EnName] [nvarchar](200) NULL,
	[ArName] [nvarchar](75) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Glasses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LabTests]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LabTests](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LabTestName] [nvarchar](75) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_LabTests] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Locations]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Locations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EnName] [nvarchar](200) NULL,
	[ArName] [nvarchar](75) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Locations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MartialStatus]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MartialStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EnName] [nvarchar](200) NULL,
	[ArName] [nvarchar](75) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_MartialStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MedicalCenters]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MedicalCenters](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EnName] [nvarchar](75) NOT NULL,
	[ArName] [nvarchar](75) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_MedicalCenters] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Medicines]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Medicines](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](100) NOT NULL,
	[MedicineName] [nvarchar](max) NOT NULL,
	[MedicineTypeId] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Medicines] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MedicineTypes]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MedicineTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EnName] [nvarchar](75) NOT NULL,
	[ArName] [nvarchar](75) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_MedicineTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MedicineUsages]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MedicineUsages](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](200) NOT NULL,
	[UsageName] [nvarchar](200) NOT NULL,
	[UsageMedicineTypeId] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_MedicineUsages] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NotPayReasons]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotPayReasons](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EnName] [nvarchar](200) NULL,
	[ArName] [nvarchar](200) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_NotPayReasons] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OldMedicineViewTable]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OldMedicineViewTable](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PatientVisitId] [int] NOT NULL,
	[VisitDate] [date] NOT NULL,
	[MedicineName] [nvarchar](max) NOT NULL,
	[MedicineType] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[_index] [int] NULL,
	[TempPrescriptionId] [int] NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_OldMedicineViewTable_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OldPatientEyeImages]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OldPatientEyeImages](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ImageNumber] [nvarchar](max) NULL,
	[ImageType] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[DeletedDate] [datetime2](7) NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[LastName] [nvarchar](200) NOT NULL,
	[FirstName] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_OldPatientEyeImages] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Operations]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Operations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](75) NOT NULL,
	[EnName] [nvarchar](200) NOT NULL,
	[ArName] [nvarchar](200) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
	[IsSergical] [bit] NOT NULL,
 CONSTRAINT [PK_Operations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PatientDiseases]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PatientDiseases](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PatientId] [int] NOT NULL,
	[DiseaseId] [int] NOT NULL,
	[AgeOfInjury] [nvarchar](200) NULL,
	[MaxMeasure] [nvarchar](200) NULL,
	[LastCheckMeasure] [nvarchar](200) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_PatientDiseases] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PatientGlass]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PatientGlass](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PatientId] [int] NOT NULL,
	[R_Sph] [nvarchar](100) NULL,
	[L_Sph] [nvarchar](100) NULL,
	[R_Sph2] [nvarchar](100) NULL,
	[L_Sph2] [nvarchar](100) NULL,
	[R_Cyl] [nvarchar](100) NULL,
	[L_Cyl] [nvarchar](100) NULL,
	[R_Cyl2] [nvarchar](100) NULL,
	[L_Cyl2] [nvarchar](100) NULL,
	[R_Axis] [nvarchar](100) NULL,
	[L_Axis] [nvarchar](100) NULL,
	[R_Axis2] [nvarchar](100) NULL,
	[L_Axis2] [nvarchar](100) NULL,
	[R_Prism] [nvarchar](100) NULL,
	[L_Prism] [nvarchar](100) NULL,
	[R_Prism2] [nvarchar](100) NULL,
	[L_Prism2] [nvarchar](100) NULL,
	[R_Base] [nvarchar](5) NULL,
	[L_Base] [nvarchar](5) NULL,
	[R_Base2] [nvarchar](5) NULL,
	[L_Base2] [nvarchar](5) NULL,
	[R_IPD] [nvarchar](100) NULL,
	[L_IPD] [nvarchar](100) NULL,
	[R_IPD2] [nvarchar](100) NULL,
	[L_IPD2] [nvarchar](100) NULL,
	[R_VA] [nvarchar](100) NULL,
	[L_VA] [nvarchar](100) NULL,
	[R_VA2] [nvarchar](100) NULL,
	[L_VA2] [nvarchar](100) NULL,
	[Add_Vision] [bit] NOT NULL,
	[ContactLenses] [bit] NOT NULL,
	[Notes] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_PatientGlass] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PatientOperations]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PatientOperations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PatientId] [int] NOT NULL,
	[OperationDate] [date] NOT NULL,
	[LeftEyeOperationId] [int] NULL,
	[RightEyeOperationId] [int] NULL,
	[MedicalCenterId] [int] NOT NULL,
	[MedicalCenterReserved] [bit] NOT NULL,
	[TotalSessions] [int] NOT NULL,
	[PhotoSource] [nvarchar](200) NULL,
	[PaymentLocation] [nvarchar](200) NULL,
	[TotalCost] [int] NOT NULL,
	[LeftEyeNote] [nvarchar](max) NULL,
	[RightEyeNote] [nvarchar](max) NULL,
	[Report] [nvarchar](max) NULL,
	[IsFinish] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[CenterCost] [int] NOT NULL,
	[ClinicCost] [int] NOT NULL,
	[DownPayment] [int] NULL,
	[Remaining]  AS (isnull(([TotalCost]-[DownPayment])-[dbo].[GetTotalPaymentsByPatientOperationId]([Id]),(0))),
	[Revenue] [int] NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
	[LeftEyeLens] [nvarchar](max) NULL,
	[LeftEyeLensType] [nvarchar](max) NULL,
	[RightEyeLens] [nvarchar](max) NULL,
	[RightEyeLensType] [nvarchar](max) NULL,
 CONSTRAINT [PK_PatientOperations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PatientOperationSessions]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PatientOperationSessions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PatientOperationId] [int] NOT NULL,
	[SessionDate] [datetime] NOT NULL,
	[PaymentValue] [int] NOT NULL,
	[Note] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
	[SessionNumber] [int] NOT NULL,
 CONSTRAINT [PK_PatientOperationSessions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Patients]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Patients](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Number] [int] NOT NULL,
	[FirstName] [nvarchar](200) NOT NULL,
	[LastName] [nvarchar](200) NOT NULL,
	[FatherName] [nvarchar](200) NULL,
	[MotherName] [nvarchar](200) NULL,
	[Age] [int] NOT NULL,
	[AgeTypeName] [nvarchar](200) NOT NULL,
	[JobTitle] [nvarchar](200) NULL,
	[Gender] [bit] NOT NULL,
	[MartialStatusId] [int] NOT NULL,
	[HomePhone] [nvarchar](200) NULL,
	[WorkPhone] [nvarchar](200) NULL,
	[Nationality] [nvarchar](200) NULL,
	[LocationId] [int] NOT NULL,
	[IsSmoking] [bit] NOT NULL,
	[IsDrinking] [bit] NOT NULL,
	[IsDrawing] [bit] NOT NULL,
	[GlassId] [int] NOT NULL,
	[Notes] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[Referral] [bit] NOT NULL,
	[TempId] [int] NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
	[EndPregnantDate] [datetime] NULL,
	[PregnantMonth] [int] NOT NULL,
	[StartPregnantDate] [datetime] NULL,
 CONSTRAINT [PK_Patients] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PatientSickStory]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PatientSickStory](
	[PatientId] [int] NOT NULL,
	[SickStory] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_PatientSickStory_PatientId] PRIMARY KEY CLUSTERED 
(
	[PatientId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PatientVisitDiagnosis]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PatientVisitDiagnosis](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PatientVisitId] [int] NOT NULL,
	[DiagnosisId] [int] NOT NULL,
	[LeftEye] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_PatientVisitDiagnosis] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PatientVisitEyeTestHistory]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PatientVisitEyeTestHistory](
	[Id] [int] NOT NULL,
	[PatientVisitId] [int] NOT NULL,
	[EyeTestId] [int] NOT NULL,
	[LeftEyeResult] [nvarchar](200) NULL,
	[RightEyeResult] [nvarchar](200) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[DeletedDate] [datetime2](7) NULL,
	[DeletedBy] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PatientVisitEyeTests]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PatientVisitEyeTests](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PatientVisitId] [int] NOT NULL,
	[EyeTestId] [int] NOT NULL,
	[LeftEyeResult] [nvarchar](200) NULL,
	[RightEyeResult] [nvarchar](200) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_PatientVisitEyeTests] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PatientVisitGlass]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PatientVisitGlass](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PatientVisitId] [int] NOT NULL,
	[R_Sph] [nvarchar](100) NULL,
	[L_Sph] [nvarchar](100) NULL,
	[R_Sph2] [nvarchar](100) NULL,
	[L_Sph2] [nvarchar](100) NULL,
	[R_Cyl] [nvarchar](100) NULL,
	[L_Cyl] [nvarchar](100) NULL,
	[R_Cyl2] [nvarchar](100) NULL,
	[L_Cyl2] [nvarchar](100) NULL,
	[R_Axis] [nvarchar](100) NULL,
	[L_Axis] [nvarchar](100) NULL,
	[R_Axis2] [nvarchar](100) NULL,
	[L_Axis2] [nvarchar](100) NULL,
	[R_Prism] [nvarchar](100) NULL,
	[L_Prism] [nvarchar](100) NULL,
	[R_Prism2] [nvarchar](100) NULL,
	[L_Prism2] [nvarchar](100) NULL,
	[R_Base] [nvarchar](5) NULL,
	[L_Base] [nvarchar](5) NULL,
	[R_Base2] [nvarchar](5) NULL,
	[L_Base2] [nvarchar](5) NULL,
	[R_IPD] [nvarchar](100) NULL,
	[L_IPD] [nvarchar](100) NULL,
	[R_IPD2] [nvarchar](100) NULL,
	[L_IPD2] [nvarchar](100) NULL,
	[R_VA] [nvarchar](100) NULL,
	[L_VA] [nvarchar](100) NULL,
	[R_VA2] [nvarchar](100) NULL,
	[L_VA2] [nvarchar](100) NULL,
	[Add_Vision] [bit] NOT NULL,
	[Notes] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[ContactLenses] [bit] NOT NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_PatientVisitGlass] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PatientVisitLabTests]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PatientVisitLabTests](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PatientVisitId] [int] NOT NULL,
	[LabTestId] [int] NOT NULL,
	[Result] [nvarchar](400) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[Done] [bit] NOT NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_PatientVisitLabTests] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PatientVisitPrescriptions]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PatientVisitPrescriptions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PatientVisitId] [int] NOT NULL,
	[MedicineId] [int] NOT NULL,
	[MedicineUsageId] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[RowIndex] [int] NOT NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_PatientVisitPrescriptions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PatientVisits]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PatientVisits](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PatientId] [int] NOT NULL,
	[VisitDate] [date] NOT NULL,
	[ReviewDate] [datetime] NULL,
	[Cost] [int] NOT NULL,
	[Payment] [int] NOT NULL,
	[NotPaymentReasonId] [int] NULL,
	[VisitStatus] [tinyint] NOT NULL,
	[VisitIndex] [int] NOT NULL,
	[Notes] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
	[MedicalReport] [nvarchar](max) NULL,
	[Remaining]  AS (case when [NotPaymentReasonId]=(4) then isnull([Cost]-[Payment],(0)) else (0) end),
	[QueueIndex] [int] NOT NULL,
	[IfRemaining] [bit] NULL,
	[RemainPayed] [bit] NOT NULL,
	[RemainPayValue] [int] NOT NULL,
	[Why] [nvarchar](max) NULL,
	[PatientVisitType] [nvarchar](max) NULL,
 CONSTRAINT [PK_PatientVisits] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PatientVisitsTests]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PatientVisitsTests](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PatientVisitId] [int] NOT NULL,
	[TestId] [int] NOT NULL,
	[LeftEye] [bit] NOT NULL,
	[LeftEyeNote] [nvarchar](max) NULL,
	[RightEye] [bit] NOT NULL,
	[RightEyeNote] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[ImageNumber] [nvarchar](300) NULL,
	[Dropped] [bit] NOT NULL,
	[CostPayment] [int] NOT NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
	[MedicalCenterId] [int] NULL,
	[ImageNameLeft] [nvarchar](max) NULL,
	[ImageNameRight] [nvarchar](max) NULL,
	[Locked] [bit] NOT NULL,
	[ImageNameBoth] [nvarchar](max) NULL,
	[ImageName] [nvarchar](max) NULL,
 CONSTRAINT [PK_PatientVisitsTests] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payments]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PaymentTypeId] [int] NOT NULL,
	[PaymentDate] [date] NOT NULL,
	[ReminderDate] [datetime] NULL,
	[PaymentValue] [int] NOT NULL,
	[Notes] [nvarchar](max) NULL,
	[Paid] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Payments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentTypes]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TypeName] [nvarchar](200) NOT NULL,
	[BeneficiaryName] [nvarchar](200) NULL,
	[TotalCost] [int] NOT NULL,
	[Note] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[TotalPayments]  AS ([dbo].[GetTotalPaymentByPaymentTypeId]([Id])),
	[Remaining]  AS (isnull([TotalCost]-[dbo].[GetTotalPaymentByPaymentTypeId]([Id]),(0))),
	[Debt] [bit] NOT NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_PaymentTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payouts]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payouts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DateTime] [date] NOT NULL,
	[Amount] [int] NOT NULL,
	[Reason] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Payouts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Producting]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Producting](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EnName] [nvarchar](max) NULL,
	[ArName] [nvarchar](max) NULL,
	[ProductingValue] [float] NOT NULL,
	[Code] [nvarchar](max) NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[LastModifiedDate] [datetime2](7) NULL,
	[DeletedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Producting] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Queues]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Queues](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PatientId] [int] NOT NULL,
	[VisitDate] [date] NOT NULL,
	[ReviewDate] [datetime] NULL,
	[Cost] [int] NOT NULL,
	[Payment] [int] NOT NULL,
	[NotPaymentReasonId] [int] NULL,
	[Remaining]  AS (case when [NotPaymentReasonId] IS NULL then isnull([Cost]-[Payment],(0)) else (0) end),
	[VisitStatus] [tinyint] NOT NULL,
	[VisitIndex] [int] NOT NULL,
	[Notes] [nvarchar](max) NULL,
	[MedicalReport] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Queues] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReadyPrescriptionMedicines]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReadyPrescriptionMedicines](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ReadyPrescriptionId] [int] NOT NULL,
	[MedicineId] [int] NOT NULL,
	[MedicineUsageId] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_ReadyPrescriptionMedicines] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReadyPrescriptions]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReadyPrescriptions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EnName] [nvarchar](75) NOT NULL,
	[ArName] [nvarchar](75) NOT NULL,
	[Disabled] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_ReadyPrescriptions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReadyProducts]    Script Date: 4/16/2024 10:28:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReadyProducts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[LastModifiedDate] [datetime2](7) NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
	[CreatedValue] [float] NOT NULL,
	[Note] [nvarchar](max) NULL,
	[ProductedValue] [float] NOT NULL,
	[ExportedValue] [float] NOT NULL,
	[ArName] [nvarchar](max) NULL,
	[WasteValue] [float] NOT NULL,
	[CreatedValueUnit] [nvarchar](max) NULL,
	[ExportedValueUnit] [nvarchar](max) NULL,
	[ProductedValueUnit] [nvarchar](max) NULL,
	[WasteValueUnit] [nvarchar](max) NULL,
	[TotalValue] [float] NOT NULL,
	[TotalWaste] [float] NOT NULL,
	[TotalWight] [float] NOT NULL,
	[TotalResult] [float] NOT NULL,
 CONSTRAINT [PK_ReadyProducts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Reminders]    Script Date: 4/16/2024 10:28:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reminders](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ReminderText] [nvarchar](400) NOT NULL,
	[ReminderDate] [date] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Reminders] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Reservations]    Script Date: 4/16/2024 10:28:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reservations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PatientName] [nvarchar](75) NOT NULL,
	[PhoneNumber] [nvarchar](75) NOT NULL,
	[ReservationDate] [datetime] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
	[ReservationTime] [nvarchar](max) NULL,
	[PatientVisitType] [nvarchar](max) NULL,
 CONSTRAINT [PK_Reservations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 4/16/2024 10:28:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EnName] [nvarchar](75) NOT NULL,
	[ArName] [nvarchar](75) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
	[PassWrong] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Table]    Script Date: 4/16/2024 10:28:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Table](
	[Id] [int] NOT NULL,
	[ContactId] [int] NULL,
	[AccountName] [nvarchar](max) NULL,
	[Notes] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[LastModifiedDate] [datetime] NULL,
	[DeletedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tests]    Script Date: 4/16/2024 10:28:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tests](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EnName] [nvarchar](75) NOT NULL,
	[ArName] [nvarchar](75) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[Code] [nvarchar](200) NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
	[ImagePath] [nvarchar](max) NULL,
	[ImagePath2] [nvarchar](max) NULL,
	[ImagePath3] [nvarchar](max) NULL,
	[Imagex] [tinyint] NOT NULL,
	[Imagex2] [varbinary](max) NULL,
	[Imagex3] [varbinary](max) NULL,
	[Imagex4] [varbinary](max) NULL,
	[Quintity] [int] NOT NULL,
	[Unit] [nvarchar](max) NULL,
	[FirstTermBalance] [nvarchar](max) NULL,
	[WasteValue] [float] NOT NULL,
	[UnitValue] [float] NOT NULL,
 CONSTRAINT [PK_Tests] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 4/16/2024 10:28:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](200) NOT NULL,
	[Password] [nvarchar](200) NOT NULL,
	[RoleId] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[LastModifiedDate] [datetime] NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VisitType]    Script Date: 4/16/2024 10:28:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VisitType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EnName] [nvarchar](max) NULL,
	[ArName] [nvarchar](max) NULL,
	[Cost] [int] NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[LastModifiedDate] [datetime2](7) NULL,
	[DeletedBy] [nvarchar](max) NULL,
	[DeletedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_VisitType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[AppLanguage] ADD  DEFAULT ('0001-01-01T00:00:00.0000000') FOR [CreatedDate]
GO
ALTER TABLE [dbo].[AppLanguage] ADD  DEFAULT (CONVERT([bit],(0))) FOR [WaitingNext]
GO
ALTER TABLE [dbo].[CartoonLabels] ADD  DEFAULT ((0.000000000000000e+000)) FOR [DEntry]
GO
ALTER TABLE [dbo].[CartoonLabels] ADD  DEFAULT ((0.000000000000000e+000)) FOR [DExtry]
GO
ALTER TABLE [dbo].[CartoonLabels] ADD  DEFAULT ((0.000000000000000e+000)) FOR [CurrentValue]
GO
ALTER TABLE [dbo].[ContactAccountPayment] ADD  DEFAULT (getdate()) FOR [PaymentDate]
GO
ALTER TABLE [dbo].[ContactAccountPayment] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[ContactAccountPayment] ADD  DEFAULT ((0)) FOR [PayoutTransaction]
GO
ALTER TABLE [dbo].[ContactAccountPayment] ADD  DEFAULT ((0)) FOR [CategoryId]
GO
ALTER TABLE [dbo].[ContactAccounts] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[ContactAccounts] ADD  DEFAULT ((0)) FOR [PayOutAccount]
GO
ALTER TABLE [dbo].[Contacts] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Diagnosis] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Diseases] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Diseases] ADD  DEFAULT ((0)) FOR [ThePNumber]
GO
ALTER TABLE [dbo].[Diseases] ADD  DEFAULT ((0.000000000000000e+000)) FOR [FirstValue]
GO
ALTER TABLE [dbo].[Diseases] ADD  DEFAULT ((0.000000000000000e+000)) FOR [CurrentValue]
GO
ALTER TABLE [dbo].[EyeTests] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Glasses] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[LabTests] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Locations] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[MartialStatus] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[MedicalCenters] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Medicines] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[MedicineTypes] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[MedicineUsages] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[NotPayReasons] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[OldMedicineViewTable] ADD  CONSTRAINT [Default_OldMedicineViewTable_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[OldPatientEyeImages] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Operations] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Operations] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsSergical]
GO
ALTER TABLE [dbo].[PatientDiseases] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[PatientGlass] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[PatientOperations] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[PatientOperations] ADD  DEFAULT ((0)) FOR [DownPayment]
GO
ALTER TABLE [dbo].[PatientOperationSessions] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[PatientOperationSessions] ADD  DEFAULT ((0)) FOR [SessionNumber]
GO
ALTER TABLE [dbo].[Patients] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Patients] ADD  DEFAULT ((0)) FOR [PregnantMonth]
GO
ALTER TABLE [dbo].[PatientSickStory] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[PatientVisitDiagnosis] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[PatientVisitEyeTestHistory] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[PatientVisitEyeTests] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[PatientVisitGlass] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[PatientVisitLabTests] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[PatientVisitPrescriptions] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[PatientVisitPrescriptions] ADD  DEFAULT ((0)) FOR [RowIndex]
GO
ALTER TABLE [dbo].[PatientVisits] ADD  CONSTRAINT [Default_PatientVisits_VisitDate]  DEFAULT (getdate()) FOR [VisitDate]
GO
ALTER TABLE [dbo].[PatientVisits] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[PatientVisits] ADD  DEFAULT ((0)) FOR [QueueIndex]
GO
ALTER TABLE [dbo].[PatientVisits] ADD  DEFAULT (CONVERT([bit],(0))) FOR [RemainPayed]
GO
ALTER TABLE [dbo].[PatientVisits] ADD  DEFAULT ((0)) FOR [RemainPayValue]
GO
ALTER TABLE [dbo].[PatientVisitsTests] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[PatientVisitsTests] ADD  DEFAULT (CONVERT([bit],(0))) FOR [Dropped]
GO
ALTER TABLE [dbo].[PatientVisitsTests] ADD  DEFAULT (CONVERT([bit],(0))) FOR [Locked]
GO
ALTER TABLE [dbo].[Payments] ADD  DEFAULT (getdate()) FOR [PaymentDate]
GO
ALTER TABLE [dbo].[Payments] ADD  DEFAULT ((1)) FOR [Paid]
GO
ALTER TABLE [dbo].[Payments] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[PaymentTypes] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[PaymentTypes] ADD  DEFAULT (CONVERT([bit],(0))) FOR [Debt]
GO
ALTER TABLE [dbo].[Payouts] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Queues] ADD  DEFAULT (getdate()) FOR [VisitDate]
GO
ALTER TABLE [dbo].[Queues] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[ReadyPrescriptionMedicines] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[ReadyPrescriptions] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[ReadyProducts] ADD  DEFAULT ((0.000000000000000e+000)) FOR [ProductedValue]
GO
ALTER TABLE [dbo].[ReadyProducts] ADD  DEFAULT ((0.000000000000000e+000)) FOR [ExportedValue]
GO
ALTER TABLE [dbo].[ReadyProducts] ADD  DEFAULT ((0.000000000000000e+000)) FOR [WasteValue]
GO
ALTER TABLE [dbo].[ReadyProducts] ADD  DEFAULT ((0.000000000000000e+000)) FOR [TotalValue]
GO
ALTER TABLE [dbo].[ReadyProducts] ADD  DEFAULT ((0.000000000000000e+000)) FOR [TotalWaste]
GO
ALTER TABLE [dbo].[ReadyProducts] ADD  DEFAULT ((0.000000000000000e+000)) FOR [TotalWight]
GO
ALTER TABLE [dbo].[ReadyProducts] ADD  DEFAULT ((0.000000000000000e+000)) FOR [TotalResult]
GO
ALTER TABLE [dbo].[Reminders] ADD  DEFAULT (getdate()) FOR [ReminderDate]
GO
ALTER TABLE [dbo].[Reminders] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Reservations] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Roles] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Roles] ADD  DEFAULT ('0001-01-01T00:00:00.0000000') FOR [PassWrong]
GO
ALTER TABLE [dbo].[Tests] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Tests] ADD  DEFAULT (CONVERT([tinyint],(0))) FOR [Imagex]
GO
ALTER TABLE [dbo].[Tests] ADD  DEFAULT ((0)) FOR [Quintity]
GO
ALTER TABLE [dbo].[Tests] ADD  DEFAULT ((0.000000000000000e+000)) FOR [WasteValue]
GO
ALTER TABLE [dbo].[Tests] ADD  DEFAULT ((0.000000000000000e+000)) FOR [UnitValue]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[ContactAccountPayment]  WITH CHECK ADD  CONSTRAINT [FK_ContactAccountPayment_ContactAccount_ContactAccountId] FOREIGN KEY([ContactAccountId])
REFERENCES [dbo].[ContactAccounts] ([Id])
GO
ALTER TABLE [dbo].[ContactAccountPayment] CHECK CONSTRAINT [FK_ContactAccountPayment_ContactAccount_ContactAccountId]
GO
ALTER TABLE [dbo].[ContactAccountPayment]  WITH CHECK ADD  CONSTRAINT [FK_ContactAccountPayment_ContactAccountPaymentCategories_CategoryId] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[AccountPaymentCategories] ([Id])
GO
ALTER TABLE [dbo].[ContactAccountPayment] CHECK CONSTRAINT [FK_ContactAccountPayment_ContactAccountPaymentCategories_CategoryId]
GO
ALTER TABLE [dbo].[ContactAccountPayment]  WITH CHECK ADD  CONSTRAINT [FK_ContactAccountPayment_ContactAccounts_ContactAccountId] FOREIGN KEY([ContactAccountId])
REFERENCES [dbo].[ContactAccounts] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ContactAccountPayment] CHECK CONSTRAINT [FK_ContactAccountPayment_ContactAccounts_ContactAccountId]
GO
ALTER TABLE [dbo].[ContactAccounts]  WITH CHECK ADD  CONSTRAINT [FK_ContactAccounts_ContactId] FOREIGN KEY([ContactId])
REFERENCES [dbo].[Contacts] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ContactAccounts] CHECK CONSTRAINT [FK_ContactAccounts_ContactId]
GO
ALTER TABLE [dbo].[Medicines]  WITH CHECK ADD  CONSTRAINT [FK_Medicines_MedicineTypes_MedicineTypeId] FOREIGN KEY([MedicineTypeId])
REFERENCES [dbo].[MedicineTypes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Medicines] CHECK CONSTRAINT [FK_Medicines_MedicineTypes_MedicineTypeId]
GO
ALTER TABLE [dbo].[MedicineUsages]  WITH CHECK ADD  CONSTRAINT [FK_MedicineUsages_MedicineTypes_UsageMedicineTypeId] FOREIGN KEY([UsageMedicineTypeId])
REFERENCES [dbo].[MedicineTypes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MedicineUsages] CHECK CONSTRAINT [FK_MedicineUsages_MedicineTypes_UsageMedicineTypeId]
GO
ALTER TABLE [dbo].[OldMedicineViewTable]  WITH CHECK ADD  CONSTRAINT [FK_OldMedicineViewTable_PatientVisit_PatientVisitId] FOREIGN KEY([PatientVisitId])
REFERENCES [dbo].[PatientVisits] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OldMedicineViewTable] CHECK CONSTRAINT [FK_OldMedicineViewTable_PatientVisit_PatientVisitId]
GO
ALTER TABLE [dbo].[PatientDiseases]  WITH CHECK ADD  CONSTRAINT [FK_PatientDiseases_Diseases_DiseaseId] FOREIGN KEY([DiseaseId])
REFERENCES [dbo].[Diseases] ([Id])
GO
ALTER TABLE [dbo].[PatientDiseases] CHECK CONSTRAINT [FK_PatientDiseases_Diseases_DiseaseId]
GO
ALTER TABLE [dbo].[PatientDiseases]  WITH CHECK ADD  CONSTRAINT [FK_PatientDiseases_Patients_PatientId] FOREIGN KEY([PatientId])
REFERENCES [dbo].[Patients] ([Id])
GO
ALTER TABLE [dbo].[PatientDiseases] CHECK CONSTRAINT [FK_PatientDiseases_Patients_PatientId]
GO
ALTER TABLE [dbo].[PatientGlass]  WITH CHECK ADD  CONSTRAINT [FK_PatientGlass_PatientId] FOREIGN KEY([PatientId])
REFERENCES [dbo].[Patients] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PatientGlass] CHECK CONSTRAINT [FK_PatientGlass_PatientId]
GO
ALTER TABLE [dbo].[PatientOperations]  WITH CHECK ADD  CONSTRAINT [FK_PatientOperations_MedicalCenters_MedicalCenterId] FOREIGN KEY([MedicalCenterId])
REFERENCES [dbo].[MedicalCenters] ([Id])
GO
ALTER TABLE [dbo].[PatientOperations] CHECK CONSTRAINT [FK_PatientOperations_MedicalCenters_MedicalCenterId]
GO
ALTER TABLE [dbo].[PatientOperations]  WITH CHECK ADD  CONSTRAINT [FK_PatientOperations_Operations_LeftEyeOperationId] FOREIGN KEY([LeftEyeOperationId])
REFERENCES [dbo].[Operations] ([Id])
GO
ALTER TABLE [dbo].[PatientOperations] CHECK CONSTRAINT [FK_PatientOperations_Operations_LeftEyeOperationId]
GO
ALTER TABLE [dbo].[PatientOperations]  WITH CHECK ADD  CONSTRAINT [FK_PatientOperations_Operations_RightEyeOperationId] FOREIGN KEY([RightEyeOperationId])
REFERENCES [dbo].[Operations] ([Id])
GO
ALTER TABLE [dbo].[PatientOperations] CHECK CONSTRAINT [FK_PatientOperations_Operations_RightEyeOperationId]
GO
ALTER TABLE [dbo].[PatientOperations]  WITH CHECK ADD  CONSTRAINT [FK_PatientOperations_Patients_PatientId] FOREIGN KEY([PatientId])
REFERENCES [dbo].[Patients] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PatientOperations] CHECK CONSTRAINT [FK_PatientOperations_Patients_PatientId]
GO
ALTER TABLE [dbo].[PatientOperationSessions]  WITH CHECK ADD  CONSTRAINT [FK_PatientOperationSessions_PatientOperations_PatientOperationId] FOREIGN KEY([PatientOperationId])
REFERENCES [dbo].[PatientOperations] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PatientOperationSessions] CHECK CONSTRAINT [FK_PatientOperationSessions_PatientOperations_PatientOperationId]
GO
ALTER TABLE [dbo].[Patients]  WITH CHECK ADD  CONSTRAINT [FK_Patients_Glasses_GlassId] FOREIGN KEY([GlassId])
REFERENCES [dbo].[Glasses] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Patients] CHECK CONSTRAINT [FK_Patients_Glasses_GlassId]
GO
ALTER TABLE [dbo].[Patients]  WITH CHECK ADD  CONSTRAINT [FK_Patients_Locations_LocationId] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Locations] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Patients] CHECK CONSTRAINT [FK_Patients_Locations_LocationId]
GO
ALTER TABLE [dbo].[Patients]  WITH CHECK ADD  CONSTRAINT [FK_Patients_MartialStatus_MartialStatusId] FOREIGN KEY([MartialStatusId])
REFERENCES [dbo].[MartialStatus] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Patients] CHECK CONSTRAINT [FK_Patients_MartialStatus_MartialStatusId]
GO
ALTER TABLE [dbo].[PatientSickStory]  WITH CHECK ADD  CONSTRAINT [FK_PatientSickStory_Patients_PatientId] FOREIGN KEY([PatientId])
REFERENCES [dbo].[Patients] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PatientSickStory] CHECK CONSTRAINT [FK_PatientSickStory_Patients_PatientId]
GO
ALTER TABLE [dbo].[PatientVisitDiagnosis]  WITH CHECK ADD  CONSTRAINT [FK_PatientVisitDiagnosis_Diagnosis_DiagnosisId] FOREIGN KEY([DiagnosisId])
REFERENCES [dbo].[Diagnosis] ([Id])
GO
ALTER TABLE [dbo].[PatientVisitDiagnosis] CHECK CONSTRAINT [FK_PatientVisitDiagnosis_Diagnosis_DiagnosisId]
GO
ALTER TABLE [dbo].[PatientVisitDiagnosis]  WITH CHECK ADD  CONSTRAINT [FK_PatientVisitDiagnosis_PatientVisits_PatientVisitId] FOREIGN KEY([PatientVisitId])
REFERENCES [dbo].[PatientVisits] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PatientVisitDiagnosis] CHECK CONSTRAINT [FK_PatientVisitDiagnosis_PatientVisits_PatientVisitId]
GO
ALTER TABLE [dbo].[PatientVisitEyeTests]  WITH CHECK ADD  CONSTRAINT [FK_PatientVisitEyeTests_EyeTests_EyeTestId] FOREIGN KEY([EyeTestId])
REFERENCES [dbo].[EyeTests] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PatientVisitEyeTests] CHECK CONSTRAINT [FK_PatientVisitEyeTests_EyeTests_EyeTestId]
GO
ALTER TABLE [dbo].[PatientVisitEyeTests]  WITH CHECK ADD  CONSTRAINT [FK_PatientVisitEyeTests_PatientVisits_PatientVisitId] FOREIGN KEY([PatientVisitId])
REFERENCES [dbo].[PatientVisits] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PatientVisitEyeTests] CHECK CONSTRAINT [FK_PatientVisitEyeTests_PatientVisits_PatientVisitId]
GO
ALTER TABLE [dbo].[PatientVisitGlass]  WITH CHECK ADD  CONSTRAINT [FK_PatientVisitGlass_PatientVisitId] FOREIGN KEY([PatientVisitId])
REFERENCES [dbo].[PatientVisits] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PatientVisitGlass] CHECK CONSTRAINT [FK_PatientVisitGlass_PatientVisitId]
GO
ALTER TABLE [dbo].[PatientVisitLabTests]  WITH CHECK ADD  CONSTRAINT [FK_PatientVisitLabTests_LabTestId] FOREIGN KEY([LabTestId])
REFERENCES [dbo].[LabTests] ([Id])
GO
ALTER TABLE [dbo].[PatientVisitLabTests] CHECK CONSTRAINT [FK_PatientVisitLabTests_LabTestId]
GO
ALTER TABLE [dbo].[PatientVisitLabTests]  WITH CHECK ADD  CONSTRAINT [FK_PatientVisitLabTests_PatientVisitId] FOREIGN KEY([PatientVisitId])
REFERENCES [dbo].[PatientVisits] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PatientVisitLabTests] CHECK CONSTRAINT [FK_PatientVisitLabTests_PatientVisitId]
GO
ALTER TABLE [dbo].[PatientVisitPrescriptions]  WITH CHECK ADD  CONSTRAINT [FK_PatientVisitPrescriptions_Medicines_MedicineId] FOREIGN KEY([MedicineId])
REFERENCES [dbo].[Medicines] ([Id])
GO
ALTER TABLE [dbo].[PatientVisitPrescriptions] CHECK CONSTRAINT [FK_PatientVisitPrescriptions_Medicines_MedicineId]
GO
ALTER TABLE [dbo].[PatientVisitPrescriptions]  WITH CHECK ADD  CONSTRAINT [FK_PatientVisitPrescriptions_MedicineUsages_UsageId] FOREIGN KEY([MedicineUsageId])
REFERENCES [dbo].[MedicineUsages] ([Id])
GO
ALTER TABLE [dbo].[PatientVisitPrescriptions] CHECK CONSTRAINT [FK_PatientVisitPrescriptions_MedicineUsages_UsageId]
GO
ALTER TABLE [dbo].[PatientVisitPrescriptions]  WITH CHECK ADD  CONSTRAINT [FK_PatientVisitPrescriptions_PatientVisits_PatientVisitId] FOREIGN KEY([PatientVisitId])
REFERENCES [dbo].[PatientVisits] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PatientVisitPrescriptions] CHECK CONSTRAINT [FK_PatientVisitPrescriptions_PatientVisits_PatientVisitId]
GO
ALTER TABLE [dbo].[PatientVisits]  WITH CHECK ADD  CONSTRAINT [FK_PatientVisits_NotPayReasons_Id] FOREIGN KEY([NotPaymentReasonId])
REFERENCES [dbo].[NotPayReasons] ([Id])
GO
ALTER TABLE [dbo].[PatientVisits] CHECK CONSTRAINT [FK_PatientVisits_NotPayReasons_Id]
GO
ALTER TABLE [dbo].[PatientVisits]  WITH CHECK ADD  CONSTRAINT [FK_PatientVisits_Patients_PatientId] FOREIGN KEY([PatientId])
REFERENCES [dbo].[Patients] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PatientVisits] CHECK CONSTRAINT [FK_PatientVisits_Patients_PatientId]
GO
ALTER TABLE [dbo].[PatientVisitsTests]  WITH CHECK ADD  CONSTRAINT [FK_PatientVisitsTests_MedicalCenters_MedicalCenterId] FOREIGN KEY([MedicalCenterId])
REFERENCES [dbo].[MedicalCenters] ([Id])
GO
ALTER TABLE [dbo].[PatientVisitsTests] CHECK CONSTRAINT [FK_PatientVisitsTests_MedicalCenters_MedicalCenterId]
GO
ALTER TABLE [dbo].[PatientVisitsTests]  WITH CHECK ADD  CONSTRAINT [FK_PatientVisitsTests_PatientVisits_PatientVisitId] FOREIGN KEY([PatientVisitId])
REFERENCES [dbo].[PatientVisits] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PatientVisitsTests] CHECK CONSTRAINT [FK_PatientVisitsTests_PatientVisits_PatientVisitId]
GO
ALTER TABLE [dbo].[PatientVisitsTests]  WITH CHECK ADD  CONSTRAINT [FK_PatientVisitsTests_Tests_TestId] FOREIGN KEY([TestId])
REFERENCES [dbo].[Tests] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PatientVisitsTests] CHECK CONSTRAINT [FK_PatientVisitsTests_Tests_TestId]
GO
ALTER TABLE [dbo].[Payments]  WITH CHECK ADD  CONSTRAINT [FK_Payments_PaymentTypes_PaymentTypeId] FOREIGN KEY([PaymentTypeId])
REFERENCES [dbo].[PaymentTypes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Payments] CHECK CONSTRAINT [FK_Payments_PaymentTypes_PaymentTypeId]
GO
ALTER TABLE [dbo].[Queues]  WITH CHECK ADD  CONSTRAINT [FK_Queues_Patients_PatientId] FOREIGN KEY([PatientId])
REFERENCES [dbo].[Patients] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Queues] CHECK CONSTRAINT [FK_Queues_Patients_PatientId]
GO
ALTER TABLE [dbo].[ReadyPrescriptionMedicines]  WITH CHECK ADD  CONSTRAINT [FK_ReadyPrescriptionMedicines_Medicines_MedicineId] FOREIGN KEY([MedicineId])
REFERENCES [dbo].[Medicines] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ReadyPrescriptionMedicines] CHECK CONSTRAINT [FK_ReadyPrescriptionMedicines_Medicines_MedicineId]
GO
ALTER TABLE [dbo].[ReadyPrescriptionMedicines]  WITH CHECK ADD  CONSTRAINT [FK_ReadyPrescriptionMedicines_MedicineUsages] FOREIGN KEY([MedicineUsageId])
REFERENCES [dbo].[MedicineUsages] ([Id])
GO
ALTER TABLE [dbo].[ReadyPrescriptionMedicines] CHECK CONSTRAINT [FK_ReadyPrescriptionMedicines_MedicineUsages]
GO
ALTER TABLE [dbo].[ReadyPrescriptionMedicines]  WITH CHECK ADD  CONSTRAINT [FK_ReadyPrescriptionMedicines_ReadyPrescriptions_ReadyPrescriptionId] FOREIGN KEY([ReadyPrescriptionId])
REFERENCES [dbo].[ReadyPrescriptions] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ReadyPrescriptionMedicines] CHECK CONSTRAINT [FK_ReadyPrescriptionMedicines_ReadyPrescriptions_ReadyPrescriptionId]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Roles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Roles_RoleId]
GO
